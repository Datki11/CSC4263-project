using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    public GameObject resourceProgress;
    public GameObject resourceText;
    private RectTransform rect;
    private Text textObj;
    private Player player;

    void Awake()
    {
        rect = resourceProgress.GetComponent<RectTransform>();
        textObj = resourceText.GetComponent<Text>();
        player = GameObject.FindWithTag("PlayerUnit").GetComponent<Player>();
    }

    void Update()
    {
        float resourcePercent = (float) player.Class.Resource.Current / player.Class.Resource.BaseMax * 75;
        rect.sizeDelta = new Vector2(resourcePercent, rect.sizeDelta.y);
        textObj.text = player.Class.Resource.Current + " / " + player.Class.Resource.BaseMax;
    }
}