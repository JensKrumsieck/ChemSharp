using ChemSharp.Molecules.Blazor.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace ChemSharp.Molecules.Blazor;

public class BlazorMolecule : ComponentBase
{
	private readonly Guid? _guid;

	public BlazorMolecule() => _guid = Guid.NewGuid();

	[Inject] public IJSRuntime JS { get; set; }

	[Parameter] public Molecule? Molecule { get; set; }

	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object> UnmatchedParameters { get; set; }

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender) return;

		await JS.InitThreeJSAsync("three-" + _guid);
	}

	protected override async Task OnParametersSetAsync() => await JS.ClearCanvas();

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		//build canvas tree
		builder.OpenElement(0, "div");
		builder.AddMultipleAttributes(1, UnmatchedParameters);

		builder.OpenElement(2, "canvas");
		builder.AddAttribute(3, "id", "three-" + _guid);

		builder.CloseElement();
		builder.CloseElement();

		if (Molecule != null)
		{
			foreach (var bond in Molecule.Bonds)
			{
				builder.OpenComponent<BlazorBond>(4);
				builder.AddAttribute(5, "Bond", bond);
				builder.CloseComponent();
			}

			//build molecule render tree
			foreach (var atom in Molecule.Atoms)
			{
				builder.OpenComponent<BlazorAtom>(6);
				builder.AddAttribute(7, "Atom", atom);
				builder.CloseComponent();
			}
		}
	}
}
