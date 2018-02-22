using System;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core
{
    [Serializable]
    public class JsonMergePatchDocumentException : Exception
    {
        public JsonMergePatchDocumentException(string message) : base(message)
        {
        }

        public JsonMergePatchDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    [Serializable]
    public class InvalidJsonMergePatchDocumentException : JsonMergePatchDocumentException
    {
        public InvalidJsonMergePatchDocumentException(string message) : base(message)
        {
        }

        public InvalidJsonMergePatchDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}