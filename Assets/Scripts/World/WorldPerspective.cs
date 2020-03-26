using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPerspective : MonoBehaviour
{
    private float initialZ;
    private SpriteRenderer render;
    void Awake()
    {
        initialZ = transform.position.z;
        render = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(transform.position.x, transform.position.y, initialZ + (render.bounds.size.y / 2 + transform.position.y) / 1000 );
    }
}
