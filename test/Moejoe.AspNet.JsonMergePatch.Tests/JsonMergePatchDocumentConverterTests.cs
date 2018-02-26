using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Moejoe.AspNet.JsonMergePatch.Tests
{
    [TestFixture]
    public class JsonMergePatchDocumentConverterTests
    {
        [Test]
        public void Test()
        {
            var patch = JObject.FromObject(TestCases.PatchObjectWithComplexList);
            var json = patch.ToString();
            var patchDocument = JsonConvert.DeserializeObject<JsonMergePatchDocument<ComplexTypeClass>>(json);
           
        }
        [Test]
        public void JsonConvertert_Returns_Null_For_NullJson()
        {
            
            var json = "null";
            var patchDocument = JsonConvert.DeserializeObject<JsonMergePatchDocument<ComplexTypeClass>>(json);
            Assert.IsNull(patchDocument);
           
        }
    }
}
