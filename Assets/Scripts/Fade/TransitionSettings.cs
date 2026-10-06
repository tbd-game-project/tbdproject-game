using UnityEngine;

// シーン遷移時のフェードの演出設定
[CreateAssetMenu(fileName = "TransitionSettings", menuName = "Transition/Transition Settings")]
public sealed class TransitionSettings : ScriptableObject
{
    [SerializeField]
    private Color   fadeColor       = Color.black;

    [SerializeField]
    [Min(0.0f)]
    private float   fadeDuration    = 0.5f;

    public Color    FadeColor       => fadeColor;

    public float    FadeDuration    => fadeDuration;
}