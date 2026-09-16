using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }

    [Serializable]
    public struct ManagerConfig
    {
        public string managerName;
        public GameObject managerPrefab;
    }

    [Header("Manager Prefabs")]
    [SerializeField]
    private List<ManagerConfig> managerConfigs = new List<ManagerConfig>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // GameSystemとその子Managerをシーン遷移後も維持
        DontDestroyOnLoad(gameObject);

        InitializeManagers();
    }

    private void InitializeManagers()
    {
        foreach (ManagerConfig config in managerConfigs)
        {
            if (config.managerPrefab == null)
            {
                Debug.LogWarning($"Manager '{config.managerName}' のPrefabが設定されていません。");
                continue;
            }

            Instantiate(config.managerPrefab, transform);
        }
    }
}
