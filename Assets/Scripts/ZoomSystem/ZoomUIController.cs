using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoomUIController : MonoBehaviour 
{
    #region Fields
    public float                _normalZoom             = 1f;
    public float                _closeZoom              = 2f;
    public float                _minZoom                = 0.5f;
    public float                _maxZoom                = 3f;
    public float                _cameraNormalSize       = 5f;
    public float                _cameraCloseSize        = 2f;
    public float                _itemZoomScale          = 10f; // Escala do item quando der zoom
    public GameObject           item;
    public bool                 _ignoreUILayerForZoomOut = true;

    private float               _zoom;
    private bool                _isZoomedIn = false;
    private CanvasScaler        _canvasScaler;
    private Vector2             _originalPosition;
    private Vector3             _originalScale;  // Guarda escala original do item
    private GraphicRaycaster    _raycaster;
    private EventSystem         _eventSystem;
    private Camera             _camera;
    private Dictionary<Graphic, float> _originalAlphas = new Dictionary<Graphic, float>();
    public FloatingBooksManager _floatingBooks;
    #endregion

    #region Private Methods
    private void Start()
    {
        _canvasScaler                   = transform.GetComponent<CanvasScaler>();
        _raycaster                      = transform.GetComponent<GraphicRaycaster>();
        _eventSystem                    = EventSystem.current;
        _camera                         = Camera.main;
        
        _zoom                           = _normalZoom;
        _canvasScaler.scaleFactor       = _zoom;
        _camera.orthographicSize        = _cameraNormalSize;

        if( item != null )
        {
            RectTransform itemRect      = item.GetComponent<RectTransform>();
            _originalPosition           = itemRect.anchoredPosition;
            _originalScale             = itemRect.localScale;  // Guarda escala original
        }

        StoreOriginalAlphas();
        _floatingBooks = FindObjectOfType<FloatingBooksManager>();
    }

    private void StoreOriginalAlphas()
    {
        _originalAlphas.Clear();
        Graphic[] graphics = transform.GetComponentsInChildren<Graphic>(true);
        foreach (Graphic graphic in graphics)
        {
            if (graphic.gameObject != item && !item.GetComponentsInChildren<Graphic>().Contains(graphic))
            {
                _originalAlphas[graphic] = graphic.color.a;
            }
        }
    }

    private void SetGraphicsVisibility(bool visible)
    {
        foreach (var kvp in _originalAlphas)
        {
            Graphic graphic = kvp.Key;
            if (graphic != null)
            {
                Color color = graphic.color;
                color.a = visible ? kvp.Value : 0f;
                graphic.color = color;
            }
        }
    }

    private void Update()
    {
        HandleZoom();
    }

    private void HandleZoom()
    {
        if( !Input.GetMouseButtonDown(0) )
            return;

        PointerEventData pointerData    = new PointerEventData( _eventSystem );
        pointerData.position            = Input.mousePosition;
        
        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast( pointerData, results );

        bool clickedUI      = results.Count > 0;
        bool clickedItem   = results.Exists(r => r.gameObject == item);

        if( _isZoomedIn && ( !clickedUI || ( _ignoreUILayerForZoomOut && !clickedItem ) ) )
            ZoomOut();
        else if (!_isZoomedIn && clickedItem)
            ZoomIn();
    }

    private void ZoomIn()
    {
        _isZoomedIn = true;
        _zoom = _closeZoom;
        
        if( item != null )
        {
            RectTransform itemRect = item.GetComponent<RectTransform>();
            _originalPosition = itemRect.anchoredPosition;
            itemRect.anchoredPosition = Vector2.zero;
            
            // Aumenta a escala do item
            itemRect.localScale = _originalScale * _itemZoomScale;
        }
                
        SetGraphicsVisibility(false);
        _camera.orthographicSize = _cameraCloseSize;
        
        if( _floatingBooks != null)
            _floatingBooks.OnZoomIn();

        ApplyZoom();
    }

    private void ZoomOut()
    {
        _isZoomedIn = false;
        _zoom = _normalZoom;
        
        if( item != null )
        {
            RectTransform itemRect = item.GetComponent<RectTransform>();
            itemRect.anchoredPosition = _originalPosition;
            
            // Restaura escala original do item
            itemRect.localScale = _originalScale;
        }
        
        SetGraphicsVisibility(true);
        _camera.orthographicSize = _cameraNormalSize;
        
        if( _floatingBooks != null) 
            _floatingBooks.OnZoomOut();
        
        ApplyZoom();
    }

    private void ApplyZoom()
    {
        _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
        _canvasScaler.scaleFactor = _zoom;
    }
    #endregion
}