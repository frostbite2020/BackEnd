using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.TodoCategoryDto
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Category title is required!")]
        public string CategoryTitle { get; set; }
    }
}
