using Microsoft.AspNetCore.Components;

namespace Opteamix.AuthorizationFramework.BlazorApp.Shared
{
    public partial class PagingComponent
    {
        [Parameter]
        public int CurrentPage { get; set; }

        [Parameter]
        public int TotalPages { get; set; }

        [Parameter]
        public EventCallback onFirstPageClicked { get; set; }

        [Parameter]
        public EventCallback onLastPageClicked { get; set; }

        [Parameter]
        public EventCallback onPreviousPageClicked { get; set; }

        [Parameter]
        public EventCallback onNextPageClicked { get; set; }

        protected async Task FirstPageClicked()
        {
            await onFirstPageClicked.InvokeAsync();
            StateHasChanged();
        }

        protected async Task LastPageClicked()
        {
            await onLastPageClicked.InvokeAsync();
        }

        protected async Task PreviousPageClicked()
        {
            await onPreviousPageClicked.InvokeAsync();
            StateHasChanged();
        }

        protected async Task NextPageClicked()
        {
            await onNextPageClicked.InvokeAsync();
            StateHasChanged();
        }
    }
}
