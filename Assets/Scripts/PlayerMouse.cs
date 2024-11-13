using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;


public class PlayerMouse : MonoBehaviour
{
    #region Fields
    private float   _startPosX;
    private float   _startPosY;
    private Camera  _mainCamera;
    #endregion

    #region Private Methods
    //Maybe inserting this on Update?
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        //0 is left button
        if( !Input.GetMouseButtonDown(0) )
            return;
    }

    private void OnMouseUp()
    {

    }
    #endregion

    #region Public Methods
    /// <summary>
    /// On click event
    /// </summary>
    public void OnMouseClick( InputAction.CallbackContext context )
    {
        if( !context.started )
            return;

        var rayHit = Physics2D.GetRayIntersection( _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue() ) );
        //we are only clicking on a object that has a Collider
        if( !rayHit.collider )
            return;

        //Check to see if click is working
        Debug.Log( rayHit.collider.gameObject.name );
    }
    #endregion
}
