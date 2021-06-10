using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject owner;
    public Weapon cannon;
    public Vector3 direction;
    public bool penetrative;
    public float speed;
    public Vector3 startPosition;
    public float effectiveRange;

    public void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, startPosition) >= effectiveRange)
        {
            GameObject effect = Instantiate(Resources.Load($"Prefabs/Particles/{cannon.hitEffectName}")) as GameObject;
            effect.transform.position = transform.position;
            effect.transform.parent = GameObject.Find("Effects").transform;
            GameObject.Destroy(gameObject);
        }
    }
}
