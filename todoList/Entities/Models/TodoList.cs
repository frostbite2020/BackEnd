using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class TodoList
    {
        [Key]
        public long TodoID { get; set; }
        public string ActivityTitle { get; set; }
        public string Priority { get; set; }
        public string Note { get; set; }
        
        public bool status { get; set; }
        [Required]
        public int TodoCategoryID { get; set; }
        [ForeignKey("TodoCategoryID")]
        public TodoCategory TodoCategory { get; set; }
    }
}
