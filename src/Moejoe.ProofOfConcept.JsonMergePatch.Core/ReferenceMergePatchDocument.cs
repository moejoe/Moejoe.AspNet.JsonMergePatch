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
    internal class ReferenceMergePatchDocument<TResource> : IJsonMergePatchDocument<TResource> where TResource : class
    {
        private readonly JsonSerializer _serializer;
        private readonly JToken _patch;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonPatchDocument"></param>
        /// <param name="settings"></param>
        public ReferenceMergePatchDocument(string jsonPatchDocument, JsonSerializerSettings settings = null)
        {
            if (typeof(TResource).GetInterface(nameof(IEnumerable)) != null)
            {
                throw new NotSupportedException($"Collections are not supported by JsonMergePatchDocument resource, since the rfc specifications states, that collections are to be replaced.");
            }
            settings = settings ?? new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Include;
            _serializer = JsonSerializer.Create(settings);
            _patch = JToken.Parse(jsonPatchDocument);
        }

        /// <summary>
        /// Applies the patch data to a resource.
        /// </summary>
        /// <param name="resource">Instance of the targeted resource to apply the patch to.</param>
        public void ApplyPatch(TResource resource)
        {
            var orig = JToken.FromObject(resource, _serializer);

            if (orig.Type != JTokenType.Object) return;
            var jOrig = (JObject)orig;
            jOrig.Merge(_patch, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace, MergeNullValueHandling = MergeNullValueHandling.Merge });
            
            _serializer.Populate(jOrig.CreateReader(), resource);
        }
    }
}
