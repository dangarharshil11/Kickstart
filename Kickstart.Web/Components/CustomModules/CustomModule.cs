using CMS;
using CMS.DataEngine;
using CMS.IO;
using Kentico.Xperience.AzureStorage;
using Kickstart.Web.Components.CustomModules;

[assembly: RegisterModule(typeof(CustomModule))]

namespace Kickstart.Web.Components.CustomModules
{
    public class CustomModule : Module
    {
        public CustomModule()
        : base("CustomInit")
        {
        }

        // Contains initialization code that is executed when the application starts
        protected override void OnInit()
        {
            base.OnInit();

            // Creates a new StorageProvider instance for Azure Blob storage
            var assetsProvider = AzureStorageProvider.Create();

            // Specifies the target container, the provider ensures its existence in the storage account
            assetsProvider.CustomRootPath = "main";

            // Makes the 'myassetscontainer' container publicly accessible
            assetsProvider.PublicExternalFolderObject = true;

            // Maps the local directory to the storage provider
            StorageHelper.MapStoragePath("~/assets", assetsProvider);
        }
    }
}
