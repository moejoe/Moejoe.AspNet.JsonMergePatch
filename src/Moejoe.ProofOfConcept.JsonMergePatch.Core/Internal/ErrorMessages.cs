using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core
{
    public static class ErrorMessages
    {
        public const string ArrayNotSupportedAsRootDocument =
                @"Arrays are not supported as root document. The way RFC 7386 is defined, using a simple CollectionBinder instead of JsonMergePatchDocument accomplishes the same thing."
            ;

        public const string DocumentNotParseable =
            @"The provided document could not be parsed. See inner exception for details";
    }
}
