using DG.Tweening;
using UnityEngine;

public class ArrestSystem : MonoBehaviour
{
    public float detectionRadius = 10f;
    public Material normalMaterial;
    public Material highlightedMaterial;
    public GameObject popup;
    [SerializeField] private Transform openPos;
    [SerializeField] private Transform closePos;
    [SerializeField] private float moveTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Outlaw"))
        {
            popup.transform.DOMove(openPos.position, moveTime).SetEase(Ease.InOutSine);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Outlaw"))
        {
            popup.transform.DOMove(closePos.position, moveTime).SetEase(Ease.InOutSine);
        }
    }
    private void CheckForOutlaw()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Outlaw"))
            {
                ChangeMaterialColor(collider.gameObject, highlightedMaterial);
            }         
        }
    }
    private void ChangeMaterialColor(GameObject obj, Material material)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    void Update()
    {
        CheckForOutlaw();
    }
}
