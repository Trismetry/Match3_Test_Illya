using UnityEngine;

public class UnityEffectFactory : IEffectFactory
{
    public IEffectHandle PlayAt(string fxId, Vector3 worldPos)
    {
        // Допустим, у тебя есть префабы эффектов в Resources/Effects
        GameObject prefab = Resources.Load<GameObject>($"Effects/{fxId}");
        if (prefab == null) return null;

        GameObject fx = Object.Instantiate(prefab, worldPos, Quaternion.identity);

        return new EffectHandle(fx);
    }

    private class EffectHandle : IEffectHandle
    {
        private GameObject fx;
        public EffectHandle(GameObject fx) { this.fx = fx; }
        public void Stop()
        {
            if (fx != null) Object.Destroy(fx);
        }
    }
}
