using System.Collections.Generic;

namespace mediastore.Models.DataModels
{
    public class Storagefile
    {
        public string Name { get; set; }
                
        public string ContentType { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}