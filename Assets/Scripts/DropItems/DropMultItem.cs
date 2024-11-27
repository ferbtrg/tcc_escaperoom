using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropMultItem : MonoBehaviour, IDropHandler
{
 #region Fields
    [Header("Sound")]
    [SerializeField] private AudioClip sound;
    SoundManager _soundManager;
    #endregion

    #region Public Methods
    /// <summary>
    /// On drop item method, this is to drop items on inventory.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop( PointerEventData eventData ) 
    {
      try
      {

          Debug.Log("OnDrop");

          if( eventData.pointerDrag == null )
                return;

            eventData.pointerDrag.GetComponent<RectTransform>().position = DragItemScript.InitialPos;
            SoundManager._instance.PlaySound(sound);
            AddBooksToShelves( eventData );

      }//try
      catch( Exception ex )
      {
          Debug.LogException( ex );
      }

    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        _soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    private void CloneBookAtInitialPosition( PointerEventData eventData )
    {
        var item        = eventData.pointerDrag;
        var parent      = item.transform.parent;
        var alpha       = item.GetComponent<CanvasGroup>().alpha = 0;
        var children    = parent.GetComponentsInChildren<Transform>();
        var gameObj     = item.gameObject;


        if( item.transform.childCount == 2 )
        {
                GameObject book                                     = Instantiate( gameObj, DragItemScript.InitialPos, item.transform.rotation, parent );
                book.transform.localScale                           = item.transform.localScale;
                book.name                                           = item.transform.name;
                book.GetComponent<CanvasGroup>().blocksRaycasts     = true;
                book.GetComponent<CanvasGroup>().alpha              = 1;
        }

    }

    private void AddBooksToShelves( PointerEventData eventData )
    {
         //Fazer a lógica pra ver se o eventDat.pointerDrag tem o nome que o transform.name, se tiver coloca o livro lá.
        GameObject      item                = eventData.pointerDrag;
        string          shelfName           = transform.tag;
        BookGroupController book            = GetComponent<BookGroupController>();
        // Identifica a cor da flor que foi dropada
        string bookColor                       = eventData.pointerDrag.tag;


        if( shelfName != null &&  bookColor != null && shelfName == bookColor )
        {
            ShowBookAccordingToColor( bookColor );
            SoundManager._instance.PlaySFX( _soundManager._success );
        }
        else
            SoundManager._instance.PlaySFX( _soundManager._error );
    }

    private void ShowBookAccordingToColor( string color )
    {

        var book = transform.GetChild( 0 );
        if( book == null || !book.name.Contains(color)  )
            return;


        Debug.Log($"Tentando mostrar flor da cor: {color}");

        book.gameObject.SetActive( true );
    }
    #endregion
}
