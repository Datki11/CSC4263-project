using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip Music;

    void Start()
    {
        GetComponent<AudioSource>().Play();
    }
}