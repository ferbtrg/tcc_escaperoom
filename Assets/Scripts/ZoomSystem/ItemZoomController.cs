using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ItemZoomController : MonoBehaviour 
{
    #region Fields
    public float _itemZoomScale = 2f;
    public float _cameraNormalSize = 5f;
    public float _cameraCloseSize = 2f;
    
    private bool _isZoomedIn = false;
    private Vector2 _originalPosition;
    private Vector3 _originalScale;
    private Camera _camera;
    private Canvas _parentCanvas;
    public FloatingBooksManager _floatingBooks;
    #endregion

    #region Private Methods
    private void Start()
    {
        _camera = Camera.main;
        _parentCanvas = GetComponentInParent<Canvas>();
        
        if (_camera == null || _parentCanvas == null)
        {
            Debug.LogError("Missing camera or canvas reference!");
            return;
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        _originalPosition = rectTransform.anchoredPosition;
        _originalScale = rectTransform.localScale;
        
        _floatingBooks = FindObjectOfType<FloatingBooksManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        if (_isZoomedIn)
        {
            // Se já está com zoom, qualquer clique fora do item reseta
            if (!IsPointerOverThis())
            {
                ZoomOut();
            }
        }
        else
        {
            // Se clicou no item, dá zoom
            if (IsPointerOverThis())
            {
                ZoomIn();
            }
        }
    }

    private bool IsPointerOverThis()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        
        var results = new System.Collections.Generic.List<RaycastResult>();
        _parentCanvas.GetComponent<GraphicRaycaster>().Raycast(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject == gameObject)
                return true;
        }

        return false;
    }

    private void ZoomIn()
    {
        _isZoomedIn = true;
        
        RectTransform rectTransform                 = GetComponent<RectTransform>();
        _originalPosition                           = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition          = Vector2.zero;
        rectTransform.localScale = _originalScale * _itemZoomScale;

        if( _parentCanvas.name == "CanvasStar" )
        {
            GameObject star = GameObject.Find("Canvas");
            foreach (var graphic in star.GetComponentsInChildren<Graphic>())
            {
                if (!graphic.transform.IsChildOf(transform) && graphic.gameObject != gameObject)
                {
                    Color color = graphic.color;
                    color.a = 0;
                    graphic.color = color;
                }
            }
        }
        else
        { 
                
            // Esconde outros elementos UI
            foreach (var graphic in _parentCanvas.GetComponentsInChildren<Graphic>())
            {
                if (!graphic.transform.IsChildOf(transform) && graphic.gameObject != gameObject)
                {
                    Color color = graphic.color;
                    color.a = 0;
                    graphic.color = color;
                }
            }
        }
        _camera.orthographicSize = _cameraCloseSize;
        
        if (_floatingBooks != null)
            _floatingBooks.OnZoomIn();
    }

    private void ZoomOut()
    {
        _isZoomedIn = false;
        
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = _originalPosition;
        rectTransform.localScale = _originalScale;
        
        if( _parentCanvas.name == "CanvasStar" )
        {
            GameObject star = GameObject.Find("Canvas");
            foreach (var graphic in star.GetComponentsInChildren<Graphic>())
            {
                if (!graphic.transform.IsChildOf(transform) && graphic.gameObject != gameObject)
                {
                    Color color = graphic.color;
                    color.a = 1;
                    graphic.color = color;
                }
            }
        }
        else
        {
            // Restaura visibilidade dos elementos UI
            foreach (var graphic in _parentCanvas.GetComponentsInChildren<Graphic>())
            {
                if (!graphic.transform.IsChildOf(transform) && graphic.gameObject != gameObject)
                {
                    Color color = graphic.color;
                    color.a = 1;
                    graphic.color = color;
                }
            }
        }
        _camera.orthographicSize = _cameraNormalSize;
        
        if (_floatingBooks != null)
            _floatingBooks.OnZoomOut();
    }
    #endregion
}