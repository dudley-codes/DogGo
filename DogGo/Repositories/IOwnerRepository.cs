using DogGo.Models;
using System.Collections.Generic;


namespace DogGo.Repositories
{
    public interface IOwnerRepository
    {
        List<Owner> GetAllOwners();
        Owner GetOwnerById(int id);
        void AddOwner(Owner owner);
        void DeleteOwner(int id);
        void UpdateOwner(Owner owner);
    }
}
