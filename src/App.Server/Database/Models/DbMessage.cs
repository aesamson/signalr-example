using System;

namespace App.Server.Database.Models
{
    public class DbMessage
    {
        public DbMessage()
        {
            Created = DateTimeOffset.Now;
        }

        public long Id { get; set; }
        
        public string Data { get; set; }

        public string Nickname { get; set; }
        
        public DateTimeOffset Created { get; set; }
    }
}