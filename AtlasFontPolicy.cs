namespace Atlas
{
    using System;

    /// <summary>The native Linux renderer retains ownership of its global font atlas.</summary>
    internal static class AtlasFontPolicy
    {
        internal static bool CanReplaceGlobalFont(string backend) =>
            !string.Equals(backend, "native-gpu", StringComparison.OrdinalIgnoreCase);
    }
}
