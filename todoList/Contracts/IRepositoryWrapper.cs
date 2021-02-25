using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IOwnerRepository Owner { get;  }
        IAccountRepository Account { get;  }
        ITodoCategoryRepository TodoCategory { get; }
        ITodoListRepository TodoList { get; }
        public void Save();
    }
}
