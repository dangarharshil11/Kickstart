using System;

using CMS;
using CMS.Core;
using CMS.DataEngine;
using CMS.DataProtection;
using CMS.Helpers;
using Microsoft.Extensions.DependencyInjection;

internal class CustomDataProtectionModule : Module
{
    private ICurrentCookieLevelProvider currentCookieLevelProvider;

    // Module class constructor, the system registers the module under the name "CustomDataProtection"
    public CustomDataProtectionModule()
        : base("CustomDataProtection")
    {
    }

    // Contains initialization code that is executed when the application starts
    protected override void OnInit(ModuleInitParameters parameters)
    {
        // Resolves service dependencies from the IoC container
        currentCookieLevelProvider = parameters.Services.GetRequiredService<ICurrentCookieLevelProvider>();

        // Assigns a handler to the RevokeConsentAgreement event
        DataProtectionEvents.RevokeConsentAgreement.Execute += RevokeConsentHandler;
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