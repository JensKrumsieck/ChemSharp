using ChemSharp.Rendering.Primitives;
using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace ChemSharp.Rendering.Extensions
{
    [Obsolete("Package will be removed soon")]
    public static class PovSdlExtension
    {
        public static string ToPovString<T>(this T input) where T : struct
        {
            if (input is float f) return f.ToString(CultureInfo.InvariantCulture);
            if (input is Vector3 v) return $"<{v.X.ToPovString()}, {v.Y.ToPovString()}, {v.Z.ToPovString()}>";
            return "";
        }

        public static string ToPovString(this Camera c)
        {
            var pov = new StringBuilder();
            pov.AppendLine($"camera {{{(c.FieldOfView.HasValue ? " perspective angle " + c.FieldOfView.Value.ToPovString() : "")}");
            pov.AppendLine($"\tlocation {c.Location.ToPovString()}");
            pov.AppendLine($"\tlook_at {c.LookAt.ToPovString()}");
            pov.AppendLine($"\tup {c.Up.ToPovString()}");
            if (c.Right.HasValue) pov.AppendLine($"\tright {c.Right.Value.ToPovString()}");
            pov.AppendLine("}");
            return pov.ToString();
        }

        public static string ToPovString(this Light l)
        {
            var pov = new StringBuilder();
            pov.AppendLine("light_source {");
            pov.AppendLine($"\t{l.Location.ToPovString()}, rgb {l.Color.ToPovString()}");
            pov.AppendLine("}");
            return pov.ToString();
        }

        public static string ToPovString(this Sphere s)
        {
            var pov = new StringBuilder();
            pov.AppendLine($"sphere {{ {s.Location.ToPovString()}, {s.Radius.ToPovString()}");
            pov.AppendLine("\tpigment {");
            pov.AppendLine($"\t\tcolor rgb {s.Color.ToPovString()}");
            pov.AppendLine("\t}");
            pov.AppendLine("}");
            return pov.ToString();
        }

        public static string ToPovString(this Cylinder c)
        {
            var pov = new StringBuilder();
            pov.AppendLine($"cylinder {{ {c.Start.ToPovString()}, {c.End.ToPovString()}, {c.Radius.ToPovString()}");
            pov.AppendLine("\tpigment {");
            pov.AppendLine($"\t\tcolor rgb {c.Color.ToPovString()}");
            pov.AppendLine("\t}");
            pov.AppendLine("}");
            return pov.ToString();
        }
    }
}
