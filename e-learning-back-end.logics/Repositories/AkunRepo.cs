using Dapper;
using e_learning_back_end.logics.Interfaces;
using e_learning_back_end.logics.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace e_learning_back_end.logics.Repositories
{
    public class AkunRepo : IAkunRepo
    {
        private readonly string? connString;
        private readonly string? JwtKey;
        private readonly string? JwtIssuer;
        private readonly string? JwtAudience;
        private readonly string? encryptionKey;
        public AkunRepo(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("React-E-Learning-Backend");
            JwtKey = configuration.GetSection("Jwt")["Key"];
            JwtIssuer = configuration.GetSection("Jwt")["Issuer"];
            JwtAudience = configuration.GetSection("Jwt")["Audience"];
            encryptionKey = configuration.GetSection("ChessOpenings")["Carro-Kann-Defence"];
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
                Console.WriteLine($"Executed query = {query}");
                var provinces = con.Query<ProvinsiModel>(query).ToList();
                return provinces;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Login(LoginModel model)
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
                var account = conn.QueryFirstOrDefault<AkunModel>(query, new {model.username, model.password});
                Console.WriteLine($"Executed query = {query}");

                if(account != null)
                {
                    string jwtToken = GenerateLoginToken(model.username, model.password);
                    var responseBody = new
                    {
                        Success = true,
                        Message = "OK",
                        //Account = account,
                        Token = jwtToken
                    };
                    returnedOutput = JsonSerializer.Serialize(responseBody);
                }
                else
                {
                    var responseBody = new
                    {
                        Success = false,
                        Message = $"Maaf akun Anda belum terdaftar"
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

        public string GenerateLoginToken(string username, string password)
        {
            try
            {
                var claims = new[]
                {
                    new Claim("Username", username),
                    new Claim("Password", password)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
                var login = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    JwtIssuer, JwtAudience, claims, expires: DateTime.UtcNow.AddHours(3),
                    signingCredentials: login
                    );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return jwtToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string EncryptCredentials(LoginModel model)
        {
            string returnedOutput = "";
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
                    aesAlg.IV = new byte[aesAlg.BlockSize / 8];

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    byte[] userNameBytes = Encoding.UTF8.GetBytes(model.username);
                    byte[] encryptedUsernameBytes = encryptor.TransformFinalBlock(userNameBytes, 0, userNameBytes.Length);
                    string encryptedUsername = Convert.ToBase64String(encryptedUsernameBytes);

                    byte[] passwordBytes = Encoding.UTF8.GetBytes(model.password);
                    byte[] encryptedPasswordBytes = encryptor.TransformFinalBlock(passwordBytes, 0, passwordBytes.Length);
                    string encryptedPassword = Convert.ToBase64String(encryptedPasswordBytes);

                    var responseBody = new
                    {
                        Success = true,
                        Message = "OK",
                        Username = encryptedUsername, 
                        Password = encryptedPassword
                    };
                    returnedOutput = JsonSerializer.Serialize(responseBody);
                }
            }
            catch(Exception ex)
            {
                var responseBody = new
                {
                    Success = false,
                    Message = $"{ex.Message}"
                };
                returnedOutput = JsonSerializer.Serialize(responseBody);
            }
            return returnedOutput;
        }
    }
}
