using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    private GameObject _gameObject;
    private Spawner _spawner;
    private LineRenderer _lineRenderer;

    private float _pushTimer = -1;

    public SpawnableObject Initialize(Spawner spawner)
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _spawner = spawner;
        _gameObject = gameObject;
        gameObject.SetActive(false);
        return this;
    }

    public void Push()
    {
        _spawner.Push(this);
    }

    public SpawnableObject Pull(Vector3 start, Vector3 end)
    {
        SetActive(true);
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
        return this;
    }

    public void SetActive(bool isActive)
    {
        _gameObject.SetActive(isActive);
    }

    private void Update()
    {
        if (_pushTimer >= 0)
        {
            _pushTimer -= Time.deltaTime;

            if (_pushTimer < 0)
            {
                _pushTimer = -1f;
                Push();
            }
        }
    }

    public void PushDelayed(float time)
    {
        _pushTimer = time;
    }
}
