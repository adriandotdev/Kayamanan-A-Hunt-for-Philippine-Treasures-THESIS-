using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * <summary> 
 *  This class is responsible for spawning the player based
 *  on the last or saved position.
 * </summary> 
 */
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;
    public string nameOfExit;
    public bool fromEnter = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnPlayerHouseLoaded;
        SceneManager.sceneLoaded += OnSceneRequiresTransitionLoaded;
        SceneManager.sceneLoaded += OnVacationSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnPlayerHouseLoaded;
        SceneManager.sceneLoaded -= OnSceneRequiresTransitionLoaded;
        SceneManager.sceneLoaded -= OnVacationSceneLoaded;
    }

    public void OnPlayerHouseLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "House")
        {
            this.nameOfExit = "Building"; // The name of gameobject that is the referenced player's position.

            Vector2 position;

            /**
             * <summary>
             *  Una, ichecheck muna kung ang ni-load na profile ay bagong created o hindi.
             *  
             *  Sa else block naman, 
             *  iche-check naman kung galing sa loob o labas ng bahay ang player
             *  kung galing siya sa loob, maseset siya sa entrance ng bahay based
             *  sa given value sa variable na 'nameOfExit'. 
             *  
             *  If hindi naman siya galing sa loob o labas ng bahay,
             *  iseset lang siya sa last saved position ng player.
             *  
             *  NOTE:
             *  ang property na 'fromEnter' ay sineset ng 
             *  SceneTransition script. 
             *  
             *  TIGNAN NA LANG ANG SceneTransition script.
             * </summary>
             */
            if (DataPersistenceManager.instance.playerData.isNewlyCreated)
            {
                DataPersistenceManager.instance.playerData.isIntroductionDone = true;
                DataPersistenceManager.instance.playerData.isNewlyCreated = false;
                position = GameObject.Find(this.nameOfExit).transform.GetChild(0).position;
                DataPersistenceManager.instance.playerData.xPos = position.x;
                DataPersistenceManager.instance.playerData.yPos = position.y;
            }
            else
            {
                if (DataPersistenceManager.instance.playerData.isFromSleeping)
                {
                    DataPersistenceManager.instance.playerData.isFromSleeping = false;
                    DataPersistenceManager.instance.playerData.isSleepPanelNotShown = true;
                    position = GameObject.Find("Bed Spawnpoint").transform.position;
                    this.SpawnPlayerCharacter(position, scene.name);
                    return; 
                }
                if (fromEnter)
                {
                    fromEnter = false;
                    position = GameObject.Find(this.nameOfExit).transform.GetChild(0).position;
                }
                else
                {
                    position = new Vector2(DataPersistenceManager.instance.playerData.xPos, DataPersistenceManager.instance.playerData.yPos);
                }
                    
            }

            this.SpawnPlayerCharacter(position, scene.name);
        }
    }

    public void OnVacationSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Vacation Scene")
        {
            print("IT IS VACATION SCENE");

            Vector2 position;

            position = GameObject.Find("Player Spawn Point").transform.position;

            GameObject player = null;

            if (DataPersistenceManager.instance.playerData.gender == "male")
            {
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            }
            else
            {
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Female"));
            }

            player.transform.position = new Vector2(position.x, position.y);
        }
    }

    void OnSceneRequiresTransitionLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Outside" || scene.name == "Museum" 
            || scene.name == "School" || scene.name == "Church")
        {
            Vector2 position;
            /**
             * <summary>
             *  Since ang default scene pag nag create ng new profile is ang 'House' scene,
             *  direct na natin ichecheck if ang outside scene ay nag load kung from entrance 
             *  ba sa loob o labas galing si player.
             *  
             *  Also the player's position sa 'Outside' scene is nasasaved din if ever na hindi
             *  siya galing sa loob ng bahay.
             * </summary>
             */
            //try
            //{
                if (fromEnter)
                {
                    fromEnter = false;
                    position = GameObject.Find(this.nameOfExit).transform.GetChild(0).position;
                }
                else
                {
                    position = new Vector2(DataPersistenceManager.instance.playerData.xPos, DataPersistenceManager.instance.playerData.yPos);
                }

                this.SpawnPlayerCharacter(position, scene.name);
            //}
            //catch (System.Exception e) { print("ERROR FROM SCENE TRANSITION MANAGER"); }
        }
    }

    private void SpawnPlayerCharacter(Vector2 position, string sceneName)
    {
        GameObject player = null;

        if (DataPersistenceManager.instance.playerData.gender == "male")
        {
            player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            this.ChangePlayerMinimapIconSize(player, sceneName);
        }
        else
        {
            player = Instantiate(Resources.Load<GameObject>("Prefabs/Female"));
            this.ChangePlayerMinimapIconSize(player, sceneName);
        }

        player.transform.position = position;
    }

    private void ChangePlayerMinimapIconSize(GameObject player, string sceneName)
    {
        if (sceneName == "Museum")
        {
            // 2.786297f
            player.transform.GetChild(0).transform.localScale = new Vector2(8.57f, 8.57f);
        }
        else if (sceneName == "Outside" || sceneName == "School")
        {
            player.transform.GetChild(0).transform.localScale = new Vector2(12.53959f, 12.53959f);
        }
    }
    public void LoadHouseScene()
    {
        SceneManager.LoadScene("House");
    }
}
