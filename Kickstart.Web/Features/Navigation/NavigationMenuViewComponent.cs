using System.Linq;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.Websites.Routing;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace Kickstart.Web.Features.Navigation
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly INavigationService navigationService;

        private readonly IPreferredLanguageRetriever preferredLanguageRetriever;
        private readonly IWebsiteChannelContext webSiteChannelContext;
        private readonly IContentQueryExecutor contentQueryExecutor;

        public NavigationMenuViewComponent(
            INavigationService navigationService, 
            IPreferredLanguageRetriever preferredLanguageRetriever, 
            IWebsiteChannelContext webSiteChannelContext, 
            IContentQueryExecutor contentQueryExecutor)
        {
            this.navigationService = navigationService;
            this.preferredLanguageRetriever = preferredLanguageRetriever;
            this.webSiteChannelContext = webSiteChannelContext;
            this.contentQueryExecutor = contentQueryExecutor;
        }

        public async Task<IViewComponentResult> InvokeAsync(string navigationMenuCodeName)
        {
            // A placeholder representing the logic that retrieves the `NavigationMenu` item from the database.
            var menu = await RetrieveMenu(navigationMenuCodeName);

            if (menu == null)
            {
                // We will define this view in the next step.
                return View("~/Features/Navigation/NavigationMenuViewComponent.cshtml", new NavigationMenuViewModel());
            }

            var model = await navigationService.GetNavigationMenuViewModel(menu);

            return View("~/Features/Navigation/NavigationMenuViewComponent.cshtml", model);
        }

        private async Task<NavigationMenu> RetrieveMenu(string navigationMenuCodeName)
        {
            var builder = new ContentItemQueryBuilder()
                .ForContentType(NavigationMenu.CONTENT_TYPE_NAME,
                config => config
                    .Where(where => where.WhereEquals(nameof(NavigationMenu.NavigationMenuCodeName), navigationMenuCodeName))
                    .WithLinkedItems(2))
                .InLanguage(preferredLanguageRetriever.Get());

            var queryExecutorOptions = new ContentQueryExecutionOptions
            {
                ForPreview = webSiteChannelContext.IsPreview
            };

            var items = await contentQueryExecutor.GetMappedResult<NavigationMenu>(builder, queryExecutorOptions);

            return items.FirstOrDefault();
        }
    }
}