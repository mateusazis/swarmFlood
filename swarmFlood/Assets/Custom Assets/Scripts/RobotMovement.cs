using UnityEngine;

class RobotMovement : MonoBehaviour
{
    public Terrain t;
    public float duration = 1.5f;
    public SwarmController controller;
    public GameObject markerPrefab = null;
    private GameObject currentMarker;
    public GameObject exclamation = null;
    public GameObject thinking = null;

    private enum State { IDLE, SEARCHING_BEST, ESCAPING }
    private State state = State.IDLE;

    //IA
    private Vector3 particleBest;
    //começar com uma posição que sempre perderá (altura negativa)
    private Vector3 velocity = new Vector3(0, -1, 0);
    private const float LEARNING_FACTOR_1 = 2, LEARNING_FACTOR_2 = 2;

    public float maxSpeed = 10;

    public Vector3 position
    {
        set{
            float destHeight = t.SampleHeight(value);
            destHeight += collider.bounds.extents.y;
            value.y = destHeight;
            transform.position = value;
        }
    }

    public static Vector3 PositionAtTerrain(Vector3 position, GameObject obj, Terrain t)
    {
        float destHeight = t.SampleHeight(position);
        destHeight += obj.collider.bounds.extents.y;
        position.y = destHeight;
        return position;
    }

    public Vector3 RandomTerrainPosition()
    {
        Bounds terrainBounds = t.collider.bounds;
        Vector3 tMin = terrainBounds.min, tMax = terrainBounds.max;
        return new Vector3(Random.Range(tMin.x, tMax.x), 0, Random.Range(tMin.z, tMax.z));
    }

    void OnInterpolationStep(Vector3 current, float pctg)
    {
        position = current;
        bool reachedDestiny = pctg == 1.0f;

        if (state != State.ESCAPING)
        {
            bool rainStarted = controller.c.state == Cloud.State.RAINING;
            if (rainStarted)
            {
                exclamation.SetActive(true);
                thinking.SetActive(false);
                RemoveMarker();
                MoveTo(controller.GlobalBest);
                state = State.ESCAPING;
            }
            else if (reachedDestiny)
            {
                RemoveMarker();
                ComputePosition();
                MoveNext();
            }
        }
        if (state == State.ESCAPING && reachedDestiny)
            exclamation.SetActive(false);
        
    }

    void MoveTo(Vector3 destiny)
    {
        state = State.SEARCHING_BEST;
        float destHeight = t.SampleHeight(destiny);
        destHeight += collider.bounds.extents.y;
        destiny.y = destHeight;

        Interpolator.Interpolate(transform.position, destiny, duration, OnInterpolationStep);
    }

    public void StartSurvivalRoutine()
    {
        thinking.SetActive(true);
        if (ComputePosition())
        {
            //se for o dono do melhor global, randomizar alguma velocidade inicial para não ficar preso em velocidade nula
            velocity = Random.onUnitSphere * maxSpeed;
        }
        MoveNext();
    }

    /// <summary>
    /// Compara a posição atual com o melhor da partícula e também com o melhor global, substituindo quando necessário e verificando
    /// se superou o melhor global.
    /// </summary>
    /// <returns>True caso supere o melhor global</returns>
    private bool ComputePosition()
    {
        Vector3 current = transform.position;
        if (SwarmController.PositionBeats(current, particleBest))
            particleBest = current;
        if (controller.BeatsGlobalBest(particleBest))
        {
            controller.GlobalBest = particleBest;
            return true;
        }
        return false;
    }

    private void MoveNext()
    {
        Vector3 present = transform.position;

        //v[] = v[] + c1 * rand() * (pbest[] - present[]) + c2 * rand() * (gbest[] - present[]) (a)
        //present[] = persent[] + v[] (b)

        velocity = velocity + LEARNING_FACTOR_1 * Random.Range(0, 1.0f) * (particleBest - present) +
            LEARNING_FACTOR_2 * Random.Range(0, 1.0f) * (controller.GlobalBest - present);

        //truncar a velocidade se for muito grande
        if (velocity.magnitude > maxSpeed)
            velocity = velocity.normalized * maxSpeed;

        present = present + velocity;

        Vector3 heightenedDest = PositionAtTerrain(present, gameObject, t);
        currentMarker = Instantiate(markerPrefab, heightenedDest, markerPrefab.transform.rotation) as GameObject;

        MoveTo(present);
    }

    private void RemoveMarker()
    {
        if (currentMarker)
        {
            Destroy(currentMarker);
            currentMarker = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxSpeed);
    }
}