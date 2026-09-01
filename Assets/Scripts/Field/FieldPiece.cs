using UnityEngine;

public class FieldPiece : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
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
}
