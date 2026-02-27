using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TypeRegistry
{
    public static class TypeLookup
    {
        static TypeRegistryData _data;
        static Dictionary<string, Type> _typeCache;
        static bool _initialized;

        static void EnsureInitialized()
        {
            if (_initialized)
                return;

            _initialized = true;
            _typeCache = new Dictionary<string, Type>();

            var asset = Resources.Load<TextAsset>("TypeRegistryData");
            if (asset == null)
            {
                Debug.LogWarning("TypeRegistryData not found. Type registry will be empty. Run 'Tools > Generate Type Registry' from the Unity menu.");
                _data = new TypeRegistryData();
                return;
            }

            _data = JsonUtility.FromJson<TypeRegistryData>(asset.text);
        }

        public static IReadOnlyList<Type> GetTypesWithAttribute<T>() where T : Attribute
        {
            return GetTypesWithAttribute(typeof(T));
        }

        public static IReadOnlyList<Type> GetTypesWithAttribute(Type attributeType)
        {
            EnsureInitialized();

            var entry = _data.AttributeToTypes.FirstOrDefault(e => e.AttributeTypeName == attributeType.FullName);
            if (entry == null)
                return Array.Empty<Type>();

            var result = new List<Type>();
            foreach (var typeName in entry.TypeNames)
            {
                var type = ResolveType(typeName);
                if (type != null)
                    result.Add(type);
            }

            return result;
        }

        public static IReadOnlyList<Type> GetTypesDerivedFrom<T>()
        {
            return GetTypesDerivedFrom(typeof(T));
        }

        public static IReadOnlyList<Type> GetTypesDerivedFrom(Type baseType)
        {
            EnsureInitialized();

            var entry = _data.BaseToDerived.FirstOrDefault(e => e.BaseTypeName == baseType.FullName);
            if (entry == null)
                return Array.Empty<Type>();

            var result = new List<Type>();
            foreach (var typeName in entry.DerivedTypeNames)
            {
                var type = ResolveType(typeName);
                if (type != null)
                    result.Add(type);
            }

            return result;
        }

        public static IReadOnlyList<FieldInfo> GetFieldsWithAttribute<T>() where T : Attribute
        {
            return GetFieldsWithAttribute(typeof(T));
        }

        public static IReadOnlyList<FieldInfo> GetFieldsWithAttribute(Type attributeType)
        {
            EnsureInitialized();

            var entry = _data.AttributeToFields.FirstOrDefault(e => e.AttributeTypeName == attributeType.FullName);
            if (entry == null)
                return Array.Empty<FieldInfo>();

            var result = new List<FieldInfo>();
            foreach (var fieldRef in entry.Fields)
            {
                var declaringType = ResolveType(fieldRef.DeclaringTypeName);
                if (declaringType == null)
                    continue;

                var field = declaringType.GetField(fieldRef.FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (field != null)
                    result.Add(field);
            }

            return result;
        }

        public static IReadOnlyList<MethodInfo> GetMethodsWithAttribute<T>() where T : Attribute
        {
            return GetMethodsWithAttribute(typeof(T));
        }

        public static IReadOnlyList<MethodInfo> GetMethodsWithAttribute(Type attributeType)
        {
            EnsureInitialized();

            var entry = _data.AttributeToMethods.FirstOrDefault(e => e.AttributeTypeName == attributeType.FullName);
            if (entry == null)
                return Array.Empty<MethodInfo>();

            var result = new List<MethodInfo>();
            foreach (var methodRef in entry.Methods)
            {
                var declaringType = ResolveType(methodRef.DeclaringTypeName);
                if (declaringType == null)
                    continue;

                var paramTypes = methodRef.ParameterTypeNames.Select(ResolveType).ToArray();
                if (paramTypes.Any(t => t == null))
                    continue;

                var method = declaringType.GetMethod(methodRef.MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, paramTypes, null);
                if (method != null)
                    result.Add(method);
            }

            return result;
        }

        static Type ResolveType(string fullName)
        {
            if (_typeCache.TryGetValue(fullName, out var cached))
                return cached;

            var typeEntry = _data.Types.FirstOrDefault(t => t.TypeName == fullName);
            if (typeEntry == null)
                return null;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.GetName().Name != typeEntry.AssemblyName)
                    continue;

                var type = assembly.GetType(fullName);
                if (type != null)
                {
                    _typeCache[fullName] = type;
                    return type;
                }
            }

            return null;
        }

        public static void ClearCache()
        {
            _initialized = false;
            _data = null;
            _typeCache?.Clear();
        }
    }
}
