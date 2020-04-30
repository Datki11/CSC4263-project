using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoinCount : MonoBehaviour
{
    private Player player;
    public Text coinValue;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        coinValue.text = player.Gold.ToString();
    }
}
