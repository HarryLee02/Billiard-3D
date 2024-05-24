using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] bgmSounds, sfxSounds;
    public AudioSource bgmSource, sfxSource;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    private void Start() {
        PlayMusic("Theme");
    }
    public void PlayMusic(string name) {
        Sound s = System.Array.Find(bgmSounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else {
            bgmSource.clip = s.clip;
            bgmSource.Play();
        }
    }
    public void PlaySFX(string name) {
        Sound s = System.Array.Find(sfxSounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void MusicVolume(float volume) {
        bgmSource.volume = volume;
    }
    public void SFXVolume(float volume) {
        sfxSource.volume = volume;
    }
}
