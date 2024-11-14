using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class DropItemScript : MonoBehaviour, IDropHandler
{
    #region Fields
    static int _firstPot   = 0;
    static int _secPot     = 0;
    static int _thirdPot   = 0;
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
              eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
           
          //var alpha = eventData.pointerDrag.GetComponent<CanvasGroup>().alpha = 0;
          //string str = string.Format( "OnDrop - Alpha: {0}", alpha );
          //Debug.Log( str );

          ChangePotNumber();
      }//try
      catch( Exception ex )
      {
          Debug.LogException(ex);
      }

    }
    #endregion

    #region Private Methods
    private void ChangePotNumber()
    {
        Transform childTest = transform.GetChild(0);
        if( !childTest.name.Contains( "Num" ) || transform.name == "PotResult" )
            return;

        var child = childTest.GetComponentInChildren<Image>();
        if( child == null )
            return;

        string name     = child.sprite.name;
        int number      = 0;
        if( int.TryParse( name, out number ) )
        { 
            //TODO: Insert error here. Don't forget
            if( number == 9 )
                return;

            int newNumber = number + 1;
            //Path to numbers img
            var sprite = Resources.Load<Sprite>( "Props/Numeros/" + newNumber.ToString() );

            if( sprite == null )
                return;

            //Add how many flowers were added on the pot.
            switch( childTest.name )
            {
                 case "FirstNum":
                     ++_firstPot;
                     break;

                 case "SecNum":
                     ++_secPot;
                     break;

                 case "ThirdNum":
                     ++_thirdPot;
                     break;
            }

            child.sprite = sprite;

            CheckPotNumbers();
        }
    }

    private void CheckPotNumbers()
    {
        int result              = 12;
        var parent              = transform.parent;
        bool numbersDifZero     =  _firstPot != 0 && _secPot != 0 && _thirdPot != 0;
        bool sumEqualsToResult  = _firstPot + _secPot + _thirdPot == result;

        //All pots must have number above 0.
        if( numbersDifZero && sumEqualsToResult && parent != null  )
        {
            foreach( Transform children in parent.GetComponentsInChildren<Transform>() )
            { 
                if( children.name != "PotResult" )
                    continue;

                var child = children.GetComponentInChildren<Image>();
                if( child == null )
                    return;

                var sprite = Resources.Load<Sprite>( "Props/" + "Vaso_Cheio");
                if( sprite == null )
                    return;

                child.sprite = sprite;
                break;
            }
        }
    }
    #endregion
}
