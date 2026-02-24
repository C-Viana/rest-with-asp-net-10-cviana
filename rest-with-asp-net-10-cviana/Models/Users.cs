using rest_with_asp_net_10_cviana.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rest_with_asp_net_10_cviana.Models
{
    [Table("users")]
    public class Users : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [Column("user_name", TypeName = "VARCHAR(50)")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        [Column("full_name", TypeName = "VARCHAR(250)")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        [Column("password", TypeName = "VARCHAR(250)")]
        public string Password { get; set; } = string.Empty;

        [MaxLength(500)]
        [Column("refresh_token", TypeName = "VARCHAR(500)")]
        public string? RefreshToken { get; set; }

        [Column("refresh_token_expiry_time", TypeName = "DATETIME2(6)")]
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
