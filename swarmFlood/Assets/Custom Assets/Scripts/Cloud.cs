using UnityEngine;

public class Cloud : MonoBehaviour
{
    public SwarmController controller;

    public enum State {IDLE, MOVING, RAINING};

    public State state = State.IDLE;

    private float movementDuration;

    private float secsToRain;
    

    public void BringStorm(Vector3 dest, float movementDuration)
    {
        gameObject.SetActive(true);
        dest.y = transform.position.y;
        this.movementDuration = movementDuration;
        state = State.MOVING;
        MovementInterpolation.Apply(gameObject, dest, movementDuration, OnMoveComplete, OnInterpolationStep);
    }

    private void OnInterpolationStep(Vector3 v, float pctg)
    {
        secsToRain = (1 - pctg) * movementDuration;
    }

    private void OnMoveComplete()
    {
        state = State.RAINING;
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

    void OnGUI()
    {
        if (state != State.IDLE)
        {
            int screenWidth = Screen.width;
            int w = 100;
            int h = 50;
            Rect r = new Rect(screenWidth - w, 0, w, h);
            GUI.Label(r, "Seconds until rain: " + string.Format("{0:0.00}", secsToRain));       
        }
    }
}