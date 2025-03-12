
using DataTablesHelper;
using System.ComponentModel.DataAnnotations;

namespace DataTablesFilters;
public class AdminTeachersFilter
{
    public int? AdminId { get; set; }
    public FilterOptionN AdminIdFilter { get; set; }
    public string? AdminCode { get; set; }
    public FilterOption AdminCodeFilter { get; set; }
    public string? AdminUserName { get; set; }
    public FilterOption AdminUserNameFilter { get; set; }
    public string? AdminFullName { get; set; }
    public FilterOption AdminFullNameFilter { get; set; }
    public int? TeacherId { get; set; }
    public FilterOptionN TeacherIdFilter { get; set; }
    public string? TeacherUserName { get; set; }
    public FilterOption TeacherUserNameFilter { get; set; }
    public string? TeacherFullName { get; set; }
    public FilterOption TeacherFullNameFilter { get; set; }
    public string? CountryName { get; set; }
    public FilterOption CountryNameFilter { get; set; }
    public string? Status { get; set; }
    public FilterOption StatusFilter { get; set; }
}
