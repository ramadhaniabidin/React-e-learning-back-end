using Dapper;
using e_learning_back_end.logics.Interfaces;
using e_learning_back_end.logics.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
