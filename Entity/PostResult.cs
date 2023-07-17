using social_network.Models;

namespace social_network.Entity;

public class PostResult
{
    public Post post { get; set; }
    public Place place { get; set; }
    public List<Media>? media { get; set; }
}