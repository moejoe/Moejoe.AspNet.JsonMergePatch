using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Moejoe.AspNet.JsonMergePatch.Tests.Validation
{
    public enum Enumeration
    {
        [EnumMember(Value = "first")]
        FirstElement,
        [EnumMember(Value = "second")]
        SecondElement,
        [EnumMember(Value = "third")]
        ThirdElement

    }

    public class ValidationSubClass
    {
        [JsonProperty("dateValue")]
        public DateTime DateValue { get; set; }

        [JsonProperty("intValue")]
        public int IntegralValue { get; set; }


        [Required(AllowEmptyStrings = false)]
        [JsonProperty("requiredProperty")]
        public string RequiredProperty { get; set; }
        
        [StringLength(10, MinimumLength = 3)]
        [JsonProperty("stringValue")]
        public string StringValue { get; set; }

        public bool Equals(SubClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DateValue.Equals(other.DateValue) && IntegralValue == other.IntegralValue &&
                   string.Equals(StringValue, other.StringValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SubClass)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DateValue.GetHashCode();
                hashCode = (hashCode * 397) ^ IntegralValue;
                hashCode = (hashCode * 397) ^ (StringValue != null ? StringValue.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
    public class ValidationClass : IEquatable<ValidationClass>
    {
        [JsonProperty("complexCollection")]
        public List<SubClass> ComplexCollection { get; set; }

        [JsonProperty("component")]
        public ValidationSubClass Component { get; set; }

        [JsonProperty("dateValue")]
        public DateTime DateValue { get; set; }

        [JsonIgnore]
        public string IgnoredProperty { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }


        [JsonConverter(typeof(StringEnumConverter))]
        public Enumeration Enumeration { get; set; }

        [JsonProperty("intValue")]
        public int IntegralValue { get; set; }

        [JsonProperty("nullableProperty")]
        public bool? NullableProperty { get; set; }

        [JsonProperty("primCollection")]
        public List<string> PrimitiveList { get; set; }


        [JsonProperty("readOnlyProperty")]
        public string ReadOnlyValue { get; set; }

        [JsonProperty("stringValue")]
        [StringLength(10, MinimumLength = 3)]
        public string StringValue { get; set; }


        public bool Equals(ValidationClass other)
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
            return Equals((ValidationClass)obj);
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