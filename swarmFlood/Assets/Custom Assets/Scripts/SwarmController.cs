using UnityEngine;
using System.Collections.Generic;

public class SwarmController : MonoBehaviour
{
    //constants
    private const float MIN_FLOOD_HEIGHT = 0, MAX_FLOOD_HEIGHT = 40, WATER_RAISE_DURATION = 5f,
        MIN_STORM_DELAY = 5, MAX_STORM_DELAY = 15;

    public float floodHeight = MIN_FLOOD_HEIGHT;

    string robotCountText = "5";

    public float stormDelay = 5;
    public GameObject robotPrefab;
    public Terrain terrain;
    public BoxCollider spawnRange;
    public Cloud c;
    public Water water;
    public GameObject waterDummy;

    public GameObject bestMarker;

    private bool robotsCreated = false;

    private Vector3 best;
    private List<RobotMovement> robots = new List<RobotMovement>();

    void Update()
    {
        if (c.state == Cloud.State.IDLE)
            SetWaterHeight(floodHeight);
    }

    void OnGUI()
    {
        if (!robotsCreated)
        {
            GUI.Label(new Rect(0, 0, 100, 50), "Robot count: ");
            robotCountText = GUI.TextField(new Rect(100, 0, 100, 30), robotCountText);
            if (GUI.Button(new Rect(0, 50, 100, 50), "Create robots"))
                SpawnRobots();
        }

        string formatedHeight = string.Format("{0:0.00}", floodHeight);
        GUI.Label(new Rect(0, 100, 100, 50), "Flood height (" + formatedHeight + "):");
        floodHeight = GUI.HorizontalSlider(new Rect(100, 100, 100, 50), floodHeight, MIN_FLOOD_HEIGHT, MAX_FLOOD_HEIGHT);

        if (GUI.Button(new Rect(0, 150, 100, 50), "Bring storm"))
        {
            Destroy(waterDummy);
            c.BringStorm(spawnRange.transform.position, stormDelay);
            StartParticleSwarm();
        }
        string formatedDelay = string.Format("{0:0.00}", stormDelay);
        GUI.Label(new Rect(0, 200, 100, 50), "Storm delay (" + formatedDelay + "):");
        stormDelay = GUI.HorizontalSlider(new Rect(100, 200, 100, 50), stormDelay, MIN_STORM_DELAY, MAX_STORM_DELAY);
    }

    private void SetWaterHeight(float y)
    {
        Vector3 waterPos = waterDummy.transform.position;
        waterPos.y = y;
        waterDummy.transform.position = waterPos;
    }

    private void SpawnRobots()
    {
        int robotCount = int.Parse(robotCountText);
        for (int i = 0; i < robotCount; i++)
        {
            GameObject newRobot = Instantiate(robotPrefab) as GameObject;
            RobotMovement mov = newRobot.GetComponent<RobotMovement>();
            mov.t = terrain;
            mov.controller = this;
            mov.position = GetRandomSpawnPos();
            robots.Add(mov);
        }
        robotsCreated = true;
    }

    private Vector3 GetRandomSpawnPos()
    {
        Bounds spawnBounds = spawnRange.bounds;
        Vector3 min = spawnBounds.min;
        Vector3 max = spawnBounds.max;
        float newX = Random.Range(min.x, max.x);
        float newZ = Random.Range(min.z, max.z);
        return new Vector3(newX, 0, newZ);
    }

    public void RaiseWater()
    {
        water.Raise(floodHeight, WATER_RAISE_DURATION);
    }

    public Vector3 GlobalBest
    {
        set
        {
            best = value;
            bestMarker.transform.position = best;
        }
        get
        {
            return best;
        }
    }

    private void StartParticleSwarm()
    {
        foreach (RobotMovement mov in robots)
        {
            mov.StartSurvivalRoutine();
        }
    }

    public bool BeatsGlobalBest(Vector3 candidate)
    {
        float candidateHeight = terrain.SampleHeight(candidate);
        float bestHeight = terrain.SampleHeight(GlobalBest);

        return candidateHeight > bestHeight;
    }

    public static bool PositionBeats(Vector3 candidate, Vector3 best)
    {
        return candidate.y > best.y;
    }
}