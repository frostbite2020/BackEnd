using Contracts;
using Entities.Context;
using Entities.Helper;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class TodoListRepository : RepositoryBase<TodoList>, ITodoListRepository
    {

        public TodoListRepository(ApplicationDBContext option) : base(option)
        {
        }
        public IEnumerable<TodoList> ListsByCategory(int todoCategoryId)
        {
            return FindByCondition(x => x.TodoCategoryID.Equals(todoCategoryId)).ToList();
        }
        public PagedList<TodoList> GetAllTodoLists(TodoListParameters todoListParameters )
        {
            return PagedList<TodoList>.ToPagedList(FindAll().OrderBy(o => o.ActivityTitle),
                todoListParameters.PageNumber,
                todoListParameters.PageSize);
        }
        public IEnumerable<TodoList> GetListByCategoryID(int todoCategoryID)
        {
            return FindByCondition(x => x.TodoCategoryID.Equals(todoCategoryID)).ToList();
        }
        public TodoList GetTodoListById(int id)
        {
            return FindByCondition(x => x.TodoID.Equals(id)).FirstOrDefault();
        }
        public IEnumerable<TodoList> GetAll( )
        {
            return FindAll()
                .OrderBy(x => x.ActivityTitle)
                .ToList();
        }
        public TodoList GetTodoListTitle(string title)
        {
            return FindByCondition(a => a.ActivityTitle.Equals(title))
                .FirstOrDefault();
        }

        public void CreateTodoList(TodoList todoList)
        {
            Create(todoList);
        }
        public void UpdateTodoList(TodoList todoList)
        {
            Update(todoList);
        }
        public void DeleteTodoList(TodoList todoList)
        {
            Delete(todoList);
        }
    }
}
