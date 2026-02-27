using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TypeRegistry.Editor
{
    public static class TypeRegistryBuilder
    {
        const string OutputPath = "Assets/Resources/TypeRegistryData.json";

        [MenuItem("Tools/Generate Type Registry")]
        public static void GenerateTypeRegistry()
        {
            var data = new TypeRegistryData();
            var allTypes = new HashSet<Type>();
            var registeredAttributes = new HashSet<Type>();

            ScanForRegisteredTypes(allTypes, registeredAttributes);
            BuildAttributeMappings(data, allTypes, registeredAttributes);
            BuildInheritanceTree(data, allTypes);
            Sort(data);

            SaveToFile(data);
            AssetDatabase.Refresh();

            Debug.Log(
                $"Type Registry generated: {data.Types.Count} types, {registeredAttributes.Count} attributes tracked"
            );
        }

        static void Sort(TypeRegistryData data)
        {
            data.AttributeToFields.Sort((a, b) => string.Compare(
                    a.AttributeTypeName,
                    b.AttributeTypeName,
                    StringComparison.Ordinal
                )
            );
            data.AttributeToMethods.Sort((a, b) => string.Compare(
                    a.AttributeTypeName,
                    b.AttributeTypeName,
                    StringComparison.Ordinal
                )
            );
            data.AttributeToTypes.Sort((a, b) => string.Compare(
                    a.AttributeTypeName,
                    b.AttributeTypeName,
                    StringComparison.Ordinal
                )
            );
            data.BaseToDerived.Sort((a, b) => string.Compare(a.BaseTypeName, b.BaseTypeName, StringComparison.Ordinal));
            data.Types.Sort((a, b) => string.Compare(a.TypeName, b.TypeName, StringComparison.Ordinal));

            foreach (var attributeToFieldsEntry in data.AttributeToFields)
            {
                attributeToFieldsEntry.Fields.Sort((a, b) => string.Compare(
                        a.FieldName,
                        b.FieldName,
                        StringComparison.Ordinal
                    )
                );
            }

            foreach (var attributeToMethodsEntry in data.AttributeToMethods)
            {
                attributeToMethodsEntry.Methods.Sort((a, b) => string.Compare(
                        a.MethodName,
                        b.MethodName,
                        StringComparison.Ordinal
                    )
                );
            }
            
            foreach (var attributeToTypesEntry in data.AttributeToTypes)
            {
                attributeToTypesEntry.TypeNames.Sort(StringComparer.Ordinal);
            }
            
            foreach (var e in data.BaseToDerived)
            {
                e.DerivedTypeNames.Sort(StringComparer.Ordinal);
            }
        }

        static void ScanForRegisteredTypes(HashSet<Type> allTypes, HashSet<Type> registeredAttributes)
        {
            var typesWithAttribute = TypeCache.GetTypesWithAttribute<RegisterTypeLookupAttribute>();

            foreach (var type in typesWithAttribute)
            {
                if (typeof(Attribute).IsAssignableFrom(type))
                {
                    registeredAttributes.Add(type);
                }
                else
                {
                    allTypes.Add(type);

                    var derivedTypes = TypeCache.GetTypesDerivedFrom(type);
                    foreach (var derivedType in derivedTypes)
                    {
                        if (!derivedType.IsAbstract)
                        {
                            allTypes.Add(derivedType);
                        }
                    }
                }
            }
        }

        static void BuildAttributeMappings(
            TypeRegistryData data,
            HashSet<Type> allTypes,
            HashSet<Type> registeredAttributes)
        {
            var attributeToTypes = new Dictionary<Type, List<string>>();
            var attributeToFields = new Dictionary<Type, List<TypeRegistryData.FieldRef>>();
            var attributeToMethods = new Dictionary<Type, List<TypeRegistryData.MethodRef>>();

            foreach (var attrType in registeredAttributes)
            {
                attributeToTypes[attrType] = new List<string>();
                attributeToFields[attrType] = new List<TypeRegistryData.FieldRef>();
                attributeToMethods[attrType] = new List<TypeRegistryData.MethodRef>();
            }

            var allScannedTypes = new HashSet<Type>(allTypes);

            foreach (var attrType in registeredAttributes)
            {
                var typesWithAttr = TypeCache.GetTypesWithAttribute(attrType);
                foreach (var type in typesWithAttr)
                {
                    attributeToTypes[attrType].Add(type.FullName);
                    allScannedTypes.Add(type);
                }

                var fieldsWithAttr = TypeCache.GetFieldsWithAttribute(attrType);
                foreach (var field in fieldsWithAttr)
                {
                    attributeToFields[attrType]
                        .Add(
                            new TypeRegistryData.FieldRef
                            {
                                DeclaringTypeName = field.DeclaringType.FullName,
                                FieldName = field.Name,
                            }
                        );
                    allScannedTypes.Add(field.DeclaringType);
                }

                var methodsWithAttr = TypeCache.GetMethodsWithAttribute(attrType);
                foreach (var method in methodsWithAttr)
                {
                    attributeToMethods[attrType]
                        .Add(
                            new TypeRegistryData.MethodRef
                            {
                                DeclaringTypeName = method.DeclaringType.FullName,
                                MethodName = method.Name,
                                ParameterTypeNames = method.GetParameters()
                                    .Select(p => p.ParameterType.FullName)
                                    .ToList(),
                            }
                        );
                    allScannedTypes.Add(method.DeclaringType);
                }
            }

            foreach (var type in allScannedTypes)
            {
                if (!data.Types.Any(t => t.TypeName == type.FullName))
                {
                    data.Types.Add(
                        new TypeRegistryData.TypeEntry
                        {
                            TypeName = type.FullName,
                            AssemblyName = type.Assembly.GetName().Name,
                        }
                    );
                }
            }

            foreach (var kvp in attributeToTypes)
            {
                data.AttributeToTypes.Add(
                    new TypeRegistryData.AttributeToTypesEntry
                    {
                        AttributeTypeName = kvp.Key.FullName,
                        TypeNames = kvp.Value,
                    }
                );
            }

            foreach (var kvp in attributeToFields)
            {
                data.AttributeToFields.Add(
                    new TypeRegistryData.AttributeToFieldsEntry
                    {
                        AttributeTypeName = kvp.Key.FullName,
                        Fields = kvp.Value,
                    }
                );
            }

            foreach (var kvp in attributeToMethods)
            {
                data.AttributeToMethods.Add(
                    new TypeRegistryData.AttributeToMethodsEntry
                    {
                        AttributeTypeName = kvp.Key.FullName,
                        Methods = kvp.Value,
                    }
                );
            }
        }

        static void BuildInheritanceTree(TypeRegistryData data, HashSet<Type> baseTypes)
        {
            var baseToDerived = new Dictionary<Type, List<string>>();
            var allTrackedTypes = data.Types.Select(t => ResolveType(t.TypeName, t.AssemblyName))
                .Where(t => t != null)
                .ToList();

            foreach (var type in allTrackedTypes)
            {
                var baseType = type.BaseType;
                while (baseType != null && baseType != typeof(object))
                {
                    if (!baseToDerived.ContainsKey(baseType))
                    {
                        baseToDerived[baseType] = new List<string>();
                    }

                    baseToDerived[baseType].Add(type.FullName);
                    baseType = baseType.BaseType;
                }

                foreach (var interfaceType in type.GetInterfaces())
                {
                    if (!baseToDerived.ContainsKey(interfaceType))
                    {
                        baseToDerived[interfaceType] = new List<string>();
                    }

                    baseToDerived[interfaceType].Add(type.FullName);
                }
            }

            foreach (var kvp in baseToDerived)
            {
                data.BaseToDerived.Add(
                    new TypeRegistryData.BaseToDerivesEntry
                    {
                        BaseTypeName = kvp.Key.FullName,
                        DerivedTypeNames = kvp.Value.Distinct().ToList(),
                    }
                );
            }
        }

        static Type ResolveType(string typeName, string assemblyName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = assemblies.FirstOrDefault(a => a.GetName().Name == assemblyName);
            return assembly?.GetType(typeName);
        }

        static void SaveToFile(TypeRegistryData data)
        {
            var json = JsonUtility.ToJson(data, true);

            var directory = Path.GetDirectoryName(OutputPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(OutputPath, json);

            Debug.Log($"TypeRegistryData saved to {OutputPath}");
        }
    }
}