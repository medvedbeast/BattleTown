using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class Element : MonoBehaviour
    {
        [SerializeField]
        public List<UnityAction> Actions = new List<UnityAction>();

        public void OnMouseEnter()
        {
            
        }

        public void OnMouseExit()
        {

        }
    }
}