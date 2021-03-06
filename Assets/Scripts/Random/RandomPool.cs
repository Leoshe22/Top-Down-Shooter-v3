using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RandomPool<T> : RandomSupplier<T>, IResettable
{
    [SerializeField]
    List<TypePair<int, T>> _objectPool;

    private List<T> pool;
    
    public bool Empty => pool.Count == 0;

    [ShowInInspector, ReadOnly]
    public int CurrentCount => pool.Count;
    [ShowInInspector, ReadOnly]
    public int DefaultCount => _objectPool.Sum(x => x.Value1);
    [ShowInInspector, ReadOnly]
    public List<(int, T)> CountOfEach => pool.CountUnique();

    public void Reset() {
        pool = new List<T>();
        foreach(var item in _objectPool) {
            for (int i = 0; i < item.Value1; i++) {
                pool.Add(item.Value2);
            }
        }
    }

    void OnEnable() {
        Reset();   
    }

    public override T GetRandom() {
        if (pool.Count == 0) return default;

        int index = (int)(RandomValue() * pool.Count);
        
        var random = pool[index];
        pool.RemoveAt(index);

        return random;
    }

    public virtual float RandomValue() => Random.value;
}
