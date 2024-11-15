using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseUIController : MonoBehaviour 
{
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

    void Start()
    {
        _canvasScaler    = transform.GetComponent<CanvasScaler>();
        _raycaster       = transform.GetComponent<GraphicRaycaster>();
        _eventSystem     = EventSystem.current;
        
        _zoom                        = _normalZoom;
        _canvasScaler.scaleFactor    = _zoom;

        if( _clock != null )
        {
            RectTransform clockRect     = _clock.GetComponent<RectTransform>();
            _originalPosition            = clockRect.anchoredPosition;
        }
    }

    void Update()
    {
        HandleZoom();
    }

    void HandleZoom()
    {
        if( !Input.GetMouseButtonDown(0) )
            return;

        // Verifica se clicou em algum elemento UI
        PointerEventData pointerData    = new PointerEventData(_eventSystem);
        pointerData.position            = Input.mousePosition;
        
        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(pointerData, results);

        bool clickedUI      = results.Count > 0;
        bool clickedClock   = results.Exists(r => r.gameObject == _clock);

        // Se está com zoom e clicou fora do relógio
        if( _isZoomedIn && (!clickedUI || (_ignoreUILayerForZoomOut && !clickedClock)) )
            ZoomOut();
        // Se não está com zoom e clicou no relógio
        else if (!_isZoomedIn && clickedClock)
            ZoomIn();
    }

    void ZoomIn()
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
    }

    void ZoomOut()
    {
        _isZoomedIn             = false;
        _zoom                   = _normalZoom;
        
        if( _clock != null )
        {
            RectTransform panelRect         = _clock.GetComponent<RectTransform>();
            panelRect.anchoredPosition      = _originalPosition;
        }
        
        ApplyZoom();
    }

    void ApplyZoom()
    {
        _zoom                       = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
        _canvasScaler.scaleFactor   = _zoom;
    }
}