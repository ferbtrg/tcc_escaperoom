using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoomUIController : MonoBehaviour 
{
    #region Fields
    public float                _normalZoom     = 1f;
    public float                _closeZoom      = 2f;
    public float                _minZoom        = 0.5f;
    public float                _maxZoom        = 3f;
    public GameObject           _clock;
    public bool                 _ignoreUILayerForZoomOut = true;

    private float               _zoom;
    private bool                _isZoomedIn = false;
    private CanvasScaler        _canvasScaler;
    private Vector2             _originalPosition;
    private GraphicRaycaster    _raycaster;
    private EventSystem         _eventSystem;
    #endregion

    #region Private Methods
    private void Start()
    {
        _canvasScaler                   = transform.GetComponent<CanvasScaler>();
        _raycaster                      = transform.GetComponent<GraphicRaycaster>();
        _eventSystem                    = EventSystem.current;
        
        _zoom                           = _normalZoom;
        _canvasScaler.scaleFactor       = _zoom;

        if( _clock != null )
        {
            RectTransform clockRect     = _clock.GetComponent<RectTransform>();
            _originalPosition            = clockRect.anchoredPosition;
        }

        string str = string.Format( " Start - Zoom: {0}, ScaleFactor: {1}, OrigPos: {2}", _zoom, _canvasScaler.scaleFactor, _originalPosition );
        Debug.Log( str );
    }

    private void Update()
    {
        HandleZoom();
    }

    private void HandleZoom()
    {
        if( !Input.GetMouseButtonDown(0) )
            return;

        //Check if UI element was clicked.
        PointerEventData pointerData    = new PointerEventData( _eventSystem );
        pointerData.position            = Input.mousePosition;
        
        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast( pointerData, results );

        bool clickedUI      = results.Count > 0;
        bool clickedClock   = results.Exists(r => r.gameObject == _clock);

        // Has zoom and clicked outside the clock
        if( _isZoomedIn && ( !clickedUI || ( _ignoreUILayerForZoomOut && !clickedClock ) ) )
            ZoomOut();
        // No zoom and has clicked on the clock.
        else if (!_isZoomedIn && clickedClock)
            ZoomIn();

        string str = string.Format(" MouseUIController - HandleZoom: ZoomedIn: {0}, ClickedClock: {1} ", _isZoomedIn, clickedClock );
        Debug.Log( str );
    }

    private void ZoomIn()
    {
        _isZoomedIn     = true;
        _zoom           = _closeZoom;
        
        if( _clock != null )
        {
            RectTransform clockRect         = _clock.GetComponent<RectTransform>();
            // Salva a posição atual antes de centralizar
            _originalPosition               = clockRect.anchoredPosition;
            // Centraliza o painel
            clockRect.anchoredPosition      = Vector2.zero;
        }
        
        ApplyZoom();
        string str = string.Format(" ZoomIn - OrigPos: {0}, Zoom:{1}", _originalPosition, _zoom );
        Debug.Log( str );
    }

    private void ZoomOut()
    {
        _isZoomedIn             = false;
        _zoom                   = _normalZoom;
        
        if( _clock != null )
        {
            RectTransform panelRect         = _clock.GetComponent<RectTransform>();
            panelRect.anchoredPosition      = _originalPosition;
        }
        
        ApplyZoom();

        string str = string.Format(" ZoomOut - OrigPos: {0}, Zoom:{1}", _originalPosition, _zoom );
        Debug.Log(str);
    }

    private void ApplyZoom()
    {
        _zoom                       = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
        _canvasScaler.scaleFactor   = _zoom;

        string str = string.Format(" ApplyZoom - ScaleFactor: {0}, Zoom:{1}", _canvasScaler.scaleFactor, _zoom );
        Debug.Log(str);
    }
    #endregion
}