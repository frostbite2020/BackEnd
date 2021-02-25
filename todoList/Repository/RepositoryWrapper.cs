using Contracts;
using Entities.Context;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDBContext _context;
        private IOwnerRepository _owner;
        private IAccountRepository _account;
        private ITodoCategoryRepository _todoCategory;
        private ITodoListRepository _todoList;

        public IOwnerRepository Owner
        {
            get
            {
                if (_owner == null)
                {
                    _owner = new OwnerRepository(_context);
                }
                return _owner;
            }
        }
        public IAccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_context);
                }
                return _account;
            }
        }
        public ITodoCategoryRepository TodoCategory
        {
            get
            {
                if(_todoCategory == null)
                {
                    _todoCategory = new TodoCategoryRepository(_context);
                }
                return _todoCategory;
            }
        }
        
        public ITodoListRepository TodoList
        {
            get
            {
                if(_todoList == null)
                {
                    _todoList = new TodoListRepository(_context);
                }
                return _todoList;
            }
        }
        public RepositoryWrapper(ApplicationDBContext context)
        {
            _context = context;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
