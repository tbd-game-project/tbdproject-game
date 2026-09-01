using UnityEngine;

public class FieldPiece : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;

    // ç¿ïWèÓïÒ
    private Vector2Int coordinate;
    private Stone putedStone = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<MeshRenderer>().material = highlightMaterial;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
        }
    }

    public bool PutStone(Stone stone)
    {
        if (putedStone != null)
        {
            return false;
        }
        putedStone = stone;
        putedStone.transform.position = this.transform.position + new Vector3(0, putedStone.transform.localScale.y, 0);
        return true;
    }

    public void SetCoodinate(float x, float y)
    {
        coordinate = new Vector2Int((int)x, (int)y);
    }
    public Vector2Int GetCoordinate()
    {
        return coordinate;
    }
}
