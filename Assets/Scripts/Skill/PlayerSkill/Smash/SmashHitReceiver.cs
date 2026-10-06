using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class SmashHitReceiver : MonoBehaviour
{
    private const float MinDirectionSqrMagnitude = 0.0001f;

    private struct KnockbackData
    {
        public Vector3 Direction { get; }
        public float Speed { get; }
        public float Duration { get; }

        public KnockbackData(
            Vector3 direction,
            float speed,
            float duration)
        {
            Direction = direction;
            Speed = speed;
            Duration = duration;
        }
    }

    private KnockbackData? pendingData;

    public bool HasPendingHit => pendingData.HasValue;

    public void ReceiveHit(
        Vector3 direction,
        SmashSettings settings)
    {
        if (!isActiveAndEnabled ||
            settings == null ||
            !settings.IsValid)
        {
            return;
        }

        // 盤面と平行に押し戻すため、高さの差は使用しない。
        direction.y = 0f;

        float directionSqrMagnitude = direction.sqrMagnitude;

        if (float.IsNaN(directionSqrMagnitude) ||
            float.IsInfinity(directionSqrMagnitude) ||
            directionSqrMagnitude < MinDirectionSqrMagnitude)
        {
            return;
        }

        // 設定アセットの変更に影響されないよう、命中時の値を保存する。
        // 未処理の命中がある場合は、新しい命中で上書きする。
        pendingData = new KnockbackData(
            direction.normalized,
            settings.KnockbackSpeed,
            settings.KnockbackDuration);
    }

    public bool TryTakeHit(
        out Vector3 direction,
        out float speed,
        out float duration)
    {
        direction = Vector3.zero;
        speed = 0f;
        duration = 0f;

        if (!pendingData.HasValue)
        {
            return false;
        }

        KnockbackData data = pendingData.Value;

        // 同じ命中情報を複数回実行しないよう、取り出したら消す。
        pendingData = null;

        direction = data.Direction;
        speed = data.Speed;
        duration = data.Duration;

        return true;
    }

    private void OnDisable()
    {
        // 再び有効になったときに、以前の命中を実行しないようにする。
        pendingData = null;
    }
}