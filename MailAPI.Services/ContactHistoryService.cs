using MailAPI.Data;
using MailAPI.Data.Models;
using MailAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailAPI.Services
{
    public class ContactHistoryService : IContactService
    {
        private readonly DbContextOptions<DataContext> dbContextOptions;

        public ContactHistoryService(DbContextOptions<DataContext> dbContext)
        {
            this.dbContextOptions = dbContext;
        }
        public async Task<bool> AddContactHistory(int userId, string email, string name, string description)
        {
            try
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    await dataContext.ContactHistory.AddAsync(new ContactHistory
                    {
                        UserID = userId,
                        ContactMail = email,
                        ContactName = name,
                        Description = description
                    });
                    await dataContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteContactHistory(int id)
        {
            try
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    var contact = await dataContext.ContactHistory.FirstOrDefaultAsync(x => x.ContactHistoryID == id);
                    if (contact != null)
                    {
                        dataContext.ContactHistory.Remove(contact);
                        await dataContext.SaveChangesAsync();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<ContactHistory> GetContactHistory(int id)
        {
            using (var dataContext = new DataContext(dbContextOptions))
            {
                var contact =await dataContext.ContactHistory.FirstOrDefaultAsync(x => x.ContactHistoryID == id);
                if (contact != null)
                {
                    return contact;
                }
                else
                    return new ContactHistory();
            }
        }

        public async Task<bool> UpdateContactHistory(int id, string email, string name, string description)
        {
            try
            {
                using (var dataContext = new DataContext(dbContextOptions))
                {
                    var contact = await dataContext.ContactHistory.FirstOrDefaultAsync(x => x.ContactHistoryID == id);
                    if (contact != null)
                    {
                        contact.Description = description;
                        contact.ContactName = name;
                        contact.ContactMail = email;
                        await dataContext.SaveChangesAsync();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
