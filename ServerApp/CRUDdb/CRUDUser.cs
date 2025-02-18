using Microsoft.EntityFrameworkCore;
using ServerApp.Models;
using System.Collections.Generic;

namespace ServerApp.CRUDdb
{
    public class CRUDUser
    {
        private MessengerDbContext _context;

        public CRUDUser(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.Include(u => u.Friends).ToListAsync();
        }

        public List<UserItem> GetFriend(User users)
        {
            List<UserItem> list = new List<UserItem>();

            foreach (var items in users.Friends)
            {
                list.Add(new UserItem()
                {
                    Username = items.Username,
                    Email = items.Email,
                    Avatar = items.AvatarUrl,
                    CreateAt = items.CreatedAt.ToString(),
                    PersonalChannel = items.Personalchannel
                });
            }
            return list;
        }
    }
}
