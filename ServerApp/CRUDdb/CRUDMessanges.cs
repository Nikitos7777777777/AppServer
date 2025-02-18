﻿using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.CRUDdb
{
    public class CRUDMessanges
    {
        private MessengerDbContext _context;

        public CRUDMessanges(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetMessagesChannel()
        {
            return await _context.Messages.Include(u => u.Sender).ToListAsync();
        }
    }
}
