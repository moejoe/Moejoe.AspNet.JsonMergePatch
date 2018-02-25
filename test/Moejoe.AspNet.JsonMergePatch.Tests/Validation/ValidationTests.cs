using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace Moejoe.AspNet.JsonMergePatch.Tests.Validation
{
    [TestFixture]
    public class ValidationTests
    {
        private static readonly JsonSerializer Serializer =
            JsonSerializer.Create(new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } });

        private static JObject EmptyPatchDocument => JObject.FromObject(new {},
            Serializer);


        private static JObject ComponentPatchDocument => JObject.FromObject(new ValidationClass
        {
            Component = new ValidationSubClass
            {
                StringValue = "Test"
            }
        }, Serializer);


        private static JObject InvalidMaxLengthPatchDocument => JObject.FromObject(new ValidationClass
        {
            StringValue = "ThisIsLongerThanItShouldBe",
            Component = new ValidationSubClass
            {
                StringValue = "ThisIsLongerThanItShouldBe"
            }
        }, Serializer);

        [Test]
        public void Empty_PatchDocument_IsValid()
        {
            var patchDocument = new JsonMergePatchDocument<ValidationClass>(EmptyPatchDocument.ToString());
            var validationContext = new ValidationContext(patchDocument);
            var validationErrors = patchDocument.Validate(validationContext).ToList();
            Console.Write(JsonConvert.SerializeObject(validationErrors));
            Assert.IsTrue(validationErrors.All(p => p == null), "No Error Result");
        }

        [Test]
        public void Required_Properties_In_Components_Are_Ignored()
        {
            var patchDocument = new JsonMergePatchDocument<ValidationClass>(ComponentPatchDocument.ToString());
            var validationContext = new ValidationContext(patchDocument);
            var validationErrors = patchDocument.Validate(validationContext).ToList();
            Console.Write(JsonConvert.SerializeObject(validationErrors));
            Assert.IsTrue(validationErrors.All(p => p == null), "No Error Result");
        }

        [Test]
        public void Validate_Returns_Error_If_MaximumStringLength_Is_Ignored()
        {
            var patchDocument = new JsonMergePatchDocument<ValidationClass>(InvalidMaxLengthPatchDocument.ToString());
            var validationContext = new ValidationContext(patchDocument);
            var validationErrors = patchDocument.Validate(validationContext).ToList();
            Console.Write(JsonConvert.SerializeObject(validationErrors));
            Assert.AreEqual(2, validationErrors.Count);
        }
    }
}