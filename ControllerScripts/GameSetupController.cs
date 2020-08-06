using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameSetupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();//Create a networked player object for each player that loads into the multiplayer lobby
    }

    private void CreatePlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomX = Random.Range(-10, -6);
            int randomY = Random.Range(7, 8);
            Debug.Log("Creating player");
            PhotonNetwork.InstantiateSceneObject("Player.prefab", new Vector3(randomX, randomY), Quaternion.identity);
        }
        }
}
