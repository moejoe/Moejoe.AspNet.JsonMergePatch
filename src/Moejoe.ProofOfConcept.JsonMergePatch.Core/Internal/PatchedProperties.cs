using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core
{
    internal class PatchedProperties
    {
        public PatchedProperties()
        {
            Values = new Dictionary<JsonProperty, JValue>();
            SubDocuments = new Dictionary<JsonProperty, PatchDocument>();
            Collections = new Dictionary<JsonProperty, JArray>();
        }
        public Dictionary<JsonProperty, JArray> Collections { get; }
        public Dictionary<JsonProperty, PatchDocument> SubDocuments { get; }
        public Dictionary<JsonProperty, JValue> Values { get; }
    }
}
