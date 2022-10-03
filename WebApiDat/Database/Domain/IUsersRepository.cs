using WebApiDat.Data.Response;
using WebApiDat.Data.Model;
using System.Collections.Generic;


namespace WebApiDat.Database.Domain
{
    public interface IUsersRepository
    {
        List<UsersResponse> GetAllUser();

        UsersResponse GetUserByUsername(string username);

        UsersResponse AddUser(UsersModel usersModel);

        void UpdateUser(string id, UsersModel usersModel);

        void DeleteUser(string id);
    }
}
