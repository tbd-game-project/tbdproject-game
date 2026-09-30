using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerStateSmashed",
    menuName = "PlayerState/Knockback")]
public class PlayerStateSmashed : PlayerState
{
    private Rigidbody playerRigidbody;

    private Vector3 knockbackDirection;
    private float knockbackSpeed;
    private float knockbackDuration;

    private float remainingTime;

    public void Setup(
        Vector3 direction,
        float speed,
        float duration)
    {
        // 高さの差で斜め上や下へ吹き飛ばされないようにする。
        direction.y = 0f;

        knockbackDirection = direction.normalized;
        knockbackSpeed = speed;
        knockbackDuration = duration;
    }

    public override void EnterState(
    Player owner,
    PlayerInputReader input)
    {
        base.EnterState(owner, input);

        // 攻撃情報を取り出し、受信側に残っている情報を消費する。
        if (!owner.TryGetComponent<SmashHitReceiver>(out var receiver) ||
            !receiver.TryTakeHit(
                out var direction,
                out var speed,
                out var duration))
        {
            owner.ChangeState("idle");
            return;
        }

        playerRigidbody = owner.GetComponent<Rigidbody>();

        if (playerRigidbody == null)
        {
            Debug.LogError(
                "PlayerStateSmashedに必要なRigidbodyがありません。",
                owner);

            owner.ChangeState("idle");
            return;
        }

        Setup(direction, speed, duration);

        remainingTime = knockbackDuration;
        playerRigidbody.linearVelocity = Vector3.zero;
    }

    public override void FixedUpdateState()
    {
        if (playerRigidbody == null)
        {
            return;
        }

        if (remainingTime <= 0f)
        {
            owner.ChangeState("idle");
            return;
        }

        // 最後の移動が、指定された時間を超えないようにする。
        float moveTime = Mathf.Min(
            Time.fixedDeltaTime,
            remainingTime);

        Vector3 movement =
            knockbackDirection * knockbackSpeed * moveTime;

        playerRigidbody.MovePosition(
            playerRigidbody.position + movement);

        remainingTime = Mathf.Max(0f, remainingTime - moveTime);
    }

    public override void ExitState()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
        }

        remainingTime = 0f;
    }
}