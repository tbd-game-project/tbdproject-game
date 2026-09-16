using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private PlayerStateList stateList;
    [SerializeField] private string initializeStatekey = "idle";

    //---------------------------------------------------------------
    [Header("Reposition Visual")]
    private GameObject normalMesh;
    private GameObject repositionMesh;

    private Renderer normalMeshRenderer;
    private Renderer repositionMeshRenderer;

    //---------------------------------------------------------------

    private PlayerState currentState;
    public FieldTile OnStandingPiece { get; private set; }

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

        //---------------------------------------------------------------
        // NormalMeshを取得
        Transform normalMeshTransform = transform.Find("NormalMesh");

        if (normalMeshTransform != null)
        {
            normalMesh = normalMeshTransform.gameObject;
        }
        else
        {
            Debug.LogError("NormalMesh is not found");
            return;
        }

        // RepositionPointerを取得
        Transform repositionMeshTransform = transform.Find("RepositionMesh");

        if (repositionMeshTransform != null)
        {
            repositionMesh = repositionMeshTransform.gameObject;
        }
        else
        {
            Debug.LogError("RepositionPointer is not found");
            return;
        }

        normalMeshRenderer = normalMesh.GetComponent<Renderer>();

        repositionMeshRenderer = repositionMesh.GetComponent<Renderer>();

        if (!normalMeshRenderer || !repositionMeshRenderer)
        {
            Debug.LogError("NormalMesh または RepositionMesh にRendererがありません。");

            return;
        }
        //---------------------------------------------------------------

        // ステータスのコピーインスタンスを生成
        stateList.CreateRunTimeCopies();
    }

    void Start()
    {
        //---------------------------------------------------------------
        TurnManager.Instance.RegisterPlayer(this);
        repositionMesh.SetActive(false);
        //---------------------------------------------------------------

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
        // 命中情報が届いていたら、操作より優先して吹き飛ばされる状態へ入る。
        if (TryGetComponent<SmashHitReceiver>(out var receiver) &&
            receiver.HasPendingHit)
        {
            ChangeState("knockback");
            return;
        }

        // 吹き飛ばされている間は、攻撃・配置入力を使わない。
        if (currentState is PlayerStateSmashed)
        {
            return;
        }

        if (input.Attack.Pressed &&
            TryGetComponent<SmashAttack>(out var skill))
        {
            // 攻撃が発動した場合は、同時に配置しない。
            if (skill.TryUse())
            {
                return;
            }
        }

        // どの状態からでも特定のイベントで遷移するトランジションはここに記述する
        if (input.Place.Pressed && TurnManager.Instance.IsAttackPlayer(this)) 
        {
            ChangeState("place");
        }
    }

    public bool TryGetFieldTileBelow(float rayStartHeight, float rayDistance, LayerMask fieldLayer)
    {
        Vector3 origin = transform.position + Vector3.up * rayStartHeight;

        FieldTile FieldTile= null;

        if (Physics.Raycast(origin,Vector3.down,out RaycastHit hit, rayDistance, fieldLayer,QueryTriggerInteraction.Ignore))
        {
            FieldTile = hit.collider.GetComponent<FieldTile>();
            if(FieldTile != null)
            {
                OnStandingPiece = FieldTile;

                Debug.DrawLine(origin, hit.point, Color.red, 1f);
                return true;
            }
        }

        OnStandingPiece = null;

        Debug.DrawLine(origin, origin + Vector3.down * rayDistance, Color.green, 1f);
        return false;
    }


    //---------------------------------------------------------------
    public void StartReposition()
    {
        ChangeState("reposition");
    }

    public void EndReposition()
    {
        ChangeState("idle");
    }

    public void EnterRepositionVisual()
    {
        normalMesh.SetActive(false);
        repositionMesh.SetActive(true);
    }

    public void ExitRepositionVisual()
    {
        repositionMesh.SetActive(false);
        normalMesh.SetActive(true);
    }

    public void SetRepositionColor(Color color)
    {
        normalMeshRenderer.material.color = color;
        repositionMeshRenderer.material.color = color;
    }

    //---------------------------------------------------------------
}
