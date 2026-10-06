
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 画面周辺エフェクトの表示と見た目を管理する。
///
/// 使用方法：
/// ・空のGameObjectに、このコンポーネントを追加する。
/// ・Renderer Dataに、エフェクトを設定したRendererを指定する。
/// ・Feature Nameに、対象のFull Screen PassのNameを指定する。
///
/// 注意：
/// 同じRenderer Featureを操作するControllerは1つだけにする。
/// 同じRendererを複数のCameraが使う場合、そのCamera全体に影響する。
/// 
/// </summary>
[DisallowMultipleComponent]
public class ScreenEdgeEffectController : MonoBehaviour
{
    [Header("描画先の設定")]

    [Tooltip("対象のFull Screen Passが登録されているRenderer Data。")]
    [SerializeField]
    private UniversalRendererData rendererData;

    [Tooltip("Full Screen Pass Renderer FeatureのNameと同じ文字列にする。")]
    [SerializeField]
    private string featureName = "ScreenEdgeFog";

    [Header("表示の設定")]

    [Tooltip("オンで表示、オフで非表示。実行中にも変更できる。")]
    [SerializeField]
    private bool effectEnabled = true;

    [Tooltip("エフェクトの色。Alphaを小さくすると薄くなる。")]
    [SerializeField]
    private Color effectColor = new Color(0.65f, 0.7f, 0.8f, 1f);

    [Tooltip("エフェクトの濃さ。0で見えなくなり、1で最も濃くなる。")]
    [SerializeField, Range(0f, 1f)]
    private float intensity = 0.6f;

    [Tooltip("画面端からの広がり。大きくすると中央側まで広がる。")]
    [SerializeField, Range(0f, 0.5f)]
    private float edgeWidth = 0.2f;

    [Tooltip("エフェクトの境界の柔らかさ。大きいほど滑らかになる。")]
    [SerializeField, Range(0.01f, 1f)]
    private float softness = 0.8f;

    [Header("模様の設定")]

    [Tooltip("模様の細かさ。大きくすると細かい模様になる。")]
    [SerializeField, Range(1f, 30f)]
    private float noiseScale = 8f;

    [Tooltip("模様による濃淡の強さ。0にすると模様がなくなる。")]
    [SerializeField, Range(0f, 1f)]
    private float noiseStrength = 0.7f;

    // 操作対象の描画機能と、実行中専用のMaterial。
    private FullScreenPassRendererFeature targetFeature;
    private Material runtimeMaterial;

    // 終了時に元の状態へ戻すため、開始前の設定を保存する。
    private Material originalMaterial;
    private bool originalActive;

    // Shader内のプロパティ名をIDに変換しておく。
    // Shader側の名前を変更した場合は、こちらも合わせて変更する。
    private static readonly int effectEnabledId =
        Shader.PropertyToID("_EffectEnabled");

    private static readonly int fogColorId =
        Shader.PropertyToID("_FogColor");

    private static readonly int intensityId =
        Shader.PropertyToID("_Intensity");

    private static readonly int edgeWidthId =
        Shader.PropertyToID("_EdgeWidth");

    private static readonly int softnessId =
        Shader.PropertyToID("_Softness");

    private static readonly int noiseScaleId =
        Shader.PropertyToID("_NoiseScale");

    private static readonly int noiseStrengthId =
        Shader.PropertyToID("_NoiseStrength");

