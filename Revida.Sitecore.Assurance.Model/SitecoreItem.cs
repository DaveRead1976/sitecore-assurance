using System;

namespace Revida.Sitecore.Assurance.Model
{
    public class SitecoreItem
    {
        public Guid ItemID { get; set; }

        public string ItemName { get; set; }

        public string ItemPath { get; set; }

        public bool HasChildren { get; set; }

        public string DisplayName { get; set; }

        public string ItemUrl { get; set; }
    }
}
