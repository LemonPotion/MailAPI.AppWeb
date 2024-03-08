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
        public int TokenID {  get; set; }
        public string Key{ get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; } 

        [ForeignKey("UserID")]
        public int UserID { get; set; }
    }
}
