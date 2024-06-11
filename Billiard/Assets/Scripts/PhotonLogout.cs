using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonLogout : MonoBehaviour
{
    public virtual void Logout() {
        Debug.Log("Logout");
        PhotonNetwork.Disconnect();
    }
}
