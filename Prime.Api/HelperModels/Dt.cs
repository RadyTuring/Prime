using DataTablesHelper;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace JqueryTb;
public  class Dt
{
    public static Expression<Func<T1, bool>> BuildCriteria<T1, T2>(T2 filter, Expression<Func<T1, bool>> baseCriteria = null)
    {

        var parameterExp = Expression.Parameter(typeof(T1));
        Expression conditionExp = Expression.Constant(true);

        try
        {

            if (filter != null)
            {
                var properties = typeof(T2).GetProperties();
                var entityTypeProperties = typeof(T1).GetProperties();

                foreach (var prop in properties)
                {
                    if (!prop.Name.ToLower().EndsWith("filter"))
                    {
                        var propName = prop.Name;
                        var propValue = prop.GetValue(filter);

                        if (propValue != null)
                        {
                            var entityProperty = entityTypeProperties.FirstOrDefault(p => p.Name == propName);
                            if (entityProperty == null)
                                continue;

                            Expression propExp = Expression.Property(parameterExp, propName);

                            if (entityProperty.PropertyType == typeof(string))
                            {
                                var filterPropertyName = propName + "Filter";
                                var filterProperty = filter.GetType().GetProperty(filterPropertyName);
                                var propFilter = (FilterOption)filterProperty.GetValue(filter);

                                switch (propFilter)
                                {
                                    case FilterOption.Equal:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.Equal(propExp, Expression.Constant(propValue)));
                                        break;
                                    case FilterOption.NotEqual:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.NotEqual(propExp, Expression.Constant(propValue)));
                                        break;
                                    case FilterOption.StartsWith:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.Call(propExp, "StartsWith", null, Expression.Constant(propValue)));
                                        break;
                                    case FilterOption.EndsWith:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.Call(propExp, "EndsWith", null, Expression.Constant(propValue)));
                                        break;
                                    case FilterOption.Contains:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.Call(propExp, "Contains", null, Expression.Constant(propValue)));
                                        break;
                                    case FilterOption.NotContains:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.Not(Expression.Call(propExp, "Contains", null, Expression.Constant(propValue))));
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                            else if (Nullable.GetUnderlyingType(entityProperty.PropertyType) == typeof(int) || entityProperty.PropertyType == typeof(int) || Nullable.GetUnderlyingType(entityProperty.PropertyType) == typeof(long) || entityProperty.PropertyType == typeof(long))
                            {
                                var filterPropertyName = propName + "Filter";
                                var filterProperty = filter.GetType().GetProperty(filterPropertyName);
                                var propFilter = (FilterOptionN)filterProperty.GetValue(filter);

                                var convertedPropExp = Expression.Convert(propExp, typeof(int?));
                                var convertedPropValue = Expression.Constant(propValue, typeof(int?));
                                switch (propFilter)
                                {
                                    case FilterOptionN.Equal:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.Equal(convertedPropExp, convertedPropValue));
                                        break;
                                    case FilterOptionN.NotEqual:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.NotEqual(convertedPropExp, convertedPropValue));
                                        break;
                                    case FilterOptionN.GreaterThan:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.GreaterThan(convertedPropExp, convertedPropValue));
                                        break;
                                    case FilterOptionN.GreaterOrEqualTo:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.GreaterThan(convertedPropExp, convertedPropValue));
                                        break;
                                    case FilterOptionN.LessThan:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.LessThan(convertedPropExp, convertedPropValue));
                                        break;
                                    case FilterOptionN.LessOrEqualTo:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.LessThanOrEqual(convertedPropExp, convertedPropValue));
                                        break;
                                    case FilterOptionN.Between:
                                        var fromProp = filter.GetType().GetProperty(propName);
                                        var toProp = filter.GetType().GetProperty(propName + "To");

                                        var fromValue = fromProp.GetValue(filter) as int?;
                                        var toValue = toProp.GetValue(filter) as int?;

                                        if (fromValue.HasValue && toValue.HasValue)
                                        {
                                            var _fromValue = Expression.Constant(fromValue, typeof(int?));
                                            var _toValue = Expression.Constant(toValue, typeof(int?));
                                            var fromExp = Expression.GreaterThanOrEqual(convertedPropExp, _fromValue);
                                            conditionExp = Expression.AndAlso(conditionExp, fromExp);
                                            var toExp = Expression.LessThanOrEqual(convertedPropExp, _toValue);
                                            conditionExp = Expression.AndAlso(conditionExp, toExp);
                                        }


                                        break;

                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                            else if (entityProperty.PropertyType == typeof(DateTime) || entityProperty.PropertyType == typeof(DateTime?))
                            {
                                var filterPropertyName = propName + "Filter";
                                var filterProperty = filter.GetType().GetProperty(filterPropertyName);
                                var propFilter = (FilterOptionN)filterProperty.GetValue(filter);

                                // Convert propExp to DateTime? if it is not already
                                var convertedPropExp = Expression.Convert(propExp, typeof(DateTime));

                                // Get the value as DateTime
                                var dateValue = (DateTime)propValue;

                                // Separate date components
                                var yearExp = Expression.Property(convertedPropExp, "Year");
                                var monthExp = Expression.Property(convertedPropExp, "Month");
                                var dayExp = Expression.Property(convertedPropExp, "Day");
                                var hourExp = Expression.Property(convertedPropExp, "Hour");
                                var minuteExp = Expression.Property(convertedPropExp, "Minute");
                                var secondExp = Expression.Property(convertedPropExp, "Second");

                                // Separate components from the dateValue
                                var yearValue = Expression.Constant(dateValue.Year, typeof(int));
                                var monthValue = Expression.Constant(dateValue.Month, typeof(int));
                                var dayValue = Expression.Constant(dateValue.Day, typeof(int));
                                var hourValue = Expression.Constant(dateValue.Hour, typeof(int));
                                var minuteValue = Expression.Constant(dateValue.Minute, typeof(int));
                                var secondValue = Expression.Constant(dateValue.Second, typeof(int));

                                // Determine if time components are all zero
                                bool hasTimeComponent = dateValue.Hour != 0 || dateValue.Minute != 0 || dateValue.Second != 0;

                                switch (propFilter)
                                {
                                    case FilterOptionN.Equal:
                                        if (hasTimeComponent)
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.AndAlso(
                                                    Expression.Equal(yearExp, yearValue),
                                                    Expression.AndAlso(
                                                        Expression.Equal(monthExp, monthValue),
                                                        Expression.AndAlso(
                                                            Expression.Equal(dayExp, dayValue),
                                                            Expression.AndAlso(
                                                                Expression.Equal(hourExp, hourValue),
                                                                Expression.AndAlso(
                                                                    Expression.Equal(minuteExp, minuteValue),
                                                                    Expression.Equal(secondExp, secondValue)
                                                                )
                                                            )
                                                        )
                                                    )
                                                )
                                            );
                                        }
                                        else
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.AndAlso(
                                                    Expression.Equal(yearExp, yearValue),
                                                    Expression.AndAlso(
                                                        Expression.Equal(monthExp, monthValue),
                                                        Expression.Equal(dayExp, dayValue)
                                                    )
                                                )
                                            );
                                        }
                                        break;

                                    case FilterOptionN.NotEqual:
                                        if (hasTimeComponent)
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.OrElse(
                                                    Expression.NotEqual(yearExp, yearValue),
                                                    Expression.OrElse(
                                                        Expression.NotEqual(monthExp, monthValue),
                                                        Expression.OrElse(
                                                            Expression.NotEqual(dayExp, dayValue),
                                                            Expression.OrElse(
                                                                Expression.NotEqual(hourExp, hourValue),
                                                                Expression.OrElse(
                                                                    Expression.NotEqual(minuteExp, minuteValue),
                                                                    Expression.NotEqual(secondExp, secondValue)
                                                                )
                                                            )
                                                        )
                                                    )
                                                )
                                            );
                                        }
                                        else
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.OrElse(
                                                    Expression.NotEqual(yearExp, yearValue),
                                                    Expression.OrElse(
                                                        Expression.NotEqual(monthExp, monthValue),
                                                        Expression.NotEqual(dayExp, dayValue)
                                                    )
                                                )
                                            );
                                        }
                                        break;

                                    case FilterOptionN.GreaterThan:
                                        if (hasTimeComponent)
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.OrElse(
                                                    Expression.GreaterThan(yearExp, yearValue),
                                                    Expression.AndAlso(
                                                        Expression.Equal(yearExp, yearValue),
                                                        Expression.OrElse(
                                                            Expression.GreaterThan(monthExp, monthValue),
                                                            Expression.AndAlso(
                                                                Expression.Equal(monthExp, monthValue),
                                                                Expression.OrElse(
                                                                    Expression.GreaterThan(dayExp, dayValue),
                                                                    Expression.AndAlso(
                                                                        Expression.Equal(dayExp, dayValue),
                                                                        Expression.OrElse(
                                                                            Expression.GreaterThan(hourExp, hourValue),
                                                                            Expression.AndAlso(
                                                                                Expression.Equal(hourExp, hourValue),
                                                                                Expression.OrElse(
                                                                                    Expression.GreaterThan(minuteExp, minuteValue),
                                                                                    Expression.GreaterThan(secondExp, secondValue)
                                                                                )
                                                                            )
                                                                        )
                                                                    )
                                                                )
                                                            )
                                                        )
                                                    )
                                                )
                                            );
                                        }
                                        else
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.OrElse(
                                                    Expression.GreaterThan(yearExp, yearValue),
                                                    Expression.AndAlso(
                                                        Expression.Equal(yearExp, yearValue),
                                                        Expression.OrElse(
                                                            Expression.GreaterThan(monthExp, monthValue),
                                                            Expression.AndAlso(
                                                                Expression.Equal(monthExp, monthValue),
                                                                Expression.GreaterThan(dayExp, dayValue)
                                                            )
                                                        )
                                                    )
                                                )
                                            );
                                        }
                                        break;

                                    case FilterOptionN.GreaterOrEqualTo:
                                        if (hasTimeComponent)
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.OrElse(
                                                    Expression.GreaterThan(yearExp, yearValue),
                                                    Expression.AndAlso(
                                                        Expression.Equal(yearExp, yearValue),
                                                        Expression.OrElse(
                                                            Expression.GreaterThan(monthExp, monthValue),
                                                            Expression.AndAlso(
                                                                Expression.Equal(monthExp, monthValue),
                                                                Expression.OrElse(
                                                                    Expression.GreaterThan(dayExp, dayValue),
                                                                    Expression.AndAlso(
                                                                        Expression.Equal(dayExp, dayValue),
                                                                        Expression.OrElse(
                                                                            Expression.GreaterThan(hourExp, hourValue),
                                                                            Expression.AndAlso(
                                                                                Expression.Equal(hourExp, hourValue),
                                                                                Expression.OrElse(
                                                                                    Expression.GreaterThan(minuteExp, minuteValue),
                                                                                    Expression.GreaterThanOrEqual(secondExp, secondValue)
                                                                                )
                                                                            )
                                                                        )
                                                                    )
                                                                )
                                                            )
                                                        )
                                                    )
                                                )
                                            );
                                        }
                                        else
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.OrElse(
                                                    Expression.GreaterThan(yearExp, yearValue),
                                                    Expression.AndAlso(
                                                        Expression.Equal(yearExp, yearValue),
                                                        Expression.OrElse(
                                                            Expression.GreaterThan(monthExp, monthValue),
                                                            Expression.AndAlso(
                                                                Expression.Equal(monthExp, monthValue),
                                                                Expression.GreaterThanOrEqual(dayExp, dayValue)
                                                            )
                                                        )
                                                    )
                                                )
                                            );
                                        }
                                        break;

                                    case FilterOptionN.LessThan:
                                        if (hasTimeComponent)
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.OrElse(
                                                    Expression.LessThan(yearExp, yearValue),
                                                    Expression.AndAlso(
                                                        Expression.Equal(yearExp, yearValue),
                                                        Expression.OrElse(
                                                            Expression.LessThan(monthExp, monthValue),
                                                            Expression.AndAlso(
                                                                Expression.Equal(monthExp, monthValue),
                                                                Expression.OrElse(
                                                                    Expression.LessThan(dayExp, dayValue),
                                                                    Expression.AndAlso(
                                                                        Expression.Equal(dayExp, dayValue),
                                                                        Expression.OrElse(
                                                                            Expression.LessThan(hourExp, hourValue),
                                                                            Expression.AndAlso(
                                                                                Expression.Equal(hourExp, hourValue),
                                                                                Expression.OrElse(
                                                                                    Expression.LessThan(minuteExp, minuteValue),
                                                                                    Expression.LessThan(secondExp, secondValue)
                                                                                )
                                                                            )
                                                                        )
                                                                    )
                                                                )
                                                            )
                                                        )
                                                    )
                                                )
                                            );
                                        }
                                        else
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.OrElse(
                                                    Expression.LessThan(yearExp, yearValue),
                                                    Expression.AndAlso(
                                                        Expression.Equal(yearExp, yearValue),
                                                        Expression.OrElse(
                                                            Expression.LessThan(monthExp, monthValue),
                                                            Expression.AndAlso(
                                                                Expression.Equal(monthExp, monthValue),
                                                                Expression.LessThan(dayExp, dayValue)
                                                            )
                                                        )
                                                    )
                                                )
                                            );
                                        }
                                        break;

                                    case FilterOptionN.LessOrEqualTo:
                                        if (hasTimeComponent)
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.OrElse(
                                                    Expression.LessThan(yearExp, yearValue),
                                                    Expression.AndAlso(
                                                        Expression.Equal(yearExp, yearValue),
                                                        Expression.OrElse(
                                                            Expression.LessThan(monthExp, monthValue),
                                                            Expression.AndAlso(
                                                                Expression.Equal(monthExp, monthValue),
                                                                Expression.OrElse(
                                                                    Expression.LessThan(dayExp, dayValue),
                                                                    Expression.AndAlso(
                                                                        Expression.Equal(dayExp, dayValue),
                                                                        Expression.OrElse(
                                                                            Expression.LessThan(hourExp, hourValue),
                                                                            Expression.AndAlso(
                                                                                Expression.Equal(hourExp, hourValue),
                                                                                Expression.OrElse(
                                                                                    Expression.LessThan(minuteExp, minuteValue),
                                                                                    Expression.LessThanOrEqual(secondExp, secondValue)
                                                                                )
                                                                            )
                                                                        )
                                                                    )
                                                                )
                                                            )
                                                        )
                                                    )
                                                )
                                            );
                                        }
                                        else
                                        {
                                            conditionExp = Expression.AndAlso(conditionExp,
                                                Expression.OrElse(
                                                    Expression.LessThan(yearExp, yearValue),
                                                    Expression.AndAlso(
                                                        Expression.Equal(yearExp, yearValue),
                                                        Expression.OrElse(
                                                            Expression.LessThan(monthExp, monthValue),
                                                            Expression.AndAlso(
                                                                Expression.Equal(monthExp, monthValue),
                                                                Expression.LessThanOrEqual(dayExp, dayValue)
                                                            )
                                                        )
                                                    )
                                                )
                                            );
                                        }
                                        break;

                                    case FilterOptionN.Between:
                                        {
                                            var fromProp = filter.GetType().GetProperty(propName);
                                            var toProp = filter.GetType().GetProperty(propName + "To");

                                            var fromValue = fromProp.GetValue(filter) as DateTime?;
                                            var toValue = toProp.GetValue(filter) as DateTime?;

                                            if (fromValue.HasValue || toValue.HasValue)
                                            {
                                                // Determine if there is a time component in the 'from' and 'to' values
                                                bool fromHasTime = fromValue.HasValue && (fromValue.Value.Hour != 0 || fromValue.Value.Minute != 0 || fromValue.Value.Second != 0);
                                                bool toHasTime = toValue.HasValue && (toValue.Value.Hour != 0 || toValue.Value.Minute != 0 || toValue.Value.Second != 0);

                                                // Define expressions for from and to comparisons
                                                Expression fromExp;
                                                Expression toExp;

                                                if (fromHasTime)
                                                {
                                                    fromExp = Expression.AndAlso(
                                                        Expression.GreaterThanOrEqual(yearExp, Expression.Constant(fromValue.Value.Year)),
                                                        Expression.AndAlso(
                                                            Expression.GreaterThanOrEqual(monthExp, Expression.Constant(fromValue.Value.Month)),
                                                            Expression.AndAlso(
                                                                Expression.GreaterThanOrEqual(dayExp, Expression.Constant(fromValue.Value.Day)),
                                                                Expression.AndAlso(
                                                                    Expression.GreaterThanOrEqual(hourExp, Expression.Constant(fromValue.Value.Hour)),
                                                                    Expression.AndAlso(
                                                                        Expression.GreaterThanOrEqual(minuteExp, Expression.Constant(fromValue.Value.Minute)),
                                                                        Expression.GreaterThanOrEqual(secondExp, Expression.Constant(fromValue.Value.Second))
                                                                    )
                                                                )
                                                            )
                                                        )
                                                    );
                                                }
                                                else
                                                {
                                                    fromExp = Expression.AndAlso(
                                                        Expression.GreaterThanOrEqual(yearExp, Expression.Constant(fromValue.Value.Year)),
                                                        Expression.AndAlso(
                                                            Expression.GreaterThanOrEqual(monthExp, Expression.Constant(fromValue.Value.Month)),
                                                            Expression.GreaterThanOrEqual(dayExp, Expression.Constant(fromValue.Value.Day))
                                                        )
                                                    );
                                                }

                                                if (toHasTime)
                                                {
                                                    toExp = Expression.AndAlso(
                                                        Expression.LessThanOrEqual(yearExp, Expression.Constant(toValue.Value.Year)),
                                                        Expression.AndAlso(
                                                            Expression.LessThanOrEqual(monthExp, Expression.Constant(toValue.Value.Month)),
                                                            Expression.AndAlso(
                                                                Expression.LessThanOrEqual(dayExp, Expression.Constant(toValue.Value.Day)),
                                                                Expression.AndAlso(
                                                                    Expression.LessThanOrEqual(hourExp, Expression.Constant(toValue.Value.Hour)),
                                                                    Expression.AndAlso(
                                                                        Expression.LessThanOrEqual(minuteExp, Expression.Constant(toValue.Value.Minute)),
                                                                        Expression.LessThanOrEqual(secondExp, Expression.Constant(toValue.Value.Second))
                                                                    )
                                                                )
                                                            )
                                                        )
                                                    );
                                                }
                                                else
                                                {
                                                    toExp = Expression.AndAlso(
                                                        Expression.LessThanOrEqual(yearExp, Expression.Constant(toValue.Value.Year)),
                                                        Expression.AndAlso(
                                                            Expression.LessThanOrEqual(monthExp, Expression.Constant(toValue.Value.Month)),
                                                            Expression.LessThanOrEqual(dayExp, Expression.Constant(toValue.Value.Day))
                                                        )
                                                    );
                                                }

                                                // Combine the from and to expressions into the condition
                                                conditionExp = Expression.AndAlso(conditionExp, fromExp);
                                                conditionExp = Expression.AndAlso(conditionExp, toExp);
                                            }
                                            break;
                                        }


                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }

                            else if (entityProperty.PropertyType == typeof(bool) || entityProperty.PropertyType == typeof(bool?))
                            {
                                var filterPropertyName = propName;
                                var filterProperty = filter.GetType().GetProperty(filterPropertyName);
                                var propFilter = (FilterOptionB)filterProperty.GetValue(filter);

                                switch (propFilter)
                                {
                                    case FilterOptionB.IsTrue:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.Equal(propExp, Expression.Constant(true)));
                                        break;
                                    case FilterOptionB.IsFalse:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.Equal(propExp, Expression.Constant(false)));
                                        break;
                                    case FilterOptionB.IsNull:
                                        conditionExp = Expression.AndAlso(conditionExp, Expression.Equal(propExp, Expression.Constant(null)));
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }
                    }
                }
            }

            // If SearchValue is not empty, add criteria to check if any column contains the SearchValue


            if (baseCriteria != null)
            {
                var invokedExpr = Expression.Invoke(baseCriteria, parameterExp);
                conditionExp = Expression.AndAlso(conditionExp, invokedExpr);
            }

        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error building criteria", ex);
        }

        var lambdaExp = Expression.Lambda<Func<T1, bool>>(conditionExp, parameterExp);
        return lambdaExp;
    }

}