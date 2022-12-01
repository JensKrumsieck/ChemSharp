using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace ChemSharp.Molecules.HelixToolkit;

public class Atom3D : ModelVisual3D
{
	private bool _isSelected;

	//start with all true
	private bool _isValid = true;

	public Atom3D(Atom atom)
	{
		var builder = new MeshBuilder();
		builder.AddSphere(atom.Location.ToPoint3D(), atom.CovalentRadius / 200d);
		var brush = new BrushConverter().ConvertFromString(atom.Color) as Brush;
		Atom = atom;
		Content = new GeometryModel3D(builder.ToMesh(), MaterialHelper.CreateMaterial(brush, 0, 0));
	}

	public Atom Atom { get; set; }

	public bool IsSelected
	{
		get => _isSelected;
		set
		{
			_isSelected = value;
			UpdateColor();
		}
	}

	public bool IsValid
	{
		get => _isValid;
		set
		{
			_isValid = value;
			UpdateColor();
		}
	}

	protected virtual void UpdateOpacity(Brush brush) => brush.Opacity = IsValid ? 1 : .25f;

	protected virtual void UpdateColor()
	{
		var brush = new BrushConverter().ConvertFromString(Atom.Color) as Brush;
		UpdateOpacity(brush!);
		((GeometryModel3D)Content).Material =
			MaterialHelper.CreateMaterial(IsSelected ? Brushes.LightGoldenrodYellow : brush, 0, 0);
	}

	public override string ToString() => Atom + (IsSelected ? " SelectedAtom" : "");
}
