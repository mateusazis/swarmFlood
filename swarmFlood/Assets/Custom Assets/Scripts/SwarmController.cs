using UnityEngine;

public class SwarmController : MonoBehaviour
{
    //constants
    private const float MIN_FLOOD_HEIGHT = 0, MAX_FLOOD_HEIGHT = 40, WATER_RAISE_DURATION = 5f;

    public float floodHeight = MIN_FLOOD_HEIGHT;

    string robotCountText = "5";

    public float stormDelay = 5;
    public GameObject robotPrefab;
    public Terrain terrain;
    public BoxCollider spawnRange;
    public Cloud c;
    public Water water;

    private bool robotsCreated = false;

    //private Rect window1Rect = new Rect(300, 400, 150, 200);

    //private void func(int n)
    //{

    //}

    void OnGUI()
    {
        //window1Rect = GUI.Window(1, window1Rect, func, "Robot setup");

        if (!robotsCreated)
        {
            GUI.Label(new Rect(0, 0, 100, 50), "Robot count: ");
            robotCountText = GUI.TextField(new Rect(100, 0, 100, 30), robotCountText);
            if (GUI.Button(new Rect(0, 50, 100, 50), "Create robots"))
                SpawnRobots();
        }
        
        GUI.Label(new Rect(0, 100, 100, 50), "Flood height:");
        floodHeight = GUI.HorizontalSlider(new Rect(100, 100, 100, 50), floodHeight, MIN_FLOOD_HEIGHT, MAX_FLOOD_HEIGHT);
        if (GUI.Button(new Rect(0, 150, 100, 50), "Bring storm"))
        {
            c.BringStorm(spawnRange.transform.position, stormDelay);
        }
    }

    private void SpawnRobots()
    {
        int robotCount = int.Parse(robotCountText);
        for (int i = 0; i < robotCount; i++)
        {
            GameObject newRobot = Instantiate(robotPrefab) as GameObject;
            RobotMovement mov = newRobot.GetComponent<RobotMovement>();
            mov.t = terrain;
            mov.position = GetRandomSpawnPos();
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
}