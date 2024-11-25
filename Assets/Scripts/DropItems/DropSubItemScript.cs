using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;
public class DropSubItemScript : MonoBehaviour, IDropHandler
{
    #region Fields
    static string                       _lastNum        = "";
    public CauldronNumberAnimation      _numberAnimation;
    const string                        NUMBER_PATH     = "Props/Numeros/";
    const int                           MAX_NUMBER      = 99;
    const int                           MIN_NUMBER      = 45;
    const string                        MAIN_PATH       = "Main";
    const string                        NUM             = "Num";
    SoundManager                        _soundManager;
    public GameObject                   _starPrefab;
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
            if( eventData.pointerDrag != null )
                eventData.pointerDrag.GetComponent<RectTransform>().position = DragItemScript.InitialPos;
            
            //Get item being dragged.
            var item          = eventData.pointerDrag.GetComponent<Transform>();
            if( item == null )
                return;
            
            _soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
            HandleItemInteraction( item );
        }//try
        catch( Exception ex )
        {
            Debug.LogException(ex);
        }
    }
    #endregion

    #region Private Methods
    private void HandleItemInteraction( Transform item )
    {
        if( item == null ) 
            return;

       var potion = item.GetComponent<Image>();
       var sprite = Resources.Load<Sprite>( "Props/Pocao/pocao" );
        if( sprite == null || potion == null )
            return;

        SoundManager._instance.PlaySFX( _soundManager._pouringWater );
        potion.sprite = sprite;

        Transform firstNumChild = transform.GetChild(0);
        if( !transform.name.Contains( MAIN_PATH ) || !firstNumChild.name.Contains( NUM ) )
            return;

        var firstImgChild = firstNumChild.GetComponentInChildren<Image>();
        if( firstImgChild == null )
            return;

        Transform secNumChild = transform.GetChild(1);
        if( !secNumChild.name.Contains( NUM ) )
            return;

        var secImgChild = secNumChild.GetComponentInChildren<Image>();
        if( secImgChild == null )
            return;


        var tag         = item.tag as string;
        int number      = 0, cauldronTag = 0;
        if( int.TryParse( tag, out number ) )
        {
            if( string.IsNullOrEmpty( _lastNum ) && transform.tag != null )
                cauldronTag         = int.Parse( transform.tag );
            else
                cauldronTag         = int.Parse( _lastNum );
    
            var animation = GetComponent<CauldronNumberAnimation>();
            if( animation != null )
                animation.OnPotionAdded(cauldronTag, number);


            int newNumber   = CalculateNewNumber( cauldronTag, number );
            var digits      = GetDigits( newNumber );

            var firstSprite = Resources.Load<Sprite>( NUMBER_PATH + digits.firstDigit.ToString() );
            if( firstSprite == null )
                return;

            firstImgChild.sprite = firstSprite;

            var secSprite = Resources.Load<Sprite>( NUMBER_PATH + digits.secondDigit.ToString() );
            if( secSprite == null )
                return;

            secImgChild.sprite = secSprite;
        }
    }

    private int CalculateNewNumber( int currentNumber, int subtractNumber )
    {
        int result      = currentNumber - subtractNumber;
        _lastNum        = result.ToString();

        if( result < MIN_NUMBER )
        {
            SoundManager._instance.PlaySFX( _soundManager._error );
            _lastNum    = MAX_NUMBER.ToString();
            return MAX_NUMBER;
        }
        //45
        else if( result == MIN_NUMBER )
        {
            Canvas canvas                   = GetComponentInParent<Canvas>();
           ItemFallManager.SpawnFallingItem( _starPrefab, transform.position, canvas.transform );



            var canvasGroup                 = canvas.GetComponent<CanvasGroup>();
            canvasGroup.interactable        = false; // Desabilita interações
            canvasGroup.blocksRaycasts      = false; // Bloqueia cliques
            SoundManager._instance.PlaySFX( _soundManager._success );
        }

        return result;
    }
    /// <summary>
    /// Using tuple so we can return both digits.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private (int firstDigit, int secondDigit) GetDigits(int number)
    {
        if( number < MIN_NUMBER )
            return (9, 9);

        int firstDigit  = number / 10;
        int secondDigit = number % 10;

        if( number < 10 )
            firstDigit = 0;

        return( firstDigit, secondDigit );
    }
    #endregion
}
