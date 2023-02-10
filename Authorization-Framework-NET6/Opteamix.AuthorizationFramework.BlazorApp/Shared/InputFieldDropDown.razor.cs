using Microsoft.AspNetCore.Components;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Dropdown;
using System.Collections.Specialized;

namespace Opteamix.AuthorizationFramework.BlazorApp.Shared
{
    public partial class InputFieldDropDown
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Width { get; set; }

        [Parameter]
        public string Height { get; set; }

        [Parameter]
        public string FieldName { get; set; }

        [Parameter]
        public string Input { get; set; }

        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public EventCallback<string> InputChanged { get; set; }

        private Task OnInputChanged(ChangeEventArgs e)
        {
            Input = e.Value.ToString();
            return InputChanged.InvokeAsync(Input);
        }

        [Parameter]
        public List<Dropdown> DropdownList { get; set; } = new List<Dropdown>();

    }
}
