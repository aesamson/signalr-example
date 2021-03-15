using System.Threading.Tasks;
using App.Server.Database.Models;

namespace App.Server.Database
{
    public interface IDataStorage
    {        
        public Task<DbMessage[]> GetAllMessages();
        
        public Task<DbMessage> StoreMessage(string message, string nick);
    }
}