using UnityEngine;

[CreateAssetMenu(
    fileName = "SmashSettings",
    menuName = "Skills/Knockback Settings")]
public class SmashSettings : ScriptableObject
{
    [Header("攻撃範囲")]
    [SerializeField, Min(0f)]
    [Tooltip("相手に届く距離。プレイヤー同士の水平距離で判定する")]
    private float range = 3f;

    [Header("ノックバック")]
    [SerializeField, Min(0f)]
    [Tooltip("吹き飛ばされる速さ。単位はUnityの距離単位/秒")]
    private float knockbackSpeed = 8f;

    [SerializeField, Min(0.01f)]
    [Tooltip("吹き飛ばされて通常操作ができない時間。単位は秒")]
    private float knockbackDuration = 0.25f;

    [Header("クールタイム")]
    [SerializeField, Min(0f)]
    [Tooltip("発動してから再使用できるまでの時間。単位は秒")]
    private float cooldownTime = 2f;

    public float Range => range;
    public float KnockbackSpeed => knockbackSpeed;
    public float KnockbackDuration => knockbackDuration;
    public float CooldownTime => cooldownTime;

    public bool IsValid =>
        IsFinite(range) && range > 0f &&
        IsFinite(knockbackSpeed) && knockbackSpeed > 0f &&
        IsFinite(knockbackDuration) && knockbackDuration > 0f &&
        IsFinite(cooldownTime) && cooldownTime >= 0f;

    private static bool IsFinite(float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }
}