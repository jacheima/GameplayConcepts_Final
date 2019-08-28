using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource[] sfx;
    public Slider volume;
    public Slider sfxVol;

    public static AudioManager instance = null;
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {

            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }


    public void GetSlider()
    {
        volume = GameManager.instance.musicVol;
        sfxVol = GameManager.instance.fxVol;

        if (!PlayerPrefs.HasKey("music"))
        {
            PlayerPrefs.SetFloat("music", 1);
            PlayerPrefs.SetFloat("sfx", 1);
            volume.value = 1;
            sfxVol.value = 1;
            music.volume = volume.value;

            for (int i = 0; i < sfx.Length; i++)
            {
                sfx[i].volume = sfxVol.value;
            }

        }
        else
        {
            volume.value = PlayerPrefs.GetFloat("music");
            sfxVol.value = PlayerPrefs.GetFloat("sfx");
            music.volume = volume.value;

            for (int i = 0; i < sfx.Length; i++)
            {
                sfx[i].volume = sfxVol.value;
            }
        }
    }

    void Start()
    {
        music.volume = PlayerPrefs.GetFloat("music");

        for (int i = 0; i < sfx.Length; i++)
        {
            sfx[i].volume = PlayerPrefs.GetFloat("sfx");
        }
    }

    void Update()
    {
        if (volume != null)
        {
            music.volume = volume.value;
        }

        if (sfxVol != null)
        {
            for (int i = 0; i < sfx.Length; i++)
            {
                sfx[i].volume = sfxVol.value;
            }
        }
       
    }

    public void VolumePrefs()
    {
        Debug.Log("Saving Again");
        PlayerPrefs.SetFloat("music", music.volume);
        PlayerPrefs.SetFloat("sfx", sfxVol.value);
        PlayerPrefs.Save();

        Debug.Log("Saved!");
    }

}
