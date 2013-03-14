using UnityEngine;

class RobotMovement : MonoBehaviour
{
    public Terrain t;
    public float duration = 3f;

    private TerrainData data;

    private enum State { IDLE, MOVING }
    private State state = State.IDLE;

    void Start()
    {
        data = t.terrainData;
        
    }

    void Update()
    {
        if (state == State.IDLE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 dest = RandomTerrainPosition();
                print("Moving to " + dest);
                MoveTo(dest);
                //MoveTo(Vector3.zero);
            }
        }
        //print(t.SampleHeight(transform.position));
    }

    public Vector3 RandomTerrainPosition()
    {
        Bounds terrainBounds = t.collider.bounds;
        Vector3 tMin = terrainBounds.min, tMax = terrainBounds.max;
        return new Vector3(Random.Range(tMin.x, tMax.x), 0, Random.Range(tMin.z, tMax.z));
    }

    //public void setPosition(Vector3 pos, Terrain t)
    //{
    //    float destHeight = t.SampleHeight(pos);
    //    destHeight += collider.bounds.extents.y;
    //    pos.y = destHeight;
    //    transform.position = pos;
    //}

    void OnInterpolationStep(Vector3 current, float pctg)
    {
        transform.position = current;
        if (pctg == 1.0f)
            state = State.IDLE;
    }

    void MoveTo(Vector3 destiny)
    {
        state = State.MOVING;
        float destHeight = t.SampleHeight(destiny);
        destHeight += collider.bounds.extents.y;
        destiny.y = destHeight;
        Interpolator.Interpolate(transform.position, destiny, duration, OnInterpolationStep);
    }

}