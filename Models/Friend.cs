using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social_network.Models
{
    public class Friend
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(User))]
        public int userId { get; set; }
        
        [ForeignKey(nameof(User))]
        public int friendId { get; set; }
    }
}
