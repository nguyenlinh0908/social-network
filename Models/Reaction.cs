using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social_network.Models
{
    public enum ReactionEnum
    {
        LIKE,
        COMMENT,
        SHARE
    }

    public class Reaction
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(User))]

        public int userId { get; set; }

        [ForeignKey(nameof(Post))]
        public int postId { get; set; }
        
        public ReactionEnum reaction { get; set; }
    }
}
