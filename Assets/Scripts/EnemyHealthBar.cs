using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject healthProgress;
    public GameObject healthText;
    private RectTransform rect;
    private Text textObj;
    private Unit unit;
    void Awake()
    {
        rect = healthProgress.GetComponent<RectTransform>();
        textObj = healthText.GetComponent<Text>();
        unit = transform.parent.GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        //rect.localPosition = new Vector3(unit.transform.position.x * 32 / 2, unit.transform.position.y * 32 / 2, 100);

        float healthPercent = (float) unit.CurrentHealth / unit.MaxHealth * 75;
        rect.sizeDelta = new Vector2(healthPercent, rect.sizeDelta.y);
        textObj.text = unit.CurrentHealth + " / " + unit.MaxHealth;
    }
}
