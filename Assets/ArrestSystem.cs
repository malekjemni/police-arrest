using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrestSystem : MonoBehaviour
{
    public float detectionRadius = 10f;
    public Color normalMaterial;
    public Color highlightedMaterial;
    public GameObject popup;
    public GameObject popup_menu;
   
    public bool isNear;
    [SerializeField] private GameObject handcuffs;
    [SerializeField] private Transform openPos;
    [SerializeField] private Transform closePos;
    [SerializeField] private float moveTime;
    [SerializeField] private float popInDuration;
    [SerializeField] private float popInScale;
    [SerializeField] private Button arrestButton;
    [SerializeField] private Button scanButton;
    public float flickerDuration = 0.2f;
    public int flickerCount = 5;
    private GameObject target;
    private PlayerInputManager _input;
    private Color originalColor;
    private bool isActivated = false;
    private HashSet<GameObject> detectedOutlaws = new HashSet<GameObject>();

    private void Start()
    {
        _input = GetComponent<PlayerInputManager>();
        popup_menu.transform.localScale = Vector3.zero;
        _input.arrest = false;
        originalColor = arrestButton.image.color;
    }
    void Update()
    {
        UpdateOutlawMaterial();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Outlaw"))
        {
            popup.transform.DOMove(openPos.position, moveTime).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                isNear = true;
                target = other.gameObject;
                arrestButton.interactable = true;
                FlickerButton();
            });
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Outlaw"))
        {
            popup.transform.DOMove(closePos.position, popInDuration).SetEase(Ease.InOutSine);
            isNear = false;
            arrestButton.interactable = false;
        }
    }
    public void CheckForOutlaw()
    {
        if (!isActivated)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
            detectedOutlaws.Clear();

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Outlaw"))
                {
                    normalMaterial = collider.GetComponent<Renderer>().material.color;
                    ChangeMaterialColor(collider.gameObject, highlightedMaterial);
                    detectedOutlaws.Add(collider.gameObject);
                }
            }
        }
        StartCoroutine(ScanAreaActivation());
    }
    private void UpdateOutlawMaterial()
    {
        List<GameObject> outlawsToRemove = new List<GameObject>();
        foreach (GameObject outlaw in detectedOutlaws)
        {
            if (outlaw == null)
            {
                continue;
            }
            if (Vector3.Distance(transform.position, outlaw.transform.position) > detectionRadius)
            {
                outlawsToRemove.Add(outlaw);
            }
        }
        foreach (GameObject outlawToRemove in outlawsToRemove)
        {
            detectedOutlaws.Remove(outlawToRemove);
            ChangeMaterialColor(outlawToRemove, normalMaterial);
        }
    }
    private void ChangeMaterialColor(GameObject obj, Color material)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        if (renderer != null)
        {        
            renderer.material.color = material;
        }
    }
    private IEnumerator ScanAreaActivation()
    {
        scanButton.interactable = false;
        yield return new WaitForSeconds(3f);
        isActivated = true;
        yield return new WaitForSeconds(3f);
        isActivated = false;
        scanButton.interactable = true;
    }
    public void ArrestOutlaw()
    {
        handcuffs.gameObject.SetActive(true);
        popup.transform.DOMove(closePos.position, popInDuration).SetEase(Ease.InOutSine);
        arrestButton.interactable = false;
        isNear = false;
        Destroy(target);
        ShowPopInEffect();
    }
    private void ShowPopInEffect()
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
        handcuffs.gameObject.SetActive(false);
    }

    private void FlickerButton()
    {
        Sequence flickerSequence = DOTween.Sequence();

        for (int i = 0; i < flickerCount; i++)
        {
            flickerSequence.Append(arrestButton.image.DOColor(new Color(originalColor.r, originalColor.g, originalColor.b, 0f), flickerDuration / 2))
                .Append(arrestButton.image.DOColor(originalColor, flickerDuration / 2));
        }
        flickerSequence.OnComplete(() =>
        {
            Debug.Log("Flickering completed!");
        });
    }
}
