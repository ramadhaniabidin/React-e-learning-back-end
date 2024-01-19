using e_learning_back_end.logics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_learning_back_end.logics.Interfaces
{
    public interface IAkunRepo
    {
        public List<ProvinsiModel> GetProvinsiList();
        public string CreateAccount(AkunModel model);
        public string Login(LoginModel model);
        public string EncryptCredentials(LoginModel model);
    }
}
