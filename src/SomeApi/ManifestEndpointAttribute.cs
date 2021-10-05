using System;

namespace SomeApi.Controllers
{
    public class ManifestEndpointAttribute : Attribute
    {
        public bool Register { get; set; } = true;
        public int ThrottleLimit { get; set; } = 2500;
        public string SomeMore{ get; set; }
        public ManifestEndpointAttribute(bool register, int throttleLimit)
        {
            Register = register;
            ThrottleLimit = throttleLimit;
        }

        public ManifestEndpointAttribute()
        {                       
        }
    }
}


