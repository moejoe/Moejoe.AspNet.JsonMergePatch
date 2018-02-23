using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core.Tests
{
    [TestFixture]
    internal class JsonMergePatchDocumentTests
    {
        
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
                yield return new TestCaseData(FromTestObject(TestCases.SimplePatchObjectWithNullValue), TestCases.SimpleTestObject,
                    TestCases.SimplePatchObjectWithNullValue).SetName("Patch Object with Primitives and Null Values");

                yield return new TestCaseData(FromTestObject(TestCases.PatchObjectWithComponent), TestCases.SimpleTestObject,
                        TestCases.PatchObjectWithComponent)
                    .SetName("Complex Patch Object with Component.");
                yield return new TestCaseData(FromTestObject(TestCases.PatchObjectWithComponent), TestCases.ComplextTestObject,
                        TestCases.PatchObjectWithComponent)
                    .SetName("Complex Patch Object with Component On ComplexTestObject.");
                yield return new TestCaseData(FromTestObject(TestCases.PatchObjectWithPrimitiveList), TestCases.ComplextTestObject,
                        TestCases.PatchObjectWithPrimitiveList)
                    .SetName("Complex Patch Object with Primitive List On CompexTestObject.");
                yield return new TestCaseData(FromTestObject(TestCases.PatchObjectWithComplexList), TestCases.ComplextTestObject,
                        TestCases.PatchObjectWithComplexList)
                    .SetName("Complex Patch Object with Complex List On CompexTestObject.");
            }
        }


        private static IEnumerable<TestCaseData> PerformanceTests
        {
            get
            {
                const int runs = 1;
                var patch = JObject.FromObject(TestCases.PatchObjectWithComplexList).ToString();
                var targetProvider = new Func<ComplexTypeClass>(() => TestCases.ComplextTestObject);
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