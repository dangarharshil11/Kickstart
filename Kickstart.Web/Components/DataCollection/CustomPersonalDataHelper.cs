using CMS.DataProtection;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

namespace Kickstart.Web.Components.DataCollection
{
    public class CustomPersonalDataHelper : PersonalDataHelper
    {
        // Customizes the output of personal data in machine-readable format
        // Replaces the default <PersonalData> XML root tag with curly brackets for JSON data
        protected override string JoinPersonalDataInternal(IEnumerable<string> personalData, string outputFormat)
        {
            // Performs custom handling for personal data in machine-readable format
            if (outputFormat.Equals(PersonalDataFormat.MACHINE_READABLE, StringComparison.OrdinalIgnoreCase))
            {
                string indentation = "  ";
                string indentedNewLine = Environment.NewLine + indentation;

                var resultBuilder = new StringBuilder();

                // Adds the opening bracket to the result
                resultBuilder.AppendLine("{");
                resultBuilder.Append(indentation);

                // Updates all new line characters in the returned personal data to include the added indentation
                var modifiedPersonalData = personalData.Select(data => data.Replace(Environment.NewLine, indentedNewLine));

                // Adds the data provided by all registered personal data collectors
                resultBuilder.AppendLine(String.Join(indentedNewLine, modifiedPersonalData));

                // Adds the closing bracket to the result
                resultBuilder.Append("}");

                return resultBuilder.ToString();
            }

            // Runs the default method for human-readable or undefined output formats
            return base.JoinPersonalDataInternal(personalData, outputFormat);
        }
    }
}
