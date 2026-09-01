using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class PlayerStateEntry
{
    [SerializeField] private string key;
    [SerializeField] private PlayerState sourceState;

    [NonSerialized] private PlayerState runtimeState;

    public string Key => key;
    public PlayerState SourceState => sourceState;
    public PlayerState RuntimeState => runtimeState;

    public bool CreateRuntimeCopy()
    {
        if(sourceState == null)
        {
            runtimeState = null;
            return false;
        }

        runtimeState = UnityEngine.Object.Instantiate(sourceState);

        runtimeState.name = $"{sourceState.name} (Runtime)";
        runtimeState.hideFlags = HideFlags.DontSave;

        return true;
    }

    public void DestroyRuntimeCopy()
    {
        if(runtimeState == null)
        {
            return;
        }

        UnityEngine.Object.Destroy(runtimeState);
        runtimeState = null;
    }
}

[Serializable]
public class PlayerStateList
{
    [SerializeField] private List<PlayerStateEntry> stateList = new();

    private readonly Dictionary<string, PlayerState> runtimeStateByKey = new(StringComparer.Ordinal);

    public void CreateRunTimeCopies()
    {
        DestroyRunTimeCopies();

        foreach (var entry in stateList)
        {
            if(entry == null || string.IsNullOrWhiteSpace(entry.Key))
            {
                continue;
            }

            if(!entry.CreateRuntimeCopy())
            {
                Debug.LogWarning($"元ステートが設定されていません: {entry.Key}");
                continue;
            }

            if(!runtimeStateByKey.TryAdd(entry.Key, entry.RuntimeState))
            {
                Debug.LogWarning($"キーが重複しています: {entry.Key}");
                entry.DestroyRuntimeCopy();
            }
        }
    }

    public bool TryGetRuntimeState(string key, out PlayerState state)
    {
        return runtimeStateByKey.TryGetValue(key, out state);
    }

    public void DestroyRunTimeCopies()
    {
        foreach (var entry in stateList)
        {
            entry?.DestroyRuntimeCopy();
        }
        runtimeStateByKey.Clear();
    }
}
