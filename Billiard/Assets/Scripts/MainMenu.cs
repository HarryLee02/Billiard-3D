using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Slider _MusicSlider, _SFXSlider;
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
    }
    public void EnterGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void GoOnline() {
        SceneManager.LoadScene(3);
    }
    public void ReturnMainMenu() {
        SceneManager.LoadScene(0);
    }
    public void EnterTraining() {
        SceneManager.LoadScene(2);
    }
    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
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
        _MusicSlider.value = StaticToken.musicVolume;
        _SFXSlider.value = StaticToken.sfxVolume;
        MusicVolume();
        SFXVolume();
    }
}
