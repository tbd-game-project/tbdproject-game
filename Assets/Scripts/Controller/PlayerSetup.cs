using UnityEngine;
using UnityEngine.InputSystem;

// バインド画面で決定したプレイヤーと入力デバイスの対応関係を、
// ゲームシーンへ反映

public sealed class PlayerSetup : MonoBehaviour
{
    [SerializeField]
    private string gameplayActionMapName        = "Player";

    [SerializeField]
    private string gamepadControlSchemeName     = "Gamepad";

    [SerializeField]
    private string joystickControlSchemeName    = "Joystick";

    [SerializeField]
    private string keyboardControlSchemeName    = "Keyboard";

    [SerializeField]
    private PlayerInput[] players = new PlayerInput[ControllerSessionManager.MaxPlayerCount];

    private void Start()
    {
        SetupPlayers();
    }

    private void OnValidate()
    {
        if (players == null)
            return;

        if (players.Length != ControllerSessionManager.MaxPlayerCount)
        {
            Debug.LogWarning(
                $"[PlayerSetup] Playersには" +
                $"{ControllerSessionManager.MaxPlayerCount}個の" +
                "PlayerInputを設定してください。",
                this
            );
        }
    }

    private void SetupPlayers()
    {
        ControllerSessionManager session = ControllerSessionManager.Instance;

        if (session == null)
        {
            Debug.LogError(
                "[PlayerSetup] " +
                "ControllerSessionManagerが存在しません。" +
                "ControllerBindSceneから実行してください。",
                this
            );

            DisableAllPlayers();
            return;
        }

        if (players == null || players.Length == 0)
        {
            Debug.LogError(
                "[PlayerSetup] PlayerInputが設定されていません。",
                this
            );

            return;
        }

        // 未登録プレイヤーをゲームシーンに残さないため、
        // 最初にすべてのPlayerを無効化する。
        DisableAllPlayers();

        int setupPlayerCount = Mathf.Min(session.PlayerCount, players.Length);

        Debug.Log(
            $"[PlayerSetup] 登録人数: {session.PlayerCount}",
            this
        );

        for (int playerIndex = 0; playerIndex < setupPlayerCount; playerIndex++)
        {
            SetupPlayer(session, playerIndex);
        }
    }

    private void SetupPlayer(ControllerSessionManager session, int playerIndex)
    {
        PlayerInput playerInput = players[playerIndex];

        if (playerInput == null)
        {
            Debug.LogError(
                $"[PlayerSetup] " +
                $"Player {playerIndex + 1}のPlayerInputが" +
                "Inspectorに設定されていません。",
                this
            );

            return;
        }

        InputDevice device = session.GetDevice(playerIndex);

        if (device == null)
        {
            Debug.LogWarning(
                $"[PlayerSetup] " +
                $"Player {playerIndex + 1}に対応する" +
                "入力デバイスが存在しません。",
                this
            );

            return;
        }

        // PlayerInputを有効化すると
        // InputUserが生成されデバイスを割り当てられる状態になる
        playerInput.gameObject.SetActive(true);

        string controlSchemeName = GetControlSchemeName(device);

        if (string.IsNullOrEmpty(controlSchemeName))
        {
            Debug.LogError(
                $"[PlayerSetup] " +
                $"{device.displayName}に対応する" +
                "Control Schemeを判定できません。" +
                $"\nDevice Type: {device.GetType().Name}" +
                $"\nLayout: {device.layout}" +
                $"\nDevice ID: {device.deviceId}",
                playerInput
            );

            playerInput.gameObject.SetActive(false);
            return;
        }

        playerInput.SwitchCurrentControlScheme(controlSchemeName, device);

        playerInput.SwitchCurrentActionMap(gameplayActionMapName);

        playerInput.ActivateInput();
    }

    private string GetControlSchemeName(InputDevice device)
    {
        if (device is Gamepad)
            return gamepadControlSchemeName;

        if (device is Joystick)
            return joystickControlSchemeName;

        if (device is Keyboard)
            return keyboardControlSchemeName;

        return null;
    }

    // ゲーム開始前にすべてのPlayerを無効化する
    private void DisableAllPlayers()
    {
        if (players == null)
            return;

        foreach (PlayerInput playerInput in players)
        {
            if (playerInput != null)
            {
                playerInput.gameObject.SetActive(false);
            }
        }
    }
}