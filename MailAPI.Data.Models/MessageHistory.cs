using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailAPI.Data.Models
{
    public class MessageHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageHistoryID { get; set; }

        [Required]
        // Внешний ключ для связи с User
        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        [Required]
        // Внешний ключ для связи с Message
        [ForeignKey("Message")]
        public int MessageID { get; set; }
        public Message Message { get; set; }
    }
}
