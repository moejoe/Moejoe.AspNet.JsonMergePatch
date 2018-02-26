using System;
using System.Collections.Generic;

namespace Moejoe.AspNet.JsonMergePatch.Tests.RfcComplianceTests
{
    public static class TestCases
    {
        public static class Simple
        {
            public static JsonMergePatchTestCase<SimpleClass> OverrideProperty => new JsonMergePatchTestCase<SimpleClass>
            {
                Original = new SimpleClass { A = "b" },
                Patch = @"{""a"" : ""c""}",
                ExpectedResult = new SimpleClass { A = "c", B = null }
            };
            public static JsonMergePatchTestCase<SimpleClass> SetPropertyOnEmptyObject => new JsonMergePatchTestCase<SimpleClass>
            {
                Original = new SimpleClass { B = null },
                Patch = @"{""a"" : ""1""}",
                ExpectedResult = new SimpleClass { A = "1", B = null }
            };
            public static JsonMergePatchTestCase<SimpleClass> AddProperty => new JsonMergePatchTestCase<SimpleClass>
            {
                Original = new SimpleClass { A = "b" },
                Patch = @"{""b"" : ""c""}",
                ExpectedResult = new SimpleClass { A = "b", B = "c" }
            };

            public static JsonMergePatchTestCase<SimpleClass> SetPropertyToNull => new JsonMergePatchTestCase<SimpleClass>
            {
                Original = new SimpleClass { A = "b" },
                Patch = @"{""a"" : null}",
                ExpectedResult = new SimpleClass { A = null, B = null }
            };
            public static JsonMergePatchTestCase<SimpleClass> IgnoreUnsetProperties => new JsonMergePatchTestCase<SimpleClass>
            {
                Original = new SimpleClass { A = "b", B = "c" },
                Patch = @"{""a"" : null}",
                ExpectedResult = new SimpleClass { A = null, B = "c" }
            };

            public static JsonMergePatchTestCase<SimpleArrayClass> SetNestedStringArrayElements => new
                JsonMergePatchTestCase<SimpleArrayClass>
                {
                    Original = new SimpleArrayClass { A = new[] { "a", "b" } },
                    Patch = @"{a: [""c"",""d""]}",
                    ExpectedResult = new SimpleArrayClass { A = new[] { "c", "d" } },
                };

            public static JsonMergePatchTestCase<SimpleClass> IgnoreUnmapableProperties = new JsonMergePatchTestCase<SimpleClass>
            {
                Original = new SimpleClass { A =  "foo"  },
                Patch = @"""bar""",
                ExpectedResult = new SimpleClass { A =  "foo"  }
            };

            public static JsonMergePatchTestCase<SimpleClass> IgnoreUnmapableArray = new JsonMergePatchTestCase<SimpleClass>
            {
                Original = new SimpleClass { A = "b" },
                Patch = @"[ ""c"" ]",
                ExpectedResult =  new SimpleClass { A = "b" }
            };
        }

        public static class Nested
        {
            public static JsonMergePatchTestCase<NestedClass> IgnoreUnsetNullProperty => new JsonMergePatchTestCase<NestedClass>
            {
                Original = new NestedClass { A = new SubClass { B = "c" } },
                Patch = @"{""a"": {""b"" : ""d"", ""c"" : null}}",
                ExpectedResult = new NestedClass { A = new SubClass { B = "d" } }
            };

            public static JsonMergePatchTestCase<NestedClass> SetPropertyToNull => new JsonMergePatchTestCase<NestedClass>
            {
                Original = new NestedClass { A = new SubClass { B = "d", C = "e" } },
                Patch = @"{""a"": null}",
                ExpectedResult = new NestedClass { A = null }
            };

            public static JsonMergePatchTestCase<NestedClass> SetSubPropertyNull => new JsonMergePatchTestCase<NestedClass>
            {
                Original = new NestedClass { A = new SubClass { B = "d", C = "e" } },
                Patch = @"{""a"": {""c"": null}}",
                ExpectedResult = new NestedClass { A = new SubClass { B = "d", C = null } }
            };
            public static JsonMergePatchTestCase<NestedClass> SetSubClass => new JsonMergePatchTestCase<NestedClass>
            {
                Original = new NestedClass { A = null },
                Patch = @"{""a"": {""b"": ""d"", ""c"": ""e""}}",
                ExpectedResult = new NestedClass { A = new SubClass { B = "d", C = "e" } }
            };

            public static JsonMergePatchTestCase<NestedClass> SetEmptySubClass => new JsonMergePatchTestCase<NestedClass>
            {
                Original = new NestedClass(),
                Patch = @"{""a"": {""b"": ""d"", ""c"": ""e"",""d"": {""A"": null}}}",
                ExpectedResult = new NestedClass { A = new SubClass { B = "d", C = "e", D = new SimpleClass()} }
            };

        }

        public static class Unsupported
        {
            public static UnsupportedJsonMergePatchTestCase<SimpleClass> TypeChangeStringToArray = new UnsupportedJsonMergePatchTestCase<SimpleClass>
            {
                Original = new SimpleClass { A = "c" },
                Patch = @"{ a : [ ""b"" ] }",
                ExpectedException = typeof(InvalidOperationException)
            };

            public static UnsupportedJsonMergePatchTestCase<SimpleArrayClass> TypeChangeNestedArrayToString = new UnsupportedJsonMergePatchTestCase<SimpleArrayClass>
            {
                Original = new SimpleArrayClass { A = new[] { "b" } },
                Patch = @"{""a"": ""c""}",
                ExpectedException = typeof(InvalidOperationException)
            };

            public static UnsupportedJsonMergePatchTestCase<SimpleArrayClass> TypeChangeOfNestedArrayElements = new UnsupportedJsonMergePatchTestCase<SimpleArrayClass>
            {
                Original = new SimpleArrayClass { B = new[] { new SimpleClass {A = "c"} } },
                Patch = @"{""b"": [1]}",
                ExpectedException = typeof(InvalidOperationException)
            };
            public static UnsupportedJsonMergePatchTestCase<List<int>> TypeChangeArrayToObject = new UnsupportedJsonMergePatchTestCase<List<int>>
            {
                Original = new List<int>{1,2},
                Patch = @"{""a"": ""b"", ""c"": null }",
                ExpectedException = typeof(InvalidOperationException)
            };
        }
    }
}