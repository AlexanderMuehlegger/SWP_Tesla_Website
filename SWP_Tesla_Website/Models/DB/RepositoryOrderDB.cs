using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace SWP_Tesla_Website.Models.DB {
    public class RepositoryOrderDB : IRepositoryOrder {

        public string ConnectionString = DB_DATA.connectionStr;
        public DbConnection _conn;
        public async Task<bool> AddOrder(Order order) {
            
            try {
                order.ID = await GetAutoIncrementNumber(DB_DATA.database, "orders");
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

                return await cmd.ExecuteNonQueryAsync() == 1;

            }
            catch (Exception e) {
                return false;
            }finally {
                await CloseConnection();
            }
        }

        public async Task<bool> ChangeStatus(int order_id, OrderStatus status) {
            try {
                await OpenConnection();

                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "UPDATE orders SET order_status = @order_stat WHERE order_id = @order_id";

                DbParameter paramOrder = cmd.CreateParameter();
                paramOrder.ParameterName = "order_id";
                paramOrder.Value = order_id;
                cmd.Parameters.Add(paramOrder);

                DbParameter paramStat = cmd.CreateParameter();
                paramStat.ParameterName = "order_stat";
                paramStat.Value = (int)status;
                cmd.Parameters.Add(paramStat);


                return await cmd.ExecuteNonQueryAsync() >= 1;
            }
            catch (Exception e) {
                return false;
            }
            finally {
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
            if(this._conn == null)
                this._conn = new MySqlConnection(ConnectionString);
            if (this._conn.State != System.Data.ConnectionState.Open)
                await this._conn.OpenAsync();
        }

        public async Task<bool> PayOrder(Order order) {
            try {
                await this.OpenConnection();

                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "Select * from orders where order_id = @order_id";

                DbParameter order_id = cmd.CreateParameter();
                order_id.ParameterName = "order_id";
                order_id.Value = order.ID;
                cmd.Parameters.Add(order_id);

                DbParameter order_saldo = cmd.CreateParameter();
                order_saldo.ParameterName = "saldo";

                Order unpaid = null;

                using(DbDataReader reader = await cmd.ExecuteReaderAsync()) {
                    if (reader.Read())
                        unpaid = getOrderData(reader);
                }
                if (unpaid == null)
                    return false;
                decimal saldo = unpaid.Saldo - order.Pay;

                if (saldo < 0)
                    return false;
                if (saldo == 0)
                    await ChangeStatus(order.ID, OrderStatus.paid);


                order_saldo.Value = saldo;
                cmd.Parameters.Add(order_saldo);

                cmd.CommandText = "UPDATE orders SET saldo=@saldo WHERE order_id = @order_id";

                if (_conn?.State != System.Data.ConnectionState.Open)
                    await OpenConnection();

                return await cmd.ExecuteNonQueryAsync() >= 1;

            }catch(Exception e) {
                return false;
            }finally {
                await this.CloseConnection();
            }
        }

        public async Task<List<Order>> GetAllOrders(int user_id) {
            try {
                await this.OpenConnection();
                List<Order> orders = new List<Order>();

                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM orders WHERE user_id=@user_id";

                DbParameter user_para = cmd.CreateParameter();
                user_para.ParameterName = "user_id";
                user_para.Value = user_id;
                cmd.Parameters.Add(user_para);

                using(DbDataReader reader = await cmd.ExecuteReaderAsync()) {
                    while(reader.Read()) {
                        orders.Add(getOrderData(reader));
                    }
                }
                return orders;
            }catch(Exception e) {
                return null;
            }finally {
                await this.CloseConnection();
            }
        }
        public async Task<Order> GetOrder(int order_id) {
            try {
                await OpenConnection();
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM orders WHERE order_id=@order_id";

                DbParameter id_para = cmd.CreateParameter();
                id_para.ParameterName = "order_id";
                id_para.Value = order_id;
                cmd.Parameters.Add(id_para);

                using(DbDataReader reader = await cmd.ExecuteReaderAsync()) {
                    if(reader.Read()) {
                        return getOrderData(reader);
                    }
                }
            }catch(Exception e) {
                return null;
            }finally {
                await CloseConnection();
            }
            return null;
        }

        public async Task<Order> GetRequiredOrderdata(string model) {
            if (this._conn?.State != System.Data.ConnectionState.Open || model == null || model == "")
                return null;

            DbCommand cmd = this._conn.CreateCommand();
            cmd.CommandText = "SELECT price, article_id FROM car WHERE model=@model";
            DbParameter modelP = cmd.CreateParameter();
            modelP.ParameterName = "model";
            modelP.Value = model;
            cmd.Parameters.Add(modelP);

            using (DbDataReader reader = await cmd.ExecuteReaderAsync()) {
                if (reader.Read())
                    return getRequiredOrderData(reader);
            }

            return null;
        }
        private Order getOrderData(DbDataReader reader) {
            return new Order() {
                ID = int.Parse(reader["order_id"].ToString()),
                user_id = int.Parse(reader["user_id"].ToString()),
                article_id = int.Parse(reader["article_id"].ToString()),
                Saldo = Convert.ToDecimal(reader["saldo"].ToString()),
                status = (OrderStatus)int.Parse(reader["order_status"].ToString())
            };
        }

        private Order getRequiredOrderData ( DbDataReader reader ) {
            return new Order() {
                article_id = int.Parse(reader["article_id"].ToString()),
                Saldo = Convert.ToDecimal(reader["price"].ToString())
            };
        }

        public async Task<bool> AddConformationKey ( Order order, string conformationKey ) {
            try {
                await OpenConnection();

                DbCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "INSERT INTO order_conformation VALUES(@key, @user_id, @order_id)";
                
                DbParameter paraKey = cmd.CreateParameter();
                paraKey.ParameterName = "key";
                paraKey.Value = conformationKey;
                cmd.Parameters.Add(paraKey);

                DbParameter paraUserID = cmd.CreateParameter();
                paraUserID.ParameterName = "user_id";
                paraUserID.Value = order.user_id;
                cmd.Parameters.Add(paraUserID);

                DbParameter paraOrderID = cmd.CreateParameter();
                paraOrderID.ParameterName = "order_id";
                paraOrderID.Value = order.ID;
                cmd.Parameters.Add(paraOrderID);

                return await cmd.ExecuteNonQueryAsync() == 1;
            }catch (Exception ex) {
                return false;
            }finally {
                await CloseConnection();
            }
        }

        public async Task<bool> CheckConformationKey (string conformationKey ) {
            
            try {
                await OpenConnection();
                Order foundOrder = null;

                DbCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM order_conformation WHERE conformation_key=@key";

                DbParameter paraKey = cmd.CreateParameter();
                paraKey.ParameterName= "key";
                paraKey.Value= conformationKey;
                cmd.Parameters.Add(paraKey);

                using(DbDataReader reader = await cmd.ExecuteReaderAsync()) {
                    if (reader.Read()) 
                        foundOrder = new Order() {
                            ID = int.Parse(reader["order_id"].ToString()),
                        };
                }

                if (foundOrder == null)
                    return false;

                cmd.CommandText = "UPDATE orders SET order_status=@status WHERE order_id=@id";

                DbParameter paraStatus = cmd.CreateParameter();
                paraStatus.ParameterName = "status";
                paraStatus.Value = OrderStatus.open;
                cmd.Parameters.Add(paraStatus);

                DbParameter paraOrderId = cmd.CreateParameter();
                paraOrderId.ParameterName = "id";
                paraOrderId.Value = foundOrder.ID;
                cmd.Parameters.Add(paraOrderId);

                using(DbDataReader reader = await cmd.ExecuteReaderAsync()) {
                    if (reader.Read())
                        return true;
                }

                return false;
            }catch (Exception ex) {
                return false;
            }finally {
                await CloseConnection();
            }
        }

        public async Task<int> GetAutoIncrementNumber ( string database, string table ) {
            try {
                await OpenConnection();
                DbCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "SELECT order_id FROM orders ORDER BY order_id DESC LIMIT 1;";

                using (DbDataReader reader = await cmd.ExecuteReaderAsync()) {
                    if (reader.Read())
                        return reader.GetInt32(0);
                }
            } catch (Exception ex) {
                return -1;
            } finally {
                await CloseConnection();
            }
            return -1;
        }

        public async Task<bool> OpenEveryOrder (OrderStatus status, OrderStatus currentstatus ) {
            try {
                await OpenConnection();

                DbCommand cmd = _conn.CreateCommand();
                cmd.CommandText = "UPDATE orders SET order_status=@status WHERE order_status=@currStat";
                
                DbParameter paraStatus = cmd.CreateParameter();
                paraStatus.ParameterName = "status";
                paraStatus.Value = status;
                cmd.Parameters.Add(paraStatus);

                DbParameter paraCurr = cmd.CreateParameter();
                paraCurr.ParameterName = "currStat";
                paraCurr.Value = currentstatus;
                cmd.Parameters.Add(paraCurr);

                return await cmd.ExecuteNonQueryAsync() >= 1;
            }catch (Exception ex) {
                return false;
            }finally {
                await CloseConnection();
            }
        }
    }
}
