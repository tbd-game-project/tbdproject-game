using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [Serializable]
    public struct EffectConfig
    {
        public string effectKey;            // Unique identifier for the effect
        public GameObject effectPrefab;     // Prefab of the effect to be pooled
        public int defaultCapacity;         // Initial pool size
        public int maxCapacity;             // Maximum pool size
    }

    [SerializeField] private List<EffectConfig> _effectConfigs = new List<EffectConfig>();
    private Dictionary<string, EffectConfig> _configDictionary = new Dictionary<string, EffectConfig>();
    private Dictionary<string, ObjectPool<GameObject>> _effectDictionary = new Dictionary<string, ObjectPool<GameObject>>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        foreach(var originalConfig in _effectConfigs)
        {
            if (string.IsNullOrWhiteSpace(originalConfig.effectKey))
            {
                Debug.LogWarning("EffectManagerÇ…ãÛÇÃEffectKeyÇ™ìoò^Ç≥ÇÍÇƒÇ¢Ç‹Ç∑ÅB");

                continue;
            }

            if (originalConfig.effectPrefab == null)
            {
                Debug.LogWarning($"EffectKey '{originalConfig.effectKey}' Ç…PrefabÇ™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒÅB");

                continue;
            }


            if (_configDictionary.ContainsKey(originalConfig.effectKey))
            {
                Debug.LogError($"EffectKey '{originalConfig.effectKey}' Ç™èdï°ÇµÇƒÇ¢Ç‹Ç∑ÅB");

                continue;
            }

            string key = originalConfig.effectKey;

            int defaultCapacity = Mathf.Max(0, originalConfig.defaultCapacity);

            int maxCapacity = Mathf.Max(1, originalConfig.maxCapacity);

            maxCapacity = Mathf.Max(defaultCapacity, maxCapacity);

            EffectConfig config = originalConfig;

            config.defaultCapacity = defaultCapacity;
            config.maxCapacity = maxCapacity;

            _configDictionary.Add(key, config);

            var pool = new ObjectPool<GameObject>(
                    createFunc: () => CreateInstance(key),
                    actionOnGet: OnGetFromPool,
                    actionOnRelease: OnReleaseToPool,
                    actionOnDestroy: OnDestroyPoolObject,
                    collectionCheck: false,
                    defaultCapacity: defaultCapacity,
                    maxSize: maxCapacity
                );

            _effectDictionary.Add(key, pool);

            // Pre-warm the pool with the default capacity
            List<GameObject> warmUpList = new List<GameObject>();
            for(int i = 0; i < config.defaultCapacity ; i++)
            {
                warmUpList.Add(pool.Get());
            }

            // Return the pre-warmed instances back to the pool
            foreach (var obj in warmUpList)
            {
                pool.Release(obj);
            }
        }

    }

    // --- Callbacks for pool control ---
    private GameObject CreateInstance(string key)
    {
        EffectConfig config = _configDictionary[key];

        GameObject obj = Instantiate(config.effectPrefab, transform);

        obj.name = $"PooledEffect_{key}";

        PooledEffect pooledEffect = obj.GetComponent<PooledEffect>();

        if (pooledEffect == null)
        {
            pooledEffect = obj.AddComponent<PooledEffect>();
        }

        pooledEffect.RegisterReturnAction(targetObj =>
        {
            if (_effectDictionary.TryGetValue(key, out var pool))
            {
                pool.Release(targetObj);
            }
        });

        obj.SetActive(false);

        return obj;
    }

    private void OnGetFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReleaseToPool(GameObject obj)
    {
        if(obj.TryGetComponent<PooledEffect>(out var ps))
        {
            ps.StopAndClear();
        }

        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        // Instances that exceed the maximum capacity will be destroyed
        Destroy(obj);
    }

    public GameObject PlayEffect(string key, Vector3 position, Quaternion rotation)
    {
        if (!_effectDictionary.TryGetValue(key, out var pool)) 
        {
            Debug.LogError($"EffectKey '{key}' ÇÕìoò^Ç≥ÇÍÇƒÇ¢Ç‹ÇπÇÒÅB");

            return null;
        }

        GameObject obj = pool.Get();
        obj.transform.SetPositionAndRotation(position, rotation);

        if (obj.TryGetComponent<PooledEffect>(out var pooledEffect))
        {
            pooledEffect.Play();
        }

        return obj;
    }

    public GameObject PlayEffect(string key, Vector3 Position)
    {
        return PlayEffect(key, Position, Quaternion.identity);
    }

    public void StopEffect(GameObject effectObject)
    {
        if (effectObject == null)
        {
            return;
        }

        if (effectObject.TryGetComponent<PooledEffect>(out var pooledEffect))
        {
            pooledEffect.StopAndReturn();
        }
    }

}
