using Dapper;
using e_learning_back_end.logics.Interfaces;
using e_learning_back_end.logics.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace e_learning_back_end.logics.Repositories
{
    public class AkunRepo : IAkunRepo
    {
        private readonly string? connString;
        public AkunRepo(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("React-E-Learning-Backend");
        }

        public string CreateAccount(AkunModel model)
        {
            string returnedOutput = "";
            try
            {
                using var conn = new SqlConnection(connString);
                conn.Open();
                var query = @"INSERT INTO dbo.master_akun (username, email, password, id_peran, tanggal_daftar)
                            VALUES (@username, @email, @password, @id_peran, @tanggal_daftar)";

                conn.Execute(query, model);
                var responseBody = new
                {
                    Success = true,
                    Message = "Successfully created new account"
                };
                returnedOutput = JsonSerializer.Serialize(responseBody);
            }
            catch (Exception ex)
            {
                var responseBody = new
                {
                    Success = false,
                    Message = $"Error : {ex.Message}"
                };
                returnedOutput = JsonSerializer.Serialize(responseBody);
            }
            return returnedOutput;
        }

        public List<ProvinsiModel> GetProvinsiList()
        {
            try
            {
                using var con = new SqlConnection(connString);
                con.Open();
                var query = $"SELECT * FROM dbo.Provinsi";
                var provinces = con.Query<ProvinsiModel>(query).ToList();
                return provinces;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Login(string username, string password)
        {
            string returnedOutput = "";
            try
            {
                using var conn = new SqlConnection(connString);
                conn.Open();
                var query = @"
                IF(SELECT COUNT(*) FROM master_akun WHERE username = @username AND password = @password) > 0
                BEGIN
                    SELECT * FROM master_akun WHERE username = @username AND password = @password
                END
                ELSE
                BEGIN
                    SELECT * FROM master_akun WHERE email = @username AND password = @password
                END
                ";
                var account = conn.QueryFirstOrDefault<AkunModel>(query, new {username, password});

                if(account != null)
                {
                    var responseBody = new
                    {
                        Success = true,
                        Message = "OK",
                        Account = account
                    };
                    returnedOutput = JsonSerializer.Serialize(responseBody);
                }
                else
                {
                    var responseBody = new
                    {
                        Success = false,
                        Message = $"Tidak ada akun dengan username/email {username}"
                    };
                    returnedOutput = JsonSerializer.Serialize(responseBody);
                }
            }
            catch(Exception ex)
            {
                var responseBody = new
                {
                    Success = false,
                    Message = $"Error : {ex.Message}",
                };
                returnedOutput = JsonSerializer.Serialize(responseBody);
            }
            return returnedOutput;
        }
    }
}
