using System.Collections.Generic;
using UnityEngine;

public class GemPool : IGemPool
{
    private readonly GameObject prefab;
    private readonly Transform parent;
    private readonly Stack<SC_Gem> pool = new();

    public GemPool(GameObject prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
    }

    public SC_Gem Get()
    {
        SC_Gem gem;

        // Reuse from pool if available
        if (pool.Count > 0)
        {
            gem = pool.Pop();
        }
        else
        {
            // Fallback: create a new instance
            GameObject go = Object.Instantiate(prefab, parent);
            gem = go.GetComponent<SC_Gem>() ?? go.AddComponent<SC_Gem>();
        }

        gem.gameObject.SetActive(true);
        return gem;
    }

    public void Return(SC_Gem gem)
    {
        if (gem == null)
            return;

        // Prevent double-return
        if (pool.Contains(gem))
            return;

        // Reset view state before pooling
        gem.ResetView();

        gem.gameObject.SetActive(false);
        gem.transform.SetParent(parent);

        pool.Push(gem);
    }
}
