# A RFC 7396 JsonMergePatch Implementation for .NET

See [RFC 7396](https://tools.ietf.org/html/rfc7396) for a description of Json Merge Patch.

This implementation uses [Newtonsoft Json.Net](https://www.newtonsoft.com/json) to generate and apply a set of changes described in a merge patch document.

Changes can be applied multiple times to different targets and should be okay performance wise.

I compared the speed by applying the same set of changes with the native
[Json Patch (RC 6902) Feature in ASP.NET Core](https://github.com/aspnet/JsonPatch) and observed similar performance for the first patch, and better timings for subsequent patches.


## Examples

### webapi2 / dotnet core

~~~cs
[HttpPatch]
[Route("resources/{resourceId}", Name="PatchResource")]
public IHttpActionResult PatchResource(
  string resourceId,
  [FromBody] JsonMergePatchDocument<Resource> patchDocument)
{
  var resource = _resourceRepository.Get(resourceId);
  if(resource is null) 
    return NotFound();

  patchDocument.ApplyPatch(resource);
  resourceRepository.Update(resource);
  return Request.CreateResponse(HttpStatusCode.NoContent, "No Content");
}
~~~

## Todos

* [ ] Cleanup error handling.
* [ ] Provide Nuget Package.
* [x] Implement Validation for the Patch Document. The model state should indicate any validation problems for properties and components provided by the patch.
* [ ] Add `application/merge-patch+json` MediaType for webapi2 and dotnet core 2.0

## Alternatives

### ASP.NET Core JsonPatch
ASP.NET has built-in support for JsonPatch (RFC 6902), but no support for Json Merge Patch (RFC 7396).

### Morcato JsonMergePatch for ASP.NET Core
Morcato has build a RFC 7396 implementation on top of ASP.NET Core's JsonPatch (RCFC 6902) built-in support.

https://github.com/Morcatko/Morcatko.AspNetCore.JsonMergePatch
