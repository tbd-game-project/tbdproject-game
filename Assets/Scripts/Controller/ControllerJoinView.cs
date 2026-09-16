using UnityEngine;
using UnityEngine.InputSystem;

// ControllerSessionManagerの登録状態に応じて、UIの更新

public sealed class ControllerJoinView : MonoBehaviour
{
    [SerializeField]
    private GameObject[] playerJoinedImages = new GameObject[ControllerSessionManager.MaxPlayerCount];

    private ControllerSessionManager session;

    private bool isSubscribed;

    private void Start()
    {
        SubscribeSessionEvents();
    }

    private void OnEnable()
    {
        if (isSubscribed)
            return;

        SubscribeSessionEvents();
    }

    private void OnDisable()
    {
        UnsubscribeSessionEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeSessionEvents();
    }

    private void OnValidate()
    {
        if (playerJoinedImages == null)
            return;

        if (playerJoinedImages.Length != ControllerSessionManager.MaxPlayerCount)
        {
            Debug.LogWarning(
                $"[ControllerJoinView] " +
                $"Player Joined Imagesには" +
                $"{ControllerSessionManager.MaxPlayerCount}個の要素を" +
                "設定してください。",
                this
            );
        }
    }

    public void Refresh()
    {
        if (session == null)
        {
            SetAllImagesInactive();
            return;
        }

        for (int playerIndex = 0; playerIndex < playerJoinedImages.Length; playerIndex++)
        {
            bool isJoined = playerIndex < session.PlayerCount;

            SetImageActive(playerIndex, isJoined);
        }
    }

    private void SubscribeSessionEvents()
    {
        if (isSubscribed)
            return;

        session = ControllerSessionManager.Instance;

        if (session == null)
            return;

        session.DeviceRegistered    += OnDeviceRegistered;
        session.RegistrationCleared += OnRegistrationCleared;

        isSubscribed = true;

        Refresh();
    }

    // プレイヤー登録時のイベント
    private void OnDeviceRegistered(int playerIndex, InputDevice device)
    {
        SetImageActive(playerIndex, true);
    }

    // 全登録解除時のイベント
    private void OnRegistrationCleared()
    {
        SetAllImagesInactive();
    }

    // 全プレイヤーの参加画像を非表示にする
    private void SetAllImagesInactive()
    {
        if (playerJoinedImages == null)
            return;

        for (int playerIndex = 0; playerIndex < playerJoinedImages.Length; playerIndex++)
        {
            SetImageActive(playerIndex, false);
        }
    }

    // 指定プレイヤーの参加画像を表示または非表示にする
    private void SetImageActive(int playerIndex, bool isActive)
    {
        if (playerJoinedImages == null)
            return;

        if (playerIndex < 0 || playerIndex >= playerJoinedImages.Length)
        {
            Debug.LogWarning(
                $"[ControllerJoinView] " +
                $"プレイヤーインデックス{playerIndex}に対応する" +
                "UIが設定されていません。",
                this
            );

            return;
        }

        GameObject targetImage = playerJoinedImages[playerIndex];

        if (targetImage == null)
        {
            Debug.LogWarning(
                $"[ControllerJoinView] " +
                $"Player {playerIndex + 1}の画像が設定されていません。",
                this
            );

            return;
        }

        targetImage.SetActive(isActive);
    }

    
    private void UnsubscribeSessionEvents()
    {
        if (session == null || !isSubscribed)
            return;

        session.DeviceRegistered    -= OnDeviceRegistered;
        session.RegistrationCleared -= OnRegistrationCleared;

        session         = null;
        isSubscribed    = false;
    }
}