using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DamageText : MonoBehaviour
{
    private Text textChild;
    public UnityEvent destroyed;
    private bool animating = true;
    private RectTransform textChildRect;
    private float velocityY = 270;
    private float gravity = 1000;
    void Awake()
    {
        textChild = transform.GetChild(0).GetComponent<Text>();
        textChildRect = textChild.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animating) {
            textChildRect.localPosition += new Vector3(0, velocityY * Time.deltaTime, 0);
            velocityY -= gravity * Time.deltaTime;
            if (textChildRect.localPosition.y < 0) {
                textChildRect.localPosition = new Vector3(textChildRect.localPosition.x, 0, textChildRect.localPosition.z);
                animating = false;
                Invoke("DestroySelf", 0.7f);
            }
        }
    }

    public void SetText(int damage) {
        var str = damage.ToString();
        textChild.text = str;
    }
    public void SetColor(Color col) {
        textChild.color = col;
    }

    private void DestroySelf() {
        destroyed.Invoke();
        Destroy(gameObject);
    }
}
