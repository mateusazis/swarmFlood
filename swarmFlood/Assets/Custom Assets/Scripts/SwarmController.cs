using UnityEngine;

public class SwarmController : MonoBehaviour
{
    public int robotCount = 5;
    public GameObject robotPrefab;
    public Terrain terrain;
    
    void Start()
    {

    }

    void DistributeInitialRobots()
    {

    }

    void OnGUI()
    {
        if (GUILayout.Button("Create robots"))
        {
            for (int i = 0; i < robotCount; i++)
            {
                GameObject newRobot = Instantiate(robotPrefab) as GameObject;
                RobotMovement mov = newRobot.GetComponent<RobotMovement>();
                newRobot.transform.position = mov.RandomTerrainPosition();
            }
        }
    }
}