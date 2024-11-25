using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.VisualScripting.Metadata;

public class DragItemScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
    #region Fields
    [SerializeField] 
    private Canvas              _canvas;
    private RectTransform       _rectTransform;
    private CanvasGroup         _canvasGroup;
    public static Vector3       _initialPos;
    #endregion

    #region Properties
    public static Vector3 InitialPos
    {
        get{ return _initialPos; }
    }
    #endregion

    private void Awake() 
    {
        _rectTransform          = GetComponent<RectTransform>();
        _canvasGroup            = GetComponent<CanvasGroup>();
    }

    #region Public Methods
    public void OnBeginDrag( PointerEventData eventData ) 
    {
        _canvasGroup.blocksRaycasts     = false;
        _initialPos                     = transform.position;

        string str = string.Format( "OnBeginDrag - Alpha: {0}, blockRayCasts: {1}", _canvasGroup.alpha, _canvasGroup.blocksRaycasts.ToString() );
        Debug.Log( str );
    }

    public void OnDrag( PointerEventData eventData ) 
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag( PointerEventData eventData )
    {
         if( _canvasGroup.alpha == 0 && transform.position == _initialPos  )
            _canvasGroup.alpha                  = 1f;

         if( _canvasGroup.alpha == 1 && transform.position != _initialPos )
            transform.position = _initialPos;

        _canvasGroup.blocksRaycasts  = true;

        string str = string.Format( "OnEndDrag - Alpha: {0}, blockRayCasts: {1}", _canvasGroup.alpha, _canvasGroup.blocksRaycasts.ToString() );
        Debug.Log( str );
    }

    public void OnPointerDown( PointerEventData eventData ) 
    {
        Debug.Log("OnPointerDown");
    }
    #endregion
}

