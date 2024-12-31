using CMS.DataProtection;
using System.Collections.Generic;
using System.Linq;

namespace Kickstart.Web.Components.DataProtection
{
    public class ConsentListingModel
    {
        public IEnumerable<Consent> Consents { get; set; } = Enumerable.Empty<Consent>();
    }
}
