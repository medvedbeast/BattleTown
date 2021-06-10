using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Randomizer
{
    public class Generator : Generator<object>
    {
        public Generator(List<Item<object>> items) : base(items) { }

        protected override object Cast(object r)
        {
            if (r is System.ICloneable)
            {
                var clone = (r as System.ICloneable).Clone();
                return clone;
            }
            else
            {
                return r;
            }
        }
    }

    public class Generator<T> where T : class
    {
        List<Item<T>> items = new List<Item<T>>();

        public Generator(List<Item<T>> items)
        {
            float weightTotal = items.Sum(x => x.Weight);
            foreach (var p in items)
            {
                p.Probability = p.Weight / weightTotal;
                this.items.Add(p);
            }
        }

        public T Get()
        {
            var guaranteed = items.FirstOrDefault(x => x.IsGuaranteed == true);
            if (guaranteed != null)
            {
                guaranteed.Hit();
                foreach (var i in items.Where(x => x != guaranteed))
                {
                    i.Miss();
                }
                return guaranteed.Content;
            }

            float offset = 0.0f;
            float number = Random.Range(0, 100) * 0.01f;

            Item<T> result = null;
            bool fetched = false;
            foreach (var i in items.OrderByDescending(x => x.Probability))
            {
                if (number <= offset + i.Probability && !fetched)
                {
                    result = i;
                    i.Hit();
                    fetched = true;
                }
                else
                {
                    i.Miss();
                }
                offset += i.Probability;
            }

            return Cast(result.Content);
        }

        protected virtual T Cast(object r)
        {
            if (r is System.ICloneable)
            {
                var clone = (r as System.ICloneable).Clone();
                return clone as T;
            }
            else
            {
                return r as T;
            }
        }
    }
}