using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core
{
    /// <summary>
    /// Representation of an RFC 7386 PatchDocument for the given Resource Type.
    /// </summary>
    /// <typeparam name="TResource">The Type of the Resource.</typeparam>
    public class JsonMergePatchDocument<TResource> where TResource : class
    {
        private readonly JsonSerializer _serializer;
        private readonly JToken _patch;

        public JsonMergePatchDocument(string jsonPatchDocument)
        {
            if (typeof(TResource).GetInterface(nameof(IEnumerable)) != null)
            {
                throw new NotSupportedException($"Collections are not supported by JsonMergePatchDocument resource, since the rfc specifications states, that collections are to be replaced.");
            }
            _serializer = JsonSerializer.Create();
            _patch = JToken.Parse(jsonPatchDocument);
        }

        public void ApplyPatch(TResource original)
        {
            var nullValueHandling = _serializer.NullValueHandling;
            var orig = JToken.FromObject(original, _serializer);

            if (orig.Type != JTokenType.Object) return;
            var jOrig = (JObject)orig;
            jOrig.Merge(_patch, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace, MergeNullValueHandling = MergeNullValueHandling.Merge });
            _serializer.NullValueHandling = NullValueHandling.Include;
            _serializer.Populate(jOrig.CreateReader(), original);
            _serializer.NullValueHandling = nullValueHandling;
        }
    }
}
