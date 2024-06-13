using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField input;
    public Transform roomContent;
    public UIRoomProfile roomPrefab;
    public List<RoomInfo> updatedRooms;
    public List<RoomProfile> rooms = new List<RoomProfile>();
    private void Start()
    {
        this.input.text = "Room1";
    }
    public virtual void CreateRoom()
    {
        string roomName = input.text;
        Debug.Log("Create room " + roomName);
        PhotonNetwork.CreateRoom(roomName);
        this.UpdateRoomProfileUI();
    }
    public virtual void JoinRoom()
    {
        string roomName = input.text;
        Debug.Log("Join room " + roomName);
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        //PhotonNetwork.LoadLevel("GameOnline");
    }
    public virtual void StartGame()
    {
        Debug.Log("Start game");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameOnlineTest");
        }
        else Debug.Log("You are not master client");...
    public virtual void LeaveRoom()
    {
        Debug.Log("Leave room");
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed: " + message);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
        this.updatedRooms = roomList;

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) this.RoomRemove(roomInfo);
            else this.RoomAdd(roomInfo);
        }
        this.UpdateRoomProfileUI();
    }

    protected virtual void RoomAdd(RoomInfo roomInfo)
    {
        RoomProfile roomProfile = new RoomProfile
        {
            name = roomInfo.Name
        };
        this.rooms.Add(roomProfile);

    }

    protected virtual void UpdateRoomProfileUI()
    {
        foreach(Transform child in this.roomContent)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomProfile roomProfile in this.rooms)
        {
            UIRoomProfile uiRoomProfile = Instantiate(this.roomPrefab);
            uiRoomProfile.SetRoomProfile(roomProfile);
            uiRoomProfile.transform.SetParent(this.roomContent);
        }
    }

    protected virtual void RoomRemove(RoomInfo roomInfo)
    {
        RoomProfile roomProfile = this.RoomByName(roomInfo.Name);
        if (roomProfile == null) return;
        this.rooms.Remove(roomProfile);
    }

    protected virtual RoomProfile RoomByName(string name)
    {
        foreach (RoomProfile roomProfile in this.rooms)
        {
            if (roomProfile.name == name) return roomProfile;
        }
        return null;
    }
}
