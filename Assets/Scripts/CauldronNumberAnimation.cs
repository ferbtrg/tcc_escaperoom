using UnityEngine;
using UnityEngine.UI;

public class CauldronNumberAnimation : MonoBehaviour 
{
    #region Fields
    [Header("Effects")]
    public ParticleSystem   _potionSplashEffect;
    public ParticleSystem   _magicEffect;

    [Header("Floating Number")]
    public GameObject       _floatingNumberPrefab;
    public float            _floatDistance = 100f;     
    // Animation duration.
    public float            _floatDuration = 1f;        

    private Image           _firstNumImage;
    private Image           _secNumImage;
    #endregion

    #region Public Methods
    public void OnPotionAdded( int currentNum, int potionValue )
    {
        if (_potionSplashEffect != null)
            _potionSplashEffect.Play();

        if (_magicEffect != null)
            _magicEffect.Play();

        //Create floating number.
        ShowFloatingNumber( potionValue );
        AnimateNumbers( currentNum, currentNum - potionValue );
    }
    #endregion

    #region Private Methods
    private void Start()
    {
        Transform firstNumChild     = transform.GetChild(0);
        Transform secNumChild       = transform.GetChild(1);
        
        _firstNumImage              = firstNumChild.GetComponentInChildren<Image>();
        _secNumImage                = secNumChild.GetComponentInChildren<Image>();
    }

    private void ShowFloatingNumber( int number )
    {
        GameObject floatingNumber           = Instantiate(_floatingNumberPrefab, transform);
        // Configurate initial pos.
        RectTransform rect                  = floatingNumber.GetComponent<RectTransform>();
        rect.anchoredPosition               = Vector2.zero;

        //Find number inside Prefab.
        Transform numberTransform = floatingNumber.transform.Find("Num");
        if( numberTransform == null )
            return;

        Image numberImage = numberTransform.GetComponent<Image>();
        if( numberImage == null )
            return;

        //Load and set sprite from number.
        var sprite = Resources.Load<Sprite>("Props/Numeros/" + number );
        if( sprite == null )
            return;

        numberImage.sprite = sprite;

        //Animate the number going up
        LeanTween.moveY(floatingNumber, rect.anchoredPosition.y + 1f, 2.5f) // 1 unity in 2.5 sec
        .setEaseOutCubic(); //smooth movement

        // Fade out mais lento também
        CanvasGroup group   = floatingNumber.GetComponent<CanvasGroup>();
        if( group != null )
        {
            LeanTween.alphaCanvas(group, 0f, 2.5f)
                .setEaseInQuad()
                .setOnComplete(() => Destroy(floatingNumber));
        }
    }

    private void AnimateNumbers( int from, int to )
    {
        // Se número for menor que 45, vai pra 99
        if( to < 45 ) 
            to = 99;

        LeanTween.value(gameObject, from, to, 1f)
            .setOnUpdate((float val) => {
                int currentNumber = Mathf.RoundToInt(val);
                UpdateNumbers(currentNumber);
            });
    }

    private void UpdateNumbers(int number)
    {
        int firstDig    = number / 10;
        int secDig      = number % 10;
        
        if (number < 10)
            firstDig = 0;

        // Carregar e atualizar sprites
        _firstNumImage.sprite   = Resources.Load<Sprite>("Props/Numeros/" + firstDig );
        _secNumImage.sprite     = Resources.Load<Sprite>("Props/Numeros/" + secDig   );
    }
    #endregion
}