using rest_with_asp_net_10_cviana.Hypermedia;
using rest_with_asp_net_10_cviana.Hypermedia.Abstract;
using System.Xml.Serialization;

namespace rest_with_asp_net_10_cviana.Data.DTO.V1
{
    [XmlRoot("Book")]
    public class BookDTO : ISupportHypermedia
    {
        public BookDTO() { }

        public BookDTO(long id, string author, string title, DateOnly launchDate, decimal price) 
        {
            this.Id = id;
            this.Author = author;
            this.Title = title;
            this.LaunchDate = launchDate;
            this.Price = price;
        }

        public long Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public DateOnly LaunchDate { get; set; }

        public decimal Price { get; set; }

        public List<HypermediaLink> Links { get; set; } = [];

        
    }
}
