using Kentico.Forms.Web.Mvc.Widgets;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Kickstart.Web.Components.Widgets.ExtendedFormWidget
{
    public class ExtendedFormWidgetProperties : FormWidgetProperties
    {
        // Defines a property and sets its default value
        // Assigns the default Kentico text input component, which allows users to enter
        // a textual value for the property in the widget's configuration dialog
        [TextInputComponent(Order = 0, Label = "Heading text")]
        public string HeadingText { get; set; } = "Extending widget works";
    }
}
