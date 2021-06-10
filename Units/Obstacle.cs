using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        var p = other.GetComponent<Projectile>();
        if (p != null)
        {
            
            GameObject effect = Instantiate(Resources.Load($"Prefabs/Particles/{p.cannon.hitEffectName}")) as GameObject;
            effect.transform.position = other.transform.position;
            effect.transform.parent = GameObject.Find("Effects").transform;

            Destroy(p.gameObject);
        }
    }
}
