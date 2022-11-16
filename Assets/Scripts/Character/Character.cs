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

    public Vector3 movement;

    public bool isMoving = false;

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

        if (joystick.gameObject.activeSelf == false)
        {
            isMoving = false;
            animator.SetFloat("Speed", 0);
            joystick.gameObject.transform.GetChild(0).gameObject.transform.localPosition = Vector2.zero;
            joystick.input = Vector2.zero;
            return;
        }

        if (horizontal >= .5f || horizontal <= -.5f || vertical >= .5f || vertical <= -.5f 
            && joystick.Direction != Vector2.zero)
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

        if (isMoving)
        {
            movement = new Vector3(horizontal, vertical) * speed * Time.deltaTime;
            transform.position += movement;
        }

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
