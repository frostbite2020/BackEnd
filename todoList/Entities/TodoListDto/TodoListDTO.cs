using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTO
{
    public class TodoListDTO
    {
        public long TodoID { get; set; }
        public string ActivityTitle { get; set; }
        public string Priority { get; set; }
        public string Note { get; set; }

        public bool status { get; set; }
        public int TodoCategoryID { get; set; }
    }
}
