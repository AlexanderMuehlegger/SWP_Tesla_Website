using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace SWP_Tesla_Website.Models.DB {
    public class RepositoryOrderDB : IRepositoryOrder {

        public string ConnectionString = "server=localhost,database=tesla,uid=root,password=''";
        public DbConnection _conn;
        public async Task<bool> AddOrder(Order order) {
            try {
                await OpenConnection();
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "INSERT INTO orders VALUES(null, @saldo, DEFAULT, @article_id, @user_id)";

                DbParameter paramSal = cmd.CreateParameter();
                paramSal.ParameterName = "saldo";
                paramSal.Value = order.Saldo;
                cmd.Parameters.Add(paramSal);

                DbParameter paramUser = cmd.CreateParameter();
                paramUser.ParameterName = "user_id";
                paramUser.Value = order.user_id;
                cmd.Parameters.Add(paramUser);

                DbParameter paramArticle = cmd.CreateParameter();
                paramArticle.ParameterName = "article_id";
                paramArticle.Value = order.article_id;
                cmd.Parameters.Add(paramArticle);

                return await cmd.ExecuteNonQueryAsync() >= 1;

            }finally {
                await CloseConnection();
            }
        }

        public async Task<bool> CancelOrder(int order_id) {
            try {
                await OpenConnection();

                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "UPDATE orders order_status = @order_stat WHERE order_id = @order_id";

                DbParameter paramOrder = cmd.CreateParameter();
                paramOrder.ParameterName = "order_id";
                paramOrder.Value = order_id;
                cmd.Parameters.Add(paramOrder);

                DbParameter paramStat = cmd.CreateParameter();
                paramStat.ParameterName = "order_stat";
                paramStat.Value = OrderStatus.cancelled;
                cmd.Parameters.Add(paramStat);


                return await cmd.ExecuteNonQueryAsync() >= 1;
            }finally {
                await CloseConnection();
            }
        }

        public async Task CloseConnection() {
            if (this._conn == null || this._conn?.State != System.Data.ConnectionState.Open)
                return;
            try {
                await this._conn.CloseAsync();
            }catch(Exception ex) {
                Console.WriteLine("Failed to close Database connection!");
            }
        }

        public async Task OpenConnection() {
            if (this._conn?.State == System.Data.ConnectionState.Open)
                return;
            try {
                if(this._conn == null)
                    this._conn = new MySqlConnection(ConnectionString);
                if (this._conn.State != System.Data.ConnectionState.Open)
                    await this._conn.OpenAsync();
            }catch(Exception e) {
                Console.WriteLine("Failed to open Database!");
            }
        }

        public Task<bool> PayOrder(Order order) {
            throw new System.NotImplementedException();
        }
    }
}
