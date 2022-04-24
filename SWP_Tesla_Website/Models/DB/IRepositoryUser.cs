using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP_Tesla_Website.Models.DB
{
    public interface IRepositoryUser  {
        public Task ConnectAsync();
        public Task DisconnectAsync();
        public Task<User> GetByIdAsync(int id);
        public Task<bool> RegisterAsync(User user);
        public Task<User> LoginAsync(User user);
        public Task<bool> UpdateAsync(User user);
        public Task<bool> DeleteAsync(int id);
        public Task<List<User>> GetAllUser ();

    }
}
