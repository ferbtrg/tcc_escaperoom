using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClockMultiplierChecker : MonoBehaviour
{
    #region Fields
    [SerializeField] 
    private RectTransform      _hourHand;
    [SerializeField] 
    private RectTransform      _minuteHand;
    [SerializeField] 
    private float              _angleErrorMargin = 5f;
    #endregion

    #region Private Methods
    /// <summary>
    /// Converts angle to hour number (1-12)
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private int GetHourFromAngle( float angle )
    {
        //Normalizes the angle to 0-360
        angle                   = ( angle + 360f ) % 360f;
        
        //In Unity, 0 is in the right and rotates counterclockwise
        //We need to transform to the clock system where 0 is at the top
         float clockAngle       = ( 450f - angle ) % 360f;
        
        //Converts to hour (1-12)
        float number            = ( clockAngle / 30f ); // 30 degrees per hour
        int roundedNumber       = Mathf.RoundToInt( number );
        
        //Adjusts to scale 1-12
        if( roundedNumber == 0 || roundedNumber > 12 )
            roundedNumber = 12;
            
        return roundedNumber;
    }

    /// <summary>
    /// Converts angle to minutes number (0-60)
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private int GetMinuteFromAngle( float angle )
    {
        //Normalizes the angle to 0-360
        angle               = ( angle + 360f ) % 360f;
        
        //Rotating counterclockwise
         float clockAngle   = ( 360f - angle ) % 360f;
        
        //Converts to minutes (0-60)
        float   number        = ( clockAngle / 6f ); // 6 degrees per minute
        int     minutes         = Mathf.RoundToInt( number );
        
        //Adjusts to scale 0-60
        if( minutes == 0 || minutes == 60 )
            minutes = 60;
            
        return minutes;
    }

    /// <summary>
    /// Checks if an angle is close to the expected value
    /// </summary>
    /// <param name="currentAngle"></param>
    /// <param name="targetAngle"></param>
    /// <returns></returns>
    private bool IsAngleNearTarget( float currentAngle, float targetAngle )
    {
        // Calculates the smallest difference between two angles and gets the absolute value
        float diff  = Mathf.Abs( Mathf.DeltaAngle( currentAngle, targetAngle ) );
        return diff <= _angleErrorMargin;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Checks if the hands are in a valid position according to the chosen number
    /// </summary>
    /// <returns></returns>
    public bool CheckValidPosition()
    {
        int hourNumber          = GetHourFromAngle( _hourHand.localEulerAngles.z );
        int minuteNumber        = GetMinuteFromAngle( _minuteHand.localEulerAngles.z );
        //List of all valid combinations for 36
        var validCombinations = new List<( int hour, int minute )>
        {
            (1, 36),
            (2, 18),
            (3, 12),
            (4, 9),
            (6, 6)
        };
        string str = string.Format( "Valid combination found: {0} x {1}", hourNumber, minuteNumber );

        // Checks if current position matches any valid combination
        foreach( var combo in validCombinations )
        {
            var equalHourMin = (hourNumber == combo.hour && minuteNumber == combo.minute ) ||
                ( hourNumber == combo.minute && minuteNumber == combo.hour );

            if( equalHourMin && IsAngleNearTarget(hourNumber, combo.hour) && 
                    IsAngleNearTarget(minuteNumber, combo.minute ) ) 
            {
                Debug.Log( str );
                return true;
            }
        }
        return false;
    }
    #endregion
}
