using System;
using System.Numerics;

namespace ChemSharp.Rendering.Primitives
{
    [Obsolete("Package will be removed soon")]
    public class Light
    {
        public Vector3 Location { get; set; } = Vector3.Zero;

        public Vector3 Color { get; set; } = Vector3.One;
    }
}
