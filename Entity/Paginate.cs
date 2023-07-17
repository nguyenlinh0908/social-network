namespace social_network.Entity;
public class PaginateScroll
{
    public int startId { get; set; } = 0;
    public int limit { get; set; } = 19;
    public string? query { get; set; }
}


public class Paginate
{
    public int pageNumber { get; set; } = 0;
    public int limit { get; set; } = 19;
    public string query { get; set; }
}