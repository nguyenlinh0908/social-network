using social_network.Models;

public class ReactionResult
{
    public int postId { get; set; }
    public ReactionEnum react { get; set; }

    public int effected { get; set; }

    public bool? oppositeReact { get; set; }

    public int currentQuantity { get; set; } = 0;
}