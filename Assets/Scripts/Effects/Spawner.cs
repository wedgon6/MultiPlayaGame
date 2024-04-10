using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnableObject _spawnableObject;
    [SerializeField] private float _pushDelay = 0.3f;

    private Queue<SpawnableObject> _spawnableObjectsQueue = new Queue<SpawnableObject>();
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void Pull(Vector3 startPoint, Vector3 endPoint)
    {
        if (_spawnableObjectsQueue.Count == 0)
        {
            Instantiate(_spawnableObject, Vector3.zero, Quaternion.identity).Initialize(this).Push();
        }

        var spawned = _spawnableObjectsQueue.Dequeue();
        spawned.Pull(startPoint, endPoint).PushDelayed(_pushDelay);
    }

    internal void Push(SpawnableObject spawnableObject)
    {
        spawnableObject.SetActive(false);
        _spawnableObjectsQueue.Enqueue(spawnableObject);
    }
}
