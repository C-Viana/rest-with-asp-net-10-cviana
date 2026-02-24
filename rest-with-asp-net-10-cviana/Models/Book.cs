using rest_with_asp_net_10_cviana.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace rest_with_asp_net_10_cviana.Models
{
    [Table("book")]
    public class Book : BaseEntity
    {
        [Required]
        [Column("author", TypeName = "varchar(80)")]
        [NotNull]
        [MaxLength(80)]
        public string Author { get; set; }

        [Required]
        [Column("title", TypeName = "varchar(250)")]
        [NotNull]
        [MaxLength(250)]
        public string Title { get; set; }

        [Column("launch_date")]
        public DateOnly LaunchDate { get; set; }

        [Required]
        [Column("price", TypeName = "decimal(10,2)")]
        [NotNull]
        [Range(0.01, 99999.99)]
        public decimal Price { get; set; }

        public Book(long id, string author, string title, DateOnly launchDate, decimal price)
        {
            this.Id = id;
            Author = author;
            Title = title;
            LaunchDate = launchDate;
            Price = price;
        }
    }
}
