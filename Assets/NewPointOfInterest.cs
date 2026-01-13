using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.UI;

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

    [Header("Motif")]
    [SerializeField] private GameObject motifPrefab;

    private int currentCard = 0;
    private Transform motifHolder;

    private void Awake()
    {
        if (!grabInteractable) grabInteractable = GetComponent<XRGrabInteractable>();

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
            uiCloseButtonRectTransform.anchoredPosition = new Vector2(0, uiCloseButtonRectTransform.anchoredPosition.y);
        }
        else
        {
            if (!uiNextPageButton.activeSelf)
            {
                RectTransform uiCloseButtonRectTransform = uiCloseButton.GetComponent<RectTransform>();
                uiCloseButtonRectTransform.anchoredPosition = new Vector2(75, uiCloseButtonRectTransform.anchoredPosition.y);

                uiNextPageButton.SetActive(true);
            }
        }

        motifHolder = GameObject.Find("MotifLocation").GetComponent<Transform>();

        if (!motifHolder)
        {
            Debug.LogError("Couldnt find transform of motif location");
        }


    }

    private void Start()
    {

        grabInteractable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        ToggleVisibility();
    }

    public void ToggleVisibility()
    {
        if (!uiElement.activeSelf)
        {
            uiElement.SetActive(true);
        }

        currentCard = 0;

        loadCard();
        Button uiButton = uiNextPageButton.GetComponent<Button>();
        uiButton.onClick.AddListener(turnPage);

        Button uiCloseButtonBtn = uiCloseButton.GetComponent<Button>();
        uiCloseButtonBtn.onClick.AddListener(removeMotifObject);

        if(motifHolder.childCount > 0)
        {
            for (int i = motifHolder.childCount - 1; i >= 0; i--)
            {
                Destroy(motifHolder.GetChild(i).gameObject);
            }
        }

        GameObject motif = Instantiate(motifPrefab, motifHolder);

        motif.transform.localPosition = Vector3.zero;
        motif.transform.localRotation = Quaternion.identity;
        motif.transform.localScale = Vector3.one;

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
