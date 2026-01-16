using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] songs;
    [SerializeField] private TMP_Text songNameTxt;
    private AudioSource audioSource;
    private int currentSongIndex = -1;
    private List<AudioClip> songsNotPlayed;
    private AudioClip currentSong;

    [Header("Volume")]
    [SerializeField] private Slider volumeSlider;
    private const string VolumePrefKey = "MusicVolume";

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        songsNotPlayed = new List<AudioClip>(songs);

        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.5f);
        audioSource.volume = savedVolume;


        if(volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    private void Start()
    {
        PlayNextSong();
    }

    private void Update()
    {
        if(!audioSource.isPlaying)
        {
            PlayNextSong();
        }    
    }

    private void PlayNextSong()
    {
        if (songsNotPlayed.Count == 0)
        {
            songsNotPlayed = new List<AudioClip>(songs);
        }

        if(currentSongIndex != -1)
        {
            songsNotPlayed.Remove(currentSong);
        }

        currentSongIndex = Random.Range(0, songsNotPlayed.Count);
        Debug.Log("Playing song "+ currentSongIndex);

        audioSource.clip = songsNotPlayed[currentSongIndex];
        audioSource.Play();

        string songTxt;

        if(audioSource.clip.name.Length > 25)
        {
            songTxt = (audioSource.clip.name + "\n- Kevin MacLeod");
        }
        else
        {
            songTxt = (audioSource.clip.name + " - Kevin MacLeod");
        }

        songNameTxt.text = songTxt;



    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat(VolumePrefKey, value);
    }


}
