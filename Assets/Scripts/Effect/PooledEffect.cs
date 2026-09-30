using System;
using System.Collections.Generic;
using UnityEngine;

public class PooledEffect : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    private ParticleSystem[] rootParticleSystems;

    private Action<GameObject> returnToPoolAction;

    private bool isPlaying;
    private bool isReturned;

    private void Awake()
    {
        CacheParticleSystems();
    }

    private void Update()
    {
        if (!isPlaying || isReturned)
        {
            return;
        }

        // どれか1つでも生きていればEffectはまだ終了していない
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps != null && ps.IsAlive(true))
            {
                return;
            }
        }

        ReturnToPool();
    }

    /// <summary>
    /// 子オブジェクトを含むParticleSystemを取得する
    /// </summary>
    private void CacheParticleSystems()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);

        if (particleSystems.Length == 0)
        {
            Debug.LogWarning($"Effect '{gameObject.name}' にParticleSystemが存在しません。",gameObject);

            rootParticleSystems = Array.Empty<ParticleSystem>();
            return;
        }

        // ParticleSystemを持つ親が存在しないものだけを
        // 再生開始用のRoot ParticleSystemとして扱う
        var roots = new List<ParticleSystem>();

        foreach (ParticleSystem ps in particleSystems)
        {
            if (!HasParentParticleSystem(ps.transform))
            {
                roots.Add(ps);
            }
        }

        rootParticleSystems = roots.ToArray();
    }

    /// <summary>
    /// 自分より上の階層にParticleSystemがあるか確認する
    /// </summary>
    private bool HasParentParticleSystem(Transform target)
    {
        Transform parent = target.parent;

        while (parent != null && parent != transform.parent)
        {
            if (parent.GetComponent<ParticleSystem>() != null)
            {
                return true;
            }

            if (parent == transform)
            {
                break;
            }

            parent = parent.parent;
        }

        return false;
    }

    /// <summary>
    /// Poolへ戻す処理を登録
    /// </summary>
    public void RegisterReturnAction(Action<GameObject> returnAction)
    {
        returnToPoolAction = returnAction;
    }

    /// <summary>
    /// Effectを最初から再生する
    /// </summary>
    public void Play()
    {
        if (particleSystems == null || particleSystems.Length == 0)
        {
            CacheParticleSystems();
        }

        isReturned = false;
        isPlaying = true;

        // 前回の再生状態を完全にリセット
        foreach (ParticleSystem ps in rootParticleSystems)
        {
            if (ps == null)
            {
                continue;
            }

            ps.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // RootとなるParticleSystemだけ再生
        // 子ParticleSystemはPlay(true)によって一緒に再生される
        foreach (ParticleSystem ps in rootParticleSystems)
        {
            if (ps == null)
            {
                continue;
            }

            ps.Play(true);
        }
    }

    /// <summary>
    /// Effectを停止して状態を初期化
    /// Poolへ戻す際に使用
    /// </summary>
    public void StopAndClear()
    {
        isPlaying = false;

        foreach (ParticleSystem ps in rootParticleSystems)
        {
            if (ps == null)
            {
                continue;
            }

            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    /// <summary>
    /// 強制的にEffectを停止してPoolへ戻す
    /// </summary>
    public void StopAndReturn()
    {
        if (isReturned)
        {
            return;
        }

        StopAndClear();
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (isReturned)
        {
            return;
        }

        isReturned = true;
        isPlaying = false;

        returnToPoolAction?.Invoke(gameObject);
    }
}
