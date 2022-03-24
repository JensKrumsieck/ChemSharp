using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChemSharp.Molecules.Blazor;

public class BlazorAtom : ComponentBase
{
	[Parameter] public Atom? Atom { get; set; }

	[Inject] public IJSRuntime JS { get; set; }

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (Atom == null) return;

		await JS.InvokeVoidAsync("chemsharpMolecules.addAtom", Atom.Title, Atom.Symbol,
		                         Atom.Location.X, Atom.Location.Y, Atom.Location.Z,
		                         Atom.CovalentRadius ?? 100, Atom.Color);
	}
}
