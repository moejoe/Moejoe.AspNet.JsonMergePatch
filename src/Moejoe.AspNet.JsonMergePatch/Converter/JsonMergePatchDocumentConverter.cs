using System;
using Moejoe.AspNet.JsonMergePatch.Exceptions;
using Moejoe.AspNet.JsonMergePatch.Internal;
using Newtonsoft.Json;

namespace Moejoe.AspNet.JsonMergePatch.Converter
{
    /// <summary>
    /// Json Converter for JsonMergePatchDocument.
    /// </summary>
    public class JsonMergePatchDocumentConverter : JsonConverter
    {
        /// <inheritdoc cref="JsonConverter"/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc cref="JsonConverter"/>
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
        
        /// <inheritdoc cref="JsonConverter"/>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
