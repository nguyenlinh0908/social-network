using Microsoft.EntityFrameworkCore;
using social_network.Models;

namespace social_network.Data
{
    public class SocialNetworkContext : DbContext
    {
        public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }

        public DbSet<Place> Place { get; set; }

        public DbSet<Post> Post { get; set; }

        public DbSet<Media> Media { get; set; }

        public DbSet<Reaction> Reaction { get; set; }
    }
}