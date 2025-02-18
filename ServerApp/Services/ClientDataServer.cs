using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using NATS.Client.Internals;
using ServerApp.CRUDdb;
using ServerApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf.Reflection;
using System.Reflection;

namespace ServerApp.Services
{
    public class ClientDataServer : ClientData.ClientDataBase
    {
        private CRUDUser _CRUDUser;
        private CRUDMessanges _CRUDMessanges;
        private string _secretKey;
        private string _issuer;
        private string _audience;
        public ClientDataServer(CRUDUser crUDUser, CRUDMessanges cRUDMessanges) 
        {
            _CRUDUser = crUDUser;
            _CRUDMessanges = cRUDMessanges;
            JwtConfig();
        }
        public void JwtConfig()
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var getJWT = config.GetSection("JwtSettings");
            _secretKey = getJWT["SecretKey"];
            _issuer = getJWT["Issuer"];
            _audience = getJWT["Audience"];
        }

        public override async Task<DataResponse> DataReturn(DataRequest request, ServerCallContext context)
        {
            string user = ValidToken(request.Token);

            if (user == null)
            {
                return new DataResponse() { };
            }

            var users = await _CRUDUser.GetAllUsers();

            foreach (var userItem in users)
            {
                if (user == userItem.Username)
                {                  
                    List<UserItem> friends = _CRUDUser.GetFriend(userItem);

                    var respons = new DataResponse()
                    {
                        Username = userItem.Username,
                        Email = userItem.Email,
                        Avatar = userItem.AvatarUrl,
                        CreateAt = userItem.CreatedAt.ToString(),
                    };

                    respons.Friends.AddRange(friends);

                    return respons;
                }
            }
            return new DataResponse { };
        }
        public override async Task<HistoryMessRespons> MessageHistoryData(MessHistRequest request, ServerCallContext context)
        {
            string user = ValidToken(request.Token);

            if (user == null)
            {
                return new HistoryMessRespons() { };
            }

            var messengsChannel = await _CRUDMessanges.GetMessagesChannel();
            List<messageItem> listMess = new List<messageItem>();

            foreach (var message in messengsChannel)
            {
                if (message.ChannelName == request.ChannelName)
                {
                    var sender = new UserItem()
                    {
                        Username = message.Sender.Username,
                        Email = message.Sender.Email,
                        Avatar = message.Sender.AvatarUrl,
                        CreateAt = message.Sender.CreatedAt.ToString(),
                    };

                    var mess = new messageItem()
                    {
                        NameChannel = message.ChannelName,
                        UserSender = sender,
                        TextMess = message.MessageText,
                        SendAt = message.SentAt.ToString(),
                    };
                    listMess.Add(mess);
                }
            }
            var histMess = new HistoryMessRespons();
            histMess.ListMess.AddRange(listMess);

            return histMess;
        }

        public override async Task<Empty> AddDBMessData(DBMessRequest request, ServerCallContext context)
        {
            var user = await _CRUDUser.GetUniqueUser(request.UserSender);

            DateTime dbTime = DateTime.SpecifyKind(request.SendAt.ToDateTime(), DateTimeKind.Unspecified);

            var mess = new Message()
            {
                ChannelName = request.ChannelName,
                Sender = user,
                MessageText = request.MessangeText,
                SentAt = dbTime,
            };
            await _CRUDMessanges.AddMessHist(mess);
            return new Empty();
        }
        public string ValidToken(string token)
        {
            string username = null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                var ClaimsMain = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer, 
                    ValidateAudience = true,
                    ValidAudience = _audience, 
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                username = ClaimsMain.FindFirst(ClaimTypes.Name)?.Value;
                return username;
            }
            catch (SecurityTokenExpiredException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
