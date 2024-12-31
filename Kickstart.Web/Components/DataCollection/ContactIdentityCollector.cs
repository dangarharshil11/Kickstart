using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kickstart.Web.Components.DataCollection
{
    public class ContactIdentityCollector : IIdentityCollector
    {
        // Stores an instance of the service for managing contacts
        private readonly IInfoProvider<ContactInfo> contactInfoProvider;

        public ContactIdentityCollector(IInfoProvider<ContactInfo> contactInfoProvider)
        {
            this.contactInfoProvider = contactInfoProvider;
        }

        public void Collect(IDictionary<string, object> dataSubjectFilter, List<BaseInfo> identities)
        {
            // Does nothing if the identifier inputs do not contain the "email" key or if its value is empty
            if (!dataSubjectFilter.ContainsKey("email"))
            {
                return;
            }
            string? email = dataSubjectFilter["email"] as string;
            if (String.IsNullOrWhiteSpace(email))
            {
                return;
            }

            // Finds contacts with a matching email address
            List<ContactInfo> contacts = contactInfoProvider.Get()
                                                .WhereEquals(nameof(ContactInfo.ContactEmail), email)
                                                .ToList();

            // Adds the matching contact objects to the list of collected identities
            identities.AddRange(contacts);
        }
    }
}
