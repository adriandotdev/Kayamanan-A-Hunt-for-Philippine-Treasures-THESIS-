using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    NavMeshAgent agent;
    // Array of waypoints to walk from one to the next one
    private Transform[] waypoints;
    private Transform startPosition;

    [Header("Normal Waypoints If Not Real NPCs")]
    [SerializeField]
    private Transform[] normalWaypoints;

    [Header("Day 1 Start Position and Waypoints")]
    [SerializeField]
    private NPC_BEHAVIOR day1Behavior;
    [SerializeField]
    private Transform day1StartPos;
    [SerializeField]
    private Transform[] day1Waypoints;
    public float timeToLook1;

    [Header("Day 2 Start Position and Waypoints")]
    [SerializeField]
    private NPC_BEHAVIOR day2Behavior;
    [SerializeField]
    private Transform day2StartPos;
    [SerializeField]
    private Transform[] day2Waypoints;
    public float timeToLook2;

    [Header("Day 3 Start Position and Waypoints")]
    [SerializeField]
    private NPC_BEHAVIOR day3Behavior;
    [SerializeField]
    private Transform day3StartPos;
    [SerializeField]
    private Transform[] day3Waypoints;
    public float timeToLook3;

    [Header("Day 4 Start Position and Waypoints")]
    [SerializeField]
    private NPC_BEHAVIOR day4Behavior;
    [SerializeField]
    private Transform day4StartPos;
    [SerializeField]
    private Transform[] day4Waypoints;
    public float timeToLook4;

    [Header("Day 5 Start Position and Waypoints")]
    [SerializeField]
    private NPC_BEHAVIOR day5Behavior;
    [SerializeField]
    private Transform day5StartPos;
    [SerializeField]
    private Transform[] day5Waypoints;
    public float timeToLook5;

    // Walk speed that can be set in Inspector
    //[SerializeField]
    //private float moveSpeed = 2f;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    // NPC Type and NPC Behavior
    public enum NPC_TYPE { MUSEUM_INDIVIDUAL, REAL_NPC }
    public enum NPC_BEHAVIOR { STATIONARY, WANDERING, GOING_HOME, NONE }

    public NPC_TYPE npcType;
    public NPC_BEHAVIOR npcBehavior;
    public float waitingTimeBeforeWalkingAgain;
    public Transform positionOfHome;
   
    [SerializeField] private NPC_INFO npcInfo;

    // Use this for initialization
    private void Start()
    {
        // Set position of Enemy as position of the first waypoint
        //transform.position = waypoints[waypointIndex].transform.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;

        }

        this.SetWaypoints();

        if (this.npcBehavior == NPC_BEHAVIOR.STATIONARY && this.startPosition != null)
        {
            print(gameObject.name + "here");
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = this.startPosition.position;
            StartCoroutine(EnableNavMeshAgent());
        }
        else
        {
            this.SetPosition();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        string nameOfNpc = GetComponent<DialogueTrigger>()?.NPC_NAME;

        if (this.npcBehavior == (int)NPC_BEHAVIOR.STATIONARY)
        {
            animator.SetFloat("Speed", 0);
        }
        else
        {
            if (this.npcBehavior == NPC_BEHAVIOR.WANDERING )
            {
                if (this.npcType != NPC_TYPE.MUSEUM_INDIVIDUAL && DialogueManager._instance != null &&
                    DialogueManager._instance.isTalking && nameOfNpc.ToUpper() == DialogueManager._instance.npcName.ToUpper())
                {
                    agent.isStopped = true;
                    return;
                }
                // Move Enemy
                this.Move();
            }
        }
    }

    IEnumerator EnableNavMeshAgent()
    {
        yield return new WaitForSeconds(.5f);

        GetComponent<NavMeshAgent>().enabled = true;
    }
    private void LateUpdate()
    {
        this.npcInfo.xPos = transform.position.x;
        this.npcInfo.yPos = transform.position.y;
    }

    // Method that actually make Enemy walk
    private void Move()
    {
        
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops
        if (this.waypointIndex <= this.waypoints.Length - 1)
        {
            agent.SetDestination(this.waypoints[this.waypointIndex].transform.position);
            agent.isStopped = false;
            
            Vector2 moveDirection = (this.waypoints[this.waypointIndex].transform.position - transform.position).normalized;
            float dotValueH = Mathf.Clamp(Vector2.Dot(Vector2.right, moveDirection), -1, 1);
            float dotValueV = Mathf.Clamp(Vector2.Dot(Vector2.up, moveDirection), -1, 1);

            animator.SetFloat("Speed", 1);
            animator.SetFloat("Horizontal", dotValueH);
            animator.SetFloat("Vertical", dotValueV);

            if (Vector2.Distance(transform.position, this.waypoints[this.waypointIndex].position) <= 0.01f)
            {
                if (this.npcType == NPC_TYPE.MUSEUM_INDIVIDUAL || this.npcType == NPC_TYPE.REAL_NPC)
                {
                    if (this.waypoints[this.waypointIndex].transform.name == "1")
                    {
                        animator.SetBool("Look Left", true);
                    }
                    else if (this.waypoints[this.waypointIndex].transform.name == "2")
                    {
                        animator.SetBool("Look Up", true);
                    }
                    else if (this.waypoints[this.waypointIndex].transform.name == "3")
                    {
                        animator.SetBool("Look Right", true);
                    }
                    else if (this.waypoints[this.waypointIndex].transform.name == "4")
                    {
                        animator.SetBool("Look Down", true);
                    }
                }
                    
                this.waypointIndex += 1;
                this.npcBehavior = NPC_BEHAVIOR.STATIONARY;
                
                StartCoroutine(WalkAgain());

                if (this.waypointIndex > this.waypoints.Length - 1)
                {
                    this.waypointIndex = 0;
                }
            }
        }
    }

    public void GoHome()
    {
        if (this.positionOfHome != null)
        {
            this.agent.SetDestination(this.positionOfHome.position);
            this.npcBehavior = NPC_BEHAVIOR.GOING_HOME;

            Vector2 moveDirection = (this.positionOfHome.position - transform.position).normalized;
            float dotValueH = Mathf.Clamp(Vector2.Dot(Vector2.right, moveDirection), -1, 1);
            float dotValueV = Mathf.Clamp(Vector2.Dot(Vector2.up, moveDirection), -1, 1);

            animator.SetFloat("Speed", 1);
            animator.SetFloat("Horizontal", dotValueH);
            animator.SetFloat("Vertical", dotValueV);

            if (Vector2.Distance(transform.position, this.positionOfHome.position) <= 0.01f)
            {
                this.spriteRenderer.color = new Color(0, 0, 0, 0);
            }
        }
    }

    public void SetPosition()
    {
        if (npcType == NPC.NPC_TYPE.MUSEUM_INDIVIDUAL)
        {
            return;
        }

        foreach (NPC_INFO npcInfo in DataPersistenceManager.instance.playerData.npcInfos)
        {
            if (gameObject.name.ToUpper() == npcInfo.name.ToUpper())
            {
                this.npcInfo = npcInfo;

                if (this.npcInfo.xPos != 0 || this.npcInfo.yPos != 0)
                {
                    transform.position = new Vector2(this.npcInfo.xPos, this.npcInfo.yPos);
                }
                return;
            }
        }
    }

    public void SetWaypoints()
    {
        if (this.npcType == NPC_TYPE.MUSEUM_INDIVIDUAL)
        {
            this.waypoints = this.normalWaypoints;
            this.startPosition = day1StartPos;
            return;
        }

        switch(DataPersistenceManager.instance.playerData.playerTime.m_DayEvent)
        {
            case 1:
                this.waypoints = day1Waypoints;
                this.startPosition = day1StartPos;
                this.npcBehavior = this.day1Behavior;
                this.waitingTimeBeforeWalkingAgain = timeToLook1;
                break;
            case 2:
                this.waypoints = day2Waypoints;
                this.startPosition = day2StartPos;
                this.npcBehavior = this.day2Behavior;
                this.waitingTimeBeforeWalkingAgain = timeToLook2;
                break;
            case 3:
                this.waypoints = day3Waypoints;
                this.startPosition = day3StartPos;
                this.npcBehavior = this.day3Behavior;
                this.waitingTimeBeforeWalkingAgain = timeToLook3;
                break;
            case 4:
                this.waypoints = day4Waypoints;
                this.startPosition = day4StartPos;
                this.npcBehavior = this.day4Behavior;
                this.waitingTimeBeforeWalkingAgain = timeToLook4;
                break;
            case 5:
                this.waypoints = day5Waypoints;
                this.startPosition = day5StartPos;
                this.npcBehavior = this.day5Behavior;
                break;
        }
    }
    IEnumerator WalkAgain()
    {
        yield return new WaitForSeconds(this.waitingTimeBeforeWalkingAgain);

        this.npcBehavior = NPC_BEHAVIOR.WANDERING;
        animator.SetBool("Look Left", false);
        animator.SetBool("Look Right", false);
        animator.SetBool("Look Up", false);
        animator.SetBool("Look Down", false);
    }
}
