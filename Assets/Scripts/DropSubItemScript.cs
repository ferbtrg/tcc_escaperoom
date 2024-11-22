using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class DropSubItemScript : MonoBehaviour, IDropHandler
{
    #region Fields
    static string _lastNum = "";
     public CauldronNumberAnimation numberAnimation;
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

        DecreaseNumAccordingToCauldron( item );
      }//try
      catch( Exception ex )
      {
          Debug.LogException(ex);
      }
    }
    #endregion

    #region Private Methods
    private void DecreaseNumAccordingToCauldron( Transform item )
    {
        //Eu vou precisar do item pra saber qual é o valor que tá dentro da tag - ok
        //Vou precisar do tranform pra subtrair o valor da tag do número do trasnform. (caldeirao). - ok
        //Checar se o trasnform é == Main - ok

        //Tem que fazer a checagem do núimero do caldeirão pra caso o player passe de 45. Talvez atribuir a tag quando subtrairmos?
        if( item == null ) 
            return;

        Transform firstNumChild = transform.GetChild(0);
        if( !transform.name.Contains( "Main" ) || !firstNumChild.name.Contains("Num" ) )
            return;

        var firstImgChild = firstNumChild.GetComponentInChildren<Image>();
        if( firstImgChild == null )
            return;

        Transform secNumChild = transform.GetChild(1);
        if( !secNumChild.name.Contains( "Num" ) )
            return;

        var secImgChild = secNumChild.GetComponentInChildren<Image>();
        if( secImgChild == null )
            return;


        //pega a tag pra saber o número linkado no item
        var tag         = item.tag as string;
        int number      = 0;
        int cauldronTag = 0;
        int firstDig    = 0;
        int secDig      = 0; 
        if(int.TryParse( tag, out number ) )
        {
            if( string.IsNullOrEmpty( _lastNum ) && transform.tag != null )
                cauldronTag         = int.Parse( transform.tag );
            else
                cauldronTag = int.Parse( _lastNum );
    
            var animation = GetComponent<CauldronNumberAnimation>();
            if( animation != null )
                animation.OnPotionAdded(cauldronTag, number);

            var newNumber       = cauldronTag - number;
            _lastNum            = newNumber.ToString();


            if(newNumber < 45)
            {
                firstDig            = 9;
                secDig              = 9;
                newNumber           = 99;
                _lastNum            = newNumber.ToString();
            }
            else
            { 
                firstDig    = newNumber / 10;
                secDig      = newNumber % 10;
            }

            if( newNumber < 10 )
                firstDig = 0;

            var firstSprite = Resources.Load<Sprite>( "Props/Numeros/" + firstDig.ToString() );
            if( firstSprite == null )
                return;

            firstImgChild.sprite = firstSprite;

            var secSprite = Resources.Load<Sprite>( "Props/Numeros/" + secDig.ToString() );
            if( secSprite == null )
                return;

            secImgChild.sprite = secSprite;
        }

    }
    #endregion
}
