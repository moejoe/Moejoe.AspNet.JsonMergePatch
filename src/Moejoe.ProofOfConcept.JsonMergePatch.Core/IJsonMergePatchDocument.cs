namespace Moejoe.ProofOfConcept.JsonMergePatch.Core
{
    /// <summary>
    /// Representation of an RFC 7386 PatchDocument for the given Resource Type.
    /// </summary>
    /// <typeparam name="TResource">The Type of the Resource.</typeparam>
    public interface IJsonMergePatchDocument<in TResource> where TResource : class
    {
        /// <summary>
        /// Applies the PatchDocuments changes to an instance of the targeted resource type.
        /// </summary>
        /// <param name="resource">An instance of the targeted resource.</param>
        /// <typeparam name="TResource">The Type of the targeted resouce.</typeparam>
        void ApplyPatch(TResource resource);
    }
}