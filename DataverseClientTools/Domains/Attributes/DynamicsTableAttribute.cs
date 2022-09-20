using System;

namespace DataverseClientTools.Domains.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DynamicsTableAttribute : Attribute
    {
        public string Name { get; }

        public DynamicsTableAttribute(string name)
        {
            Name = name;
        }
    }
}