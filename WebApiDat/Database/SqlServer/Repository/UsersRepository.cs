using WebApiDat.Data.Response;
using WebApiDat.Data.Model;
using WebApiDat.Database.Domain;
using WebApiDat.Database.SqlServer.Entity;
using System.Collections.Generic;
using System.Linq;
using System;

namespace WebApiDat.Database.SqlServer.Repository
{

    public class UsersRepository : Repository<UsersEntity>, IUsersRepository
    {
        private readonly MyDbContext myContext;
        public UsersRepository(MyDbContext context) : base(context)
        {
            myContext = context;
        }

        public UsersResponse AddUser(UsersModel usersModel)
        {
            var user = new UsersEntity
            {
                UserName = usersModel.UserName,
                LoginPw = usersModel.LoginPw,
                Name = usersModel.Name,
                Email = usersModel.Email,
                UserId = Guid.NewGuid().ToString(),
            };

            myContext.UsersEntity.Add(user);
            myContext.SaveChanges();

            return new UsersResponse
            {
                UserName = usersModel.UserName,
            };
        }

        public void DeleteUser(string username)
        {
            var user = myContext.UsersEntity.SingleOrDefault(u => u.UserName == username);

            if (user != null)
            {
                myContext.UsersEntity.Remove(user);
                myContext.SaveChanges();
            } 
        }

        public List<UsersResponse> GetAllUser()
        {
            var users = myContext.UsersEntity.Select(user => new UsersResponse
            {
                UserName = user.UserName,
                Name = user.Name,
                Email = user.Email,
                
            }).ToList();

            return users;
        }

        public UsersResponse GetUserByUsername(string userName)
        {
            var user = myContext.UsersEntity.SingleOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                return new UsersResponse
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    Email = user.Email,
                };
            }

            return null;
        }

        public void UpdateUser(string userName, UsersModel usersModel)
        {
            var user = myContext.UsersEntity.SingleOrDefault(u => u.UserName == userName);

            user.UserName = usersModel.UserName;
            user.LoginPw = usersModel.LoginPw;
            user.Name = usersModel.Name;
            user.Email = usersModel.Email;

            myContext.SaveChanges();
        }

        public UsersEntity ValidateUser(string username, string password)
        {
            return myContext.UsersEntity.SingleOrDefault(u => u.UserName == username && u.LoginPw == password);
        }
    }
}
