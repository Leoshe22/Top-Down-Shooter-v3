using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityAtoms.BaseAtoms;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] BulletPrefabReference _bulletPrefab;

    [SerializeField] PlayerInput _input;

    [SerializeField] PlayerStats _stats;

    public event Action<BulletManager> OnShoot;

    [ShowInInspector, ReadOnly]
    float timer = 0;

    void Update() {
        if (GameManager.I.State != GameState.Game) return;
        timer += Time.deltaTime;
        if (timer > 1f/_stats.ShotsPerSecond) {
            if (_input.Shoot) {
                Fire();
                timer = 0;
            }
        }
    }

    void Fire() {
        var bullet = Instantiate(_bulletPrefab.Value, transform.position, transform.rotation);
        bullet.SetStats(_stats);
        OnShoot?.Invoke(bullet);
    }
}