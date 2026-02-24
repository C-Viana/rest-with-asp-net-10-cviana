using rest_with_asp_net_10_cviana.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rest_with_asp_net_10_cviana.Models
{
    [Table("person")]
    public class Person : BaseEntity
    {
        [Required]
        [MaxLength(80)]
        [Column("first_name", TypeName = "varchar(80)")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(80)]
        [Column("last_name", TypeName = "varchar(80)")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(9)]
        [Column("gender", TypeName = "varchar(9)")]
        public string Gender { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("address", TypeName = "varchar(150)")]
        public string Address { get; set; }

        [Column("enabled", TypeName = "bit")]
        public bool Enabled { get; set; }

        public Person(long id, string firstName, string lastName, string gender, string address, bool enabled)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Address = address;
            Enabled = enabled;
        }
    }
}
