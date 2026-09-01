using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputReader : MonoBehaviour
{
    // 以下にボタン入力の種類を追加
    public Vector2 MoveValue { get; private set; }

    // ============================

    [Header("References")]
    [SerializeField, Tooltip("PlayerInputコンポーネントへの参照")]private PlayerInput _playerInput;
    
    // 以下に種別InputActionを追加
    private InputAction _moveAction;

    // ===========================

    private void Awake()
    {
        if (!_playerInput)
        {
            _playerInput = GetComponent<PlayerInput>();
            if(!_playerInput)
            {
                Debug.LogError("PlayerInputコンポーネントが見つかりません。");
            }
        }

        // ここでInputActionのアタッチ
        _moveAction = _playerInput.actions.FindAction("Player/Move", true);

        // ===========================

        // InputActionのイベント登録
        _moveAction.performed += HandleMove;
        _moveAction.canceled += HandleMoveCanceled;

        // =========================
    }

    private void OnDestroy()
    {
        // ここでInputActionの解除

        _moveAction.canceled -= HandleMoveCanceled;
        _moveAction.performed -= HandleMove;
        // ===========================
    }

    public void SetInputEnable(bool enable)
    {
        if (enable)
        {
            _playerInput.ActivateInput();
        }
        else
        {
            _playerInput.DeactivateInput();
            MoveValue = Vector2.zero;
        }
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        MoveValue = context.ReadValue<Vector2>();
    }

    private void HandleMoveCanceled(InputAction.CallbackContext context)
    {
        MoveValue = Vector2.zero;
    }
}
