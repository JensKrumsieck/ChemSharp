using ChemSharp.Generator.Extension;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ChemSharp.Generator
{
    [Generator]
    public class MathFGenerator : ISourceGenerator
    {
        private const string Indent = "    ";

        /// <summary>
        /// To make sure it does not generate with higher NET Version
        /// May be changed in future
        /// </summary>
        private readonly string[] _filter =
            {"Sin", "Cos", "Tan", "Atan", "Acos", "Asin", "Atan2", "Sqrt", "Pow", "Log", "Exp"};

        public void Initialize(GeneratorInitializationContext context)
        { }

        public void Execute(GeneratorExecutionContext context)
        {
            var math = typeof(Math);
            var methods = math.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m => _filter.Contains(m.Name) && m.ReturnType == typeof(double)).ToArray();
            var constants = math.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            var src = new StringBuilder();
            src.AppendLine("//Auto Generated Code");
            src.AppendLine("#if NETSTANDARD2_0");
            src.AppendLine("namespace ChemSharp.Mathematics");
            src.AppendLine("{");
            src.AppendLine($"{Indent}public static class MathF");
            src.AppendLine($"{Indent}{{");

            AppendConstants(src, constants);
            AppendMethods(src, methods);

            src.AppendLine($"{Indent}}}"); //close class
            src.AppendLine("}"); //close ns
            src.AppendLine("#endif");

            context.AddSource("MathF.cs", SourceText.From(src.ToString(), Encoding.UTF8));
        }

        private static void AppendMethods(StringBuilder src, MethodInfo[] methods)
        {
            foreach (var m in methods.DistinctBy(s => s.Name))
            {
                var head = $"public static float {m.Name} (";
                var body = $"(float)System.Math.{m.Name}(";
                var param = m.GetParameters().Where(s => s.ParameterType == typeof(double)).ToArray();
                var signatures = string.Join(",", param.Select(s => "float " + s.Name));
                var call = string.Join(",", param.Select(s => s.Name));
                head += signatures + ")";
                body += call + ");";

                src.AppendLine($"{Indent}{Indent}{head} => {body}");
            }
        }

        private static void AppendConstants(StringBuilder src, FieldInfo[] fields)
        {
            foreach (var f in fields)
            {
                if (f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(double))
                    src.AppendLine($"{Indent}{Indent}public const float {f.Name} = {Convert.ToDouble(f.GetRawConstantValue()).ToString(CultureInfo.InvariantCulture)}f;");
            }
        }
    }
}
