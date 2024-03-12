using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailAPI.Data.Models
{
    public class MailToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TokenID { get; set; }
        [Required]
        public string Key { get; set; } = string.Empty;
        [Required]
        public DateTime ExpirationDate { get; set; }

        // Внешний ключ для связи с User
        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
