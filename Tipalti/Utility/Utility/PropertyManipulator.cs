using System.Reflection;
using System.Text.RegularExpressions;

namespace Utility;

public class PropertyManipulator
{
    public IEnumerable<(string PropertyName, string ValueName)> GetProperties(object obj)
    {
        var stringPropertyInfo = StringPropertyInfo(obj);
        var objectProperties = PropertInfos(obj);

        var result = new List<(string, string)>();

        var mapedStringsProperties = stringPropertyInfo
            .Select(x => (x.Name.ToString(), x.GetValue(obj)?.ToString()))
            .ToList();

        result.AddRange(mapedStringsProperties);

        foreach (var property in objectProperties)
        {
            var x = GetProperties(property.GetValue(obj));
            result.AddRange(x);
        }

        return result;
    }

    public List<PropertyInfo> StringPropertyInfo(object obj)
    {
        if (obj != null)
        {
            PropertyInfo[] propertyInfo = obj.GetType()
                .GetProperties();

            var result = new List<PropertyInfo>();

            foreach (var property in propertyInfo)
            {
                if (property.PropertyType == typeof(string))
                {
                    result.Add(property);
                }
            }

            return result;
        }

        return new List<PropertyInfo>();
    }

    public List<PropertyInfo> PropertInfos(object obj)
    {
        if (obj != null)
        {
            PropertyInfo[] propertyInfo = obj.GetType()
                .GetProperties();

            var result = new List<PropertyInfo>();

            foreach (var property in propertyInfo)
            {
                if (property.PropertyType.IsClass &&
                    !(property.PropertyType == typeof(string)))
                {
                    result.Add(property);
                }
            }

            return result;
        }

        return new List<PropertyInfo>();
    }

    public List<(string, string)> GetRightStrings(IEnumerable<(string, string)> strings)
    {
        string pattern = "[^a-zA-Z0-9 ]";

        var result = new List<(string, string)>();
        if (strings != null)
        {
            foreach (var item in strings)
            {
                string filteredName = Regex.Replace(item.Item2, pattern, "");
                result.Add((filteredName, item.Item1));
            }
        }

        return result;
    }

    public void Print(IEnumerable<(string, string)> strings)
    {
        Console.WriteLine($"Object of Class 'Payment'");
        foreach (var x in strings)
        {
            Console.WriteLine($"{x.Item2} = '{x.Item1}'");
        }
    }
}