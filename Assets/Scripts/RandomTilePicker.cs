using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class RandomTilePicker : MonoBehaviour
{
    public List<Sprite> tiles = new List<Sprite>();
    private SpriteRenderer render;
    // Start is called before the first frame update
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Start()
    {
        int i = Mathf.RoundToInt(Random.Range(0, tiles.Count));
        render.sprite = tiles[i];
    }
}
