using System;
using System.Collections;
using System.Collections.Generic;

namespace knapsack
{
    class Program
    {
        static void Main(string[] args)
        {
            
           item chocolate = new item("chocolate", 8, 7);
           item cabbage = new item("cabbage", 1, 5);
           item chickpeas = new item("chickpeas", 4, 4);
           item apple = new item("apple", 5, 4);
           item bannana = new item("bannana", 6, 3);
            List<item> groceries = new List<item>();
            groceries.Add(chocolate);
            groceries.Add(cabbage);
            groceries.Add(chickpeas);
            groceries.Add(apple);
            groceries.Add(bannana);
            problem p1 = new problem(groceries,15);
            //////////////// brute ///////////////
            int best_value = approaches.bruteforce(p1);
            Console.WriteLine(best_value);
            /////////// greedy ////////////////
            best_value = approaches.greedy(p1);
            Console.WriteLine(best_value);
            /////////// d&d/////////////
            best_value = approaches.divide_conquer(p1, 5, 15);
            Console.WriteLine(best_value);
            ////////// dynamic /////////////
            Console.WriteLine(approaches.dynamic_programming(p1, 5, 15));
            ////////////// genetic ////////////////
            Console.WriteLine(approaches.genetic_approach(p1));
            /////////////// backtracking /////////////
            Console.WriteLine(approaches.backtracking(p1, 0, 0, 0));
            ///////////////// branch and bound /////////////////
            Console.WriteLine(approaches.branch_bound(p1));
        }
    }
}
