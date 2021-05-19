using System.Numerics;

namespace ChemSharp.Rendering.Primitives
{
    public class Light
    {
        public Vector3 Location { get; set; } = Vector3.Zero;

        public Vector3 Color { get; set; } = Vector3.One;
    }
}
