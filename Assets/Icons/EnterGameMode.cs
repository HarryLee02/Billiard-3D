using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterGameMode : MonoBehaviour
{
    // Start is called before the first frame update
    public void PwF_Button()
    {
        SceneManager.LoadScene(3); //0 la game room 2 la sanh game 1 la giao dien dang nhap
    }
    public void Challenge_Button()
    {
        SceneManager.LoadScene(0);
    }
    public void Training_Button()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitRoom_Button ()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(2);
    }
}
