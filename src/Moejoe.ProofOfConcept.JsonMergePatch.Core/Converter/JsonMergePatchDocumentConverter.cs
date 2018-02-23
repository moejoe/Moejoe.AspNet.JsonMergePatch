using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core.Converter
{
    public class JsonMergePatchDocumentConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!objectType.IsGenericType || objectType.GetGenericTypeDefinition() != typeof(JsonMergePatchDocument<>))
            {
                throw new ArgumentException(string.Format(ErrorMessages.FormatParameterMustMatchType, "objectType", "JsonMergePatchDocument"), nameof(objectType));
            }

            try
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return null;
                }
                var patchMergeDocument = Activator.CreateInstance(objectType, reader, serializer);
                return patchMergeDocument;
            }
            catch (Exception ex)
            {
                throw new InvalidJsonMergePatchDocumentException(ErrorMessages.DocumentNotParseable, ex);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
