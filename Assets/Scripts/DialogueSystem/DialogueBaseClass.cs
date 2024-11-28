using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        #region Properties
        public bool Finished{ get; private set; }
        #endregion


        protected IEnumerator WriteText( string input, Text textHolder, Color textColor, Font textFont, float delay, AudioClip sound, float delayBetweenLines )
        {
            textHolder.color    = textColor;
            textHolder.font     = textFont;
            textHolder.text     = "";

            bool isSkipping = false;
            for( int i = 0; i < input.Length; i++ )
            {
                //Check if there's mouse click.
                if( Input.GetMouseButton(0) && !isSkipping )
                {
                    //Show the rest of the text.
                    isSkipping      = true;
                    textHolder.text = input;
                    if( sound != null )
                        SoundManager._instance.PlaySound(sound);
                    break;
                }
                

                if( !isSkipping )
                { 
                    //Normal behaviour.
                    textHolder.text += input[i];
                    if( sound != null )
                        SoundManager._instance.PlaySound(sound);

                    yield return new WaitForSeconds(delay);
                }
            }

             yield return new WaitUntil(() => !Input.GetMouseButton(0));
             yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
             Finished = true;
    }
    }
}

