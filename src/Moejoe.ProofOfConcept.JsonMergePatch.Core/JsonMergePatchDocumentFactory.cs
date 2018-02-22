using System;
using System.IO;
using Newtonsoft.Json;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core
{
    /// <summary>
    /// Builds a JsonMergePatchDocument
    /// </summary>
    public class JsonMergePatchDocumentFactory
    {
        private readonly JsonSerializerSettings _settings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Settings for the Json Serializer</param>
        public JsonMergePatchDocumentFactory(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="patchData"></param>
        /// <returns></returns>
        public JsonMergePatchDocument<TResource> CreateJsonMergePatchDocument<TResource>(Stream patchData) where TResource : class
        {    
            using (var sr = new StreamReader(patchData))
            {
                var json = sr.ReadToEnd();
                return new JsonMergePatchDocument<TResource>(json,_settings);
            }
        }
    }

    /// <summary>
    /// Extensions
    /// </summary>
    public static class JsonMergePatchDocumentFactoryExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="resourceType"></param>
        /// <param name="patchData"></param>
        /// <returns></returns>
        public static object CreateJsonMergePatchDocument(this JsonMergePatchDocumentFactory f, Type resourceType, Stream patchData)
        {
            var methodInfo = typeof(JsonMergePatchDocumentFactory).GetMethod("CreateJsonMergePatchDocument");
            if (methodInfo == null) return null;
            var generic = methodInfo.MakeGenericMethod(resourceType);
            return generic.Invoke(f, new object[] {patchData});
        }
    }

}