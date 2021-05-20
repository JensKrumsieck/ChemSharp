using ChemSharp.Molecules;
using ChemSharp.Rendering.Export;
using ChemSharp.Rendering.Extensions;
using ChemSharp.Rendering.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        public void CylinderTest()
        {
            var exp = string.Join(Environment.NewLine,
                "cylinder { <0, 0, 0>, <0, 1, 0>, 1", "\tpigment {", "\t\tcolor rgb <1, 0, 0>", "\t}", "}", "");
            var c = new Cylinder();
            Assert.AreEqual(c.ToPovString(), exp);
        }

        [TestMethod]
        public void ParseBasicScene()
        {
            //saves a .pov file - test the render yourself
            var c = new Camera
            {
                Location = new Vector3(7, 7, 7),
                LookAt = Vector3.Zero
            };
            var l = new Light
            {
                Location = new Vector3(5, 5, 5),
                Color = new Vector3(1, 1, 1)
            };
            var s = new Sphere();

            var pov = string.Join(Environment.NewLine, c.ToPovString(), l.ToPovString(), s.ToPovString());
            File.WriteAllText("test.pov", pov);
        }

        [TestMethod]
        public void ParseMoleculeScene()
        {
            var m = MoleculeFactory.Create("files/cif.cif");
            var c = new Camera
            {
                Location = new Vector3(10, 7, 40),
                LookAt = m.Atoms.FirstOrDefault(s => s.IsMetal)?.Location ?? Vector3.Zero,
                FieldOfView = 60
            };
            var l = new Light
            {
                Location = new Vector3(15, 5, 5),
                Color = new Vector3(1, 1, 1)
            };
            var atoms = m.Atoms.Select(a =>
                    new Sphere { Location = a.Location, Radius = (a.CovalentRadius ?? 100) / 200f, Color = ToVec3(a.Color) }
                        .ToPovString())
                .ToList();
            Assert.AreEqual(atoms.Count, m.Atoms.Count);
            var a = string.Join(Environment.NewLine, atoms);
            var pov = "background {rgb < 0.1, 0.1, 0.1>}" + //hardcode background to gray
                      string.Join(Environment.NewLine, c.ToPovString(), l.ToPovString(), a);
            File.WriteAllText("mol.pov", pov);
        }

        [TestMethod]
        public void TestExporter()
        {
            var m = MoleculeFactory.Create("files/cif.cif");
            var c = new Camera
            {
                Location = new Vector3(10, 7, 40),
                LookAt = m.Atoms.FirstOrDefault(s => s.IsMetal)?.Location ?? Vector3.Zero,
                FieldOfView = 60
            };
            var l = new List<Light>
            {
                new()
                {
                    Location = new Vector3(15, 5, 5),
                    Color = new Vector3(1, 1, 1)
                }
            };
            PovRayExporter.Export(m, "export.pov", c, l);
        }

        public static Vector3 ToVec3(string hex)
        {
            var col = ColorTranslator.FromHtml(hex);
            return new Vector3(col.R / 255f, col.G / 255f, col.B / 255f);
        }
    }
}
