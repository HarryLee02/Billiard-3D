using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SimpleObjectMover : MonoBehaviourPun
{
    private float speed = 0.1f;
    private void Update() {
        if (photonView.IsMine) {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            transform.Translate(new Vector3(horizontal, vertical, 0) * speed);
        }
    }
}
