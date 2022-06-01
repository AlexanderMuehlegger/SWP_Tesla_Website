using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP_Tesla_Website.Models.DB {
    public interface IRepositoryCar {
        public Task ConnectAsync();
        public Task DisconnectAsync();
        public Task<Car> GetByIdAsync(int id);
        public Task<Car> GetByArticleID(int id);
        public Task<List<Car>> GetAllAsync();
        public Task<bool> UpdateAsync(Car car);
        public Task<bool> InsertAsync(Car car);
        public Task<bool> DeleteAsync(int id);
        public Task<Car> GetByModelAsync(String model);
    }
}
