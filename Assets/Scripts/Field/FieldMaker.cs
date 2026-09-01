using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class FieldMaker : MonoBehaviour
{

    [Header("Reference")]
    [SerializeField] private GameObject field;

    [Header("Field Setting")]
    [SerializeField] private Vector2Int fieldSize = new(10, 10);
    [SerializeField] private float fieldSpacing = 2.0f;

    [Header("Generated Object")]
    [SerializeField] private Transform generatedRoot;

    private bool isGenerating = false;
    private bool rebuildQueued = false;

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            return;
        }

        QueueGenerateField();
    }

    [ContextMenu("Generate Field")]
    public void GenerateField()
    {
        if(field == null || isGenerating)
        {
            return;
        }

        isGenerating = true;

        try
        {
            CreateGenerateRoot();
            ClearField();

            for (int x = 0; x < fieldSize.x; x++)
            {
                for (int y = 0; y < fieldSize.y; y++)
                {
                    GameObject newField = Instantiate(field, generatedRoot);

                    newField.name = $"Field_{x}_{y}";
                    newField.transform.localPosition = new Vector3(x * 1.0f * fieldSpacing, 0.0f, y * 1.0f * fieldSpacing);
                    newField.transform.localRotation = Quaternion.identity;
                }
            }
        }
        finally
        {
            isGenerating = false;
        }
    }

    private void CreateGenerateRoot()
    {
        if(generatedRoot != null)
        {
            return;
        }

        GameObject rootObject = new GameObject("Generated Fields");
        rootObject.transform.SetParent(transform);
        rootObject.transform.localPosition = Vector3.zero;
        rootObject.transform.localRotation = Quaternion.identity;

        generatedRoot = rootObject.transform;
    }

    private void ClearField()
    {
        for(int i = generatedRoot.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(generatedRoot.GetChild(i).gameObject);
        }
    }

    private void QueueGenerateField()
    {
#if UNITY_EDITOR
        if (rebuildQueued)
        {
            return;
        }

        rebuildQueued = true;
        EditorApplication.delayCall += GenerateFieldAfterValidate;
#endif
    }

#if UNITY_EDITOR
    private void GenerateFieldAfterValidate()
    {
        rebuildQueued = false;

        // スクリプト削除・再生開始などで無効になった場合は何もしない
        if (this == null || Application.isPlaying)
        {
            return;
        }

        GenerateField();
    }
#endif
}
