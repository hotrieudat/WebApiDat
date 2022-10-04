using WebApiDat.Data.Response;
using WebApiDat.Data.Model;
using WebApiDat.Database.Domain;
using WebApiDat.Database.SqlServer.Entity;
using System.Collections.Generic;
using System.Linq;
using System;

namespace WebApiDat.Database.SqlServer.Repository
{

    public class UsersRepository : IUsersRepository
    {
        private readonly MyDbContext Context;
        public UsersRepository(MyDbContext context)
        {
            Context = context;
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

            Context.UsersEntity.Add(user);
            Context.SaveChanges();

            return new UsersResponse
            {
                UserName = usersModel.UserName,
            };
        }

        public void DeleteUser(string username)
        {
            var user = Context.UsersEntity.SingleOrDefault(u => u.UserName == username);

            if (user != null)
            {
                Context.UsersEntity.Remove(user);
                Context.SaveChanges();
            } 
        }

        public List<UsersResponse> GetAllUser()
        {
            var users = Context.UsersEntity.Select(user => new UsersResponse
            {
                UserName = user.UserName,
                Name = user.Name,
                Email = user.Email,
                
            }).ToList();

            return users;
        }

        public UsersResponse GetUserByUsername(string userName)
        {
            var user = Context.UsersEntity.SingleOrDefault(u => u.UserName == userName);

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
            var user = Context.UsersEntity.SingleOrDefault(u => u.UserName == userName);

            user.UserName = usersModel.UserName;
            user.LoginPw = usersModel.LoginPw;
            user.Name = usersModel.Name;
            user.Email = usersModel.Email;

            Context.SaveChanges();
        }

        public UsersEntity ValidateUser(string username, string password)
        {
            return Context.UsersEntity.SingleOrDefault(u => u.UserName == username && u.LoginPw == password);
        }
    }
}
