using WebApiDat.Data.Client;
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

        public UsersClient AddUser(UsersModel usersModel)
        {
            var user = new UsersEntity
            {
                UserName = usersModel.UserName,
                LoginPw = usersModel.LoginPw,
                UserId = Guid.NewGuid().ToString(),
            };

            Context.UsersEntity.Add(user);
            Context.SaveChanges();

            return new UsersClient
            {
                UserName = usersModel.UserName,
            };
        }

        public void DeleteUser(string id)
        {
            var user = Context.UsersEntity.SingleOrDefault(u => u.UserId == id);

            if (user != null)
            {
                Context.UsersEntity.Remove(user);
                Context.SaveChanges();
            } 
        }

        public List<UsersClient> GetAllUser()
        {
            var users = Context.UsersEntity.Select(user => new UsersClient
            {
                UserName = user.UserName
            }).ToList();

            return users;
        }

        public UsersClient GetUserById(string name)
        {
            var user = Context.UsersEntity.SingleOrDefault(u => u.UserName == name);

            if (user != null)
            {
                return new UsersClient
                {
                    UserName = user.UserName,
                };
            }

            return null;
        }

        public void UpdateUser(string id, UsersModel usersModel)
        {
            var user = Context.UsersEntity.SingleOrDefault(u => u.UserId == id);

            user.UserName = usersModel.UserName;

            user.LoginPw = usersModel.LoginPw;

            Context.SaveChanges();
        }
    }
}
