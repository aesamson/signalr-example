using App.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Server.Database
{
    public class DbChatContext : DbContext
    {
        public DbChatContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<DbMessage> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<DbMessage>(m =>
            {
                m.HasKey(x => x.Id);
            });
        }
    }
}