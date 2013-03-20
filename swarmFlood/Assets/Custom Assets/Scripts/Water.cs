using UnityEngine;

public class Water : MonoBehaviour
{
    public Cloud c;
    public SwarmController controller;
    public float raiseDuration = 2.5f;

    public void Raise(float height)
    {
        gameObject.SetActive(true);
        Vector3 dest = transform.position;
        dest.y = height;
        MovementInterpolation.Apply(gameObject, dest, raiseDuration, OnComplete);
    }

    private void OnComplete()
    {
        c.StopRain();
        CameraFocus focus = Camera.main.GetComponent<CameraFocus>();
        focus.Focus();
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("robot"))
        {
            controller.operationSuccessful = false;
        }
    }
}