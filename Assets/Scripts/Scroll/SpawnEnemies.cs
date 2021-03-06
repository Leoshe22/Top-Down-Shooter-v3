using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using Sirenix.OdinInspector;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] int totalEnemies = 10;
    [SerializeField] FloatReference _currentProgress;
    [SerializeField] AnimationCurve _curve;
    [SerializeField] Transform _currentPathPosition;

    [SerializeField] Vector2 _xSpawnRange = new Vector2(-5,5);
    [SerializeField] RandomSupplier<GameObject> enemiesToSpawn;
    [ShowInInspector, ReadOnly]
    int enemiesSpawned = 0;

    private List<GameObject> used = new List<GameObject>();
    [ShowInInspector, ReadOnly]
    public List<(int, GameObject)> CountOfEachUsed => used.CountUnique();

    public event System.Action<GameObject> OnSpawnEnemy;

    void Update() {
        var currentValue = GetCurrentValue();
        if (currentValue > enemiesSpawned) {
            SpawnEnemy(currentValue - enemiesSpawned);
            enemiesSpawned = currentValue;
        }
    }

    int GetCurrentValue() {
        return (int)(totalEnemies * _curve.Evaluate(_currentProgress.Value));
    }

    public void SpawnEnemy(int times) {
        for (int i = 0; i < times; i++) SpawnEnemy();
    }

    public void SpawnEnemy() {
        Vector3 offset = Vector2.up * 14.4f;

        var enemyToSpawn = enemiesToSpawn.GetRandom();
        if (enemyToSpawn == null) return;

        var enemy = Instantiate(
            enemyToSpawn, 
            GetPosition(offset), 
            Quaternion.Euler(0,0,180)
        );
        used.Add(enemyToSpawn);
        //bad
        if (enemy.TryGetComponent<DestroyIfTooFarAwayFrom>(out var destroyIf)) {
            destroyIf.SetTransform(_currentPathPosition);
        }

        OnSpawnEnemy?.Invoke(enemy);
    }

    private Vector3 GetPosition(Vector3 offset) {
        return _currentPathPosition.position + offset + 
               (Vector3.right * Random.Range(_xSpawnRange.x, _xSpawnRange.y));
    }

}
