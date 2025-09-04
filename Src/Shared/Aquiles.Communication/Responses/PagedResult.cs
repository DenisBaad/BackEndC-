namespace Aquiles.Communication.Responses;
public class PagedResult<T>
{
    public IList<T> Items { get; set; }
    public int TotalCount { get; set; }
}
