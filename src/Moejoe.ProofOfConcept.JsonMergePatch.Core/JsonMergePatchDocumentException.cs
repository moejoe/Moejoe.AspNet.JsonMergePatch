using System;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core
{
    public class JsonMergePatchDocumentException : Exception
    {
        public JsonMergePatchDocumentException(string message) : base(message)
        {
        }

        public JsonMergePatchDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}