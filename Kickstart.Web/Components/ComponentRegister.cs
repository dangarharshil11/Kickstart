using CMS;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base;
using Kickstart.Web.Components.DataCollection;
using Kickstart.Web.Components.DataEraser;
using Kickstart.Web.Components.Sections.CustomSection;
using Kickstart.Web.Components.Widgets.ExtendedFormWidget;
using Kickstart.Web.Components.Widgets.NumberWidget;

// Sections
[assembly: RegisterSection("Kickstart.Sections.CustomSection",
                           "Custom section",
                           typeof(CustomSectionProperties),
                           customViewName: "~/Components/Sections/CustomSection/_CustomSection.cshtml",
                           IconClass = "icon-square")]


// Widgets
[assembly: RegisterWidget("Kickstart.Web.Components.Widgets.NumberWidget",
                         "Number selector",
                         typeof(NumberWidgetProperties),
                         customViewName: "~/Components/Widgets/NumberWidget/_NumberWidget.cshtml")]
[assembly: RegisterWidget("Kickstart.Web.Components.Widgets.ExtendedFormWidget",
                          "Extended form",
                          typeof(ExtendedFormWidgetProperties),
                          customViewName: "~/Components/Widgets/ExtendedFormWidget/_ExtendedFormWidget.cshtml",
                          IconClass = "icon-form")]


// Registers the custom module into the system
[assembly: RegisterModule(typeof(CustomDataProtectionModule))]

// Registers the CustomPersonalDataHelper class to replace the default PersonalDataHelper
[assembly: RegisterCustomHelper(typeof(CustomPersonalDataHelper))]

// Registers the Extender to extend CustomDataErasureDialogModel configuration dialog
[assembly: PageExtender(typeof(RightToBeForgottenExtender))]
