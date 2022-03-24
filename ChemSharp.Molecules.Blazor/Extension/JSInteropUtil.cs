using Microsoft.JSInterop;

namespace ChemSharp.Molecules.Blazor.Extension;

public static class JSInteropUtil
{
	/// <summary>
	///     Inits ThreeJS Canvas
	/// </summary>
	/// <param name="JS"></param>
	/// <param name="querySelector"></param>
	/// <returns></returns>
	public static async Task InitThreeJSAsync(this IJSRuntime JS, string querySelector = "three-webgl")
		=> await JS.InvokeVoidAsync("chemsharpMolecules.init", querySelector);

	public static async Task ClearCanvas(this IJSRuntime JS) =>
		await JS.InvokeVoidAsync("chemsharpMolecules.clearCanvas");
}
