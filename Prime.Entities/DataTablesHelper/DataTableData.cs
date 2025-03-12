namespace DataTablesHelper;
public class DataTableData<T> where T : class
{
    public int recordsFiltered { get; set; }
    public int recordsTotal { get; set; }
    public List<T>? data { get; set; }
}
