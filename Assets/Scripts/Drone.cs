using UnityEngine;
using System.Collections;

public class Drone : Enemy
{

    private float scoutTimer;
    private float detectTimer;
    private float scoutTime = 10.0f;
    private float detectTime = 5.0f;
    private float detectionRadius = 400.0f;
    private int newResourceVal;
    public GameObject newResourceObject;
    public GameObject motherShip;
    public Vector3 scoutPosition;
    public enum DroneBehaviours
    {
        Idle,
        Scouting,
        Foraging
    }

    public DroneBehaviours droneBehaviour;

    GameManager gameManager;

    Rigidbody rb;

    //Movement & Rotation Variables
    public float speed = 50.0f;
    private float rotationSpeed = 5.0f;
    private float adjRotSpeed;
    private Quaternion targetRotation;
    public GameObject target;
    public float targetRadius = 200f;

    //Boid Steering/Flocking Variables
    public float separationDistance = 25.0f;
    public float cohesionDistance = 50.0f;
    public float separationStrength = 250.0f;
    public float cohesionStrength = 25.0f;
    private Vector3 cohesionPos = new Vector3(0f, 0f, 0f);
    private int boidIndex = 0;


    // Use this for initialization
    void Start()
    {

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        rb = GetComponent<Rigidbody>();

        motherShip = gameManager.alienMothership;
        scoutPosition = motherShip.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //Acquire player if spawned in
        if (gameManager.gameStarted)
            target = gameManager.playerDreadnaught;

        //Move towards valid targets
        if (target)
            MoveTowardsTarget();

        //Boid cohesion/segregation
        BoidBehaviour();
        switch (droneBehaviour)
        {
            case DroneBehaviours.Scouting:
                Scouting();
                break;
        }
        boidIndex++;
        if (boidIndex >= gameManager.enemyList.Length)
        {
            Vector3 cohesiveForce = (cohesionStrength / Vector3.Distance(cohesionPos, transform.position)) * (cohesionPos - transform.position);
            rb.AddForce(cohesiveForce);
            boidIndex = 0;
            cohesionPos.Set(0f, 0f, 0f);
        }
        Vector3 pos = gameManager.enemyList[boidIndex].transform.position;
        Quaternion rot = gameManager.enemyList[boidIndex].transform.rotation;
        float dist = Vector3.Distance(transform.position, pos);

        if (dist > 0f)
        {
            if (dist <= separationDistance)
            {
                float scale = separationStrength / dist;
                rb.AddForce(scale * Vector3.Normalize(transform.position - pos));
            }
            else if (dist < cohesionDistance && dist > separationDistance)
            {
                cohesionPos = cohesionPos + pos * (1f / (float)gameManager.enemyList.Length);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 1f);
            }
        }

        

    }

    private void MoveTowardsTarget()
    {
        //Rotate and move towards target if out of range
        if (Vector3.Distance(target.transform.position, transform.position) > targetRadius)
        {

            //Lerp Towards target
            targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);

            rb.AddRelativeForce(Vector3.forward * speed * 20 * Time.deltaTime);
        }
    }

    private void BoidBehaviour()
    {
    }

    private void Scouting()
    {
        if (!newResourceObject)
        {
            if (Vector3.Distance(transform.position, scoutPosition) < detectionRadius && Time.time > scoutTimer)
            {
                
            }
        }
        else
        {

        } 
    }

}
