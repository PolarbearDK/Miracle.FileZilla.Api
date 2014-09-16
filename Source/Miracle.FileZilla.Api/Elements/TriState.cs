namespace Miracle.FileZilla.Api.Elements
{
    /// <summary>
    /// Enum representing a Tri-state checkbox
    /// </summary>
    public enum TriState: byte
    {
        /// <summary>
        /// Not set
        /// </summary>
        No = 0,
        /// <summary>
        /// Set
        /// </summary>
        Yes = 1,
        /// <summary>
        /// Use default/inherited value.
        /// NOTE! Groups are not allowed to use this value anywhere in the object graph.
        /// </summary>
        Default = 2,
    }
}