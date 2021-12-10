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
        private readonly List<UserDTO> _users = new List<UserDTO>();

        public UserRepository()
        {
            _users.Add(new UserDTO
            {
                UserName = "stale.myakish",
                Password = "admin",
                Role = "Developer"
            });
        }
        public UserDTO GetUser(string userName, string password)
        {
            return _users.Where(x => x.UserName.ToLower() == userName.ToLower()
                && x.Password == password).FirstOrDefault();
        }
    }
}
