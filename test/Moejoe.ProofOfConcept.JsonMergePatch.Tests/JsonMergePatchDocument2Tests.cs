using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core.Tests
{
    [TestFixture]
    internal class JsonMergePatchDocument2Tests
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
            public string ReadOnlyValue => "Readonly";

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


        private static ComplexTypeClass SimpleTestObject => new ComplexTypeClass
        {
            DateValue = DateTime.Now.AddYears(-1),
            IntegralValue = -100,
            StringValue = "oldValue",
            NullableProperty = true
        };


        private static ComplexTypeClass ComplextTestObject => new ComplexTypeClass
        {
            DateValue = DateTime.Parse("2017-02-22T14:21:08.1755454+01:00"),
            IntegralValue = -100,
            StringValue = "oldValue",
            NullableProperty = true,
            Component = new SubClass
            {
                DateValue = DateTime.Parse("2017-02-22T14:21:08.1755454+01:00"),
                IntegralValue = 0,
                StringValue = "oldSubValue"
            }
        };

        private static ComplexTypeClass SimplePatchObject => new ComplexTypeClass
        {
            DateValue = DateTime.Parse("2018-02-22"),
            IntegralValue = 1234,
            StringValue = "Hello",
            NullableProperty = false,
            IgnoredProperty = "I'm not here"
        };

        private static ComplexTypeClass SimplePatchObjectWithNullValue => new ComplexTypeClass
        {
            DateValue = DateTime.Today,
            IntegralValue = 1234,
            StringValue = null,
            NullableProperty = null,
            IgnoredProperty = "I'm not here"
        };

        private static ComplexTypeClass PatchObjectWithComponent => new ComplexTypeClass
        {
            DateValue = DateTime.Parse("2018-02-22"),
            IntegralValue = 1234,
            StringValue = null,
            NullableProperty = null,
            IgnoredProperty = "I'm not here",
            Component = new SubClass
            {
                DateValue = DateTime.MinValue,
                IntegralValue = int.MaxValue,
                StringValue = "6ac5e8ce-ce7d-4e45-a29d-e30a6ab7282d"
            }
        };

        private static ComplexTypeClass PatchObjectWithPrimitiveList => new ComplexTypeClass
        {
            DateValue = DateTime.Parse("2018-02-22"),
            IntegralValue = 1234,
            StringValue = null,
            NullableProperty = null,
            IgnoredProperty = "I'm not here",
            Component = new SubClass
            {
                DateValue = DateTime.MinValue,
                IntegralValue = int.MaxValue,
                StringValue = "6ac5e8ce-ce7d-4e45-a29d-e30a6ab7282d"
            }
            //PrimitiveList = new List<string> { "test1", "test2", "test3" }
        };

        private static ComplexTypeClass PatchObjectWithComplexList => new ComplexTypeClass
        {
            DateValue = DateTime.Parse("2018-02-22"),
            IntegralValue = 1234,
            StringValue = null,
            NullableProperty = null,
            IgnoredProperty = "I'm not here",
            Component = new SubClass
            {
                DateValue = DateTime.MinValue,
                IntegralValue = int.MaxValue,
                StringValue = "6ac5e8ce-ce7d-4e45-a29d-e30a6ab7282d"
            },
            //PrimitiveList = new List<string> { "test1", "test2", "test3" },
            ComplexCollection = new[]
            {
                new SubClass
                {
                    IntegralValue = 123,
                    StringValue = "Test123",
                    DateValue = DateTime.MinValue
                },
                new SubClass
                {
                    IntegralValue = 2345,
                    StringValue = "Test2345",
                    DateValue = DateTime.MaxValue
                },
                new SubClass
                {
                    IntegralValue = int.MinValue,
                    StringValue = string.Empty,
                    DateValue = DateTime.MaxValue
                }
            }.ToList()
        };

        private static readonly JsonSerializer Serializer = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Include
        };

        private static JObject FromTestObject(object obj)
        {
            return JObject.FromObject(obj, Serializer);
        }


        public static IEnumerable<TestCaseData> PatchTestData
        {
            get
            {
                yield return new TestCaseData(FromTestObject(SimplePatchObjectWithNullValue), SimpleTestObject,
                    SimplePatchObjectWithNullValue).SetName("Patch Object with Primitives and Null Values");

                yield return new TestCaseData(FromTestObject(PatchObjectWithComponent), SimpleTestObject,
                        PatchObjectWithComponent)
                    .SetName("Complex Patch Object with Component.");
                yield return new TestCaseData(FromTestObject(PatchObjectWithComponent), ComplextTestObject,
                        PatchObjectWithComponent)
                    .SetName("Complex Patch Object with Component On ComplexTestObject.");
                yield return new TestCaseData(FromTestObject(PatchObjectWithPrimitiveList), ComplextTestObject,
                        PatchObjectWithPrimitiveList)
                    .SetName("Complex Patch Object with Primitive List On CompexTestObject.");
                yield return new TestCaseData(FromTestObject(PatchObjectWithComplexList), ComplextTestObject,
                        PatchObjectWithComplexList)
                    .SetName("Complex Patch Object with Complex List On CompexTestObject.");
            }
        }


        private static IEnumerable<TestCaseData> PerformanceTests
        {
            get
            {
                var runs = 10000;
                var patch = JObject.FromObject(PatchObjectWithComplexList).ToString();
                var targetProvider = new Func<ComplexTypeClass>(() => ComplextTestObject);
                yield return new TestCaseData(patch,
                        new Func<string, IJsonMergePatchDocument<ComplexTypeClass>>(
                            s => new ReferenceMergePatchDocument<ComplexTypeClass>(s)), targetProvider, runs)
                    .SetName("Serialize -> Merge -> Deserialize");
                yield return new TestCaseData(patch,
                        new Func<string, IJsonMergePatchDocument<ComplexTypeClass>>(
                            s => new JsonMergePatchDocument<ComplexTypeClass>(s)), targetProvider, runs)
                    .SetName("JProperties");
            }
        }

        [TestCaseSource(nameof(PerformanceTests))]
        public void PerformanceTest1(string patchDocument,
            Func<string, IJsonMergePatchDocument<ComplexTypeClass>> patchFactory, Func<ComplexTypeClass> targetProvider,
            int runs)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var doc = patchFactory.Invoke(patchDocument);

            RunPerformanceTest(doc, targetProvider, runs);
            stopWatch.Stop();
            Assert.Pass($"{runs} patches in {stopWatch.ElapsedMilliseconds} ms");
        }

        private static void RunPerformanceTest(IJsonMergePatchDocument<ComplexTypeClass> document,
            Func<ComplexTypeClass> resourceProvider, int runs)
        {
            for (var i = 0; i < runs; i++)
            {
                var patchTarget = resourceProvider.Invoke();
                document.ApplyPatch(patchTarget);
            }
        }
    }
}