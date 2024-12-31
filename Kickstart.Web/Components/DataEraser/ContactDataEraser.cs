using CMS.Activities;
using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.DataProtection;
using CMS.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Kickstart.Web.Components.DataEraser
{
    public class ContactDataEraser : IPersonalDataEraser
    {
        // Stores instances of services for managing activities and contacts
        private readonly IInfoProvider<ActivityInfo> activityInfoProvider;
        private readonly IInfoProvider<ContactInfo> contactInfoProvider;

        public ContactDataEraser(IInfoProvider<ActivityInfo> activityInfoProvider, IInfoProvider<ContactInfo> contactInfoProvider)
        {
            this.activityInfoProvider = activityInfoProvider;
            this.contactInfoProvider = contactInfoProvider;
        }

        public void Erase(IEnumerable<BaseInfo> identities, IDictionary<string, object> configuration)
        {
            // Gets all contact objects added by registered IIdentityCollector implementations
            var contacts = identities.OfType<ContactInfo>();

            // Does nothing if no contacts were collected
            if (!contacts.Any())
            {
                return;
            }

            // Gets a list of identifiers for the contacts
            List<int> contactIds = contacts.Select(c => c.ContactID).ToList();

            // Deletes the activities of the given contacts (if enabled in the configuration)
            DeleteActivities(contactIds, configuration);

            // Deletes the given contacts (if enabled in the configuration)
            // Also automatically deletes activities of the given contacts (contacts are parent objects of activities)
            DeleteContacts(contacts, configuration);
        }

        private void DeleteActivities(List<int> contactIds, IDictionary<string, object> configuration)
        {
            // Checks whether deletion of activities is enabled in the configuration options
            object deleteActivities;
            if (configuration.TryGetValue("DeleteActivities", out deleteActivities)
                && ValidationHelper.GetBoolean(deleteActivities, false))
            {
                // Deletes the activities of the specified contacts
                // The system may contain a very large number of activity records, so the example uses the BulkDelete API
                activityInfoProvider.BulkDelete(new WhereCondition().WhereIn("ActivityContactID", contactIds));
            }
        }

        private void DeleteContacts(IEnumerable<ContactInfo> contacts, IDictionary<string, object> configuration)
        {
            // Checks whether deletion of contacts is enabled in the configuration options
            object deleteContacts;
            if (configuration.TryGetValue("DeleteContacts", out deleteContacts)
                && ValidationHelper.GetBoolean(deleteContacts, false))
            {
                // Deletes the specified contacts
                foreach (ContactInfo contact in contacts)
                {
                    contactInfoProvider.Delete(contact);
                }
            }
        }
    }
}
