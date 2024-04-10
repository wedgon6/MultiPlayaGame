using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private HealthPressenter _healthPressenter;
    [SerializeField] private MiniMap _miniMap;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            var player = PhotonNetwork.Instantiate(_prefab.name, transform.position, Quaternion.identity).GetComponent<Player>();
            _cameraTransform.SetParent(player.transform.GetChild(0));
            _cameraTransform.transform.localPosition = Vector3.zero;
            _cameraTransform.transform.localRotation = Quaternion.identity;
            _healthPressenter.Connect(player);
            _miniMap.Initialize(player.transform);
        }
    }
}
