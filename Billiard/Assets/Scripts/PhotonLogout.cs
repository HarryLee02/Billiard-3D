using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PhotonLogout : MonoBehaviour
{
    public virtual void Logout() {
        Debug.Log("Logout");
        PhotonNetwork.Disconnect();
    }
    public void BackToMenu() {
        SceneManager.LoadScene("InGameMenu");
        Logout();
    }
}
