using System.Threading.Tasks;

namespace SWP_Tesla_Website.Models.DB
{
    public interface IRepositoryOrder {
        public Task OpenConnection();
        public Task CloseConnection();
        public Task<bool> AddOrder(Order order);
        public Task<bool> CancelOrder(int order_id);
        public Task<bool> PayOrder(Order order);

    }
}
