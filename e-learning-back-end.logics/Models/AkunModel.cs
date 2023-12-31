using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_learning_back_end.logics.Models
{
    public class AkunModel
    {
        public int id {  get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? email { get; set; }
        public int id_peran { get; set; }
        public string? nama { get; set; }
        public string? provinsi{ get; set; }
        public string? kabupaten { get; set; }
        public string? kecamatan { get; set; }
        public string? desa { get; set; }
        public string? no_telp { get; set; }
        public bool akun_aktif { get; set; }
        public DateTime? tanggal_daftar { get; set; }
    }
}
