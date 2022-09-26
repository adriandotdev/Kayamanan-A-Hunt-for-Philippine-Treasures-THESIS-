using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    NavMeshAgent agent;
    // Array of waypoints to walk from one to the next one
    [SerializeField]
    private Transform[] waypoints;
    // Walk speed that can be set in Inspector
    [SerializeField]
    private float moveSpeed = 2f;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    // STATES
    public enum NPC_BEHAVIOR { STATIONARY, WANDERING, GOING_HOME, NONE }
    public NPC_BEHAVIOR npcBehavior;
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
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        this.SetPosition();
    }

    // Update is called once per frame
    private void Update()
    {
        string nameOfNpc = GetComponent<DialogueTrigger>()?.NPC_NAME;

        if (TimeManager.instance.playerData.playerTime.m_ActualHourInRealLife >= 19 && TimeManager.instance.playerData.playerTime.m_IsDaytime == false)
        {
            this.GoHome(); return;
        }
        else
        {
            if (TimeManager.instance.playerData.playerTime.m_IsDaytime == true && TimeManager.instance.playerData.playerTime.m_ActualHourInRealLife >= 8 
                && this.npcBehavior == NPC_BEHAVIOR.GOING_HOME)
            {
                this.npcBehavior = NPC_BEHAVIOR.WANDERING;
                this.spriteRenderer.color = new Color(255, 255, 255, 1);
            }

            if (this.npcBehavior == (int)NPC_BEHAVIOR.STATIONARY)
            {
                animator.SetFloat("Speed", 0);
            }
            else
            {
                if (this.npcBehavior == NPC_BEHAVIOR.WANDERING)
                {
                    if (DialogueManager._instance.isTalking && nameOfNpc.ToUpper() == DialogueManager._instance.npcName.ToUpper())
                    {
                        agent.isStopped = true;
                        return;
                    }
                    // Move Enemy
                    this.Move();
                }
            }
        }
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
        foreach (NPC_INFO npcInfo in DataPersistenceManager.instance.playerData.npcInfos)
        {
            if (gameObject.name.ToUpper() == npcInfo.name.ToUpper())
            {
                this.npcInfo = npcInfo;

                if (this.npcInfo.xPos != 0f || this.npcInfo.yPos != 0f)
                    transform.position = new Vector2(this.npcInfo.xPos, this.npcInfo.yPos);
            }
        }
    }

    IEnumerator WalkAgain()
    {
        yield return new WaitForSeconds(2);

        this.npcBehavior = NPC_BEHAVIOR.WANDERING;
    }
}
