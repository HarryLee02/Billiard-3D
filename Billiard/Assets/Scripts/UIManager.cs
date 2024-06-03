using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    [SerializeField] Slider _MusicSlider, _SFXSlider;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    private void Start() {
        if (PlayerPrefs.HasKey("MusicVolume") 
        && PlayerPrefs.HasKey("SFXVolume")
        ) {
            LoadVolume();
        }
        else {
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
            PlayerPrefs.SetFloat("SFXVolume", 0.5f);
            LoadVolume();
        }
        if (_MusicSlider.value != StaticToken.musicVolume && StaticToken.musicVolume != -1f) {
            _MusicSlider.value = StaticToken.musicVolume;
            PlayerPrefs.SetFloat("MusicVolume", StaticToken.musicVolume);
        }
        if (_SFXSlider.value != StaticToken.sfxVolume && StaticToken.sfxVolume != -1f) {
            _SFXSlider.value = StaticToken.sfxVolume;
            PlayerPrefs.SetFloat("SFXVolume", StaticToken.sfxVolume);
        }
    }
    //Functions to change the login screen UI
    public void LoginScreen() //Back button
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
    }
    public void RegisterScreen() // Regester button
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
    }
    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(_MusicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", _MusicSlider.value);
        StaticToken.musicVolume = _MusicSlider.value;
    }
    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(_SFXSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", _SFXSlider.value);
        StaticToken.sfxVolume = _SFXSlider.value;
    }
    private void LoadVolume() {
        _MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        _SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        MusicVolume();
        SFXVolume();
    }
}
