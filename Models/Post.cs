using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace social_network.Models
{
    public enum StatusPostEnum
    {
        PENDING,
        APPROVED,
        REFUSE
    }

    public class Post
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(User))]
        public int userId { get; set; }

        [ForeignKey(nameof(Place))]
        public int placeId { get; set; }
        
        [DataType(DataType.Text)]
        public string content { get; set; }

        public StatusPostEnum status { get; set; }
    }
}
