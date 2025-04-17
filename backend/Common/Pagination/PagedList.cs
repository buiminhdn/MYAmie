namespace Common.Pagination;
public class PagedList<T> : List<T>
{
    public PaginationData PaginationData { get; set; }

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        PaginationData = new PaginationData
        {
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };

        AddRange(items);
    }
    public static PagedList<T> ToPagedList(IEnumerable<T> source, int? pageNumber = 1, int? pageSize = 10)
    {
        var page = Math.Max(1, pageNumber ?? 1);
        var size = Math.Clamp(pageSize ?? 10, 1, 50);

        var count = source.Count();

        if (count == 0 || (page - 1) * size >= count)
        {
            return new PagedList<T>([], 0, page, size);
        }

        var items = source
            .Skip((page - 1) * size)
            .Take(size)
            .ToList();

        return new PagedList<T>(items, count, page, size);
    }
}