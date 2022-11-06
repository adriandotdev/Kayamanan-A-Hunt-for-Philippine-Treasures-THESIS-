using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDataPersistence
{ 
    [Header("UI")]
    public TMPro.TextMeshProUGUI characterNameText;

    [Header("Joystick Controller")]
    public Joystick joystick;

    [Header("Animator")]
    public Animator animator;

    [Header("Speed")]
    public float speed = 7;

    [Header("Virtual Camera")]
    public Cinemachine.CinemachineVirtualCamera virtualCam;

    [Header("Sprites")]
    public Sprite up;
    public Sprite down;

    Vector3 movement;

    bool isMoving = false;

    // Player Data
    public PlayerData playerData;

    private void Start()
    {
        this.joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        this.virtualCam = GameObject.Find("Virtual Cam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        this.virtualCam.Follow = transform;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        

        if (horizontal != 0 || vertical != 0)
        {
            animator.SetFloat("Speed", 1);
            isMoving = true;
        }
        else
        {
            animator.SetFloat("Speed", 0);
            isMoving = false;
        }

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        movement = new Vector3(horizontal, vertical) * speed * Time.deltaTime;

        //if (movement != Vector3.zero)
        //    SoundManager.instance?.PlaySound("Wood Footsteps");

        transform.position += movement;

        if (isMoving == false && DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.playerData.xPos = transform.position.x;
            DataPersistenceManager.instance.playerData.yPos = transform.position.y;
            DataPersistenceManager.instance.SaveGame();
        }
    }

    public void LoadPlayerData(PlayerData playerData)
    {
        this.playerData = playerData;
    }

    public void SavePlayerData()
    {
        throw new System.NotImplementedException();
    }
    public void LoadSlotsData(Slots slots)
    {
        throw new System.NotImplementedException();
    }
    public void SaveSlotsData(ref Slots slots)
    {
        throw new System.NotImplementedException();
    }
}
