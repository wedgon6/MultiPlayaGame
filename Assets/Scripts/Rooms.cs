using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Rooms : MonoBehaviour
{
   public void QuicGame()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
}
