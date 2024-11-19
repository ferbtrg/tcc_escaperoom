using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.VisualScripting.Metadata;

public class DragItemScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
    #region Fields
    [SerializeField] private Canvas canvas;
    private RectTransform   _rectTransform;
    private CanvasGroup     _canvasGroup;
    public static Vector3          _initialPos;
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
        _initialPos = transform.position;

        // Obtém todos os Children do objeto pai
        var children = transform.parent.GetComponentsInChildren<Transform>();
        
        // Verifica se o objeto atual (this) não tem outros filhos
        if( transform.childCount == 0 && transform.parent.childCount < 15 )
        {
            GameObject newFlower            = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
            newFlower.name                  = gameObject.name; 
            newFlower.transform.localScale  = transform.localScale;
        }

        _canvasGroup.alpha           = .6f;
        _canvasGroup.blocksRaycasts  = false;

        string str = string.Format( "OnBeginDrag - Alpha: {0}, blockRayCasts: {1}", _canvasGroup.alpha, _canvasGroup.blocksRaycasts.ToString() );
        Debug.Log( str );
    }

    public void OnDrag( PointerEventData eventData ) 
    {
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
     //   string str = string.Format( "OnDrag - Alpha: {0}, blockRayCasts: {1}", canvasGroup.alpha, canvasGroup.blocksRaycasts.ToString() );
     //   Debug.Log( str );
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        //_canvasGroup.alpha           = 1f;
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

