using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 -> top need bottom
    // 2 -> bottom need top
    // 3 -> right need left
    // 4 -> left need right

    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;

    private void Start()
    { 
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", .1f);
    }
    private void Spawn()
    {
        if (spawned == false)
        {
            // need ng bottom
            if (openingDirection == 1)
            {
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 2)
            {
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity);
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint") && collision.GetComponent<RoomSpawner>().spawned == true)
        {
            //if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            //{
            //    Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
            //    Destroy(gameObject);
            //}
            spawned = true;
            //Destroy(gameObject);
        }
    }
}
