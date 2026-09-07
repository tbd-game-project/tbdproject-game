using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private PlayerStateList stateList;
    [SerializeField] private string initializeStatekey = "idle";

    private PlayerState currentState;
    public FieldPiece OnStandingPiece { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (!input)
        {
            input = GetComponent<PlayerInputReader>();
            if (!input)
            {
                Debug.LogError("PlayerInputReader is not found");
                return;
            }
        }

        // ステータスのコピーインスタンスを生成
        stateList.CreateRunTimeCopies();
    }

    void Start()
    {
        ChangeState(initializeStatekey);
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.UpdateState();

        AnyStateTransition();
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdateState();
    }

    void OnDestroy()
    {
        stateList.DestroyRunTimeCopies();
    }

    public void ChangeState(string newStateKey)
    {
        if(!stateList.TryGetRuntimeState(newStateKey, out var newState))
        {
            Debug.LogError($"State not found: {newStateKey}");
            return;
        }

        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState(this, input);
    }

    private void AnyStateTransition()
    {
        // どの状態からでも特定のイベントで遷移するトランジションはここに記述する
        if(input.Place.Pressed)
        {
            ChangeState("place");
        }
    }

    public bool TryGetFieldPieceBelow(float rayStartHeight, float rayDistance, LayerMask fieldLayer)
    {
        Vector3 origin = transform.position + Vector3.up * rayStartHeight;

        FieldPiece fieldPiece= null;

        if (Physics.Raycast(origin,Vector3.down,out RaycastHit hit, rayDistance, fieldLayer,QueryTriggerInteraction.Ignore))
        {
            fieldPiece = hit.collider.GetComponent<FieldPiece>();
            if(fieldPiece != null)
            {
                OnStandingPiece = fieldPiece;

                Debug.DrawLine(origin, hit.point, Color.red, 1f);
                return true;
            }
        }

        OnStandingPiece = null;

        Debug.DrawLine(origin, origin + Vector3.down * rayDistance, Color.green, 1f);
        return false;
    }
}
