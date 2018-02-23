using System;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core.Tests.RfcComplianceTests
{
    public class UnsupportedJsonMergePatchTestCase<TTargetClass> where TTargetClass : class
    {
        public TTargetClass Original { get; set; }
        public string Patch { get; set; }
        public Type ExpectedException { get; set; }
    }
    public class JsonMergePatchTestCase<TTargetClass> where TTargetClass : class
    {
        public TTargetClass Original { get; set; }
        public string Patch { get; set; }
        public TTargetClass ExpectedResult { get; set; }
    }

    public class SimpleClass : IEquatable<SimpleClass>
    {
        public string A { get; set; }
        public string B { get; set; }

        public bool Equals(SimpleClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(A, other.A) && string.Equals(B, other.B);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SimpleClass) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((A != null ? A.GetHashCode() : 0) * 397) ^ (B != null ? B.GetHashCode() : 0);
            }
        }
    }

    public class SubClass : IEquatable<SubClass>
    {
        public string B { get; set; }
        public string C { get; set; }

        public SimpleClass D { get; set; }

        public bool Equals(SubClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(B, other.B) && string.Equals(C, other.C) && Equals(D, other.D);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SubClass) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (B != null ? B.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (C != null ? C.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (D != null ? D.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    public class NestedClass: IEquatable<NestedClass>
    {
        public SubClass A { get; set; }

        public bool Equals(NestedClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(A, other.A);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NestedClass) obj);
        }

        public override int GetHashCode()
        {
            return (A != null ? A.GetHashCode() : 0);
        }
    }

    public class SimpleArrayClass
    {
        public string[] A { get; set; }
        public SimpleClass[] B { get; set; }
    }
}
