using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social_network.Models
{
    public class Place
    {
        [Key]
        public int id { get; set; }

        public string countryCode { get; set; }

        [DataType(DataType.Text)]
        public string title { get; set; }

        [DataType(DataType.Text)]
        public string description { get; set; }

        public double reviewPoint { get; set; }
    }
}
