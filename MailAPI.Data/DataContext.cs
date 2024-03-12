﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<MessageHistory> MessageHistory { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<MailToken> MailToken { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageHistory>()
                .HasOne(mh => mh.User)
                .WithMany()
                .HasForeignKey(mh => mh.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageHistory>()
                .HasOne(mh => mh.Message)
                .WithMany()
                .HasForeignKey(mh => mh.MessageID)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
