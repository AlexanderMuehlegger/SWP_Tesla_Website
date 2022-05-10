using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP_Tesla_Website.Models.DB
{
    public interface IRepositoryOrder {
        public Task OpenConnection();
        public Task CloseConnection();
        public Task<bool> AddOrder(Order order);
        public Task<bool> ChangeStatus(int order_id, OrderStatus status);
        public Task<bool> PayOrder(Order order);
        public Task<List<Order>> GetAllOrders(int user_id);

    }
}
