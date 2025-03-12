namespace DataTablesHelper;
public class DataTableInfo
{
    public int PageSize { get; set; } = 5;
    public int skip { get; set; } = 0;
    public string? SearchValue { get; set; }
    public string? SortColumn { get; set; }
    public string? SortColumnDirection { get; set; }
}
