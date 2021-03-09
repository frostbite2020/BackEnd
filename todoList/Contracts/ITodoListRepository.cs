using Entities.Helper;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ITodoListRepository
    {
        IEnumerable<TodoList> ListsByCategory(int todoCategoryId);
        PagedList<TodoList> GetAllTodoLists(TodoListParameters todoListParameters);
        IEnumerable<TodoList> GetAll();
        IEnumerable<TodoList> GetListByCategoryID(int todoCategoryID);
        TodoList GetTodoListById(int id);
        TodoList GetTodoListTitle(string title);
        void CreateTodoList(TodoList todoList);
        void UpdateTodoList(TodoList todoList);
        void DeleteTodoList(TodoList todoList);
    }
}
