using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class TodoCategory
    {

        [Key]
        public int ID { get; set; }
        [Required]
        public string CategoryTitle { get; set; }
        public ICollection<TodoList> TodoLists { get; set; }
    }
}
