using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnershipTransfer : MonoBehaviourPunCallbacks {
    private void Update() {
        Player p1 = PhotonNetwork.PlayerList[0];
        Player p2 = PhotonNetwork.PlayerList[1];
        if (StaticToken.p1Turn) {
            base.photonView.TransferOwnership(p1);
        } else {
            base.photonView.TransferOwnership(p2);
        }
    }
}
