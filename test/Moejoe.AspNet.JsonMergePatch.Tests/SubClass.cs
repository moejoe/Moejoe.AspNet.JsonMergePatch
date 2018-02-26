using System;
using Newtonsoft.Json;

namespace Moejoe.AspNet.JsonMergePatch.Tests
{
    public class SubClass : IEquatable<SubClass>
    {
        [JsonProperty("dateValue")]
        public DateTime DateValue { get; set; }

        [JsonProperty("intValue")]
        public int IntegralValue { get; set; }

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
            return Equals((SubClass) obj);
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
}