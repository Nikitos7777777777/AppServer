using Grpc.Core;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace ServerApp.Services
{
    public class SendingNotificationsService : CreatSendingNotificationProto.CreatSendingNotificationProtoBase
    {
        private static readonly ConcurrentDictionary<string, IServerStreamWriter<SignalingMessage>> Clients = new();
        private static readonly ConcurrentDictionary<string, ConcurrentQueue<SignalingMessage>> PendingMessages = new();

        public override async Task<ResponsCall> SendOffer(MessangeCall request, ServerCallContext context)
        {
            await StoreOrSendMessage(new SignalingMessage { Ms = request });
            return new ResponsCall { Success = true };
        }

        public override async Task<ResponsCall> SendAnswer(MessangeCall request, ServerCallContext context)
        {
            await StoreOrSendMessage(new SignalingMessage { Ms = request });
            return new ResponsCall { Success = true };
        }

        public override async Task StreamMessages(StreamRequest request, IServerStreamWriter<SignalingMessage> responseStream, ServerCallContext context)
        {
            Clients[request.ClientId] = responseStream;

            try
            {
                if (PendingMessages.TryRemove(request.ClientId, out var queue))
                {
                    foreach(var mess in queue)
                    {
                        if(Clients.ContainsKey(mess.Ms.FromClient))
                        {
                            while (queue.TryDequeue(out var message))
                            {
                                await responseStream.WriteAsync(message);
                            }
                        }
                    }
                }

                await Task.Run(() => context.CancellationToken.WaitHandle.WaitOne(), context.CancellationToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Client {request.ClientId} disconnected: {ex.Message}");
            }
            finally
            {
                Debug.WriteLine($"Client {request.ClientId} disconnected:");
                Clients.TryRemove(request.ClientId, out _);
            }
        }

        private static async Task StoreOrSendMessage(SignalingMessage message)
        {
            if (Clients.TryGetValue(message.Ms.ToClient, out var clientStream))
            {
                await clientStream.WriteAsync(message);
            }
            else
            {
                var queue = PendingMessages.GetOrAdd(message.Ms.ToClient, _ => new ConcurrentQueue<SignalingMessage>());
                queue.Enqueue(message);
            }
        }
    }
}

