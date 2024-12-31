using System;
using CMS.Activities;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.DataProtection;
using CMS.Helpers;
using Kickstart.Web.Components.DataCollection;
using Kickstart.Web.Components.DataEraser;
using Microsoft.Extensions.DependencyInjection;

internal class CustomDataProtectionModule : Module
{
    private ICurrentCookieLevelProvider currentCookieLevelProvider;
    private IInfoProvider<ContactInfo> contactInfoProvider;
    private IInfoProvider<ActivityInfo> activityInfoProvider;

    // Module class constructor, the system registers the module under the name "CustomDataProtection"
    public CustomDataProtectionModule()
        : base("CustomDataProtection")
    {
    }

    // Contains initialization code that is executed when the application starts
    protected override void OnInit(ModuleInitParameters parameters)
    {
        // Resolves service dependencies from the IoC container
        base.OnInit();

        contactInfoProvider = Service.Resolve<IInfoProvider<ContactInfo>>();
        activityInfoProvider = Service.Resolve<IInfoProvider<ActivityInfo>>();
        currentCookieLevelProvider = parameters.Services.GetRequiredService<ICurrentCookieLevelProvider>();

        // Assigns a handler to the RevokeConsentAgreement event
        DataProtectionEvents.RevokeConsentAgreement.Execute += RevokeConsentHandler;

        // Adds the ContactIdentityCollector to the collection of registered identity collectors
        IdentityCollectorRegister.Instance.Add(new ContactIdentityCollector(contactInfoProvider));

        // Adds the ContactDataCollector to the collection of registered personal data collectors
        PersonalDataCollectorRegister.Instance.Add(new ContactDataCollector());

        // Adds the ContactDataEraser to the collection of registered personal data erasers
        PersonalDataEraserRegister.Instance.Add(new ContactDataEraser(activityInfoProvider, contactInfoProvider));
    }

    private void RevokeConsentHandler(object? sender, RevokeConsentAgreementEventArgs e)
    {
        // For the tracking consent, lowers the cookie level to the website channel's default in order to disable tracking
        if (e.Consent.ConsentName.Equals("SampleTrackingConsent", StringComparison.OrdinalIgnoreCase))
        {
            currentCookieLevelProvider.SetCurrentCookieLevel(currentCookieLevelProvider.GetDefaultCookieLevel());
        }
    }
}