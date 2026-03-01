using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip swordSwingClip;

    public void PlaySwordSwing()
    {
        audioSource.PlayOneShot(swordSwingClip);
    }
}