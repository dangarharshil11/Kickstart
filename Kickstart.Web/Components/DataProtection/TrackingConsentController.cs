using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.DataProtection;
using CMS.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kickstart.Web.Components.DataProtection
{
    public class TrackingConsentController : Controller
    {
        private readonly IConsentAgreementService consentAgreementService;
        private readonly ICurrentCookieLevelProvider currentCookieLevelProvider;
        private readonly IInfoProvider<ConsentInfo> consentInfoProvider;

        public TrackingConsentController(IConsentAgreementService consentAgreementService,
                                ICurrentCookieLevelProvider currentCookieLevelProvider,
                                IInfoProvider<ConsentInfo> consentInfoProvider)
        {
            this.consentAgreementService = consentAgreementService;
            this.currentCookieLevelProvider = currentCookieLevelProvider;
            this.consentInfoProvider = consentInfoProvider;
        }

        [HttpPost("Agree")]
        [ValidateAntiForgeryToken]
        public ActionResult Agree(string returnUrl)
        {
            // Gets the related tracking consent
            ConsentInfo consent = consentInfoProvider.Get("SampleTrackingConsent");

            if (consent != null)
            {
                // Sets the visitor's cookie level to 'All' (enables contact tracking)
                currentCookieLevelProvider.SetCurrentCookieLevel(Kentico.Web.Mvc.CookieLevel.All.Level);

                // Gets the current contact and creates a consent agreement
                ContactInfo currentContact = ContactManagementContext.GetCurrentContact();

                if (currentContact != null)
                {
                    consentAgreementService.Agree(currentContact, consent);
                }

                return Redirect(returnUrl);
            }

            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }
    }
}
