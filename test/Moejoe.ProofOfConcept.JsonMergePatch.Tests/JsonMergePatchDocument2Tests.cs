using NUnit.Framework;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core.Tests
{
    [TestFixture]
    public class JsonMergePatchDocument2Tests
    {
        [Test]
        [TestCase(@"{""b"" : ""c""}", PropertyPresence.None, PropertyPresence.Value)]
        [TestCase(@"{""a"" : ""c""}", PropertyPresence.Value, PropertyPresence.None)]
        [TestCase(@"{""a"" : ""c"", ""b"" : ""d""}", PropertyPresence.Value, PropertyPresence.Value)]
        [TestCase(@"{""a"" : null, ""b"" : ""d""}", PropertyPresence.Null, PropertyPresence.Value)]
        [TestCase(@"{""a"" : null, ""b"" : null}", PropertyPresence.Null, PropertyPresence.Null)]
        [TestCase(@"{}", PropertyPresence.None, PropertyPresence.None)]
        public void Constructs_Expected_PatchDocument(string jsonMergePatchDoc, PropertyPresence presenceA, PropertyPresence presenceB)
        {
            var actualDoc = new JsonMergePatchDocument2<SimpleClass>(jsonMergePatchDoc);
            Assert.AreEqual(presenceA, actualDoc.GetPropertyPresence(p => p.A), "PropertyPresence(A)");
            Assert.AreEqual(presenceB, actualDoc.GetPropertyPresence(p => p.B), "PropertyPresence(B)");
        }

        [Test]
        [TestCase(@"{""a"": {""b"" : ""d"", ""c"" : null}}", PropertyPresence.Value, PropertyPresence.Value, PropertyPresence.Null)]
        [TestCase(@"{}", PropertyPresence.None, PropertyPresence.None, PropertyPresence.None)]
        [TestCase(@"{""a"": null}", PropertyPresence.Null, PropertyPresence.None, PropertyPresence.None)]        
        public void Constructs_Expected_PatchDocument_For_Nested_Classes(string jsonMergePatchDoc, PropertyPresence presenceA, PropertyPresence presenceB, PropertyPresence presenceC)
        {
            var actualDoc = new JsonMergePatchDocument2<NestedClass>(jsonMergePatchDoc);
            Assert.AreEqual(presenceA, actualDoc.GetPropertyPresence(p => p.A), "PropertyPresence(A)");
            Assert.AreEqual(presenceB, actualDoc.GetPropertyPresence(p => p.A.B), "PropertyPresence(B)");
            Assert.AreEqual(presenceC, actualDoc.GetPropertyPresence(p => p.A.C), "PropertyPresence(C)");
        }
    }
}