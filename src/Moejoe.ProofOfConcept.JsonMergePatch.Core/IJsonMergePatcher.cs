using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moejoe.ProofOfConcept.JsonMergePatch.Core
{
    /// <summary>
    /// Applies JsonMergePatches to a resource
    /// </summary>
    public interface IJsonMergePatcher<TResource> where TResource : class, new()
    {
        /// <summary>
        /// Applies the JsonMergePatch to the given resource.
        /// </summary>
        /// <typeparam name="TResource">The Type of the resource.</typeparam>
        /// <param name="original">The original resource.</param>
        /// <param name="patch">The patch to be applied.</param>
        /// <returns>The patched resource.</returns>
        TResource Patch(TResource original, JsonMergePatchDocument<TResource> patch);
    }
}
