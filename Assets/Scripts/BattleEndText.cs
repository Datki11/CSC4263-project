using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleEndText : MonoBehaviour
{
    private Text textChild;
    private bool animating = true;
    private RectTransform textChildRect;
    private float velocityY = 35f;
    private float timeUntilFadeOut = 0.8f;
    private float timeInFadeOut = 0.7f;
    void Awake()
    {
        textChild = transform.GetChild(0).GetComponent<Text>();
        textChildRect = textChild.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animating) {
            timeUntilFadeOut -= Time.deltaTime;
            if (timeUntilFadeOut < 0) {
                Color myColor = textChild.color;
                SetColor(new Color(myColor.r,myColor.g,myColor.b, timeInFadeOut / 0.7f));
                timeInFadeOut -= Time.deltaTime;
                if (timeInFadeOut < 0) {
                    DestroySelf();
                }
            }
            textChildRect.localPosition += new Vector3(0, velocityY * Time.deltaTime, 0);
        }
    }

    public void SetText(string str) {
        textChild.text = str;
    }
    public void SetColor(Color col) {
        textChild.color = col;
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }
}
