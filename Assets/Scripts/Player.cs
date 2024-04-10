using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(ConstantForce))]
[RequireComponent(typeof(Rigidbody))]
public class Player : Damageble, IPunObservable
{
    [SerializeField] private float _forwardForce;
    [SerializeField] private float _strafeForce;
    [SerializeField] private float _rollForce;
    [SerializeField] private float _sensitivityX;
    [SerializeField] private float _sensitivityY;
    [SerializeField] private Transform _weapon;
    [SerializeField] private float _damage;
    [SerializeField] private Spawner _spawner;

    private ConstantForce _constantForce;
    private Rigidbody _rigidbody;
    private PhotonView _photonView;
    private Vector3 _startPosition;

    public override void OnDie()
    {
        Respawn();
    }

    [PunRPC]
    public void ApplayDamage(int id, float damage)
    {
        PhotonView photonView = PhotonView.Find(id);

        if (photonView == null)
            return;

        var player = photonView.GetComponent<Player>();

        if (player == null)
            return;

        player.TakeDamage(_damage);
    }

    [PunRPC]
    public void SpawnLaser(Vector3 startPoint, Vector3 endPoint)
    {
        _spawner.Pull(startPoint, endPoint);
    }

    public void SetStartPosition(Vector3 position)
    {
        _startPosition = position;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
    }

    private void Awake()
    {
        _spawner = FindObjectOfType<Spawner>();
        FindObjectOfType<MiniMap>().Create(transform);
        _health = 100f;
        _constantForce = GetComponent<ConstantForce>();
        _rigidbody = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            _constantForce.relativeForce = Vector3.forward * _forwardForce * Input.GetAxis("Vertical") + Vector3.right * _strafeForce * Input.GetAxis("Horizontal");
            _constantForce.relativeTorque = Vector3.forward * _rollForce * Input.GetAxis("Roll") + Vector3.right * _sensitivityY * Input.GetAxis("Mouse Y") 
                + Vector3.up * _sensitivityX * Input.GetAxis("Mouse X");

            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        if(Physics.Raycast(_weapon.position,_weapon.forward, out RaycastHit hitInfo, 500f))
        {
            var victim = hitInfo.rigidbody?.GetComponent<Player>();

            if (victim != null)
            {
                victim.TakeDamage(_damage);
                _photonView.RPC("ApplayDamage", RpcTarget.Others, victim.gameObject.GetPhotonView().ViewID, _damage);
            }

            _photonView.RPC("SpawnLaser", RpcTarget.All, _weapon.position, hitInfo.point);
        }
        else
        {
            _photonView.RPC("SpawnLaser", RpcTarget.All, _weapon.position, _weapon.position + _weapon.forward * 500f);
        }
    }

    private void Respawn()
    {
        if (_photonView.IsMine)
        {
            transform.position = _startPosition;
            _health = 100f;
        }
    }
}
