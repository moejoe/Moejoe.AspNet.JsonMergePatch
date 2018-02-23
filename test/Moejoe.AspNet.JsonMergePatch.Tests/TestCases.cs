using System;
using System.Collections.Generic;
using System.Linq;

namespace Moejoe.AspNet.JsonMergePatch.Tests
{
    public static class TestCases
    {
        public static ComplexTypeClass SimpleTestObject => new ComplexTypeClass
        {
            DateValue = DateTime.Now.AddYears(-1),
            IntegralValue = -100,
            StringValue = "oldValue",
            NullableProperty = true
        };


        public static ComplexTypeClass ComplextTestObject => new ComplexTypeClass
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

        public static ComplexTypeClass SimplePatchObject => new ComplexTypeClass
        {
            DateValue = DateTime.Parse("2018-02-22"),
            IntegralValue = 1234,
            StringValue = "Hello",
            NullableProperty = false,
            IgnoredProperty = "I'm not here"
        };

        public static ComplexTypeClass SimplePatchObjectWithNullValue => new ComplexTypeClass
        {
            DateValue = DateTime.Today,
            IntegralValue = 1234,
            StringValue = null,
            NullableProperty = null,
            IgnoredProperty = "I'm not here"
        };

        public static ComplexTypeClass PatchObjectWithComponent => new ComplexTypeClass
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

        public static ComplexTypeClass PatchObjectWithPrimitiveList => new ComplexTypeClass
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

        public static ComplexTypeClass PatchObjectWithComplexList => new ComplexTypeClass
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
            PrimitiveList = new List<string> { "test1", "test2", "test3" },
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
    }
}