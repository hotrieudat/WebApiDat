using WebApiDat.Data.Client;
using WebApiDat.Data.Model;
using System.Collections.Generic;


namespace WebApiDat.Database.Domain
{
    public interface IUsersRepository
    {
        List<UsersClient> GetAllUser();

        UsersClient GetUserById(string id);

        UsersClient AddUser(UsersModel usersModel);

        void UpdateUser(string id, UsersModel usersModel);

        void DeleteUser(string id);
    }
}
