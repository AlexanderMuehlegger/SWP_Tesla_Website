using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace SWP_Tesla_Website.Models.DB {
    public class RepositoryUserDB : IRepositoryUser {
       
        public DbConnection _conn;
        public string DbConnection_string = "server=localhost;database=tesla;uid=root; password=''";

        public async Task ConnectAsync() {
            if (this._conn == null)
                _conn = new MySqlConnection(DbConnection_string);
            if(this._conn?.State == System.Data.ConnectionState.Closed)
                await _conn.OpenAsync();
        }

        public async Task<bool> DeleteAsync(int id) {
            if (this._conn?.State == System.Data.ConnectionState.Open) {

                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "delete from user where user_id=@user_id";

                DbParameter idParam = cmd.CreateParameter();
                idParam.ParameterName = "user_id";
                idParam.Value = id;
                cmd.Parameters.Add(idParam);

                return await cmd.ExecuteNonQueryAsync() > 0;

            }
            return false;
        }

        public async Task DisconnectAsync() {
            if(this._conn?.State == System.Data.ConnectionState.Open) {
                await this._conn.CloseAsync();
            }
        }

        public async Task<User> GetByIdAsync(int id) {
            if (this._conn?.State == System.Data.ConnectionState.Open) {
                DbCommand cmdUser = this._conn.CreateCommand();
                cmdUser.CommandText = "Select * from user where user_id=@user_id";

                DbParameter dbParameter = cmdUser.CreateParameter();
                dbParameter.ParameterName = "user_id";
                dbParameter.Value = id;
                cmdUser.Parameters.Add(dbParameter);

                using (DbDataReader reader = await cmdUser.ExecuteReaderAsync()) {
                    if (reader.Read())
                        return getUserData(reader);
                }
            }
            return null;

        }

        public async Task<User> LoginAsync(User user) {
            if (user == null)
                return null;

            if (this._conn?.State != System.Data.ConnectionState.Open)
                return null;

            if(user.Username == null && user.Email != null && user.Password != null) {
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM user where email=@email and password=@password";

                DbParameter emailPar = cmd.CreateParameter();
                emailPar.ParameterName = "email";
                emailPar.Value = user.Email;
                cmd.Parameters.Add(emailPar);

                DbParameter passwordPara = cmd.CreateParameter();
                passwordPara.ParameterName = "password";
                passwordPara.Value = user.Password;
                cmd.Parameters.Add(passwordPara);

                using (DbDataReader reader = await cmd.ExecuteReaderAsync()) {
                    if (reader.Read())
                        return getUserData(reader);
                }
                    
            } else if(user.Username != null && user.Password != null){
                DbCommand cmd = this._conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM user WHERE username=@username and password=@password";

                DbParameter userPara = cmd.CreateParameter();
                userPara.ParameterName = "username";
                userPara.Value = user.Username;
                cmd.Parameters.Add(userPara);

                DbParameter pwPara = cmd.CreateParameter();
                pwPara.ParameterName = "password";
                pwPara.Value = user.Password;
                cmd.Parameters.Add(pwPara);

                using (DbDataReader reader = await cmd.ExecuteReaderAsync()) {
                    if (reader.Read())
                        return getUserData(reader);
                }
            }
            return null;
            
        }

        public async Task<bool> RegisterAsync(User user) {
            if (user == null)
                return false;
            if(this._conn?.State == System.Data.ConnectionState.Open) {
                if(user.Username != null && user.Password != null && user.Password != null){
                    DbCommand cmd = _conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM user where username=@username or email=@email";

                    DbParameter userPara = cmd.CreateParameter();
                    userPara.ParameterName="username";
                    userPara.Value=user.Username;
                    cmd.Parameters.Add(userPara);

                    DbParameter emailPara = cmd.CreateParameter();
                    emailPara.ParameterName="email";
                    emailPara.Value=user.Email;
                    cmd.Parameters.Add(emailPara);

                    DbParameter pwPara = cmd.CreateParameter() ;
                    pwPara.ParameterName="password";
                    pwPara.Value=user.Password;
                    cmd.Parameters.Add(pwPara);

                    using (DbDataReader reader = await cmd.ExecuteReaderAsync()) {
                        if (reader.Read()) 
                            return false;
                    }

                    cmd.CommandText = "INSERT INTO user VALUES(null, email=@email, username=@username, password=@password, access=DEFAULT)";


                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
            return false;
        }

        public Task<bool> UpdateAsync(User user) {
            throw new System.NotImplementedException();
        }

        public User getUserData (DbDataReader reader) {
            return new User() {
                Username = Convert.ToString(reader["username"]),
                Password = Convert.ToString(reader["password"]),
                Email = Convert.ToString(reader["email"]),
                access = (Access)Convert.ToInt32(reader["access"])
            };
        }
    }
}
