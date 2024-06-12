using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallInitializer : MonoBehaviour
{
    public GameObject ball;
    private void Start() {
        PhotonNetwork.Instantiate(ball.name, new Vector3(-0.21f, 0.967f, -0.4427283f), Quaternion.identity);
    }
}
