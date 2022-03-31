using Helperland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Repository
{
    public interface IAccountControllerRepository
    {
        //User Table
        User GetUserByEmailAndPassword(string email, string password);

        User GetUserByEmail(string email);

        User Update(User user);

        User Add(User user);
    }
}
