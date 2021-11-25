using ChemSharp.Molecules.Blazor.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace ChemSharp.Molecules.Blazor
{
    public class BlazorMolecule : ComponentBase
    {
        [Inject]
        public IJSRuntime JS { get; set; }

        [Parameter]
        public Molecule? Molecule { get; set; }

        private Guid? _guid;

        public BlazorMolecule()
        {
            _guid = Guid.NewGuid();
        }

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

            builder.OpenElement(1, "canvas");
            builder.AddAttribute(2, "id", "three-" + _guid);

            builder.CloseElement();
            builder.CloseElement();

            if (Molecule != null)
            {
                foreach (var bond in Molecule.Bonds)
                {
                    builder.OpenComponent<BlazorBond>(3);
                    builder.AddAttribute(4, "Bond", bond);
                    builder.CloseComponent();
                }
                //build molecule render tree
                foreach (var atom in Molecule.Atoms)
                {
                    builder.OpenComponent<BlazorAtom>(5);
                    builder.AddAttribute(6, "Atom", atom);
                    builder.CloseComponent();
                }
            }
        }
    }
}
