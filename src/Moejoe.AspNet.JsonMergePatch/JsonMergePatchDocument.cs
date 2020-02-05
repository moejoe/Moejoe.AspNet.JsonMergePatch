using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Moejoe.AspNet.JsonMergePatch.Converter;
using Moejoe.AspNet.JsonMergePatch.Exceptions;
using Moejoe.AspNet.JsonMergePatch.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moejoe.AspNet.JsonMergePatch
{
    /// <summary>
    ///     Patch Document
    /// </summary>
    /// <remarks>
    ///     This implementation uses Newtonsoft.Json and its Linq Api to generate the document and apply it's content.
    /// </remarks>
    /// <typeparam name="TResource">Resource Type</typeparam>
    [JsonConverter(typeof(JsonMergePatchDocumentConverter))]
    public class JsonMergePatchDocument<TResource> : IJsonMergePatchDocument<TResource>, IValidatableObject where TResource : class
    {
        private readonly PatchDocument _internalDocument;

        private readonly InternalValidator<TResource> _internalValidator;

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
            _internalValidator = new InternalValidator<TResource>(settings);
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
        /// <summary>
        /// Constructor for given json content and optional JsonSerializer Settings.
        /// </summary>
        /// <param name="reader">JsonReader.</param>
        /// <param name="settings">settings</param>
        /// <exception cref="InvalidJsonMergePatchDocumentException">
        ///     if <paramref name="reader" /> points to anything other than a
        ///     parsable json object.
        /// </exception>
        public JsonMergePatchDocument(JsonReader reader, JsonSerializerSettings settings = null)
        {
            settings = settings ?? new JsonSerializerSettings();
            _internalValidator = new InternalValidator<TResource>(settings);
            var serializer = JsonSerializer.Create(settings);
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
        }
        /// <summary>
        /// Applies this patch documents content to the target resource.
        /// </summary>
        /// <param name="target">The Resource instance the patch will be applied to.</param>
        /// <exception cref="ArgumentNullException">if the resource instance is null.</exception>
        public void ApplyPatch(TResource target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            _internalDocument.ApplyPatch(target);
        }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return _internalValidator.Validate(validationContext, _internalDocument.Patch);
        }
    }
}