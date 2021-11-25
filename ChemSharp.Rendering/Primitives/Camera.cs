using System;
using System.Numerics;

namespace ChemSharp.Rendering.Primitives
{
    [Obsolete("Package will be removed soon")]
    public class Camera
    {
        public Vector3 Location { get; set; } = Vector3.Zero;

        public Vector3 LookAt { get; set; } = Vector3.UnitZ;

        public Vector3 Up { get; set; } = Vector3.UnitY;

        public Vector3? Right { get; set; } = null;

        public float? FieldOfView { get; set; } = null;
    }
}
