using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public Sound bgSound;
    public static float previousVolumeValue;

    public Sound[] sounds;
    public Slider slider;
    public Toggle BGMToggle;
    public Toggle SFXToggle;

    public static bool BGToggle;
    public static bool fxToggle;

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.src = gameObject.AddComponent<AudioSource>();
            sound.src.clip = sound.clip;
            sound.src.volume = sound.volume;
            sound.src.pitch = sound.pitch;
            sound.src.loop = sound.isLooping;
        }

        if (instance == null)
        {
            instance = this;
            BGToggle = true;
            fxToggle = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        this.PlayBGMusic("Main BG");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "Menu" || scene.name == "Outside" ||
            scene.name == "House" || scene.name == "Church" || scene.name == "School" ||
            scene.name == "Museum")
        {
            this.slider = GameObject.Find("Slider Initializer").GetComponent<SliderInitializer>().slider;
            this.BGMToggle = GameObject.Find("Slider Initializer").GetComponent<SliderInitializer>().toggleBGMusic;
            this.SFXToggle = GameObject.Find("Slider Initializer").GetComponent<SliderInitializer>().toggleSFX;
            this.BGMToggle.isOn = BGToggle;
            this.SFXToggle.isOn = fxToggle;

            try
            {
                this.slider.value = bgSound.src.volume;
            }
            catch (System.Exception e) { }

            this.BGMToggle.onValueChanged.AddListener(this.ToggleBGM);
            this.SFXToggle.onValueChanged.AddListener(this.ToggleSFX);
            this.slider.onValueChanged.AddListener(Volume);
        }
    }

    public void PlayBGMusic(string name)
    {
        this.PlaySound(name);

        try
        {
            this.slider.value = this.bgSound.src.volume;
        }
        catch(System.Exception e) { }
    }

    public void PlaySound(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name.ToUpper() == name.ToUpper());

        if (sound.name.ToUpper() == "MAIN BG")
        {
            bgSound = sound;
            sound.volume = 0.086f;
            previousVolumeValue = bgSound.src.volume;
        }

        if (sound.name.ToUpper() == "MAIN BG")
        {
            if (!sound.src.isPlaying)
            {
                sound.src.Play();
            }
            else
            {
                sound.src.Stop();
                sound.src.Play();
            }
        }
        else
        {
            if (fxToggle)
            {
                if (!sound.src.isPlaying)
                {
                    sound.src.Play();
                }
                else
                {
                    sound.src.Stop();
                    sound.src.Play();
                }
            }
        }
    }

    public void ToggleBGM(bool toggle)
    {
        BGToggle = toggle;

        if (toggle == true)
        {
            bgSound.src.volume = previousVolumeValue;
            this.slider.value = bgSound.src.volume;
        }
        else
        {
            bgSound.src.volume = 0;
        }
    }

    public void ToggleSFX(bool toggle)
    {
        this.SFXToggle.isOn = toggle;
        fxToggle = toggle;
    } 

    public void Volume(float value)
    {
        if (BGToggle)
        {
            bgSound.volume = value;
            bgSound.src.volume = value;
            previousVolumeValue = bgSound.src.volume;
        }
    }
}

[System.Serializable] 
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.3f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource src;

    public bool isLooping;
}
