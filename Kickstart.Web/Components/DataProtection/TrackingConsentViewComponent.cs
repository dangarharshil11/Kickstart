using CMS.DataEngine;
using CMS.DataProtection;
using CMS.Helpers;
using CMS.Websites;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using CMS.ContactManagement;
using System.Threading.Tasks;

namespace Kickstart.Web.Components.DataProtection
{
    public class TrackingConsentViewComponent : ViewComponent
    {

        private readonly IConsentAgreementService consentAgreementService;
        private readonly ICurrentCookieLevelProvider currentCookieLevelProvider;
        private readonly IInfoProvider<ConsentInfo> consentInfoProvider;
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly IPreferredLanguageRetriever preferredLanguageRetriever;

        public TrackingConsentViewComponent(IConsentAgreementService consentAgreementService,
                                          ICurrentCookieLevelProvider currentCookieLevelProvider,
                                          IInfoProvider<ConsentInfo> consentInfoProvider,
                                          IWebPageDataContextRetriever webPageDataContextRetriever,
                                          IWebPageUrlRetriever urlRetriever,
                                          IPreferredLanguageRetriever preferredLanguageRetriever)
        {
            this.consentAgreementService = consentAgreementService;
            this.currentCookieLevelProvider = currentCookieLevelProvider;
            this.consentInfoProvider = consentInfoProvider;
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.urlRetriever = urlRetriever;
            this.preferredLanguageRetriever = preferredLanguageRetriever;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Gets the related tracking consent
            // Fill in the code name of the appropriate consent object in Xperience
            ConsentInfo consent = consentInfoProvider.Get("SampleTrackingConsent");

            if (consent != null)
            {
                // Gets the current contact
                ContactInfo currentContact = ContactManagementContext.GetCurrentContact();

                // Gets the code name of the currently selected language
                var preferredLanguage = preferredLanguageRetriever.Get();

                // Sets the default cookie level for contacts who have revoked the tracking consent
                // Required for scenarios where one contact uses multiple browsers
                if ((currentContact != null) && !consentAgreementService.IsAgreed(currentContact, consent))
                {
                    // Gets the default cookie level for the current channel and sets it for the contact
                    var defaultCookieLevel = currentCookieLevelProvider.GetDefaultCookieLevel();
                    currentCookieLevelProvider.SetCurrentCookieLevel(defaultCookieLevel);
                }

                var consentModel = new TrackingConsentViewModel
                {
                    // Adds the consent's short text to the model
                    ShortText = (await consent.GetConsentTextAsync(preferredLanguage)).ShortText,

                    // Checks whether the current contact has given an agreement for the tracking consent
                    IsAgreed = (currentContact != null) && consentAgreementService.IsAgreed(currentContact, consent),

                    // Adds the current page URL to the model
                    ReturnUrl = webPageDataContextRetriever.TryRetrieve(out var currentWebPageContext)
                        ? (await urlRetriever.Retrieve(currentWebPageContext.WebPage.WebPageItemID, preferredLanguage)).RelativePath
                        // Gets the URL from the request context for pages using custom routing
                        : (HttpContext.Request.PathBase + HttpContext.Request.Path).Value

                };

                // Displays a view with tracking consent information and actions
                return View("~/Components/DataProtection/_TrackingConsent.cshtml", consentModel);
            }

            return Content(string.Empty);
        }
    }
}
