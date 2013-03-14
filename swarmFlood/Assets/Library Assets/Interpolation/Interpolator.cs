using UnityEngine;

class Interpolator : MonoBehaviour
{
    public delegate void VectorInterpolationStep(Vector3 currentVector, float percentage);
    public delegate void QuaternionInterpolationStep(Quaternion currentVector, float percentage);

    private float duration, elapsed = 0f;

    public static void Interpolate(Vector3 start, Vector3 end, float duration, VectorInterpolationStep function)
    {
        if (duration != 0)
        {
            GameObject obj = new GameObject("Interpolator");
            VectorInterpolator i = obj.AddComponent<VectorInterpolator>();
            i.duration = duration;
            i.SetInitialData(start, end, function);
        }
    }

    public static void Interpolate(Quaternion start, Quaternion end, float duration, QuaternionInterpolationStep function)
    {
        if (duration != 0)
        {
            GameObject obj = new GameObject("Interpolator");
            QuaternionInterpolator i = obj.AddComponent<QuaternionInterpolator>();
            i.duration = duration;
            i.SetInitialData(start, end, function);
        }
    }

    public virtual void Update()
    {
        elapsed = Mathf.Clamp(elapsed + Time.deltaTime, 0, duration);
        if (elapsed >= duration)
            Destroy(gameObject);
    }

    public float Percentage
    {
        get
        {
            return elapsed / duration;
        }
    }

    void Interpolate2<T>(T start, T end, float duration, VectorInterpolationStep callback)
    {

    }
}