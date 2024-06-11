using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Slider _MusicSlider, _SFXSlider;

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    private void Start() {
        Cursor.lockState = CursorLockMode.None;
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ExitGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(1);
    }
    public void Restart() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
