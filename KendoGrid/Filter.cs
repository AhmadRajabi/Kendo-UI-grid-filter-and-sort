using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace KendoGrid
{
    internal class Filter<TEntity>
    {
        private static string GetPropertyType(Type type, string field)
        {
            var info = type.GetProperty(field);

            if (info == null)
                return string.Empty;

            var nullableType = Nullable.GetUnderlyingType(info.PropertyType);
            return nullableType != null ? nullableType.Name.ToLower() + "?" : info.PropertyType.Name.ToLower();
        }
        const string PersiandatedRegex = @"^1[34][0-9][0-9](\/|-|)((1[0-2])|(0[1-9])|([1-9]))(\/|-|)(([12][0-9])|(3[01])|0[1-9]|([1-9]))$";

        public static bool IsPersianDate(string date)
        {
            return Regex.IsMatch(date, PersiandatedRegex);
        }
        private static DateTime GetDate(string value)
        {
            if (value.Contains("T"))
            {
                //"1392-08-06T19:30:00.000Z";
                value = value.Split('T')[0].Replace('-', '/');
            }

            DateTime date;
            if (IsPersianDate(value))
            {
                date= DNTPersianUtils.Core.PersianDateTimeUtils.ToGregorianDateTime(value).Value;

                return date;
            }
            else
            {
                var provider = CultureInfo.GetCultureInfo("en-US");

                date = DateTime.ParseExact(value, new[] { "yyyy/MM/dd", "yyyy-MM-dd", "yyyy/MM/ddTHH:mm:ss" }, provider, DateTimeStyles.None);
                return date;
            }
        }
        private static TimeSpan GetTime(string time)
        {
            var dateTime = DateTime.ParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture);
            return dateTime.TimeOfDay;
        }
        private static string GetExpression(string field, string op, string param)
        {
            var dataType = GetPropertyType(typeof(TEntity), field);

            if (string.IsNullOrWhiteSpace(dataType))
                return string.Empty;

            if (dataType == "string")
            {
                param = @"""" + param + @"""";
            }

            if (dataType == "datetime" || dataType == "datetime?" || dataType== "datetimeoffset" || dataType == "datetimeoffset?")
            {
                if (dataType == "datetime?" || dataType == "datetimeoffset?")
                    field += ".Value";

                var date = GetDate(param);

                var eq = $"({field}.Year == {date.Year} && {field}.Month == {date.Month} && {field}.Day == {date.Day})";
                var gt = $"(({field}.Year > {date.Year}) || ({field}.Year == {date.Year} && {field}.Month > {date.Month} ) || ({field}.Year == {date.Year} && {field}.Month == {date.Month} && {field}.Day > {date.Day}))";
               // var lt = $"(({field}.Year < {date.Year}) || ({field}.Year == {date.Year} && {field}.Month < {date.Month} ) || ({field}.Year == {date.Year} && {field}.Month == {date.Month} && {field}.Day < {date.Day}))";

                string exDate;
                switch (op)
                {
                    case "eq":
                        exDate = $"({eq})";
                        break;

                    case "neq":
                        exDate = $"!({eq})";
                        break;

                    case "gte":
                        exDate = $"({eq} || {gt})";
                        break;

                    case "gt":
                        exDate = $"({gt})";
                        break;

                    case "lte":
                        exDate = $"{eq} || !({gt})";
                        break;

                    case "lt":
                        exDate = $"!({eq}) && !({gt})";
                        break;
                    default:
                        exDate = "";
                        break;
                }

                return exDate;
            }

            if (dataType == "timespan" || dataType == "timespan?")
            {
                if (dataType == "timespan?")
                    field += ".Value";

                var date = GetTime(param);

                var eq = $"({field}.{nameof(date.Hours)} == {date.Hours} && {field}.{nameof(date.Minutes)} == {date.Minutes} && {field}.{nameof(date.Seconds)} == {date.Seconds})";
                var gt = $"(({field}.{nameof(date.Hours)} > {date.Hours}) || ({field}.{nameof(date.Hours)} == {date.Hours} && {field}.{nameof(date.Minutes)}  > {date.Minutes} ) || ({field}.{nameof(date.Hours)}  == {date.Hours} && {field}.{nameof(date.Minutes)}  == {date.Minutes} && {field}.{nameof(date.Seconds)}  > {date.Seconds}))";
               // var lt = $"(({field}.{nameof(date.Hours)} < {date.Hours}) || ({field}.{nameof(date.Hours)}  == {date.Hours} && {field}.{nameof(date.Minutes)} < {date.Minutes} ) || ({field}.{nameof(date.Hours)}  == {date.Hours} && {field}.{nameof(date.Minutes)}  == {date.Minutes} && {field}.{nameof(date.Seconds)}  < {date.Seconds}))";

                
                string exDate;
                switch (op)
                {
                    case "eq":
                        exDate = $"{eq}";
                        break;

                    case "neq":
                        exDate = $"!({eq})";
                        break;

                    case "gte":
                        exDate = $"{eq} || {gt}";
                        break;

                    case "gt":
                        exDate = $"{gt}";
                        break;
                    case "lte":
                        exDate = $"{eq} || !({gt})";
                        break;

                    case "lt":
                        exDate = $"!({eq}) && !({gt})";
                        break;
                    default:
                        exDate = "";
                        break;
                }

                return exDate;
            }

            if (dataType == "boolean" || dataType == "boolean?")
            {
                if (param.ToLower() == Boolean.TrueString.ToLower())
                    param = Boolean.TrueString.ToLower();

                if (param.ToLower() == Boolean.FalseString.ToLower())
                    param = Boolean.FalseString.ToLower();
            }

            string exStr,
                caseMod = string.Empty;
            switch (op)
            {
                case "eq":
                    exStr = string.Format("{0}{2} == {1}", field, param, caseMod);
                    break;

                case "neq":
                    exStr = string.Format("{0}{2} != {1}", field, param, caseMod);
                    break;

                case "contains":
                    exStr = string.Format("{0}{2}.Contains({1})", field, param, caseMod);
                    break;

                case "doesnotcontain":
                    exStr = string.Format("!{0}{2}.Contains({1})", field, param, caseMod);
                    break;

                case "startswith":
                    exStr = string.Format("{0}{2}.StartsWith({1})", field, param, caseMod);
                    break;

                case "endswith":
                    exStr = string.Format("{0}{2}.EndsWith({1})", field, param, caseMod);
                    break;

                case "gte":
                    exStr = string.Format("{0}{2} >= {1}", field, param, caseMod);
                    break;

                case "gt":
                    exStr = string.Format("{0}{2} > {1}", field, param, caseMod);
                    break;

                case "lte":
                    exStr = string.Format("{0}{2} <= {1}", field, param, caseMod);
                    break;

                case "lt":
                    exStr = string.Format("{0}{2} < {1}", field, param, caseMod);
                    break;

                default:
                    exStr = "";
                    break;
            }

            return exStr;
        }

        public static string GetExpression(FilterDescription filter)
        {
            if (filter == null)
                return null;

            if (filter.Filters == null || !filter.Filters.Any())
            {
                if (string.IsNullOrWhiteSpace(filter.Value))
                    return string.Empty;

                var exprList = GetExpression(filter.Field, filter.Operator, filter.Value);
                return exprList;
            }

            var logic = filter.Logic.ToLower() == "or" ? "||" : "&&";

            var expressions = new List<string>();

            foreach (var item in filter.Filters)
            {
                var expr = GetExpression(item);

                if (!string.IsNullOrWhiteSpace(expr))
                    expressions.Add(expr);
            }

            if (expressions.Count == 0)
                return null;

            return "(" + string.Join(" " + logic + " ", expressions) + ")";
        }
    }
}