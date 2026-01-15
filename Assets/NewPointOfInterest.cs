using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NewPointOfInterest : MonoBehaviour
{

    [Header("XR Interaction")]
    [SerializeField] private XRBaseInteractable grabInteractable;

    [Header("Info Cards")]
    [SerializeField] private Sprite[] infoCards;
    [SerializeField] private GameObject uiElement;
    [SerializeField] private Image uiCard;
    [SerializeField] private GameObject uiCloseButton;
    [SerializeField] private GameObject uiNextPageButton;
    [SerializeField] private GameObject uiButtonBackground;

    [Header("Motif")]
    [SerializeField] private GameObject motifPrefab;

    [Header("Visual Feedback & Animation")]
    [SerializeField] private float moveDuration = 0.75f;
    [SerializeField] private float rotateDuration = 0.4f;
    [SerializeField] private bool doDoTween = true;

    private int currentCard = 0;
    private Transform motifHolder;

    private Sequence poiShowSequence;
    private GameObject pointOfInterest;
    private Vector3 poiPosition;

    private void Awake()
    {
        if (!grabInteractable) grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Start()
    {

        grabInteractable.selectEntered.AddListener(OnSelectEntered);

        if (doDoTween)
        {
            pointOfInterest = Instantiate(motifPrefab, transform.position, Quaternion.identity);

            poiPosition = grabInteractable.transform.position;
            poiPosition.y += 1f;

            pointOfInterest.transform.position = poiPosition;
            pointOfInterest.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        ToggleVisibility();
    }

    public void ToggleVisibility()
    {

        if (infoCards.Length < 1)
        {
            Debug.LogError("Missing info card sprites");
        }
        else if (infoCards.Length < 2)
        {
            if (uiNextPageButton.activeSelf)
            {
                uiNextPageButton.SetActive(false);
            }

            RectTransform uiCloseButtonRectTransform = uiCloseButton.GetComponent<RectTransform>();
            uiCloseButtonRectTransform.anchoredPosition = new Vector2(190, uiCloseButtonRectTransform.anchoredPosition.y);

            RectTransform uiButtonBackgroundRectTransform = uiButtonBackground.GetComponent<RectTransform>();
            uiButtonBackgroundRectTransform.sizeDelta = new Vector2(200, uiButtonBackgroundRectTransform.sizeDelta.y);
        }
        else
        {
            if (!uiNextPageButton.activeSelf)
            {
                Debug.Log("Enabling next page button");
                RectTransform uiCloseButtonRectTransform = uiCloseButton.GetComponent<RectTransform>();
                uiCloseButtonRectTransform.anchoredPosition = new Vector2(265, uiCloseButtonRectTransform.anchoredPosition.y);

                RectTransform uiButtonBackgroundRectTransform = uiButtonBackground.GetComponent<RectTransform>();
                uiButtonBackgroundRectTransform.sizeDelta = new Vector2(350, uiButtonBackgroundRectTransform.sizeDelta.y);

                uiNextPageButton.SetActive(true);
            }
            Debug.Log("Multiple info cards detected");
        }

        motifHolder = GameObject.Find("MotifLocation").GetComponent<Transform>();

        if (!motifHolder)
        {
            Debug.LogError("Couldnt find transform of motif location");
        }

        if (!uiElement.activeSelf)
        {
            uiElement.SetActive(true);
        }

        currentCard = 0;

        loadCard();
        

        Button uiButton = uiNextPageButton.GetComponent<Button>();
        uiButton.onClick.RemoveAllListeners();
        uiButton.onClick.AddListener(turnPage);

        Button uiCloseButtonBtn = uiCloseButton.GetComponent<Button>();
        uiCloseButtonBtn.onClick.RemoveAllListeners();
        uiCloseButtonBtn.onClick.AddListener(removeMotifObject);

        if (motifHolder.childCount > 0)
        {
            for (int i = motifHolder.childCount - 1; i >= 0; i--)
            {
                Destroy(motifHolder.GetChild(i).gameObject);
            }
        }

        if (doDoTween)
        {
            pointOfInterest.transform.SetParent(null);
            pointOfInterest.transform.position = poiPosition;
            pointOfInterest.transform.rotation = Quaternion.identity;


            poiShowSequence?.Kill();

            poiShowSequence = DOTween.Sequence();

            poiShowSequence.Append(pointOfInterest.transform.DOMove(motifHolder.position, moveDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                pointOfInterest.transform.SetParent(motifHolder);
                pointOfInterest.transform.localPosition = Vector3.zero;
            }
            ));
            poiShowSequence.Append(pointOfInterest.transform.DOLookAt(new Vector3(-2.0f, 1.12f, 4.4f), rotateDuration).SetEase(Ease.OutSine));


            poiShowSequence.Play();
        }
        else
        {
            GameObject motif = Instantiate(motifPrefab, motifHolder);

            motif.transform.localPosition = Vector3.zero;
            motif.transform.localRotation = Quaternion.identity;
            motif.transform.localScale = Vector3.one;
        }


    }

    public void turnPage()
    {
        currentCard += 1;
        if(currentCard== infoCards.Length)
        {
            currentCard = 0;
        }
        loadCard();

    }

    private void loadCard()
    {

        if (uiCard != null)
        {
            uiCard.sprite = infoCards[currentCard];
        }
        else
        {
            Debug.LogError("Image component could not be found on uiCard");
        }
    }

    public void removeMotifObject()
    {
        Destroy(motifHolder.GetChild(0).gameObject);
    }

}
