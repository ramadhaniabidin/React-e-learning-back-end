using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace e_learning_back_end.logics.Models
{
    public class AkunModel
    {
        public int id {  get; set; }
        [JsonPropertyName("username")]
        public string? username { get; set; }
        [JsonPropertyName("password")]
        public string? password { get; set; }
        [JsonPropertyName("email")]
        public string? email { get; set; }
        [JsonPropertyName("id_peran")]
        public int id_peran { get; set; }
        [JsonPropertyName("nama")]
        public string? nama { get; set; }
        [JsonPropertyName("provinsi")]
        public string? provinsi{ get; set; }
        [JsonPropertyName("kabupaten")]
        public string? kabupaten { get; set; }
        [JsonPropertyName("kecamatan")]
        public string? kecamatan { get; set; }
        [JsonPropertyName("desa")]
        public string? desa { get; set; }
        [JsonPropertyName("no_telp")]
        public string? no_telp { get; set; }
        [JsonPropertyName("akun_aktif")]
        public bool akun_aktif { get; set; }
        [JsonPropertyName("tanggal_daftar")]
        public DateTime? tanggal_daftar { get; set; }
    }
}
