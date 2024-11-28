using UnityEngine;
using System.Collections;

/// <summary>
/// Deals with cursor and circle animation.
/// </summary>
public class CursorSystem : MonoBehaviour
{
    #region Fields
    [Header("Cursor Settings")]
    [SerializeField] 
    private Texture2D   _cursorTexture;
    [SerializeField]
    private Vector2     _hotSpot    = new Vector2(16, 16);
    [SerializeField] 
    private CursorMode  _cursorMode = CursorMode.Auto;

    [Header("Click Effect Settings")]
    [SerializeField] 
    private GameObject  _circleEffectPrefab;
    [SerializeField] 
    private float       _animationDuration  = 0.3f;
    [SerializeField] 
    private float       _startScale         = 0.1f;
    [SerializeField] 
    private float       _endScale           = 2f;
    [SerializeField] 
    private Color       _startColor         = new Color(1f, 1f, 1f, 0.5f);
    [SerializeField] 
    private Color       _endColor           = new Color(1f, 1f, 1f, 0f);
    [SerializeField] 
    private float       _zOffset            = 0f;

    #endregion

    #region Private Methods
    private void Start()
    {
        //Set cursor.
        if( _cursorTexture != null )
            Cursor.SetCursor( _cursorTexture, _hotSpot, _cursorMode );
    }

    private void Update()
    {
        if( Input.GetMouseButtonDown(0) )
            CreateClickEffect();
    }

    private void CreateClickEffect()
    {
        //Convert mouse position to world position
        Vector3 mousePos    = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        //Sets the depth of the effect
        mousePos.z          = _zOffset; 

        GameObject effect               = Instantiate( _circleEffectPrefab, mousePos, Quaternion.identity );
        SpriteRenderer spriteRenderer   = effect.GetComponent<SpriteRenderer>();
        
        if( spriteRenderer != null )
        {
            spriteRenderer.color            = _startColor;
            effect.transform.localScale     = Vector3.one * _startScale;
            StartCoroutine( AnimateCircle( effect, spriteRenderer ) );
        }
    }

    private IEnumerator AnimateCircle(GameObject circle, SpriteRenderer renderer)
    {
        float elapsed                       = 0f;
        while( elapsed < _animationDuration )
        {
            float t                         = elapsed / _animationDuration;
            
            //Use a smooth curve for animation
            float smoothT                   = Mathf.Sin(t * Mathf.PI * 0.5f);
            
            //Animate the size
            float currentScale              = Mathf.Lerp(_startScale, _endScale, smoothT);
            circle.transform.localScale     = Vector3.one * currentScale;
            
            //Animates color
            renderer.color                  = Color.Lerp(_startColor, _endColor, smoothT);
            elapsed                         += Time.deltaTime;

            //yield so we don't block the main thread.
            yield return null;
        }

        //Destroys the circle so that there is no fixed instance left on the screen.
        Destroy(circle);
    }
    #endregion
}