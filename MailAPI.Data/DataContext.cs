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
        DbSet<User> Users { get; set; }
        DbSet<Role> Role { get; set; }
        DbSet<MessageHistory> MessageHistory { get; set; }
        DbSet<Message> Message { get; set; }
        DbSet<MailToken> MailToken { get; set; }
        protected override void OnModelCreating(ModelBuilder Modelbuilder)
        {

        }
    }
}
