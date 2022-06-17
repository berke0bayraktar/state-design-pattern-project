//deneme comment;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCState currentState;

    public NPCState wanderState = new NPCWanderState();
    public NPCState collectState = new NPCCollectState();
    public NPCState attackState = new NPCAttackState();

    public List<GameObject> allies = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> points = new List<GameObject>();

    public GameObject currentTarget;
    public float health;

    public  Transform BASE;
    public  float REACHED_BASE_RANGE;
    public  float ENEMY_SEARCH_RANGE;
    public  float ATTACK_ENEMY_RANGE;
    public  float POINT_SEARCH_RANGE;
    public  float CAN_COLLECT_POINT_RANGE;
    public  float WAYPOINT_REACHED_RANGE;


    // Start is called before the first frame update
    void Start()
    {
        currentState = wanderState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.UpdateState(this);
    }

    public void MoveToDestiation(Vector2 point, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, point, speed * Time.deltaTime);
    }

    public bool ReachedDestination(Vector2 point, float threshold)
    {
        return Nearby(transform.position, point, threshold);
    }

    public bool Nearby(List<GameObject> gameObjects, float range, bool setTarget = true)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.activeSelf && Nearby(transform.position, gameObject.transform.position, range))
            {
                if (setTarget)
                {
                    currentTarget = gameObject;
                }
                return true;
            }
        }
        return false;
    }

    public bool Nearby(Vector2 point1, Vector2 point2, float range)
    {
        return Vector2.Distance(point1, point2) <= range;
    }
}