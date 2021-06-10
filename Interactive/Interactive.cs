using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public float range = 2.5f;
    [SerializeField]
    public BoxCollider triggerCollider;
    [SerializeField]
    public List<Unit> interactors = new List<Unit>();

    public void Start()
    {
        triggerCollider.size = new Vector3(range, range, range);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit")
        {
            var unit = other.GetComponent<Unit>();
            unit.OnInteractionEnter(this);
            interactors.Add(unit);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Unit")
        {
            var unit = other.GetComponent<Unit>();
            unit.OnInteractionLeave(this);
            interactors.Remove(unit);
        }
    }

    public virtual void Use(Unit user) { }

    public virtual void Destroy() { }
}
