﻿using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace ChemSharp.Molecules.HelixToolkit;

public class Bond3D : ModelVisual3D
{
	private Color _color = Colors.Green;

	private bool _isValid;

	public Bond3D(Bond bond)
	{
		Bond = bond;
		UpdateBond();
	}

	public Bond Bond { get; set; }

	public bool IsValid
	{
		get => _isValid;
		set
		{
			_isValid = value;
			UpdateBond();
		}
	}

	public Color Color
	{
		get => _color;
		set
		{
			_color = value;
			UpdateBond();
		}
	}

	protected virtual void UpdateBond()
	{
		var builder = new MeshBuilder();
		builder.AddCylinder(Bond.Atom1.Location.ToPoint3D(), Bond.Atom2.Location.ToPoint3D(),
		                    IsValid ? .24 : 0.075, 10);
		var brush = Brushes.DarkGray.Clone();
		if (IsValid) brush = new SolidColorBrush(Color);

		Content = new GeometryModel3D(builder.ToMesh(), MaterialHelper.CreateMaterial(brush, 0, 0));
	}
}
