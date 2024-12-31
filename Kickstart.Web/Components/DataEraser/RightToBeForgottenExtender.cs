using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.DigitalMarketing.UIPages;
using System.Threading.Tasks;

namespace Kickstart.Web.Components.DataEraser
{
    public class RightToBeForgottenExtender : PageExtender<RightToBeForgotten>
    {
        public override Task ConfigurePage()
        {
            // Assigns a custom erasure dialog model to the page configuration
            Page.PageConfiguration.DataErasureDialogModel = new CustomDataErasureDialogModel();

            return base.ConfigurePage();
        }
    }
}
