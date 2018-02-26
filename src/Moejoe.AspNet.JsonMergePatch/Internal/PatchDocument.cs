using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Moejoe.AspNet.JsonMergePatch.Internal
{
    internal class PatchDocument
    {
        private readonly JObject _patchPatch;
        
        private readonly PatchedProperties _patchedProperties = new PatchedProperties();
        private readonly JsonSerializer _serializer;
        private readonly Type _targetType;

        public PatchDocument(JObject patchDocument, Type targetType, JsonSerializer serializer = null)
        {
            _patchPatch = patchDocument ?? throw new ArgumentNullException(nameof(patchDocument));
            _targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            _serializer = serializer ?? new JsonSerializer();
            CompilePatchedProperties();
        }

        public void ApplyPatch(object target)
        {
            if (target.GetType() != _targetType)
                throw new ArgumentException($"Expected Object of type '{_targetType.Name}'", nameof(target));
            foreach (var prop in _patchedProperties.Values)
                prop.Key.ValueProvider.SetValue(target, prop.Value.ToObject(prop.Key.PropertyType));
            foreach (var sub in _patchedProperties.SubDocuments)
            {
                var currentValue = sub.Key.ValueProvider.GetValue(target);
                if (currentValue == null)
                    sub.Key.ValueProvider.SetValue(target, sub.Value.ToObject());
                else
                    sub.Value.ApplyPatch(currentValue);
            }
            foreach (var coll in _patchedProperties.Collections)
                coll.Key.ValueProvider.SetValue(target, coll.Value.ToObject(coll.Key.PropertyType));
        }

        private void CompilePatchedProperties()
        {
            if (!(_serializer.ContractResolver.ResolveContract(_targetType) is JsonObjectContract resolver))
                throw new InvalidOperationException(
                    $"Serializer could not provide ContractResolver for target type: '{_targetType.Name}'");
            foreach (var prop in _patchPatch.Properties())
            {
                var targetProp = resolver.Properties.GetClosestMatchProperty(prop.Name);
                if (targetProp == null || targetProp.Ignored || !targetProp.Writable) continue;
                // Create a new PatchDocument for each provided complex Type 
                if (prop.Value.Type == JTokenType.Object)
                    _patchedProperties.SubDocuments.Add(targetProp,
                        new PatchDocument(prop.Value as JObject, targetProp.PropertyType, _serializer));
                else if (prop.Value.Type == JTokenType.Array)
                    _patchedProperties.Collections.Add(targetProp, prop.Value as JArray);
                else
                    _patchedProperties.Values.Add(targetProp, prop.Value as JValue);
            }
        }

        internal JObject Patch => _patchPatch;
        internal object ToObject()
        {
            return _patchPatch.ToObject(_targetType);
        }
    }
}