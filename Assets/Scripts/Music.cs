using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip music;

    void Start()
    {
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        
    }
}
