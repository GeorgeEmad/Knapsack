using System;
using System.Collections.Generic;
using System.Text;

namespace knapsack
{
    /* This class is made to be the blueprint for the kanpsack problem to be solved with its properties as Maximum Capacity, items,values and weights*/
    class problem
    {
        public int capacity; // Knapsack capacity
        public List<item> items; // list of items that we want to get the maximum value from them
        public problem(List<item> items, int capacity)
        {
            this.items = items;
            this.capacity = capacity;
        }
    }
}
