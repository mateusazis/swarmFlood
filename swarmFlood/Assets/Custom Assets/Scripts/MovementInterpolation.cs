using UnityEngine;

public class MovementInterpolation : MonoBehaviour
{
    public delegate void MovementEndCallback();

    private GameObject target;
    private MovementEndCallback callback;

    public static void Apply(GameObject obj, Vector3 dest, float duration, MovementEndCallback callback = null)
    {
        MovementInterpolation mov = obj.AddComponent<MovementInterpolation>();
        mov.target = obj;
        mov.callback = callback;
        Interpolator.Interpolate(obj.transform.position, dest, duration, mov.OnInterpolationStep);
    }

    private void OnInterpolationStep(Vector3 v, float pctg)
    {
        target.transform.position = v;
        if (pctg == 1)
        {
            Destroy(this);
            if (callback != null)
                callback();
        }
    }
}