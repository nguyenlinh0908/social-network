using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace social_network.Models
{
    public enum MediaTypeEnum
    {
        AVATAR,
        COVER
    }

    public class Media
    {
        [Key]
        public int id { get; set; }

        [ForeignKey(nameof(User))]
        public int? userId { get; set; }

        [ForeignKey(nameof(Post))]
        public int? postId { get; set; }

        [ForeignKey(nameof(Place))]
        public int? placeId { get; set; }

        public MediaTypeEnum type { get; set; } = MediaTypeEnum.AVATAR;

        [DataType(DataType.Url)]
        public string url { get; set; }

    }
}
