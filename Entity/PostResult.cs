using social_network.Models;

namespace social_network.Entity;

public class PostResult
{
    public Post post { get; set; }
    public Place place { get; set; }

    public User owner { get; set; }
    public List<Media>? media { get; set; }

    public List<Reaction>? reaction { get; set; }

    public bool? liked { get; set; } = false;

    public int? likeQuantity { get; set; } = 0;

    public int? commentQuantity { get; set; } = 0;

    public int? shareQuantity { get; set; } = 0;
}