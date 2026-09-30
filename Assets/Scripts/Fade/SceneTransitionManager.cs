using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// フェード処理の管理
public sealed class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager    Instance { get; private set; }

    [SerializeField]
    private FadeController                  fadeController;

    private bool                            isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // シーン読込後も同じFadeControllerでフェード処理を行うため、保持する
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void LoadScene(string sceneName, TransitionSettings transitionSettings)
    {
        // 遷移入力が連続しても、複数のCoroutineを開始しない
        if (isTransitioning)
            return;

        StartCoroutine(TransitionScene(sceneName, transitionSettings));
    }

    private IEnumerator TransitionScene(string sceneName, TransitionSettings transitionSettings)
    {
        isTransitioning = true;

        yield return fadeController.FadeOut(transitionSettings);

        SceneManager.LoadScene(sceneName);

        yield return fadeController.FadeIn(transitionSettings);

        isTransitioning = false;
    }
}