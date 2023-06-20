using ChemSharp.Molecules.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChemSharp.Molecules.Blazor;

public class BlazorBond : ComponentBase
{
	[Parameter] public Bond? Bond { get; set; }

	[Inject] public IJSRuntime JS { get; set; }

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (Bond == null) return;

		var (loc, matrix) = Bond.CalculateRotation();
		await JS.InvokeVoidAsync("chemsharpMolecules.addBond", loc.X, loc.Y, loc.Z, matrix.X, matrix.Y, matrix.Z,
		                         matrix.W, Bond.Length);
	}
}
