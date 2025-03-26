using Kentico.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Kickstart;
using Kickstart.Web.Features.Navigation;
using Kentico.Membership;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Collections.Generic;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookieLevelOptions>(options =>
{
    options.CookieConfigurations.Add("CustomCookie", CookieLevel.Essential);
});

// Enable desired Kentico Xperience features
builder.Services.AddKentico(features =>
{
    features.UsePageBuilder(new PageBuilderOptions
    {
        ContentTypeNames = [
            LandingPage.CONTENT_TYPE_NAME,
        ]
    });
    // features.UseActivityTracking();
    features.UseWebPageRouting();
    // features.UseEmailStatisticsLogging();
    // features.UseEmailMarketing();
});

builder.Services.AddAuthentication();
// builder.Services.AddAuthorization();

builder.Services.AddAdminExternalAuthenticationProvider(
	builder => builder.AddMicrosoftIdentityWebApp(options =>
	{
		// Your Entra ID tenant
		options.Domain = "";
		// Your tenant ID - found on the Overview tab of your application registration in Entra ID
		options.TenantId = "e27f54ab-9525-4f1a-b4af-899ab5654027";
		// Your application ID - found on the Overview tab of your application registration in Entra ID
		options.ClientId = "e81962d0-4ef9-4d10-9aea-58896aa0c146";
		// The authorization server to use (login.microsoftonline.com unless using a custom one)
		options.Instance = "https://login.microsoftonline.com/";
		// The callback path must match the redirect URI specified when configuring the application in Entra ID
		options.CallbackPath = new PathString("/admin-oidc");
		// Sets the response type for the Authorization Code Flow
		options.ResponseType = OpenIdConnectResponseType.Code;

		// Add the credentials used to prove your application's identity,
		// either a certificate OR a client secret.

		// Secret ID generated for a client secret
		options.ClientSecret = "client-secret";

		// Certificate
		options.ClientCertificates = new List<CertificateDescription>
		{
            // Replace the examples of certificate descriptions with your own

            // Certificate from Azure Key Vault
            CertificateDescription.FromKeyVault("https://keyvault-name.vault.azure.net/", "certificate-name"),
            // Certificate from a local file
           CertificateDescription.FromPath("path_to_certificate.pfx","certificate_password")
		};
	}),
	options =>
	{
		// Sets cookies as the sign in scheme
		options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		// Sets OIDC as the authentication method
		options.AuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
	}
);

builder.Services.Configure<AdminIdentityOptions>(options =>
{
	options.AuthenticationOptions.Mode = AdminAuthenticationMode.MaintainForms;
});

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<INavigationService, NavigationService>();

var app = builder.Build();
app.InitKentico();

app.UseStaticFiles();

app.UseCookiePolicy();

app.UseAuthentication();


app.UseKentico();

// app.UseAuthorization();

app.Kentico().MapRoutes();

app.Run();
