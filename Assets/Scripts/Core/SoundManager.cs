using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager      _instance { get; private set; }
    [Header("Audio Source")]
    [SerializeField]AudioSource     _source;
    [SerializeField]AudioSource     _SFXSource;

    [Header("Audio Clip")]
    [SerializeField]public AudioClip       _background;
    [SerializeField]public AudioClip       _potResult;
    [SerializeField]public AudioClip       _error;
    [SerializeField]public AudioClip       _bubblingCauldron;

    private void Awake()
    {
        _instance    = this;
        _source.clip = _background;
        _source.Play();


    }

    public void PlaySound( AudioClip sound )
    {
        _source.PlayOneShot(sound);
    }
    
    public void PlaySFX(AudioClip clip)
    {
        _SFXSource.PlayOneShot(clip);
    }
}
