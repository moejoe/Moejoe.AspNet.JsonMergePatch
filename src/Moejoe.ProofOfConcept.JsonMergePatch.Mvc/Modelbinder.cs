using System;
using System.Reflection;
using System.Web.Mvc;
using Moejoe.ProofOfConcept.JsonMergePatch.Core;
using Newtonsoft.Json;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Mvc
{
    public class JsonMergePatchModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = bindingContext.ModelType;
            if (!model.IsGenericType || model.GetGenericTypeDefinition() != typeof(JsonMergePatchDocument<>))
                throw new NotSupportedException("This Binder only supports Models of the type JsonMergePatchDocument<TResource>");

            var content = controllerContext.HttpContext.Request.InputStream;
            var factory = new JsonMergePatchDocumentFactory(JsonConvert.DefaultSettings.Invoke());
            try
            {
                return factory.CreateJsonMergePatchDocument(model.GetTypeInfo().GenericTypeArguments[0], content);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(string.Empty, ex);
            }
            return null;
        }
    }
}
