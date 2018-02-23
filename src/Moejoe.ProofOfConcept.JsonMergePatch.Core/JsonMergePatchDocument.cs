using System;
using Moejoe.ProofOfConcept.JsonMergePatch.Core.Converter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core
{
    /// <summary>
    ///     Patch Document
    /// </summary>
    /// <remarks>
    ///     This implementation uses Newtonsoft.Json and its Linq Api to generate the document and apply it's content.
    /// </remarks>
    /// <typeparam name="TResource">Resource Type</typeparam>
    [JsonConverter(typeof(JsonMergePatchDocumentConverter))]
    public class JsonMergePatchDocument<TResource> : IJsonMergePatchDocument<TResource> where TResource : class
    {
        private readonly PatchDocument _internalDocument;

        /// <summary>
        /// Constructor for given json content and optional JsonSerializer Settings.
        /// </summary>
        /// <param name="patchDocument">json MergePatchDocument content.</param>
        /// <param name="settings">Serializer settings to use. NullValueHandling will always be NullValueHandling.Include though.</param>
        /// <exception cref="ArgumentException">if <paramref name="patchDocument" /> is null or whitespace.</exception>
        /// <exception cref="InvalidJsonMergePatchDocumentException">
        ///     if <paramref name="patchDocument" /> anything other than a
        ///     parsable json object.
        /// </exception>
        public JsonMergePatchDocument(string patchDocument, JsonSerializerSettings settings = null)
        {
            if (string.IsNullOrWhiteSpace(patchDocument))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(patchDocument));

            settings = settings ?? new JsonSerializerSettings();
            var serializer = JsonSerializer.Create(settings);
            // Ensure that the serializer does not ignore null values to comply with RFC 7386
            serializer.NullValueHandling = NullValueHandling.Include;
            if (!patchDocument.Trim().StartsWith("{"))
                throw new InvalidJsonMergePatchDocumentException(ErrorMessages.DocumentRootMustBeObject);
            try
            {
                var patchObject = JObject.Parse(patchDocument);
                _internalDocument = new PatchDocument(patchObject, typeof(TResource),
                    serializer);
            }
            catch (JsonReaderException ex)
            {
                throw new InvalidJsonMergePatchDocumentException(ErrorMessages.DocumentNotParseable, ex);
            }
        }
        public JsonMergePatchDocument(JsonReader reader, JsonSerializer serializer)
        {
            var originalNullValueHandling = serializer.NullValueHandling;
            // Ensure that the serializer does not ignore null values to comply with RFC 7386
            serializer.NullValueHandling = NullValueHandling.Include;
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new InvalidJsonMergePatchDocumentException(ErrorMessages.DocumentRootMustBeObject);
            }

            try
            {
                var patchObject = JObject.Load(reader);
                _internalDocument = new PatchDocument(patchObject, typeof(TResource),
                    serializer);
            }
            catch (JsonReaderException ex)
            {
                throw new InvalidJsonMergePatchDocumentException(ErrorMessages.DocumentNotParseable, ex);
            }
            finally
            {
                serializer.NullValueHandling = originalNullValueHandling;
            }
        }

        public void ApplyPatch(TResource target)
        {
            _internalDocument.ApplyPatch(target);
        }
    }
}