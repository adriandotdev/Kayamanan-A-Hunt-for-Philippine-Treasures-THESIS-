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

    public Animator animator;

    public enum NPC_BEHAVIOR { STATIONARY, WANDERING }
    public NPC_BEHAVIOR npcBehavior;
    public Transform positionOfHome;

    // Use this for initialization
    private void Start()
    {
        // Set position of Enemy as position of the first waypoint
        //transform.position = waypoints[waypointIndex].transform.position;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        string nameOfNpc = GetComponent<DialogueTrigger>().NPC_NAME;

        if (this.npcBehavior == NPC_BEHAVIOR.STATIONARY)
        {
            animator.SetFloat("Speed", 0);
        }
        else
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

            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if (transform.position.x == waypoints[this.waypointIndex].transform.position.x)
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

    IEnumerator WalkAgain()
    {
        yield return new WaitForSeconds(2);

        this.npcBehavior = NPC_BEHAVIOR.WANDERING;
    }
}
