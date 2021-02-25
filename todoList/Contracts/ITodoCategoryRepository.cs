using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ITodoCategoryRepository
    {
        IEnumerable<TodoCategory> GetAllTodoCategories();
        TodoCategory GetTodoCategoryById(int id);
        TodoCategory GetCategoryWithTodoLists(int id);
        TodoCategory GetCategoryTitle(string title);
        void CreateTodoCategory(TodoCategory todoCategory);
        void UpdateTodoCategory(TodoCategory todoCategory);
        void DeleteTodoCategory(TodoCategory todoCategory);
    }
}
