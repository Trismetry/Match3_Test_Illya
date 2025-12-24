using UnityEngine;
using System.Threading.Tasks;

public interface IGemAnimator
{
    Task AnimateMove(Vector3 from, Vector3 to, float duration);
    Task AnimateDestroy(float duration);
}
