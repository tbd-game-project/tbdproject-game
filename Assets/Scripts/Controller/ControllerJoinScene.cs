using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// コントローラー登録画面の進行を管理する。
// 登録人数の確認、登録解除、シーン遷移を担当する。
public sealed class ControllerJoinScene : MonoBehaviour
{
    // ゲーム開始に必要な最低人数
    private const int MinimumPlayerCount = 2;

    // 次に遷移するシーン名
    [SerializeField]
    private string nextSceneName = "DevelopScene";

    [SerializeField]
    private InputActionReference proceedAction;

    [SerializeField]
    private InputActionReference clearRegistrationAction;

    // 同一フレームで複数回シーン遷移することを防ぐ
    private bool isLoadingScene;

    private void OnEnable()
    {
        if (proceedAction != null)
        {
            proceedAction.action.performed += OnProceedPerformed;
        }

        if (clearRegistrationAction != null)
        {
            clearRegistrationAction.action.performed += OnClearRegistrationPerformed;
        }
    }

    private void OnDisable()
    {
        if (proceedAction != null)
        {
            proceedAction.action.performed -= OnProceedPerformed;
        }

        if (clearRegistrationAction != null)
        {
            clearRegistrationAction.action.performed -= OnClearRegistrationPerformed;
        }
    }

    // 条件を満たしていれば次のシーンへ遷移する
    public void LoadNextScene()
    {
        // InputActionとUI Buttonの両方から呼ばれても
        // 一度しか遷移しないようにする
        if (isLoadingScene)
            return;

        ControllerSessionManager session =
            ControllerSessionManager.Instance;

        if (session == null)
        {
            Debug.LogError(
                "[JoinSceneController] " +
                "ControllerSessionManagerが存在しません。",
                this
            );

            return;
        }

        // 最低人数に達していない場合は開始しない
        if (session.PlayerCount < MinimumPlayerCount)
        {
            Debug.Log(
                $"[JoinSceneController] " +
                $"開始には{MinimumPlayerCount}人以上必要です。" +
                $" 現在の登録人数: {session.PlayerCount}",
                this
            );

            return;
        }

        if (string.IsNullOrWhiteSpace(nextSceneName))
        {
            Debug.LogError(
                "[JoinSceneController] " +
                "遷移先シーン名が設定されていません。",
                this
            );

            return;
        }

        isLoadingScene = true;

        // シーン遷移中に入力を受け付けないようにする
        proceedAction.action.Disable();
        clearRegistrationAction.action.Disable();

        SceneManager.LoadScene(nextSceneName);
    }

    // 登録済みデバイスをすべて解除する
    public void ClearRegistration()
    {
        ControllerSessionManager session =
            ControllerSessionManager.Instance;

        if (session == null)
        {
            Debug.LogError(
                "[JoinSceneController] " +
                "ControllerSessionManagerが存在しません。",
                this
            );

            return;
        }

        session.ClearRegistration();
    }

    private void OnProceedPerformed(InputAction.CallbackContext context)
    {
        LoadNextScene();
    }

    private void OnClearRegistrationPerformed(InputAction.CallbackContext context)
    {
        ClearRegistration();
    }
}