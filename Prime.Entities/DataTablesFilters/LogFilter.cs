
using DataTablesHelper;

namespace DataTablesFilters;
public class LogFilter: LogFilterAdmin
{
    public int? AdminId { get; set; } = null;
    public int? AdminIdTo { get; set; } = null;
    public FilterOptionN AdminIdFilter { get; set; }
 
}

public class LogFilterAdmin
{
    public int? UserId { get; set; } = null;
    public int? UserIdTo { get; set; } = null;
    public FilterOptionN UserIdFilter { get; set; }
    public string? LogAction { get; set; }
    public FilterOption LogActionFilter { get; set; }
    public DateTime? TranDate { get; set; }
    public DateTime? TranDateTo { get; set; }
    public FilterOptionN? TranDateFilter { get; set; }
    public string? UserName { get; set; }
    public FilterOption UserNameFilter { get; set; }
    public string? FullName { get; set; }
    public FilterOption FullNameFilter { get; set; }

}