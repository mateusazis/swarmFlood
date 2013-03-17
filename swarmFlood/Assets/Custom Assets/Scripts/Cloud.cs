using UnityEngine;

public class Cloud : MonoBehaviour
{
    public SwarmController controller;
    

    public void BringStorm(Vector3 dest, float movementDuration)
    {
        gameObject.SetActive(true);
        dest.y = transform.position.y;
        MovementInterpolation.Apply(gameObject, dest, movementDuration, OnMoveComplete);
    }

    private void OnMoveComplete()
    {
        ParticleSystem[] systs = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem s in systs)
            s.Play();
        Invoke("StartRaising", 3.0f);
    }

    private void StartRaising()
    {
        controller.RaiseWater();
    }

    public void StopRain()
    {
        ParticleSystem[] systs = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem s in systs)
            s.Stop();
    }
}