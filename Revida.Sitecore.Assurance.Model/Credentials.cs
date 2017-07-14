using System;

namespace Revida.Sitecore.Assurance.Model
{
    [Serializable]
    public class Credentials
    {
        public string Domain { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
