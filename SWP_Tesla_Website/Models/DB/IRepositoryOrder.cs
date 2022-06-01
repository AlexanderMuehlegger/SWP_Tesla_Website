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

        public Task<Order> GetOrder(int order_id);
        public Task<Order> GetRequiredOrderdata(string model);
        public Task<bool> AddConformationKey ( Order order, string conformationKey );
        public Task<bool> CheckConformationKey (string conformationKey);
        public Task<int> GetAutoIncrementNumber ( string database, string table );
        public Task<bool> OpenEveryOrder (OrderStatus access, OrderStatus currAcc);
    }
}
