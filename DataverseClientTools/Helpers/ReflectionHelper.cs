using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataverseClientTools.Helpers
{
    public static class ReflectionHelper
    {
        public static List<PropertyInfo> GetPropertyInfoWithAttribute<TModel, TAttribute>() where TAttribute : Attribute
        {
            var allProperties = typeof(TModel).GetProperties();
            var result = new List<PropertyInfo>();
            foreach (var propertyInfo in allProperties)
            {
                var attributeList = propertyInfo.GetCustomAttributes(true)
                    .OfType<TAttribute>();
                if (attributeList.Count() != 0)
                {
                    result.Add(propertyInfo);
                }
            }

            return result;
        }

        public static List<TAttribute> GetAttributeValue<TAttribute>(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(true)
                .OfType<TAttribute>()
                .ToList();
        }
    }
}