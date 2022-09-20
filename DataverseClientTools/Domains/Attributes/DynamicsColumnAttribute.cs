using System;
using DataverseClientTools.Domains.Enums;

namespace DataverseClientTools.Domains.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DynamicsColumnAttribute : Attribute
    {
        public DynamicsColumnAttribute(string name, DynamicAttributeType type = DynamicAttributeType.NormalValue,
            string entityReference = "")
        {
            DynamicsTableType = type;
            Name = name;
            EntityReference = entityReference;
        }

        public string Name { get; set; }

        public string EntityReference { get; set; }

        public DynamicAttributeType DynamicsTableType { get; set; }
    }
}