    private void OnEnable()
    {
        // Rendererが未設定の場合は、原因をConsoleに表示する。
        if (rendererData == null)
        {
            Debug.LogError(
                "ScreenEdgeEffectController: Renderer Dataを設定してください。",
                this);
            return;
        }

        // Rendererに登録されたFeatureから、指定した名前のものを探す。
        foreach (var feature in rendererData.rendererFeatures)
        {
            if (feature is FullScreenPassRendererFeature fullScreenFeature
                && feature.name == featureName)
            {
                targetFeature = fullScreenFeature;
                break;
            }
        }

        if (targetFeature == null)
        {
            Debug.LogError(
                $"ScreenEdgeEffectController: 「{featureName}」という名前の"
                + "Full Screen Passが見つかりません。",
                this);
            return;
        }

        if (targetFeature.passMaterial == null)
        {
            Debug.LogError(
                "ScreenEdgeEffectController: Full Screen Passの"
                + "Pass Materialを設定してください。",
                this);
            targetFeature = null;
            return;
        }

        originalMaterial = targetFeature.passMaterial;
        originalActive = targetFeature.isActive;

        // 元のMaterialを直接変更せず、実行中専用のコピーを作る。
        runtimeMaterial = new Material(originalMaterial);
        runtimeMaterial.name = originalMaterial.name + " (Runtime)";
        runtimeMaterial.hideFlags = HideFlags.HideAndDontSave;

        targetFeature.passMaterial = runtimeMaterial;

        ApplySettings();
    }

    private void Update()
    {
        // 実行中にInspectorで変更した値も、画面へ反映する。
        ApplySettings();
    }

    /// <summary>
    /// Inspectorの設定を、描画機能とMaterialへ反映する。
    /// </summary>
    private void ApplySettings()
    {
        // 初期設定に失敗した場合などは、処理を行わない。
        if (targetFeature == null || runtimeMaterial == null)
        {
            return;
        }

        // 非表示のときは、エフェクトの描画処理自体を停止する。
        if (targetFeature.isActive != effectEnabled)
        {
            targetFeature.SetActive(effectEnabled);
        }

        runtimeMaterial.SetFloat(effectEnabledId, effectEnabled ? 1f : 0f);
        runtimeMaterial.SetColor(fogColorId, effectColor);
        runtimeMaterial.SetFloat(intensityId, intensity);
        runtimeMaterial.SetFloat(edgeWidthId, edgeWidth);
        runtimeMaterial.SetFloat(softnessId, softness);
        runtimeMaterial.SetFloat(noiseScaleId, noiseScale);
        runtimeMaterial.SetFloat(noiseStrengthId, noiseStrength);
    }

    /// <summary>
    /// エフェクトを表示する。
    /// 他のスクリプトから、controller.Show(); の形で呼び出す。
    /// </summary>
    public void Show()
    {
        effectEnabled = true;
        ApplySettings();
    }

    /// <summary>
    /// エフェクトを非表示にする。
    /// </summary>
    public void Hide()
    {
        effectEnabled = false;
        ApplySettings();
    }

    /// <summary>
    /// 濃さを変更する。指定できる値は0?1。
    /// 範囲外の値は、自動的に0?1へ収める。
    /// </summary>
    public void SetIntensity(float value)
    {
        intensity = Mathf.Clamp01(value);
        ApplySettings();
    }

    /// <summary>
    /// エフェクトの色を変更する。
    /// </summary>
    public void SetColor(Color value)
    {
        effectColor = value;
        ApplySettings();
    }

    /// <summary>
    /// 画面端からの広がりを変更する。指定できる値は0?0.5。
    /// </summary>
    public void SetEdgeWidth(float value)
    {
        edgeWidth = Mathf.Clamp(value, 0f, 0.5f);
        ApplySettings();
    }

    private void OnDisable()
    {
        // 自分が設定したMaterialがまだ使われている場合、
        // 開始前のMaterialと表示状態に戻す。
        if (targetFeature != null
            && runtimeMaterial != null
            && targetFeature.passMaterial == runtimeMaterial)
        {
            targetFeature.passMaterial = originalMaterial;
            targetFeature.SetActive(originalActive);
        }

        // 実行中専用に作ったMaterialを片付ける。
        if (runtimeMaterial != null)
        {
            Destroy(runtimeMaterial);
        }

        runtimeMaterial = null;
        originalMaterial = null;
        targetFeature = null;
    }
}