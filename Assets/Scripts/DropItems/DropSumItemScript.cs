using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class DropItemScript : MonoBehaviour, IDropHandler
{
    #region Fields
    static int _firstPot   = 0;
    static int _secPot     = 0;
    static int _thirdPot   = 0;

    static GameObject _thirdPotPrefab;
    static GameObject _secPotPrefab;

    [SerializeField] private GameObject _flowerGroup;
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

          if( eventData.pointerDrag == null )
                return;

          if( transform.name == "PotResult" )
          {
              eventData.pointerDrag.GetComponent<RectTransform>().position = DragItemScript.InitialPos;
              return;
          }
               
           
        var item        = eventData.pointerDrag;
        var parent      = item.transform.parent;
        var alpha       = item.GetComponent<CanvasGroup>().alpha = 0;
        var children    = parent.GetComponentsInChildren<Transform>();
        var gameObj     = item.gameObject;


        if( item.transform.childCount == 0 )
        {
            GameObject newFlower                            = Instantiate( gameObj, DragItemScript.InitialPos, item.transform.rotation, parent );
            newFlower.transform.localScale                  = item.transform.localScale;
            newFlower.name                                  = item.transform.name;
            newFlower.GetComponent<CanvasGroup>().blocksRaycasts  = true;
            newFlower.GetComponent<CanvasGroup>().alpha = 1;
            }

          SoundManager._instance.PlaySound(sound);

          AddFlowersToPot( eventData );
          ChangePotNumber( eventData );

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

    private void AddFlowersToPot( PointerEventData eventData )
    {
         // Instancia o grupo de flores
        GameObject      flower          = eventData.pointerDrag;
        GameObject      newFlowerGroup  = null;
        string          potName         = transform.name;
        bool isClone                    = false;

        // Pega o componente FlowerGroup para gerenciar a visibilidade
        FlowerGroupController flowerGroup;
        switch( potName )
        {
            case "PotOne":
                flowerGroup = _flowerGroup.GetComponent<FlowerGroupController>();
                break;

            case "PotSec":
                if( _secPotPrefab == null )
                {
                    newFlowerGroup                  = Instantiate( _flowerGroup );
                    newFlowerGroup.name             = _flowerGroup.name;
                    _secPotPrefab                   = newFlowerGroup;

                    GetPotsPosition(_secPotPrefab, eventData);
                    flowerGroup                     = newFlowerGroup.GetComponent<FlowerGroupController>();
                    flowerGroup.SetFlowerVisibility();
                    isClone                         = true;
                }
                else
                { 
                    newFlowerGroup                  = _secPotPrefab;
                    flowerGroup                     = newFlowerGroup.GetComponent<FlowerGroupController>();
                }
                break;    

            case "PotThird":
                if( _thirdPotPrefab == null )
                {
                    newFlowerGroup                  = Instantiate( _flowerGroup );
                    newFlowerGroup.name             = _flowerGroup.name;
                   _thirdPotPrefab                  = newFlowerGroup;

                    GetPotsPosition( _thirdPotPrefab, eventData );
                    flowerGroup                     = newFlowerGroup.GetComponent<FlowerGroupController>();
                    flowerGroup.SetFlowerVisibility();
                    isClone                         = true;

                }
                else
                { 
                    newFlowerGroup                  = _thirdPotPrefab;
                    flowerGroup                     = newFlowerGroup.GetComponent<FlowerGroupController>();
                }

                break;

                default:
                    newFlowerGroup       = Instantiate( _flowerGroup );
                    flowerGroup          = newFlowerGroup.GetComponent<FlowerGroupController>();
                break;
        }

        flowerGroup.isClone                     = isClone;
        // Identifica a cor da flor que foi dropada
        string flowerName                       = eventData.pointerDrag.name;
        string flowerColor                      = "";
        switch( flowerName )
        {
            case "BlueFlower":
                flowerColor = "blue";
                break;

            case "PinkFlower":
                flowerColor = "pink";
                break;

            case "PurpleFlower":
                 flowerColor = "purple";
                break;

            case "YellowFlower":
                flowerColor = "yellow";
            break;

        }

        // Mostra a flor correspondente no grupo
        flowerGroup.ShowFlower(flowerColor);
    }

    private void ChangePotNumber( PointerEventData eventData )
    {
        Transform childTest = transform.GetChild(0);
        if( !childTest.name.Contains( "Num" ) || transform.name == "PotResult" )
            return;

        var child = childTest.GetComponentInChildren<Image>();
        if( child == null )
            return;

        string name     = child.sprite.name;
        int number      = 0;
        FlowerGroupController flowerGroup;
        if( int.TryParse( name, out number ) )
        { 
            //TODO: Insert error here. Don't forget
            if( number == 9 )
                return;

            //Add how many flowers were added on the pot.
            switch( childTest.name )
            {
                 case "FirstNum":
                    flowerGroup = _flowerGroup.GetComponent<FlowerGroupController>();
                    if( flowerGroup != null )
                        ResetFlowersOnMaxCount( flowerGroup, ref number, eventData );

                    if( number == -1 )
                        _firstPot   = 0;
                     else
                        ++_firstPot;
                     break;

                 case "SecNum":
                    if( _secPotPrefab != null )
                    { 
                        flowerGroup = _secPotPrefab.GetComponent<FlowerGroupController>();
                        if( flowerGroup != null )
                            ResetFlowersOnMaxCount( flowerGroup, ref number, eventData );
                    }
                     if( number == -1 )
                        _secPot   = 0;
                     else
                        ++_secPot;
                     break;

                 case "ThirdNum":
                    if( _thirdPotPrefab != null )
                    { 
                        flowerGroup = _thirdPotPrefab.GetComponent<FlowerGroupController>();
                        if( flowerGroup != null )
                            ResetFlowersOnMaxCount( flowerGroup, ref number, eventData );
                    }
                     
                    if( number == -1 )
                        _thirdPot = 0;
                     else
                        ++_thirdPot;

                     break;
            }


            
            int newNumber = number + 1;
            //Path to numbers img
            var sprite = Resources.Load<Sprite>( "Props/Numeros/" + newNumber.ToString() );

            if( sprite == null )
                return;

            child.sprite = sprite;
            CheckPotNumbers();
        }
    }

    private void GetPotsPosition( GameObject newFlowerGroup, PointerEventData eventData )
    {
        var parent                  = eventData.pointerDrag.transform.parent;
        var newFlowerGroupRect      = newFlowerGroup.GetComponent<RectTransform>();
        var vasoRect                = GetComponent<RectTransform>();
        newFlowerGroupRect.SetParent(parent, false);

        // Pega o vaso original e calcula o offset
        Vector2 firstPotPos     = new Vector2(-300f, -150f);    // Posição do vaso 1
        Vector2 newPotPos       = vasoRect.anchoredPosition;    // Posição do vaso onde dropou

        // Calcula a diferença entre as posições dos vasos
        Vector2 DifBetweenPots                          = newPotPos - firstPotPos;
        
        // Aplica essa diferença ao grupo de flores
        newFlowerGroupRect.anchoredPosition             += DifBetweenPots;
    }

    private void ResetFlowersOnMaxCount( FlowerGroupController flowerGroup, ref int number, PointerEventData eventData  )
    {
        var item        = eventData.pointerDrag;
        if( number == 4 )
        {
            number = -1;
            flowerGroup.SetFlowerVisibility();
            SoundManager._instance.PlaySFX( _soundManager._error );
            Destroy(item);
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

                var sprite = Resources.Load<Sprite>( "Props/Vasos/vaso_cheio_45" );
                if( sprite == null )
                    return;

                SoundManager._instance.PlaySFX(_soundManager._potResult);
                child.sprite = sprite;
                break;
            }
        }
    }
    #endregion
}
