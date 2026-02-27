using System;

namespace TypeRegistry
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
    public sealed class RegisterTypeLookupAttribute : Attribute
    {
    }
}
