using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer               _audioMixer;
    Resolution[]                    _resolutions;
    public TMPro.TMP_Dropdown       _resolutionDropDown;

    private void Start()
    {
        _resolutions = Screen.resolutions;

        _resolutionDropDown.ClearOptions();

        var options = new List<string>(); 
        int currentResolutionIndex = 0;
        for( int i = 0; i < _resolutions.Length; i++)
        {
            string option =  _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add( option );

            if( _resolutions[i].width == Screen.currentResolution.width && 
                _resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        _resolutionDropDown.AddOptions(options);
        _resolutionDropDown.value = currentResolutionIndex;
        _resolutionDropDown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution( resolution.width, resolution.height, Screen.fullScreen );
    }

    public void SetVolume( float volume )
    {
        _audioMixer.SetFloat( "volume", volume );
    
        Debug.Log(volume);
    }

    public void SetQuality( int qualityIndex )
    {
        QualitySettings.SetQualityLevel( qualityIndex );
    }
}
