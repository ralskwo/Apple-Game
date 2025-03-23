using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip selectionClip;
    [SerializeField] private AudioClip removalClip;
    [SerializeField] private AudioClip bgmClip;

    void Awake()
    {
        ServiceLocator.Register<SoundManager>(this);
    }

    public void PlaySelectionSound()
    {
        if (selectionClip != null)
            audioSource.PlayOneShot(selectionClip);
    }

    public void PlayRemovalSound()
    {
        if (removalClip != null)
            audioSource.PlayOneShot(removalClip);
    }

    public void PlayBGM()
    {
        if (bgmClip != null)
        {
            audioSource.clip = bgmClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
