using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailAPI.Data.Models;

namespace MailAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<ContactHistory> ContactHistory { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<MailToken> MailToken { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

    }
}
