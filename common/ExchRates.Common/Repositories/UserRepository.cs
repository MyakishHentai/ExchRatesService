using ExchRates.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRates.Common.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<string, FakeUser> _users = new Dictionary<string, FakeUser>();

        public UserRepository()
        {
            var admin = new FakeUser
            {
                UserName = "admin",
                Password = "admin",
                Role = "admin"
            };
            _users.Add(admin.UserName.ToLower(), admin);
            var me = new FakeUser
            {
                UserName = "stale",
                Password = "myakish",
                Role = "developer"
            };
            _users.Add(me.UserName.ToLower(), me);
        }
        public FakeUser GetUser(string userName, string password)
        {
            try
            {
                var user = _users[userName.ToLower()];
                if (user is null)
                    return null;
                if (user.Password != password)
                    return null;
                return user;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}
