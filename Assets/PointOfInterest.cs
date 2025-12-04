using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PointOfInterest : MonoBehaviour
{
    [Header("XR Interaction")]
    [SerializeField] private XRBaseInteractable grabInteractable;

    [Header("Point of Interest Object")]
    [SerializeField] private GameObject pointOfInterestPrefab;
    [SerializeField] private Vector3 poiRotation;
    [SerializeField] private Vector3 poiPosition;
    [SerializeField] private Light spotLight;

    [SerializeField] private GameObject uiElement;

    [Header("Visual Feedback & Animation")]
    [SerializeField] private Color hoverColorTint = new(1.2f, 1.2f, 1.2f, 1f);
    [SerializeField] private float moveDuration = 0.75f;
    [SerializeField] private float rotateDuration = 0.4f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float uiFadeInDelay = 0.2f;
    [SerializeField] private float uiFadeDuration = 0.3f;
    [SerializeField] private float downwardOffset = 0.5f;
    [SerializeField] private float forwardOffset = 1.0f;

    [Header("Information")]
    [SerializeField] private string title;
    [SerializeField] private string description;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    private Renderer _objectRenderer;
    private Material _poiMaterialInstance;
    private CanvasGroup _uiCanvasGroup;
    private Color _originalColor;
    private bool _isPoiVisible;
    private Transform _mainCameraTransform;
    private Sequence _poiShowSequence;
    private Sequence _poiHideSequence;
    private bool _allowContinuousLookAt;
    private GameObject _pointOfInterest;

    private void Awake()
    {
        if (!grabInteractable) grabInteractable = GetComponent<XRGrabInteractable>();

        if (!pointOfInterestPrefab)
        {
            Debug.LogError("Point Of Interest GameObject not assigned!", this);
            enabled = false; return;
        }
        if (!uiElement)
        {
            Debug.LogError("UI Element GameObject not assigned!", this);
            enabled = false; return;
        }

        _objectRenderer = grabInteractable.GetComponent<Renderer>();
        if (_objectRenderer)
        {
            _objectRenderer.material = new Material(_objectRenderer.material);
            _originalColor = _objectRenderer.material.color;
        }
        else
        {
            Debug.LogWarning($"{nameof(XRGrabInteractable)} is missing a Renderer component. Hover color change will not work.", this);
        }


        _mainCameraTransform = Camera.main?.transform;
        if (_mainCameraTransform) return;

        Debug.LogError("Main Camera not found. Ensure your main VR camera is tagged 'MainCamera'.", this);
        enabled = false;
    }

    private void Start()
    {
        titleText.text = title;
        descriptionText.text = description;

        _pointOfInterest = Instantiate(pointOfInterestPrefab, transform.position, Quaternion.identity);

        poiPosition = grabInteractable.transform.position;
        poiPosition.y += 1f;

        _pointOfInterest.transform.position = poiPosition;
        _pointOfInterest.transform.rotation = Quaternion.Euler(poiRotation);


        var poiRenderer = _pointOfInterest.GetComponent<Renderer>();
        if (poiRenderer)
        {
            if(poiRenderer.material) {
                 _poiMaterialInstance = new Material(poiRenderer.material);
                 poiRenderer.material = _poiMaterialInstance;
            } else {
                 Debug.LogWarning($"Point Of Interest GameObject has a Renderer but no Material assigned. Fading may not work.", this);
            }
        }
        else
        {
            Debug.LogWarning("Point Of Interest GameObject is missing a Renderer component. Fade effect might not work as expected.", this);
        }

        grabInteractable.gameObject.transform.position = poiPosition;

        _pointOfInterest.SetActive(false);
        uiElement.SetActive(false);

        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.hoverEntered.AddListener(OnHoverEntered);
        grabInteractable.hoverExited.AddListener(OnHoverExited);

    }

     private void Update()
     {
         if (!_isPoiVisible || !_allowContinuousLookAt || !_mainCameraTransform || !_pointOfInterest.activeSelf) return;

         _pointOfInterest.transform.LookAt(_mainCameraTransform.position);

         uiElement.transform.LookAt(_mainCameraTransform.position, Vector3.up);
         uiElement.transform.Rotate(0f, 180f, 0f);

         spotLight.transform.LookAt(_pointOfInterest.transform.position);
         spotLight.transform.position = _pointOfInterest.transform.position + Vector3.back * 2.5f;
     }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (!_objectRenderer || !_objectRenderer.material) return;

        _objectRenderer.material.DOKill();
        _objectRenderer.material.DOColor(_originalColor * hoverColorTint, 0.1f);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (!_objectRenderer || !_objectRenderer.material) return;

        _objectRenderer.material.DOKill();
        _objectRenderer.material.DOColor(_originalColor, 0.1f);
    }

    public void ToggleVisibility()
    {
        _poiShowSequence?.Kill();
        _poiHideSequence?.Kill();
        _allowContinuousLookAt = false;

        if (!_isPoiVisible)
        {
            _pointOfInterest.transform.position = poiPosition;
            _pointOfInterest.transform.rotation = Quaternion.Euler(poiRotation);

            _pointOfInterest.SetActive(true);
            grabInteractable.gameObject.SetActive(false);

            if (_poiMaterialInstance)
            {
                var currentPoiColor = _poiMaterialInstance.color;
                _poiMaterialInstance.color = new Color(currentPoiColor.r, currentPoiColor.g, currentPoiColor.b, 1f);
            }

            var targetPosition = _mainCameraTransform.position
                                 + _mainCameraTransform.forward * forwardOffset
                                 + Vector3.down * downwardOffset;

            _poiShowSequence = DOTween.Sequence();

            _poiShowSequence.Append(_pointOfInterest.transform.DOMove(targetPosition, moveDuration).SetEase(Ease.OutQuad));
            var lookAt = _mainCameraTransform.position;
            _poiShowSequence.Append(_pointOfInterest.transform.DOLookAt(lookAt, rotateDuration).SetEase(Ease.OutSine));

            _poiShowSequence.OnComplete(() => {
                _allowContinuousLookAt = true;
                 uiElement.SetActive(true);
                 uiElement.gameObject.transform.position = targetPosition + Vector3.left * 0.8f + Vector3.up * 0.1f;

                 spotLight.gameObject.SetActive(true);
                 spotLight.transform.position = targetPosition + Vector3.up * 0.5f;
                 spotLight.transform.LookAt(_pointOfInterest.transform.position);
            });


            _poiShowSequence.Play();
            _isPoiVisible = true;
        }
        else
        {
             _allowContinuousLookAt = false;

            _poiHideSequence = DOTween.Sequence();

            _poiHideSequence.Append(_pointOfInterest.transform.DORotate(poiRotation, rotateDuration).SetEase(Ease.OutSine));
            _poiHideSequence.Append(_pointOfInterest.transform.DOLocalMove(poiPosition, moveDuration).SetEase(Ease.OutQuad));

            if (_poiMaterialInstance)
            {
                _poiHideSequence.Append(_poiMaterialInstance.DOFade(0f, fadeDuration).SetEase(Ease.InQuad));
            }

            _poiHideSequence.OnComplete(() => {
                _pointOfInterest.SetActive(false);
                uiElement.SetActive(false);
                _isPoiVisible = false;
                grabInteractable.gameObject.SetActive(true);
                spotLight.gameObject.SetActive(false);
             });

             _poiHideSequence.Play();
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        ToggleVisibility();
    }

    private void OnDestroy()
    {
        if (grabInteractable)
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
            grabInteractable.hoverExited.RemoveListener(OnHoverExited);
        }

        _poiShowSequence?.Kill();
        _poiHideSequence?.Kill();

        if (_poiMaterialInstance)
        {
            Destroy(_poiMaterialInstance);
        }
    }
}