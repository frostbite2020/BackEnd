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
    public class OwnerRepository  : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(ApplicationDBContext option) : base(option)
        {
        }

        public IEnumerable<Owner> GetAllOwners()
        {
            return FindAll()
                .OrderBy(o => o.Name)
                .ToList();
        }
        public Owner GetOwnerById(Guid ownerId)
        {
            return FindByCondition(x => x.Id.Equals(ownerId))
                .FirstOrDefault();
        }
        public Owner GetOwnerWithDetails(Guid ownerId)
        {
            return FindByCondition(x => x.Id.Equals(ownerId))
                .Include(a => a.Accounts)
                .FirstOrDefault();
        }
        public void CreateOwner(Owner owner)
        {
            Create(owner);
        }
        public void UpdateOwner(Owner owner)
        {
            Update(owner);
        }
        public void DeleteOwner(Owner owner)
        {
            Delete(owner);
        }
    }
}
