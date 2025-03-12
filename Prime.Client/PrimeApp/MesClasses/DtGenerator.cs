using DataTablesFilters;
using DataTablesHelper;
using Entities;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;
using System.Linq.Expressions;
using System.Text;

namespace Datatable;
public static class DtGenerator
{
   
    public static string GenDtScript<T1, T2>(string url, string links = null, bool withFilter = false)
    {
        string filters = "";
        string filtersJsFunctions = "";
        string refreshMethods = "";
        if (withFilter)
        {
            filters = GenFilters<T2>();
            refreshMethods = GenRefreshMethods();
            filtersJsFunctions = GenFiltersJsFunctions<T2>();
        }

        string columns = GenCols<T1>(links);
        string domButtons = GetDomButtons();

        return $@"
        $(document).ready(function () {{
            var table = $('#jqtb').DataTable({{
                ""processing"": true,
                ""serverSide"": true,
                ""ajax"": {{
                    ""url"": ""{url}"",
                    ""type"": ""POST"",
                    ""datatype"": ""json"",
                    {filters}
                }},
                ""columnDefs"": [{{
                    ""targets"": '_all',
                    ""className"": 'dt-center'
                }}],
                {columns},
                {domButtons}

            }});
            {refreshMethods}
        }});
         {filtersJsFunctions}   
    ";
    }

    public static string GenerateHtmlTb(string[] columnHeaders)
    {
        var tableHtml = new StringBuilder();

        tableHtml.AppendLine("<div class='row'>");
        tableHtml.AppendLine("    <div class='col-md-12'>");
        tableHtml.AppendLine("        <table id='jqtb' class='table table-hover dataTable' style='width:100%'>");
        tableHtml.AppendLine("            <thead>");
        tableHtml.AppendLine("                <tr>");

        // Loop through column names and create table headers
        foreach (var header in columnHeaders)
        {
            tableHtml.AppendLine($"                    <th>{header}</th>");
        }

        tableHtml.AppendLine("                </tr>");
        tableHtml.AppendLine("            </thead>");
        tableHtml.AppendLine("        </table>");
        tableHtml.AppendLine("    </div>");
        tableHtml.AppendLine("</div>");

        return tableHtml.ToString();
    }

    public static string GetSweetAlert(string ajaxUrl, string dataKey, string[] msgs)
    {
        return $@"
       $(document).on('click', '.js-myaction', function () {{
         var btn = $(this);
         const swal = Swal.mixin({{
             customClass: {{
                 confirmButton: 'btn btn-danger mx-2',
                 cancelButton: 'btn btn-light'
             }},
             buttonsStyling: false
         }});

         swal.fire({{
             title: '{msgs[0]}',
             text: ""{msgs[1]}"",
             icon: 'warning',
             showCancelButton: true,
             confirmButtonText: '{msgs[2]}',
             cancelButtonText: '{msgs[3]}',
             reverseButtons: true
         }}).then((result) => {{
             if (result.isConfirmed) {{
                 $.ajax({{
                     url: ""{ajaxUrl}"",
                     method: 'POST',
                     data: {{ {dataKey}: btn.data('id') }},
                     success: function (response) {{
                         if (response.success) {{
                             toastr.success(response.message || '{msgs[4]}');
                         }} else {{
                             toastr.error(response.message || '{msgs[5]}');
                         }}
                         $('#jqtb').DataTable().ajax.reload();
                     }},
                     error: function () {{
                         toastr.error('{msgs[5]}');
                     }}
                 }});
             }}
         }});
     }});";
    }


    #region PrivateMethods
    private static string GenCols<T>(string links = null, string actions = null)
    {
        var properties = typeof(T).GetProperties();
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("columns: [");

        foreach (var prop in properties)
        {
            if (prop.PropertyType == typeof(DateTime?) || prop.PropertyType == typeof(DateTime))
            {
                stringBuilder.AppendLine($"    {{ \"data\": \"{ToCamelCase(prop.Name)}\", \"name\": \"{prop.Name}\",");
                stringBuilder.AppendLine("      \"render\": function (data, type, row) {");
                stringBuilder.AppendLine("          return data ? '<span>' + new Date(data).toLocaleString() + '</span>' : ''; } },");
            }
            else
            {
                stringBuilder.AppendLine($"    {{ \"data\": \"{ToCamelCase(prop.Name)}\", \"name\": \"{prop.Name}\" }},");
            }
        }

        if (links != null)
        {
            stringBuilder.AppendLine("    { \"data\": null, \"orderable\": false,");
            stringBuilder.AppendLine("      \"render\": function (data, type, row) {");
            stringBuilder.AppendLine("          return `");
            stringBuilder.AppendLine(links);
            stringBuilder.AppendLine("`;}},");
        }

        if (actions != null)
        {
            stringBuilder.AppendLine("    { \"data\": null, \"orderable\": false,");
            stringBuilder.AppendLine("      \"render\": function (data, type, row) {");
            stringBuilder.AppendLine("          return `");
            stringBuilder.AppendLine(actions);
            stringBuilder.AppendLine("    }");
        }

        stringBuilder.AppendLine("]");

        return stringBuilder.ToString();
    }

