using WebApiDat.Data.Response;
using WebApiDat.Data.Model;
using System.Collections.Generic;
using WebApiDat.Database.SqlServer.Entity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebApiDat.Database.Domain
{
    public interface IUsersRepository
    {
        List<UsersResponse> GetAllUser();

        UsersResponse GetUserByUsername(string username);

        UsersResponse AddUser(UsersModel usersModel);

        void UpdateUser(string id, UsersModel usersModel);

        void DeleteUser(string id);

        UsersEntity ValidateUser(string username, string password);
    }
}
