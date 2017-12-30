using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core
{
    /// <summary>
    /// Implementation withouth double serialization
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public class JsonMergePatchDocument2<TResource> where TResource : class
    {
        private readonly Dictionary<string, PropertyPresence> _propertyPresenceMap = new Dictionary<string, PropertyPresence>();
        private readonly JsonSerializer _serializer;
        private readonly Dictionary<string, JToken> _value = new Dictionary<string, JToken>();
        private readonly JToken _patch;

        public JsonMergePatchDocument2(string jsonPatchDocument)
        {
            _serializer = JsonSerializer.Create();



            _patch = JToken.Parse(jsonPatchDocument);
            if (_patch.Type == JTokenType.Array)
            {
                throw new NotSupportedException();
            }
            if (_patch.Type != JTokenType.Object)
            {
                throw new NotSupportedException();
            }
            var resolver = _serializer.ContractResolver.ResolveContract(typeof(TResource)) as JsonObjectContract;
            if (resolver == null)
            {
                throw new InvalidOperationException("Expected TResource to be an Object");
            }
            var patchObject = _patch as JObject;
            if (patchObject == null) throw new Exception("Expected Json Object");
            foreach (var prop in patchObject.Properties())
            {
                var targetProp = resolver.Properties.GetClosestMatchProperty(prop.Name);
                if (targetProp != null && !targetProp.Ignored && targetProp.Writable)
                {
                    SetPropertyPresence(targetProp, prop.Value);
                    switch (prop.Value.Type)
                    {
                        case JTokenType.Object:
                            SetSubDocument(targetProp, prop);
                            break;
                        case JTokenType.Array:
                            throw new NotSupportedException("Arrays werden nicht unterstüzt");
                        default:
                            SetPropertyValue(targetProp, prop);
                            break;
                    }

                }
            }
        }
        private void SetSubDocument(JsonProperty targetProp, JProperty prop)
        {
            throw new NotImplementedException();
        }

        private void SetPropertyValue(JsonProperty prop, JToken token)
        {
            _value[prop.PropertyName] = token;
        }

        public TMember GetValue<TMember>(Expression<Func<TResource, TMember>> expression)
        {
            var member = ReflectionHelper.FindProperty(expression);
            if (!_value.ContainsKey(member.Name))
            {
                throw new ArgumentOutOfRangeException(nameof(expression), "The requested member is not part of the PatchMerge document.");
            }
            return _value[member.Name].ToObject<TMember>();
        }

        public JsonMergePatchDocument<TMember> GetSubDocument<TMember>(LambdaExpression expression) where TMember : class
        {
            throw new NotImplementedException();
        }

        private void SetPropertyPresence(JsonProperty prop, JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Null:
                    _propertyPresenceMap[prop.PropertyName] = PropertyPresence.Null;
                    break;
                case JTokenType.Undefined:
                    _propertyPresenceMap[prop.PropertyName] = PropertyPresence.None;
                    break;
                default:
                    _propertyPresenceMap[prop.PropertyName] = PropertyPresence.Value;
                    break;
            }
        }

        public PropertyPresence GetPropertyPresence<TMember>(Expression<Func<TResource, TMember>> expression)
        {
            var member = ReflectionHelper.FindProperty(expression);
            return _propertyPresenceMap.ContainsKey(member.Name)
                ? _propertyPresenceMap[member.Name]
                : PropertyPresence.None;
        }


        public PropertyPresence GetPropertyPresence(LambdaExpression expression)
        {
            var member = ReflectionHelper.FindProperty(expression);
            return _propertyPresenceMap.ContainsKey(member.Name)
                ? _propertyPresenceMap[member.Name]
                : PropertyPresence.None;
        }


        public void ApplyPatch(TResource original)
        {
            foreach (var prop in typeof(TResource).GetFields(BindingFlags.Public).Cast<MemberInfo>().Concat(typeof(TResource).GetProperties()))
            {
                var expression = ReflectionHelper.ToMemberExpression<TResource>(prop);
                switch (GetPropertyPresence(expression))
                {
                    case PropertyPresence.None:
                        break;
                    case PropertyPresence.Null:
                        ApplyNullValue(original, prop);
                        break;
                    case PropertyPresence.Value:
                        ApplyValue(original, prop);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ApplyNullValue(TResource original, MemberInfo member)
        {
            ReflectionHelper.SetMemberValue(member, original, ReflectionHelper.GetNullValue(member));
        }

        private void ApplyValue(TResource original, MemberInfo member)
        {
            throw new NotImplementedException();
            //if (_value.ContainsKey(member.Name))
            //{

            //}
        }

    }
}