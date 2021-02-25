using Contracts;
using Entities.Context;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
   public class TodoCategoryRepository : RepositoryBase<TodoCategory>, ITodoCategoryRepository
    {
        
        public TodoCategoryRepository(ApplicationDBContext option) : base(option)
        {
        }

        public IEnumerable<TodoCategory> GetAllTodoCategories()
        {
            return FindAll()
                .OrderBy(x => x.CategoryTitle)
                .ToList();
        }
        public TodoCategory GetTodoCategoryById(int id)
        {
            return FindByCondition(x => x.ID.Equals(id)).FirstOrDefault();
        }

        public TodoCategory GetCategoryWithTodoLists(int id)
        {
            return FindByCondition(x => x.ID.Equals(id))
                .Include(a => a.TodoLists)
                .FirstOrDefault();
        }
        public TodoCategory GetCategoryTitle(string title)
        {
            return FindByCondition(a => a.CategoryTitle.Equals(title))
                .FirstOrDefault();
        }

        public void CreateTodoCategory(TodoCategory todoCategory)
        {
            Create(todoCategory);
        }
        public void UpdateTodoCategory(TodoCategory todoCategory)
        {
            Update(todoCategory);
        }
        public void DeleteTodoCategory(TodoCategory todoCategory)
        {
            Delete(todoCategory);
        }
    }
}
