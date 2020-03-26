using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransfer : MonoBehaviour
{

    private static DataTransfer _instance;

    public static DataTransfer Instance { get { return _instance; } }
    public Player player;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        player = gameObject.AddComponent<Player>();

        DontDestroyOnLoad(gameObject);
    }
}
