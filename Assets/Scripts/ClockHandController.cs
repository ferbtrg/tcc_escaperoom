using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClockHandController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    #region Fields
    [SerializeField] private float _rotationSpeed       = 1.5f;
    private bool            _isDragging                 = false;
    private RectTransform   _rectTransform;
    private Camera          _canvasCamera;
    private float           _lastAngle;
    private Canvas          _parentCanvas;
    #endregion

    #region Private Methods
    private void Awake()
    {
        _rectTransform  = GetComponent<RectTransform>();
        _parentCanvas   = GetComponentInParent<Canvas>();
        _canvasCamera   = _parentCanvas.worldCamera;
    }
    #endregion

    #region Public Methods
    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging                 = true;
      // Converte a posição do mouse considerando o zoom do canvas
        Vector2 mousePosition       =  eventData.position;
        Vector2 centerPosition      = GetCanvasSpacePosition(transform.parent.position);
        Vector2 direction           = mousePosition - centerPosition;
        _lastAngle                  = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if( !_isDragging )
            return;

        //Save mouse position
        Vector2 mousePosition   = eventData.position;
        //Get clock's center position on screen.
        Vector2 centerPosition  = GetCanvasSpacePosition(transform.parent.position);
        //Calculates the direction of the mouse relative to the center.
        Vector2 direction       = mousePosition - centerPosition;


        //Calculate the angle in degress from the mouse position relative to the center
        float currentAngle      = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Calculate the smallest difference between the last angle and current angle
        //DeltaAngle returns the shortest distance between two angles
        float angleDifference   = Mathf.DeltaAngle( _lastAngle, currentAngle );

        Vector3 newRotation             = _rectTransform.eulerAngles;
        newRotation.z                   += angleDifference * _rotationSpeed;
        _rectTransform.eulerAngles      = newRotation;

        //Store the current angle for the next frame's calculation
        _lastAngle                      = currentAngle;
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        _isDragging = false;
    }
    private Vector2 GetCanvasSpacePosition(Vector3 worldPosition)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint( _canvasCamera, worldPosition );
        return screenPoint;
    }
    #endregion

}
