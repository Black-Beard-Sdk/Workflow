using System.Collections.Generic;

namespace Bb.Workflows.Models.Configurations
{
    public class InitializationOnEventConfig
    {

        public string EventName { get; set; }

        public List<InitializationConfig> Switchs { get; set; } = new List<InitializationConfig>();
        public bool Recursive { get; set; }
    }

}
