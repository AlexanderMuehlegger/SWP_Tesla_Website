using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace SWP_Tesla_Website.Models.DB {
    public class RepositoryCarDB : IRepositoryCar {

        public DbConnection _conn;
        public string DbConnection_string = "server=localhost;database=tesla;uid=root;password=''";

        public async Task ConnectAsync() {
            if (this._conn == null) 
                _conn = new MySqlConnection(DbConnection_string);
            
            if (this._conn?.State == System.Data.ConnectionState.Closed) 
                await _conn.OpenAsync();
            
        }

        public async Task DisconnectAsync() {
            if (this._conn?.State == System.Data.ConnectionState.Open) 
                await this._conn.CloseAsync();
            
        }

        public async Task<List<Car>> GetAllAsync() {
            List<Car> cars = new List<Car>();

            if (this._conn?.State == System.Data.ConnectionState.Open) {
                DbCommand cmdCar = this._conn.CreateCommand();
                cmdCar.CommandText = "select * from car";

                using (DbDataReader readerCar = await cmdCar.ExecuteReaderAsync()) {
                    while (await readerCar.ReadAsync()) {

                        cars.Add(new Car() {
                            Model = Convert.ToString(readerCar["Model"]),
                            Ps = Convert.ToInt32(readerCar["Ps"]),
                            Acceleration = double.Parse(readerCar["acceleration"].ToString()),
                            Price = Convert.ToDecimal(readerCar["Price"]),
                            Max_range = Convert.ToInt32(readerCar["Max_range"]),
                            Max_speed = Convert.ToInt32(readerCar["Max_speed"])
                        });
                    }

                }

            }
            return cars;
        }

        public async Task<Car> GetByIdAsync(int id) {
            if (this._conn?.State == System.Data.ConnectionState.Open) {
                DbCommand cmdCar = this._conn.CreateCommand();
                cmdCar.CommandText = "select * from car where car_id=@_id";
                DbParameter dbParam = cmdCar.CreateParameter();
                dbParam.ParameterName = "@_id";
                dbParam.Value = id;
                cmdCar.Parameters.Add(dbParam);

                using (DbDataReader readerCar = await cmdCar.ExecuteReaderAsync()) {
                    if (readerCar.Read())
                        return getCarData(readerCar);
                }

            }
            return null;
        }

        public async Task<bool> InsertAsync(Car car) {
            if (this._conn?.State == System.Data.ConnectionState.Open) {
                DbCommand cmdInsert = this._conn.CreateCommand();
                cmdInsert.CommandText = "insert into car values(null, Model, @Ps, @Acceleration, @Price, @Max_range, @Max_speed);";

                DbParameter paramM = cmdInsert.CreateParameter();
                paramM.ParameterName = "Model";
                paramM.DbType = System.Data.DbType.String;
                paramM.Value = car.Model;

                DbParameter paramPs = cmdInsert.CreateParameter();
                paramPs.ParameterName = "Ps";
                paramPs.DbType = System.Data.DbType.Int32;
                paramPs.Value = car.Ps;

                DbParameter paramACC = cmdInsert.CreateParameter();
                paramACC.ParameterName = "Acceleration";
                paramACC.DbType = System.Data.DbType.Decimal;
                paramACC.Value = car.Acceleration;

                DbParameter paramPr = cmdInsert.CreateParameter();
                paramPr.ParameterName = "Price";
                paramPr.DbType = System.Data.DbType.Decimal;
                paramPr.Value = car.Price;

                DbParameter paramMR = cmdInsert.CreateParameter();
                paramMR.ParameterName = "Max_range";
                paramMR.DbType = System.Data.DbType.Int32;
                paramMR.Value = car.Max_range;

                DbParameter paramMS = cmdInsert.CreateParameter();
                paramMS.ParameterName = "Max_speed";
                paramMS.DbType = System.Data.DbType.Int32;
                paramMS.Value = car.Max_speed;

                cmdInsert.Parameters.Add(paramM);
                cmdInsert.Parameters.Add(paramPs);
                cmdInsert.Parameters.Add(paramACC);
                cmdInsert.Parameters.Add(paramPr);
                cmdInsert.Parameters.Add(paramMR);
                cmdInsert.Parameters.Add(paramMS);

                return await cmdInsert.ExecuteNonQueryAsync() == 1;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Car car) {
            if (this._conn?.State == System.Data.ConnectionState.Open) {
                DbCommand cmdUpdate = this._conn.CreateCommand();
                cmdUpdate.CommandText = "update car set Model=Model, Ps=@Ps, Acceleration=@Acceleration, Price=@Price, Max_range=@Max_range, Max_speed=@Max_speed where car_id=@_id";

                DbParameter paramM = cmdUpdate.CreateParameter();
                paramM.ParameterName = "Model";
                paramM.DbType = System.Data.DbType.String;
                paramM.Value = car.Model;

                DbParameter paramPs = cmdUpdate.CreateParameter();
                paramPs.ParameterName = "Ps";
                paramPs.DbType = System.Data.DbType.Int32;
                paramPs.Value = car.Ps;

                DbParameter paramACC = cmdUpdate.CreateParameter();
                paramACC.ParameterName = "Acceleration";
                paramACC.DbType = System.Data.DbType.Decimal;
                paramACC.Value = car.Acceleration;

                DbParameter paramPr = cmdUpdate.CreateParameter();
                paramPr.ParameterName = "Price";
                paramPr.DbType = System.Data.DbType.Decimal;
                paramPr.Value = car.Price;

                DbParameter paramMR = cmdUpdate.CreateParameter();
                paramMR.ParameterName = "Max_range";
                paramMR.DbType = System.Data.DbType.Int32;
                paramMR.Value = car.Max_range;

                DbParameter paramMS = cmdUpdate.CreateParameter();
                paramMS.ParameterName = "Max_speed";
                paramMS.DbType = System.Data.DbType.Int32;
                paramMS.Value = car.Max_speed;

                cmdUpdate.Parameters.Add(paramM);
                cmdUpdate.Parameters.Add(paramPs);
                cmdUpdate.Parameters.Add(paramACC);
                cmdUpdate.Parameters.Add(paramPr);
                cmdUpdate.Parameters.Add(paramMR);
                cmdUpdate.Parameters.Add(paramMS);

                return await cmdUpdate.ExecuteNonQueryAsync() == 1;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id) {
            if (this._conn?.State == System.Data.ConnectionState.Open) {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "delete from car where car_id=@_id";
                DbParameter idParam = cmd.CreateParameter();
                idParam.ParameterName = "@_id";
                idParam.Value = id;
                cmd.Parameters.Add(idParam);
                return await cmd.ExecuteNonQueryAsync() > 0;
            }
            return false;
        }

        public Car getCarData(DbDataReader reader) {
            return new Car() {
                Model = Convert.ToString(reader["Model"]),
                Ps = Convert.ToInt32(reader["Ps"]),
                Acceleration = (double)Convert.ToDecimal(reader["Acceleration"]),
                Price = Convert.ToDecimal(reader["Price"]),
                Max_range = Convert.ToInt32(reader["Max_range"]),
                Max_speed = Convert.ToInt32(reader["Max_speed"])
            };
        }
    }
}
