using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Kickstart.Web.Components.Widgets.NumberWidget
{
    public class NumberWidgetProperties : IWidgetProperties
    {
        // Defines a property and sets its default value
        // Assigns the default Xperience number input component, which allows users to enter
        // a numeric value for the property in the widget's configuration dialog
        [NumberInputComponent(Order = 0, Label = "Number")]
        public int Number { get; set; } = 22;
    }
}
