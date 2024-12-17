using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Kickstart.Web.Components.Sections.CustomSection
{
    public class CustomSectionProperties : ISectionProperties
    {
        // Defines a property and sets its default value
        // Assigns the default Xperience text input component, which allows users to enter
        // a string value for the property in the section's configuration dialog
        [TextInputComponent(Order = 0, Label = "Color")]
        public string Color { get; set; } = "yellow";

        [NumberInputComponent(Order = 0, Label = "Number of Columns")]
        public int Number { get; set; } = 1;
    }
}
