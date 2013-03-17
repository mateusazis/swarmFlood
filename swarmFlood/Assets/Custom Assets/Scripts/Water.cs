using UnityEngine;

public class Water : MonoBehaviour
{
    public Cloud c;

    public void Raise(float height, float duration)
    {
        gameObject.SetActive(true);
        Vector3 dest = transform.position;
        dest.y = height;
        MovementInterpolation.Apply(gameObject, dest, duration, OnComplete);
    }

    private void OnComplete()
    {
        c.StopRain();
    }
}