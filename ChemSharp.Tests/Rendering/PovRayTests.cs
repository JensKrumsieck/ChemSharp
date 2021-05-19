using ChemSharp.Rendering.Extensions;
using ChemSharp.Rendering.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Numerics;

namespace ChemSharp.Tests.Rendering
{
    [TestClass]
    public class PovRayTests
    {
        [TestMethod]
        public void CameraTest()
        {
            var exp = string.Join(Environment.NewLine,
                "camera {", "\tlocation <7, 7, 7>", "\tlook_at <0, 0, 0>", "\tup <0, 1, 0>", "}", "");
            var c = new Camera
            {
                Location = new Vector3(7, 7, 7),
                LookAt = Vector3.Zero
            };
            Assert.AreEqual(c.ToPovString(), exp);
        }

        [TestMethod]
        public void LightTest()
        {
            var exp = string.Join(Environment.NewLine,
                "light_source {", "\t<5, 5, 5>, rgb <1, 1, 1>", "}", "");
            var l = new Light
            {
                Location = new Vector3(5, 5, 5),
                Color = new Vector3(1, 1, 1)
            };
            Assert.AreEqual(l.ToPovString(), exp);
        }

        [TestMethod]
        public void SphereTest()
        {
            var exp = string.Join(Environment.NewLine,
                "sphere { <0, 0, 0>, 1", "\tpigment {", "\t\tcolor rgb <1, 0, 0>", "\t}", "}", "");
            var s = new Sphere();
            Assert.AreEqual(s.ToPovString(), exp);
        }

        [TestMethod]
        public void ParseBasicScene()
        {
            //saves a .pov file - test the render yourself
            var c = new Camera
            {
                Location = new Vector3(7, 7, 7),
                LookAt = Vector3.Zero
            }; var l = new Light
            {
                Location = new Vector3(5, 5, 5),
                Color = new Vector3(1, 1, 1)
            };
            var s = new Sphere();

            var pov = string.Join(Environment.NewLine, c.ToPovString(), l.ToPovString(), s.ToPovString());
            File.WriteAllText("test.pov", pov);
        }
    }
}
