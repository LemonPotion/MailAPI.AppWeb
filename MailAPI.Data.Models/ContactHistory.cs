using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailAPI.Data.Models
{
    public class ContactHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactHistoryID { get; set; }

        [Required]
        public string ContactName{ get; set; }
        [Required]
        public string ContactMail { get; set; }

        public string Description { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
