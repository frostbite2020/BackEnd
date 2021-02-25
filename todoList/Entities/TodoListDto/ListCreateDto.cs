using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.TodoListDto
{
    public class ListCreateDto
    {
        
        [Required(ErrorMessage = "This field is required!")]
        public string ActivityTitle { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string Priority { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string Note { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public bool status { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public int TodoCategoryID { get; set; }
    }
}
