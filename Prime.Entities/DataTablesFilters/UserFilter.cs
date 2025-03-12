
using DataTablesHelper;

namespace DataTablesFilters;
public class UserFilter : UserFilterForAdmin
{

    public string? CountryName { get; set; }
    public FilterOption CountryNameFilter { get; set; }

    public string? Location { get; set; }
    public FilterOption LocationFilter { get; set; }
    public string? Notes { get; set; }
    public FilterOption NotesFilter { get; set; }
    public int? AdminId { get; set; }
    public int? AdminIdTo { get; set; }
    public FilterOptionN AdminIdFilter { get; set; }

    public int? PatchNumber { get; set; }
    public int? PatchNumberTo { get; set; }
    public FilterOptionN PatchNumberFilter { get; set; }
    public string? AdminCode { get; set; }
    public FilterOption AdminCodeFilter { get; set; }
    public string? AdminUserName { get; set; }
    public FilterOption AdminUserNameFilter { get; set; }
    public string? AdminFullName { get; set; }
    public FilterOption AdminFullNameFilter { get; set; }
}
public class UserFilterForAdmin
{
    public int? UserId { get; set; }
    public int? UserIdTo { get; set; }
    public FilterOptionN UserIdFilter { get; set; }
    
    public string? UserName { get; set; }
    public FilterOption UserNameFilter { get; set; }
    public string? FullName { get; set; }
    public FilterOption FullNameFilter { get; set; }
    public string? RoleName { get; set; }
    public FilterOption RoleNameFilter { get; set; }
    public string? Status { get; set; }
    public FilterOption StatusFilter { get; set; }

}