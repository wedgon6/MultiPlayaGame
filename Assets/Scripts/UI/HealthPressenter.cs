using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPressenter : MonoBehaviour
{
    private Damageble _damageble;
    private float _maxSize;
    private RectTransform _healthBar;

    public void Connect(Damageble damageble)
    {
        _healthBar = transform as RectTransform;
        _damageble = damageble;
        _maxSize = _healthBar.sizeDelta.x;
    }

    private void Update()
    {
        if(_damageble != null)
        {
            _healthBar.sizeDelta = new Vector2((_maxSize * _damageble.Health) / 100, _healthBar.sizeDelta.y);
        }
    }
}
