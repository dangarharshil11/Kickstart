using CMS.Base;
using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.DataProtection;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Kickstart.Web.Components.DataProtection
{
    public class PrivacyPageController : Controller
    {
        private readonly IConsentAgreementService consentAgreementService;
        private readonly IInfoProvider<ConsentInfo> consentInfoProvider;
        private readonly IPreferredLanguageRetriever preferredLanguageRetriever;

        public PrivacyPageController(IConsentAgreementService consentAgreementService,
                                     IInfoProvider<ConsentInfo> consentInfoProvider,
                                     IPreferredLanguageRetriever preferredLanguageRetriever)
        {
            this.consentAgreementService = consentAgreementService;
            this.consentInfoProvider = consentInfoProvider;
            this.preferredLanguageRetriever = preferredLanguageRetriever;
        }

        /// <summary>
        /// Loads and displays consents for which visitors have given agreements.
        //  / </summary>
        [Route("PrivacyPage")]
        public ActionResult Index()
        {
            // Gets the current visitor's contact
            ContactInfo currentContact = ContactManagementContext.GetCurrentContact();

            var consentListingModel = new ConsentListingModel();

            // Does not attempt to load consent data if the current contact is not available
            // This occurs if contact tracking is disabled, or for visitors who have not given an agreement with the tracking consent
            if (currentContact != null)
            {
                // Gets all consents for which the current contact has given an agreement
                consentListingModel.Consents = consentAgreementService.GetAgreedConsents(currentContact);
            }

            return View("PrivacyPage", consentListingModel);
        }

        /// <summary>
        /// Display details of the specified consent.
        /// </summary>
        [Route("ConsentDetails")]
        public ActionResult ConsentDetails(int Id)
        {
            // Gets a list of consents for which the current contact has given an agreement
            ContactInfo currentContact = ContactManagementContext.GetCurrentContact();
            IEnumerable<Consent> consents = consentAgreementService.GetAgreedConsents(currentContact);

            // Gets the consent matching the identifier for which the details were requested
            // Using this approach to get objects of the 'Consent' class is necessary to ensure that the correct consent text
            // is displayed, either from the current consent text or the archived consent version for which the agreement was given
            Consent consent = consents.FirstOrDefault(c => c.Id == Id);

            // Displays the privacy page (consent list) if the specified consent identifier is not valid
            if (consent == null)
            {
                return View("PrivacyPage");
            }

            // Gets the consent text in the currently selected language
            ConsentText consentText = consent.GetConsentText(preferredLanguageRetriever.Get());

            // Sets the consent's details into the view model
            var model = new ConsentDetailsModel
            {
                ConsentDisplayName = consent.DisplayName,
                ConsentShortText = consentText.ShortText,
                ConsentFullText = consentText.FullText
            };

            return View("ConsentDetails", model);
        }

        /// <summary>
        /// Revokes the current contact's agreement with the specified consent.
        /// </summary>
        [HttpPost("Revoke")]
        [ValidateAntiForgeryToken]
        public ActionResult Revoke(string consentId)
        {
            // Gets the related consent object
            ConsentInfo consent = consentInfoProvider.Get(consentId.ToInteger(0));

            // Gets the current visitor's contact
            ContactInfo currentContact = ContactManagementContext.GetCurrentContact();

            if (consent != null && currentContact != null)
            {
                // Revokes the consent agreement
                consentAgreementService.Revoke(currentContact, consent);
            }

            return RedirectToAction("Index");
        }
    }
}