    private static string GenFilters<T>()
    {
        var properties = typeof(T).GetProperties();
        var stringBuilder = new StringBuilder("\"data\": function (d) {");
        foreach (var prop in properties)
        {
            stringBuilder.AppendLine($"d.{prop.Name} = $('#{prop.Name}').val();");
        }
        stringBuilder.AppendLine("}");
        return stringBuilder.ToString();
    }
    private static string GenFiltersJsFunctions<T>()
    {
        var classType = typeof(T);
        var jsBuilder = new StringBuilder();
        jsBuilder.AppendLine("document.addEventListener('DOMContentLoaded', function () {");

        // Identify properties with int, int?, DateTime, or DateTime? types that do not end with "To"
        var intProperties = classType.GetProperties()
            .Where(p => (p.PropertyType == typeof(int) || p.PropertyType == typeof(int?) ||
                         p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
                         && !p.Name.EndsWith("To"))
            .ToList();

        foreach (var prop in intProperties)
        {
            string baseName = prop.Name;
            string filterName = $"{baseName}Filter";
            string toName = $"{baseName}To";

            jsBuilder.AppendLine($"    const {filterName} = document.getElementById('{filterName}');");
            jsBuilder.AppendLine($"    const {toName} = document.getElementById('{toName}');");

            // Add the toggle function for the property
            jsBuilder.AppendLine($"    function toggle{baseName}To() {{");
            jsBuilder.AppendLine($"        if ({filterName} && {filterName}.value === '6') {{ // Assuming 6 corresponds to 'Between'");
            jsBuilder.AppendLine($"            if ({toName}) {{ {toName}.style.display = 'block'; }}");
            jsBuilder.AppendLine("        } else {");
            jsBuilder.AppendLine($"            if ({toName}) {{ {toName}.style.display = 'none'; }}");
            jsBuilder.AppendLine("        }");
            jsBuilder.AppendLine("    }");

            // Initialize the toggle function
            jsBuilder.AppendLine($"    if ({filterName}) toggle{baseName}To();");

            // Add event listener for changes
            jsBuilder.AppendLine($"    if ({filterName}) {{");
            jsBuilder.AppendLine($"        {filterName}.addEventListener('change', toggle{baseName}To);");
            jsBuilder.AppendLine("    }");
        }

        // Add reset button functionality
        jsBuilder.AppendLine("    const resetButton = document.querySelector('button[type=\"reset\"]');");
        jsBuilder.AppendLine("    if (resetButton) {");
        jsBuilder.AppendLine("        resetButton.addEventListener('click', function (event) {");
        jsBuilder.AppendLine("            // Delay the table reload to allow the reset action to apply");
        jsBuilder.AppendLine("            setTimeout(function () {");
        foreach (var prop in intProperties)
        {
            string toName = $"{prop.Name}To";
            jsBuilder.AppendLine($"                const {toName} = document.getElementById('{toName}');");
            jsBuilder.AppendLine($"                if ({toName}) {{ {toName}.style.display = 'none'; }}");
        }
        jsBuilder.AppendLine("                const dataTable = $('#jqtb').DataTable();");
        jsBuilder.AppendLine("                if (dataTable) {");
        jsBuilder.AppendLine("                    dataTable.ajax.reload(null, false); // Reload without resetting pagination");
        jsBuilder.AppendLine("                } else {");
        jsBuilder.AppendLine("                    console.error('DataTable instance not found.');");
        jsBuilder.AppendLine("                }");
        jsBuilder.AppendLine("            }, 0); // Allow reset action to complete before reloading");
        jsBuilder.AppendLine("        });");
        jsBuilder.AppendLine("    }");

        jsBuilder.AppendLine("});");

        return jsBuilder.ToString();
    }


    private static string GenRefreshMethods()
    {
        return @"
        $(document).keydown(function (event) {
            if (event.key === 'Enter') {
                $('#btnSearch').click();
            }
        });
        
        $('#btnSearch').click(function () {
            table.ajax.reload();
        });
    ";
    }

    private static string GetDomButtons()
    {
        return @"
        ""pageLength"": 10,
        ""lengthMenu"": [[5, 10, 15, 20], [5, 10, 15, 20]],
        ""dom"": 
            ""<'row'<'col-sm-12 d-flex justify-content-end mb-2'B>>"" +
            ""<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6 d-flex justify-content-end'f>>"" +
            ""<'row'<'col-sm-12'tr>>"" +
            ""<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 d-flex justify-content-end'p>>"",
        ""buttons"": [
            {
                ""extend"": ""collection"",
                ""text"": ""Export"",
                ""buttons"": [
                    { ""extend"": ""csvHtml5"", ""text"": ""Export as CSV"", ""exportOptions"": { ""columns"": "":not(:last-child)"" } },
                    { ""extend"": ""excelHtml5"", ""text"": ""Export as Excel"", ""exportOptions"": { ""columns"": "":not(:last-child)"" } },
                    { ""extend"": ""pdfHtml5"", ""text"": ""Export as PDF"", ""exportOptions"": { ""columns"": "":not(:last-child)"" } },
                    { ""extend"": ""print"", ""text"": ""Print Table"", ""exportOptions"": { ""columns"": "":not(:last-child)"" } },
                    { ""extend"": ""copy"", ""text"": ""Copy Table"", ""exportOptions"": { ""columns"": "":not(:last-child)"" } }
                ]
            },
            { ""extend"": ""colvis"", ""text"": ""<i class='fas fa-columns'></i>"" },
            { ""extend"": ""colvisRestore"", ""text"": ""<i class='fas fa-undo'></i>"" }
        ]
    ";
    }
    private static string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || str.Length < 2)
            return str;
        return char.ToLower(str[0]) + str.Substring(1);
    } 
    #endregion
}
