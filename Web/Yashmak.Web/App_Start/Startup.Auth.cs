namespace Yashmak.Web
{
    using System;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;

    using Owin;

    using Yashmak.Data;
    using Yashmak.Models;

    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(YashmakDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            var provider = new CookieAuthenticationProvider
                               {
                                   // Enables the application to validate the security stamp when the user logs in.
                                   // This is a security feature which is used when you change a password or add an external login to your account.  
                                   OnValidateIdentity =
                                       SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, AppUser>(
                                           TimeSpan.FromMinutes(30),
                                           (manager, user) => user.GenerateUserIdentityAsync(manager)),
                                   OnException = context => { }
                               };

            app.UseCookieAuthentication(
                new CookieAuthenticationOptions()
                    {
                        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie, 
                        LoginPath = new PathString("/Account/Login"), 
                        Provider = provider
                    });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            // app.UseFacebookAuthentication(appId: "1498636617090099", appSecret: "db0c1261fbb2a401e0ee24164008946a");

            // app.UseGoogleAuthentication(
            // new GoogleOAuth2AuthenticationOptions()
            // {
            // ClientId =
            // "330109337482-slv30e2dnb6tmhpj0769hcmppsjkfai7.apps.googleusercontent.com", 
            // ClientSecret = "Ldn-0UKwjcUn2ig-KVdbkvLF"
            // });

            // app.UseMicrosoftAccountAuthentication(
            // clientId: "",
            // clientSecret: "");

            // app.UseTwitterAuthentication(
            // consumerKey: "",
            // consumerSecret: "");
        }
    }
}