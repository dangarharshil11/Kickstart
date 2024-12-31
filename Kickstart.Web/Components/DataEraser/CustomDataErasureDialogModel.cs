using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.Xperience.Admin.Base.Forms;
using Kentico.Xperience.Admin.DigitalMarketing;
using System.Threading.Tasks;

namespace Kickstart.Web.Components.DataEraser
{
    public class CustomDataErasureDialogModel : IDataErasureDialogModel
    {
        // Erasure dialog onfiguration properties and their editing components
        [CheckBoxComponent(Label = "Delete contacts", Order = 1)]
        public bool DeleteContacts { get; set; }

        [CheckBoxComponent(Label = "Delete activities", Order = 2)]
        public bool DeleteActivities { get; set; }

        // Validates the output of the dialog
        public virtual async Task<ValidationResult> Validate()
        {
            if (DeleteContacts || DeleteActivities)
            {
                return await ValidationResult.SuccessResult();
            }

            return new ValidationResult(isValid: false, errorMessage: "At least one item needs to be selected!");
        }
    }
}
