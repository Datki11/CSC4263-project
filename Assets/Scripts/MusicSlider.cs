using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class MusicSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetVolume()
    {
        Slider slider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        AudioListener.volume = slider.value;
    }
}
