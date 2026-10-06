using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class SmashAttack : MonoBehaviour
{
    private const float MinDirectionSqrMagnitude = 0.0001f;

    [Header("スキル設定")]
    [SerializeField]
    private SmashSettings settings;

    private Player player;
    private float nextUseTime;
    private int lastUseFrame = -1;

    public float RemainingCooldown =>
        Mathf.Max(0f, nextUseTime - Time.time);

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    // ノックバック中などの行動制限は、Player側で確認してから呼ぶ。
    public bool TryUse()
    {
        if (!isActiveAndEnabled ||
            player == null ||
            !player.isActiveAndEnabled ||
            settings == null ||
            !settings.IsValid)
        {
            return false;
        }

        if (Time.timeScale <= 0f ||
            Time.time < nextUseTime ||
            lastUseFrame == Time.frameCount)
        {
            return false;
        }

        if (!TryGetOpponent(out var opponent))
        {
            return false;
        }

        // 相手が射程外でも、空振りとしてクールタイムを消費する。
        nextUseTime = Time.time + settings.CooldownTime;
        lastUseFrame = Time.frameCount;

        Vector3 direction =
            opponent.transform.position - transform.position;

        direction.y = 0f;

        if (direction.magnitude > settings.Range)
        {
            return true;
        }

        // 同じ位置に重なった場合は、自分の正面へ押し戻す。
        if (direction.sqrMagnitude < MinDirectionSqrMagnitude)
        {
            direction = transform.forward;
            direction.y = 0f;

            if (direction.sqrMagnitude < MinDirectionSqrMagnitude)
            {
                direction = Vector3.forward;
            }
        }

        opponent.ReceiveHit(direction, settings);

        return true;
    }

    private bool TryGetOpponent(out SmashHitReceiver opponent)
    {
        opponent = null;

        SmashHitReceiver[] receivers =
            UnityEngine.Object.FindObjectsByType<SmashHitReceiver>();

        foreach (SmashHitReceiver receiver in receivers)
        {
            if (!receiver.isActiveAndEnabled ||
                receiver.gameObject == gameObject ||
                receiver.gameObject.scene != gameObject.scene)
            {
                continue;
            }

            if (!receiver.TryGetComponent<Player>(out var targetPlayer) ||
                !targetPlayer.isActiveAndEnabled)
            {
                continue;
            }

            // 1対1の前提が崩れた場合は、検索順で相手を決めない。
            if (opponent != null)
            {
                Debug.LogWarning(
                    "スマッシュの対象が複数います。" +
                    "同じシーンの有効な対戦プレイヤーを2体にしてください。",
                    this);

                opponent = null;
                return false;
            }

            opponent = receiver;
        }

        return opponent != null;
    }
}