using rest_with_asp_net_10_cviana.Hypermedia;
using rest_with_asp_net_10_cviana.Hypermedia.Abstract;
using rest_with_asp_net_10_cviana.JsonSerializers;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace rest_with_asp_net_10_cviana.Data.DTO.V1
{
    [XmlRoot("Person")]
    public class PersonDTO : ISupportHypermedia
    {
        [JsonPropertyName("code")]
        [XmlElement("Code")]
        public long Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        //[JsonConverter(typeof(GenderSerializer))]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Gender { get; set; }

        public string Address { get; set; }

        //[JsonConverter(typeof(DateSerializer))]
        //public DateTime? Birthdate { get; set; }
        public bool Enabled { get; set; }

        public List<HypermediaLink> Links { get; set; } = [];
    }
}
