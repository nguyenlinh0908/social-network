namespace social_network.Entity;
public class PaginateResult<T>
{
    public List<T> data { get; set; }
    public int totalRecords { get; set; }
    public int limit { get; set; }

    public int currentPage { get; set; }
}