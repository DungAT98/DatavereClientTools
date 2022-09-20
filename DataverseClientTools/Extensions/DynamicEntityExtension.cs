using System;
using System.Collections.Generic;
using System.Linq;
using DataverseClientTools.Domains.Abstractions;
using DataverseClientTools.Domains.Attributes;
using DataverseClientTools.Domains.Enums;
using DataverseClientTools.Helpers;
using Microsoft.Xrm.Sdk;

namespace DataverseClientTools.Extensions
{
    public static class DynamicEntityExtension
    {
        public static Entity ConvertToCrmObject<T>(this IDynamicEntity entityBase)
        {
            var model = (T)entityBase;
            var entity = new Entity();
            var allColumnProperties = ReflectionHelper.GetPropertyInfoWithAttribute<T, DynamicsColumnAttribute>()
                .ToList();
            foreach (var column in allColumnProperties)
            {
                if (column.GetValue(model) == null || string.IsNullOrWhiteSpace(column.GetValue(model).ToString()))
                {
                    continue;
                }

                if (column.Name == "Id")
                {
                    entity.Id = Guid.Parse(column.GetValue(model)?.ToString());
                    continue;
                }

                var dynamicAttributeValue =
                    ReflectionHelper.GetAttributeValue<DynamicsColumnAttribute>(column).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(dynamicAttributeValue?.EntityReference) &&
                    dynamicAttributeValue.DynamicsTableType == DynamicAttributeType.EntityReference)
                {
                    entity.Attributes[dynamicAttributeValue?.Name] = new EntityReference(
                        dynamicAttributeValue?.EntityReference, Guid.Parse(column.GetValue(model)?.ToString()));
                    continue;
                }

                switch (dynamicAttributeValue?.DynamicsTableType)
                {
                    case DynamicAttributeType.OptionSet:
                    {
                        entity.Attributes[dynamicAttributeValue?.Name] =
                            new OptionSetValue(int.Parse(column.GetValue(model)?.ToString()));
                        break;
                    }
                    case DynamicAttributeType.Money:
                    {
                        entity.Attributes[dynamicAttributeValue?.Name] =
                            new Money(decimal.Parse(column.GetValue(model)?.ToString()));
                        break;
                    }
                    case DynamicAttributeType.OptionSetCollection:
                    {
                        var inputList = column.GetValue(model) as List<int>;
                        entity.Attributes[dynamicAttributeValue?.Name] =
                            new OptionSetValueCollection(inputList?.Select(n => new OptionSetValue(n)).ToList());
                        break;
                    }
                    default:
                    {
                        entity.Attributes[dynamicAttributeValue?.Name] = column.GetValue(model);
                        break;
                    }
                }
            }

            var tableAttribute = Attribute.GetCustomAttributes(typeof(T))
                .OfType<DynamicsTableAttribute>().FirstOrDefault();
            entity.LogicalName = tableAttribute?.Name;

            return entity;
        }
    }
}