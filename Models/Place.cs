using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social_network.Models
{
    public class Place
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(User))]
        public string countryCode { get; set; }

        [DataType(DataType.Text)]
        public string title { get; set; }

        [DataType(DataType.Text)]
        public string description { get; set; }

        public decimal reviewPoint { get; set; }
    }
}
