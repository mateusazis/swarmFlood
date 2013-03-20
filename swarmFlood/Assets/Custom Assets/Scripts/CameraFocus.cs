using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public Transform globalBestMarker;
    public SwarmController controller;
    public float finalDistance = 1f;
    public float duration = 3f;

    private Quaternion originalRotation, destRotation;

    public void Focus()
    {
        originalRotation = transform.rotation;
        Vector3 distance = globalBestMarker.position - transform.position;
        float mag = distance.magnitude;
        distance = distance.normalized * (mag - finalDistance);

        destRotation = Quaternion.LookRotation(distance, Vector3.up);

        Vector3 dest = transform.position + distance;

        MovementInterpolation.Apply(gameObject, dest, duration, OnMovementComplete, OnInterpolationStep);
    }

    private void OnInterpolationStep(Vector3 v, float pctg)
    {
        transform.rotation = Quaternion.Lerp(originalRotation, destRotation, pctg);
    }

    public void OnMovementComplete()
    {
        controller.ShowFinalResult();
    }
}