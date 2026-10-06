using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public sealed class InputState
{
    public bool Pressed { get; private set; }
    public bool Released { get; private set; }
    public bool Hold { get; private set; }

    public void SetPressed()
    {
        Pressed = true;
        Hold = true;
    }

    public void SetReleased()
    {
        Hold = false;
        Released = true;
    }

    public void ClearFrameFlags()
    {
        Pressed = false;
        Released = false;
    }
    
    public void ClearAllFlags()
    {
        Pressed = false;
        Released = false;
        Hold = false;
    }

}

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputReader : MonoBehaviour
{
    // 以下にボタン入力の種類を追加
    public Vector2 MoveValue { get; private set; }
    
    public InputState Attack { get; } = new InputState();
    public InputState Skill { get; } = new InputState();
    public InputState Place { get; } = new InputState();

    // ============================

    [Header("References")]
    [SerializeField, Tooltip("PlayerInputコンポーネントへの参照")]private PlayerInput _playerInput;
    
    // 以下に種別InputActionを追加
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _skillAction;
    private InputAction _placeAction;

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
        _attackAction = _playerInput.actions.FindAction("Player/Attack", true);
        _skillAction = _playerInput.actions.FindAction("Player/Skill", true);
        _placeAction = _playerInput.actions.FindAction("Player/Place", true);

        // ===========================

        // InputActionのイベント登録
        _moveAction.performed += HandleMove;
        _moveAction.canceled += HandleMoveCanceled;

        _attackAction.performed += HandleAttack;
        _attackAction.canceled += HandleAttackCanceled;

        _skillAction.performed += HandleSkill;
        _skillAction.canceled += HandleSkillCanceled;

        _placeAction.performed += HandlePlace;
        _placeAction.canceled += HandlePlaceCanceled;

        // =========================
    }

    private void OnDestroy()
    {
        // ここでInputActionの解除

        _moveAction.canceled -= HandleMoveCanceled;
        _moveAction.performed -= HandleMove;

        _attackAction.performed -= HandleAttack;
        _attackAction.canceled -= HandleAttackCanceled;

        _skillAction.performed -= HandleSkill;
        _skillAction.canceled -= HandleSkillCanceled;

        _placeAction.performed -= HandlePlace;
        _placeAction.canceled -= HandlePlaceCanceled;
        // ===========================
    }

    private void LateUpdate()
    {
        // フレームごとに入力状態をクリア
        Attack.ClearFrameFlags();
        Skill.ClearFrameFlags();
        Place.ClearFrameFlags();
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

    private void HandleAttack(InputAction.CallbackContext context)
    {
        Attack.SetPressed();
    }

    private void HandleAttackCanceled(InputAction.CallbackContext context)
    {
        Attack.SetReleased();
    }

    private void HandleSkill(InputAction.CallbackContext context)
    {
        Skill.SetPressed();
    }

    private void HandleSkillCanceled(InputAction.CallbackContext context)
    {
        Skill.SetReleased();
    }

    private void HandlePlace(InputAction.CallbackContext context)
    {
        Place.SetPressed();
    }

    private void HandlePlaceCanceled(InputAction.CallbackContext context)
    {
        Place.SetReleased();
    }
}
