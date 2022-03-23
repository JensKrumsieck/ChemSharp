using System.Numerics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Molecules.Tests;

[TestClass]
public class AtomTests
{
    [TestMethod]
    public void Atom_ShouldHaveSymbolAndLocation()
    {
        var atom = new Atom("H");
        atom.Symbol.Should().Be("H");
        atom.Location.Should().Be(Vector3.Zero);
    }
}
