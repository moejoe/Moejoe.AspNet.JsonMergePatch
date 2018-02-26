using System;

namespace Moejoe.AspNet.JsonMergePatch.Exceptions
{
    /// <summary>
    /// Exception hints to an invalid Json Merge Patch Document content during parsing.
    /// </summary>
    [Serializable]
    public class InvalidJsonMergePatchDocumentException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error Message</param>
        public InvalidJsonMergePatchDocumentException(string message) : base(message)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="innerException">Inner Exception</param>
        public InvalidJsonMergePatchDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}