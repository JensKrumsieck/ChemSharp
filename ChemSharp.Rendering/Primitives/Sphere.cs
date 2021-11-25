using System;
using System.Numerics;

namespace ChemSharp.Rendering.Primitives
{
    [Obsolete("Package will be removed soon")]
    public class Sphere
    {
        public Vector3 Location { get; set; } = Vector3.Zero;
        public float Radius { get; set; } = 1f;

        public Vector3 Color { get; set; } = Vector3.UnitX; //red
    }
}
