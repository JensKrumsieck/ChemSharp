using System;
using System.Numerics;

namespace ChemSharp.Rendering.Primitives
{
    [Obsolete("Package will be removed soon")]
    public class Cylinder
    {
        public Vector3 Start { get; set; } = Vector3.Zero;
        public Vector3 End { get; set; } = Vector3.UnitY;

        public float Radius { get; set; } = 1f;

        public Vector3 Color { get; set; } = new(1f, 0, 0);
    }
}
