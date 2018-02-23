using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core.Tests
{
    public class ComplexTypeClass : IEquatable<ComplexTypeClass>
    {
        [JsonProperty("complexCollection")]
        public List<SubClass> ComplexCollection { get; set; }

        [JsonProperty("component")]
        public SubClass Component { get; set; }

        [JsonProperty("dateValue")]
        public DateTime DateValue { get; set; }

        [JsonIgnore]
        public string IgnoredProperty { get; set; }

        [JsonProperty("intValue")]
        public int IntegralValue { get; set; }

        [JsonProperty("nullableProperty")]
        public bool? NullableProperty { get; set; }

        [JsonProperty("primCollection")]
        public List<string> PrimitiveList { get; set; }


        [JsonProperty("readOnlyProperty")]
        public string ReadOnlyValue { get; set; }

        [JsonProperty("stringValue")]
        public string StringValue { get; set; }


        public bool Equals(ComplexTypeClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DateValue.Equals(other.DateValue) && IntegralValue == other.IntegralValue &&
                   string.Equals(StringValue, other.StringValue) && NullableProperty == other.NullableProperty &&
                   Equals(Component, other.Component) &&
                   (PrimitiveList != null && PrimitiveList.SequenceEqual(other.PrimitiveList) ||
                    PrimitiveList == null && other.PrimitiveList == null) &&
                   (ComplexCollection != null && ComplexCollection.SequenceEqual(other.ComplexCollection) ||
                    ComplexCollection == null && other.ComplexCollection == null);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ComplexTypeClass) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DateValue.GetHashCode();
                hashCode = (hashCode * 397) ^ (IgnoredProperty != null ? IgnoredProperty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IntegralValue;
                hashCode = (hashCode * 397) ^ (StringValue != null ? StringValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ NullableProperty.GetHashCode();
                hashCode = (hashCode * 397) ^ (Component != null ? Component.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PrimitiveList != null ? PrimitiveList.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ComplexCollection != null ? ComplexCollection.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return JObject.FromObject(this).ToString();
        }
    }
}