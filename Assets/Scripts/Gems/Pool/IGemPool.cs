public interface IGemPool
{
    /// <summary>
    /// Retrieves a gem view instance from the pool or creates a new one if the pool is empty.
    /// </summary>
    SC_Gem Get();

    /// <summary>
    /// Returns a gem view instance back to the pool for reuse.
    /// </summary>
    void Return(SC_Gem gem);
}
