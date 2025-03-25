using CMS.ContactManagement;
using CMS.Core;
using CMS.DataProtection;
using Kentico.PageBuilder.Web.Mvc.Personalization;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kickstart.Web.Components.PageBuilder.PersonalizationConditions.Custom;


[assembly: RegisterPersonalizationConditionType(
	identifier: "Kickstart.Web.Components.PageBuilder.PersonalizationConditions.Custom",
	type: typeof(CustomPersonalization),
	name: "Has given consent agreement",
	Description = "Evaluates whether the contact has given an agreement with a specified consent declaration.",
	IconClass = "icon-clipboard-checklist",
	Hint = "Enter the code name of a consent. The condition is fulfilled for visitors who have given an agreement with the given consent.")]

namespace Kickstart.Web.Components.PageBuilder.PersonalizationConditions.Custom
{
	public class CustomPersonalization : ConditionType
	{
		[TextInputComponent(Order = 0, Label = "Consent code name")]
		public string ConsentCodeName { get; set; }

		/// <summary>
		/// Default property representing the name of the personalization variant.
		/// </summary>
		public override string VariantName
		{
			get
			{
				// Uses the specified consent code name as the name of the variant
				return ConsentCodeName;
			}
			set
			{
				// No need to set the variant name property
			}
		}

		public override bool Evaluate()
		{
			// Gets the contact object of the current visitor
			ContactInfo currentContact = ContactManagementContext.GetCurrentContact(false);

			// Creates an instance of the consent agreement service
			var consentAgreementService = Service.Resolve<IConsentAgreementService>();

			// Gets the consent object based on its code name
			ConsentInfo consent = ConsentInfo.Provider.Get(ConsentCodeName);
			if (consent == null || currentContact == null)
			{
				return false;
			}

			// Checks if the contact has given a consent agreement
			return consentAgreementService.IsAgreed(currentContact, consent);
		}
	}
}