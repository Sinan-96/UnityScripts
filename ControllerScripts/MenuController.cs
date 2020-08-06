using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Random = UnityEngine.Random;

public class MenuController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject quickStartButton; // button used for creating and joining a game
    [SerializeField]
    private GameObject quickCancelButton;// button used to stop searching for a game to join
    [SerializeField]
    private byte roomSize; // Maximum number of players in a room at a time
    private LoadBalancingClient lbc; // to handle searching for room withs specific properties


    public void Start()
    {
        quickCancelButton.SetActive(false);
    }
    public override void OnConnectedToMaster() // Callback function for when the first connection is established
    {
        PhotonNetwork.AutomaticallySyncScene = true; // Makes it so whatever scene the master client has, the other client also has
        quickStartButton.SetActive(true);
    }

    public void QuickStart()
    {
        if (roomSize == 0) // if quickstart button is pressed
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            quickStartButton.SetActive(false);
            quickCancelButton.SetActive(true);
            OpJoinRandomRoomParams opJoinRandomRoomParams = new OpJoinRandomRoomParams();
            opJoinRandomRoomParams.ExpectedMaxPlayers = roomSize;
            lbc.OpJoinRandomRoom();
        }
        Debug.Log("Quick start"); // Tries to join an existing room
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    void CreateRoom()//Trying to create our own room
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000); // Creating a random name for the room
        if (roomSize == 0) // if quickstart player
        {
            if (randomRoomNumber % 2 == 0) // 50-50 chance that they create a room for 2 or 3 players
            {
                RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
            }
            else
            {
                RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 3 };
            }
        }
        else
        {
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = roomSize };
            PhotonNetwork.CreateRoom("Room " + randomRoomNumber, roomOps);
            Debug.Log(randomRoomNumber);
        }

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room ... trying again");
        CreateRoom();//Rettrying to create a room with a different name(most likely cause for failure)
    }

    public void QuickCancel() 
    {
        quickCancelButton.SetActive(false);
        quickStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

}
