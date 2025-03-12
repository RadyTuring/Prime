using System;
namespace Custom_Filter;
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ExcludeAction : Attribute
{
}
