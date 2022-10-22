using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Singleton Class */
public class AlbumManager : MonoBehaviour
{
    public static AlbumManager Instance { get; private set; }

    public ItemGiver[] itemGivers;
    public Item[] items;
    public bool isFirstAlbum;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
