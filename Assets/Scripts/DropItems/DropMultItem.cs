using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropMultItem : MonoBehaviour, IDropHandler
{
 #region Fields
    [Header("Shelves")]
    [SerializeField] private GameObject _mainShelf;
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
           
        //TODO: 
        //Fazer a l�gica de enfiar o livro em cada buraco da estante.

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

          SoundManager._instance.PlaySound(sound);
          _mainShelf.GetComponent<BookGroupController>();

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

    private void AddBooksToShelves( PointerEventData eventData )
    {
         //Fazer a l�gica pra ver se o eventDat.pointerDrag tem o nome que o transform.name, se tiver coloca o livro l�.
        GameObject      item                = eventData.pointerDrag;
        string          shelfName           = transform.name;
        BookGroupController book            = _mainShelf.GetComponent<BookGroupController>();
        
        // Identifica a cor da flor que foi dropada
        string bookColor                       = eventData.pointerDrag.tag;
        _mainShelf.GetComponent<BookGroupController>();

        // Mostra a flor correspondente no grupo
        book.ShowBook( bookColor );
    }
    #endregion
}
