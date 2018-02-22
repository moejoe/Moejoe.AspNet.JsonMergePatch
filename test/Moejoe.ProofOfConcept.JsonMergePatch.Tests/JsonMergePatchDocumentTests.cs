using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core.Tests
{
    [TestFixture]
    public class JsonMergePatchDocumentTests
    {
        protected IJsonMergePatchDocument<TResource> BuildPatchDocument<TResource>(string patchData)
            where TResource : class
        {
            return new JsonMergePatchDocument<TResource>(patchData);
        }

        protected static IEnumerable<TestCaseData> JsonMergePatchSimpleTests
        {
            get
            {
                yield return new TestCaseData(TestCases.Simple.OverrideProperty).SetName("OverrideProperty");
                yield return new TestCaseData(TestCases.Simple.AddProperty).SetName("AddProperty");
                yield return new TestCaseData(TestCases.Simple.SetPropertyToNull).SetName("SetPropertyToNull");
                yield return new TestCaseData(TestCases.Simple.IgnoreUnsetProperties).SetName("IgnoreUnsetProperties");
                yield return new TestCaseData(TestCases.Simple.SetPropertyOnEmptyObject).SetName(
                    "SetPropertyOnEmptyObject");
                yield return new TestCaseData(TestCases.Simple.IgnoreUnmapableProperties).SetName(
                    "IgnoreUnmapableProperties");
                yield return new TestCaseData(TestCases.Simple.IgnoreUnmapableArray).SetName("IgnoreUnmapableArray");
            }
        }

        protected static IEnumerable<TestCaseData> JsonMergePatchNestedArrayTests
        {
            get
            {
                yield return new TestCaseData(TestCases.Simple.SetNestedStringArrayElements).SetName(
                    "SetNestedStringArrayElements");
            }
        }


        protected static IEnumerable<TestCaseData> JsonMergePatchNestedTests
        {
            get
            {
                yield return new TestCaseData(TestCases.Nested.IgnoreUnsetNullProperty).SetName(
                    "IgnoreUnsetNullProperty");
                yield return new TestCaseData(TestCases.Nested.SetPropertyToNull).SetName("SetPropertyToNull");
                yield return new TestCaseData(TestCases.Nested.SetSubPropertyNull).SetName("SetSubPropertyNull");
                yield return new TestCaseData(TestCases.Nested.SetSubClass).SetName("SetSubClass");
                yield return new TestCaseData(TestCases.Nested.SetEmptySubClass).SetName("SetEmptySubClass");
            }
        }

        protected static IEnumerable<TestCaseData> UnsupportedTestCases
        {
            get
            {
                yield return new TestCaseData(TestCases.Unsupported.TypeChangeStringToArray).SetName(
                    "TypeChangeStringToArray");
            }
        }

        protected static IEnumerable<TestCaseData> UnsupportedNestedArrayTestCases
        {
            get
            {
                yield return new TestCaseData(TestCases.Unsupported.TypeChangeNestedArrayToString).SetName(
                    "TypeChangeNestedArrayToString");
                yield return new TestCaseData(TestCases.Unsupported.TypeChangeOfNestedArrayElements).SetName(
                    "TypeChangeOfNestedArrayElements");
            }
        }


        [TestCaseSource(nameof(UnsupportedTestCases))]
        public void ApplyPatch_Throws_Exception(UnsupportedJsonMergePatchTestCase<SimpleClass> testcase)
        {
            var doc = BuildPatchDocument<SimpleClass>(testcase.Patch);
            Assert.Throws<ArgumentException>(() => doc.ApplyPatch(testcase.Original));
        }

        [TestCaseSource(nameof(UnsupportedNestedArrayTestCases))]
        public void ApplyPatch_Throws_Exception(UnsupportedJsonMergePatchTestCase<SimpleArrayClass> testcase)
        {
            var doc = BuildPatchDocument<SimpleArrayClass>(testcase.Patch);
            Assert.Throws<JsonSerializationException>(() => doc.ApplyPatch(testcase.Original));
        }

        private static void AreEqual(NestedClass expected, NestedClass actual)
        {
            Assert.AreEqual(expected.A, actual.A);
        }

        private static void AreEqual(SubClass expected, SubClass actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                Assert.IsNotNull(actual);
                Assert.AreEqual(expected.B, actual.B, "B");
                Assert.AreEqual(expected.C, actual.C, "C");
            }
        }


        [Test]
        [TestCaseSource(nameof(JsonMergePatchNestedTests))]
        public void ApplyPatch_Returns_Expected_Result(JsonMergePatchTestCase<NestedClass> testcase)
        {
            var patcher = BuildPatchDocument<NestedClass>(testcase.Patch);
            patcher.ApplyPatch(testcase.Original);

            AreEqual(testcase.ExpectedResult, testcase.Original);
        }


        [Test]
        [TestCaseSource(nameof(JsonMergePatchNestedArrayTests))]
        public void ApplyPatch_Returns_Expected_Result(JsonMergePatchTestCase<SimpleArrayClass> testcase)
        {
            var patcher = BuildPatchDocument<SimpleArrayClass>(testcase.Patch);
            patcher.ApplyPatch(testcase.Original);
            if (testcase.ExpectedResult == null)
            {
                Assert.IsNull(testcase.Original);
            }
            else
            {
                Assert.IsNotNull(testcase.Original);
                CollectionAssert.AreEquivalent(testcase.ExpectedResult.A, testcase.Original.A);
            }
        }

        [Test]
        public void Constructor_Throws_For_Collections()
        {
            Assert.Throws<NotSupportedException>(() => new ReferenceMergePatchDocument<List<string>>(string.Empty));
        }


        [Test]
        [TestCaseSource(nameof(JsonMergePatchSimpleTests))]
        public void Patch_Returns_Expected_Result(JsonMergePatchTestCase<SimpleClass> testcase)
        {
            var patcher = BuildPatchDocument<SimpleClass>(testcase.Patch);
            patcher.ApplyPatch(testcase.Original);
            Assert.AreEqual(testcase.ExpectedResult, testcase.Original);
        }
    }
}