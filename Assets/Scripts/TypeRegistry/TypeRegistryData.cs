using System;
using System.Collections.Generic;

namespace TypeRegistry
{
    [Serializable]
    public class TypeRegistryData
    {
        public List<TypeEntry> Types = new();
        public List<AttributeToTypesEntry> AttributeToTypes = new();
        public List<AttributeToFieldsEntry> AttributeToFields = new();
        public List<AttributeToMethodsEntry> AttributeToMethods = new();
        public List<BaseToDerivesEntry> BaseToDerived = new();

        [Serializable]
        public class TypeEntry
        {
            public string TypeName;
            public string AssemblyName;
        }

        [Serializable]
        public class AttributeToTypesEntry
        {
            public string AttributeTypeName;
            public List<string> TypeNames = new();
        }

        [Serializable]
        public class AttributeToFieldsEntry
        {
            public string AttributeTypeName;
            public List<FieldRef> Fields = new();
        }

        [Serializable]
        public class AttributeToMethodsEntry
        {
            public string AttributeTypeName;
            public List<MethodRef> Methods = new();
        }

        [Serializable]
        public class BaseToDerivesEntry
        {
            public string BaseTypeName;
            public List<string> DerivedTypeNames = new();
        }

        [Serializable]
        public class FieldRef
        {
            public string DeclaringTypeName;
            public string FieldName;
        }

        [Serializable]
        public class MethodRef
        {
            public string DeclaringTypeName;
            public string MethodName;
            public List<string> ParameterTypeNames = new();
        }
    }
}
