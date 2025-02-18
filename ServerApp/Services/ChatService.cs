using Grpc.Core;
using System.Threading.Tasks;
using ServerApp;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity.Data;
using System.Text;
using System.Threading.Tasks;
using NATS.Client;
using NATS.Client.JetStream;

namespace ServerApp.Services
{
    public class ChatService : Chat.ChatBase
    {
        private readonly IConnection _natsConnection;
        private readonly IJetStream _jetStream;
        private const string Subject = "chat.messages";
        private const string QueueGroup = "chat-workers"; // Группа для балансировки

        public ChatService()
        {
            var options = ConnectionFactory.GetDefaultOptions();
            options.Url = "nats://localhost:4222"; // Адрес NATS сервера
            _natsConnection = new ConnectionFactory().CreateConnection(options);
            _jetStream = _natsConnection.CreateJetStreamContext();
        }

        public override async Task JoinChat(JoinRequest request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            string userChannel = $"chat.{request.Username}";
            var sub = _natsConnection.SubscribeAsync(userChannel, QueueGroup); // Балансируем подписки

            sub.MessageHandler += async (_, msg) =>
            {
                var receivedMessage = Encoding.UTF8.GetString(msg.Message.Data);
                await responseStream.WriteAsync(new ChatMessage { Username = "Server", Message = receivedMessage });
            };
            sub.Start();

            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
            }
        }

        public override async Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
        {
            string userChannel = $"chat.{request.Username}";
            var msgBytes = Encoding.UTF8.GetBytes(request.Message);
            await _jetStream.PublishAsync(userChannel, msgBytes);
            return new SendMessageResponse { Success = true };
        }
    }
}




//private static ConcurrentBag<IServerStreamWriter<ChatMessage>> _clients = new();

//    public override async Task ChatStream(IAsyncStreamReader<ChatMessage> requestStream, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
//    {
//        // Добавляем клиента в список
//        _clients.Add(responseStream);

//        try
//        {
//            // Запускаем получение сообщений от клиента
//            await foreach (var message in requestStream.ReadAllAsync())
//            {
//                // Логируем входящее сообщение
//                Console.WriteLine($"[{message.Timestamp}] {message.Username}: {message.Text}");

//                // Рассылаем сообщение всем клиентам
//                foreach (var client in _clients)
//                {
//                    await client.WriteAsync(message);
//                }
//            }
//        }
//        catch
//        {
//            // Если клиент отключился — удаляем его
//            _clients.TryTake(out _);
//        }
//    }

//}
