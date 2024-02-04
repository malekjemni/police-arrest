using DG.Tweening;
using StarterAssets;
using System.Collections;
using UnityEngine;

public class ArrestSystem : MonoBehaviour
{
    public float detectionRadius = 10f;
    public Material normalMaterial;
    public Material highlightedMaterial;
    public GameObject popup;
    public GameObject popup_menu;
    public bool isNear;
    [SerializeField] private Transform openPos;
    [SerializeField] private Transform closePos;
    [SerializeField] private float moveTime;
    [SerializeField] private float popInDuration;
    [SerializeField] private float popInScale;
    private StarterAssetsInputs _input;
    private void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _input.arrest = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Outlaw"))
        {
            popup.transform.DOMove(openPos.position, moveTime).SetEase(Ease.InOutSine);
            isNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Outlaw"))
        {
            popup.transform.DOMove(closePos.position, moveTime).SetEase(Ease.InOutSine);
            isNear = false;
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


    public void ShowPopInEffect()
    {
        popup_menu.transform.localScale = Vector3.zero;
        popup_menu.transform.DOScale(Vector3.one * popInScale, popInDuration).SetEase(Ease.OutBack);
        popup_menu.GetComponent<CanvasGroup>().DOFade(1f, popInDuration);
        StartCoroutine(HidePopInEffect());
    }

    private IEnumerator HidePopInEffect()
    {     
        yield return new WaitForSeconds(3f);
        popup_menu.transform.DOScale(Vector3.zero, popInDuration).SetEase(Ease.InBack);
        popup_menu.GetComponent<CanvasGroup>().DOFade(0f, popInDuration);
        popup_menu.transform.localScale = Vector3.one;
    }
}
