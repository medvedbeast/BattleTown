using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Randomizer
{
    public class Item<T>
    {
        public T Content { get; set; }
        public float Weight { get; set; }
        public float Probability { get; set; }
        public bool IsGuaranteed => missCount >= Mathf.Ceil(1 / Probability) ? true : false;

        private int missCount = 0;

        public void Hit()
        {
            missCount = 0;
        }

        public void Miss()
        {
            missCount++;
        }
    }
}