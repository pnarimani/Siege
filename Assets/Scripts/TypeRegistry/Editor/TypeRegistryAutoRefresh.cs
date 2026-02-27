using UnityEditor;
using UnityEditor.Compilation;

namespace TypeRegistry.Editor
{
    [InitializeOnLoad]
    public static class TypeRegistryAutoRefresh
    {
        static TypeRegistryAutoRefresh()
        {
            CompilationPipeline.compilationFinished -= OnCompilationFinished;
            CompilationPipeline.compilationFinished += OnCompilationFinished;
        }

        static void OnCompilationFinished(object obj)
        {
            TypeRegistryBuilder.GenerateTypeRegistry();
        }
    }
}
