using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailAPI.Data.Models
{
    internal class Role
    {
        [Key]
        public int RoleId { get; set; }
        public  string RoleName { get; set; }
    }
}
