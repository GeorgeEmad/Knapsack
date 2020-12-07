using System;
using System.Collections.Generic;
using System.Text;

namespace knapsack
{
    class item
    {
        public string name;
        public int value;
        public int weight;
        public int s; // a value that can be 0 or 1 to indicate if an item is taken in the knapsack or not
        public float density; // value divided by weight
        public item(string name, int value, int weight)
        {
            this.name = name;
            this.value = value;
            this.weight = weight;
            density = (float)(((float)value) / ((float)weight));
        }

    }
}
