using System;

namespace Common
{
    public class ManifestEndpointAttribute : Attribute
    {
        public bool Register { get; set; } = true;
        public int ThrottleLimit { get; set; } = 2500;
        
        /// <summary>
        /// Used to resolve conflicts, In case the endpoint exists in other Service, 
        /// </summary>
        public int? EndpointsPriority { get; set; }
        public string SomeMore{ get; set; }
        public ManifestEndpointAttribute(bool register, int throttleLimit)
        {
            Register = register;
            ThrottleLimit = throttleLimit;
        }

        public ManifestEndpointAttribute(bool register, int throttleLimit, int endpointsPriority)
        {
            Register = register;
            ThrottleLimit = throttleLimit;
            EndpointsPriority = endpointsPriority;
        }

        public ManifestEndpointAttribute()
        {                       
        }
    }
}


