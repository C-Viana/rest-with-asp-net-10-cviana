using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace rest_with_asp_net_10_cviana.Data.DTO.V1
{
    public class UsersDTO
    {
        [Column("user_name")]
        public string Username { get; set; }
        public string Password { get; set; }

        public UsersDTO() { }

        public UsersDTO(string username, string password)
        {
            Username = username;
            Password = password;
        }

    }
}
