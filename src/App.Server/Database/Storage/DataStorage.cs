using System.Threading.Tasks;
using App.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Server.Database.Storage
{
    public class DataStorage : IDataStorage
    {
        private readonly DbChatContext _context;

        public DataStorage(DbChatContext context)
        {
            _context = context;
        }

        public Task<DbMessage[]> GetAllMessages()
        {
            return _context.Messages.ToArrayAsync();
        }

        public async Task<DbMessage> StoreMessage(string message, string nick)
        {
            var entity = new DbMessage
            {
                Data = message,
                Nickname = nick
            };

            await _context.Messages.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}