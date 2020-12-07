using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace knapsack
{
    class approaches
    {
        /*************************************************  BRUTE FORCE APPROACH ************************************************/
        public static byte[] inc(byte[] binarr)
        {
            bool carriage = true;
            for (int i = (binarr.Length - 1); i >= 0; i--)
            {
                if (carriage)
                {
                    if (binarr[i] == 0)
                    {
                        binarr[i] = 1;
                        carriage = false;
                    }
                    else
                    {
                        binarr[i] = 0;
                        carriage = true;
                    }
                }
            }

            return binarr;
        }
        public static String binaryprinter(byte[] binarr)
        {
            String res = "";
            for (int i = 0; i < binarr.Length; i++)
            {

                if (binarr[i] == 0)
                {
                    res += "0";
                }
                else
                    res += "1";
            }
            return res;
        }
        public static int bruteforce(problem knapsack0_1)
        { 
            int n = knapsack0_1.items.Count;
            int best_value = 0;
            int best_sol_weight = 0;
            byte[] final_solution = new byte[n];
            byte[] s = new byte[n]; // array containing the solution of the problem as an array of 0s and 1s indicating whether an item is taken or not

            for (int i = 0; i < n; i++) {// initialising the array with 0s at the start which means no item is taken in the knapsack
                s[i] = 0;
            }

            for (int i = 0; i < Math.Pow(2,n); i++)
            {
                int tmp_value = 0; // temp value of the current solutin
                int tmp_weight = 0; // temp weight of the current solution
                for (int j = 0; j <n; j++)
                {
                    if (s[j] ==1)
                    {
                        tmp_weight += knapsack0_1.items[j].weight;
                        tmp_value += knapsack0_1.items[j].value;
                    }
                }
                if(tmp_weight <= knapsack0_1.capacity && tmp_value > best_value)
                {
                    best_value = tmp_value;
                    best_sol_weight = tmp_weight;
                    final_solution = s.ToArray<byte>();
                }
                inc(s);
            }
            return best_value;
        }
        /************************************************* GREEDY METHOD ****************************************************/
        public static int greedy(problem knapsack0_1)
        {
             List<item> list_items = knapsack0_1.items.OrderByDescending(i => i.density).ToList<item>();
            int temp_weight = 0;
            int value_sum = 0;
            int i = 0;
            while (i < list_items.Count && (temp_weight + list_items[i].weight <= knapsack0_1.capacity))
            {
                temp_weight += list_items[i].weight;
                value_sum += list_items[i].value;
                i++;
            }
            return value_sum;
        }



        /***************************************************** DIVIDE AND CONQUER *******************************************/

        public static int divide_conquer(problem knapsack0_1, int curr_item , int curr_rem_weight)
        {
            if(curr_item <= 0 || curr_rem_weight <=0 )
            {
                return 0;
            }

            if ((knapsack0_1.items[curr_item - 1].weight > knapsack0_1.capacity) || (curr_rem_weight - knapsack0_1.items[curr_item - 1].weight <0)) 
            {
                return divide_conquer(knapsack0_1, curr_item - 1, curr_rem_weight);
            }
            else
            {
             int temp = Math.Max((divide_conquer(knapsack0_1, curr_item - 1, curr_rem_weight)),
             (knapsack0_1.items[curr_item -1].value + divide_conquer(knapsack0_1, curr_item - 1, curr_rem_weight - knapsack0_1.items[curr_item - 1].weight)));
             return temp;
            }
        }
        ///////////////////////////////////////////// GENETIC APPROACH ///////////////////////////////////////////////
        public static List<List<int>> fittnes_selction(problem knapsack0_1, List<List<int>> population) // to select the fittest selection among solutions
        {
            
            // produce the fittnes value for each chromosome
            foreach (List<int> chromosome in population)
            {
                int tmp_value = 0;
                int tmp_weight = 0;
                for (int i =0; i< chromosome.Count; i++)
                {
                    if ( (chromosome[i] == 1) && (knapsack0_1.items[i].weight + tmp_weight > knapsack0_1.capacity ))
                    {
                        tmp_value = 0;
                        break;
                    }
                    if ((chromosome[i] == 1) && (knapsack0_1.items[i].weight + tmp_weight <= knapsack0_1.capacity))
                    {
                        
                        tmp_weight += knapsack0_1.items[i].weight;
                        tmp_value += knapsack0_1.items[i].value;
                    }  
                }
                chromosome.Insert(0, tmp_value);
            }
            // order chromosomes by their fittnes value and select the best 2 to return them
            List<List<int>> ordered_population  =  population.OrderByDescending(li => li[0]).ToList<List<int>>();
            List<List<int>> selection = new List<List<int>>();
            selection.Add(ordered_population[0]);
            selection.Add(ordered_population[1]);
            return selection;

        }

        public static List<List<int>> crossover_mutatinon(List<List<int>> selection)
        {
            int parity = 0;
            if(((selection[0].Count-1) %2 != 0))
            {
                parity = 1;
            }
            List<int> sel_1 = selection[0].ToList(); sel_1.RemoveAt(0);
            List<int> sel_2 = selection[1].ToList(); sel_2.RemoveAt(0);
            // dividing
            List<int> half_1_1 = selection[0].GetRange(0, sel_1.Count/2);
            List<int> half_2_1 = selection[0].GetRange(sel_1.Count / 2, (sel_1.Count / 2) + parity);
            List<int> half_1_2 = selection[0].GetRange(0, sel_2.Count / 2);
            List<int> half_2_2 = selection[0].GetRange(sel_2.Count / 2, (sel_2.Count / 2) + parity);
            // production produced with crossover
            List<int> prod_1 = half_1_1.ToList(); prod_1.AddRange(half_2_2);
            List<int> prod_2 = half_1_2.ToList(); prod_2.AddRange(half_2_1);
            //Mutatuion for prod 1
            Random random_bit1 = new Random();
            int bit1 = random_bit1.Next(prod_1.Count-1);
            if (prod_1[bit1] == 1)
            {
                prod_1[bit1] = 0;
            }
            else prod_1[bit1] = 1;

            //Mutatuion for prod2
            Random random_bit2 = new Random();
            int bit2 = random_bit2.Next(prod_2.Count - 1);
            if (prod_2[bit2] == 1)
            {
                prod_2[bit2] = 0;
            }
            else prod_2[bit2] = 1;
            List<List<int>> fittest_with_productions = new List<List<int>>();
            fittest_with_productions.Add(sel_1); fittest_with_productions.Add(sel_2); // old solutions added
            fittest_with_productions.Add(prod_1); fittest_with_productions.Add(prod_2);// new solutions added
            return fittest_with_productions;
        }

        public static List<List<int>> complete_population(problem knapsack0_1, int population_size, List<List<int>> new_population)
        {
            for (int i = new_population.Count; i < population_size; i++)
            {
                List<int> temp_list2 = new List<int>();
                for (int j = 0; j < knapsack0_1.items.Count; j++)
                {
                    Random random_bit = new Random();
                    int bit = random_bit.Next(2);     // randomly it creates 0 or 1 to be the bit for taking or not taking the item
                    temp_list2.Add(bit);
                }
                new_population.Add(temp_list2);
            }
            return new_population;
        }

        public static int genetic_approach(problem knapsack0_1, int population_size = 30, int iterations = 10){
            List<List<int>> chromosomes = new List<List<int>>(); // list of solutions
            // Initialize the population with the number indicated in the argumet 
            for(int i=0; i< population_size ; i++)
            {
                List<int> temp_list = new List<int>();
                for (int j = 0; j < knapsack0_1.items.Count; j++)
                {
                    Random random_bit = new Random();
                    int bit = random_bit.Next(2);     // randomly it creates 0 or 1 to be the bit for taking or not taking the item
                    temp_list.Add(bit);
                }
                chromosomes.Add(temp_list);
            }
            List<List<int>> selection  = fittnes_selction(knapsack0_1, chromosomes);
            List<List<int>> new_population = crossover_mutatinon(selection);
            new_population = complete_population(knapsack0_1, population_size, new_population);
            for(int i =1; i< iterations; i++)
            {
                selection = fittnes_selction(knapsack0_1, new_population);
                new_population = crossover_mutatinon(selection);
                new_population = complete_population(knapsack0_1, population_size, new_population);
            }
            List <List<int>> solution = fittnes_selction(knapsack0_1, new_population);
            return solution[0][0];

        }
        ///////////////////////////////////////// Dynamic Programming /////////////////////////////////////////////
        static int[,] values_memory = new int[5, 15]; // 2d Array of memoization to store values of solved sub problems 
        public static int dynamic_programming(problem knapsack0_1, int curr_item, int curr_rem_weight)
        {   
            int value_max=0;
            foreach(item i in knapsack0_1.items)
            { value_max += i.value;

            }
            if (curr_item <= 0 || curr_rem_weight <= 0)
            {
                return 0;
            }

            if ((knapsack0_1.items[curr_item - 1].weight > knapsack0_1.capacity) || (curr_rem_weight - knapsack0_1.items[curr_item - 1].weight < 0))
            {
                return dynamic_programming(knapsack0_1, curr_item - 1, curr_rem_weight);
            }
            else
            {
                if (!((values_memory[curr_item - 1, curr_rem_weight-1] > 0) && (values_memory[curr_item - 1, curr_rem_weight-1] < value_max)))
                {
                    int temp = Math.Max((dynamic_programming(knapsack0_1, curr_item - 1, curr_rem_weight)),
                    (knapsack0_1.items[curr_item - 1].value + dynamic_programming(knapsack0_1, curr_item - 1, curr_rem_weight - knapsack0_1.items[curr_item - 1].weight)));
                    values_memory[curr_item - 1, curr_rem_weight-1] = temp;
                    return temp;
                }
                else return values_memory[curr_item - 1, curr_rem_weight-1];
            }
        }
        /////////////////////////////////////////////////////////// BRANCH AND BOUND ////////////////////////////////////////////////////////

        public static int BB_bound(problem knapsack0_1, BB_node use_node)
        {
            if (use_node.weight >= knapsack0_1.capacity)// if weight bigger than capacity bounding value = 0
                return 0;
            int bounding_value = use_node.curr_value_possible; //give value to boundin value
            int indexing = use_node.tree_level + 1;
            int weight_now = (int)use_node.weight;
            while ((indexing < knapsack0_1.items.Count) && (weight_now + knapsack0_1.items[indexing].weight <= knapsack0_1.capacity))
            {
                weight_now += knapsack0_1.items[indexing].weight;
                bounding_value += knapsack0_1.items[indexing].value;
                indexing++;
            }

            if (indexing < knapsack0_1.items.Count)
                bounding_value += (knapsack0_1.capacity - weight_now) * (int)knapsack0_1.items[indexing].density;
            return bounding_value;
        }

        public static int branch_bound(problem knapsack0_1)
        {
            int best_solution = 0;
            knapsack0_1.items = knapsack0_1.items.OrderByDescending(i => i.density).ToList<item>();
            BB_node imag = new BB_node(); imag.tree_level = -1; // imaginary node for initialisation
            imag.curr_value_possible = 0;
            imag.weight = 0;
            BB_node curr_node = new BB_node();
            Queue<BB_node> tree = new Queue<BB_node>(); // create tree of possibilites nodes or states with a queue
            tree.Enqueue(imag);  // enqueue the imaginary temporary node
            while (tree.Count != 0)
            {
                imag = tree.Dequeue(); // get the first element entered into the queue from the current objects
                if (imag.tree_level == -1)
                    curr_node.tree_level = 0;
                if (imag.tree_level == knapsack0_1.items.Count - 1)
                    continue;
                curr_node.tree_level = 1 + imag.tree_level; // increment level
                curr_node.weight = knapsack0_1.items[curr_node.tree_level].weight + imag.weight;
                curr_node.curr_value_possible = knapsack0_1.items[curr_node.tree_level].value + imag.curr_value_possible;
                if (curr_node.weight <= knapsack0_1.capacity && curr_node.curr_value_possible > best_solution)
                    best_solution = curr_node.curr_value_possible; //update the best solution 
                curr_node.node_bounding_value = BB_bound(knapsack0_1, curr_node);// get the bound value for the current node
                if (curr_node.node_bounding_value > best_solution)
                    tree.Enqueue(curr_node); //enqueuing current node
                curr_node.weight = imag.weight;
                curr_node.curr_value_possible = imag.curr_value_possible;
                curr_node.node_bounding_value = BB_bound(knapsack0_1, curr_node);// get the bound value for the current node
                if (curr_node.node_bounding_value > best_solution)
                    tree.Enqueue(curr_node);
            }
            return best_solution; // the optimal solution
        }





        ////////////////////////////////////////  BACKTRACKING APPROACH  ////////////////////////////////////////
        public static float bounding_func(problem knapsack0_1, int curr_best_value, int curr_weight, int curr_index)
        {

            for (int i = curr_index + 1; i< knapsack0_1.items.Count;i++)
            {
                curr_weight += knapsack0_1.items[i].weight;
                if (curr_weight <= knapsack0_1.capacity)
                {
                    curr_best_value += knapsack0_1.items[i].value;
                }
                else return curr_best_value + (knapsack0_1.items[i].value *(knapsack0_1.items[i].density *  (knapsack0_1.capacity - (curr_weight - knapsack0_1.items[i].weight))));
            }
            return curr_best_value;
        }


        static int best_solution = 0;
        static int best_weight = 0;
        public static int backtracking(problem knapsack0_1, int curr_best_value, int curr_weight, int curr_index)
        {
            
            int[] items_bool = new int[knapsack0_1.items.Count];
            int[] sol_bool = new int[knapsack0_1.items.Count]; 
            if ( curr_index < knapsack0_1.items.Count && curr_weight + knapsack0_1.items[curr_index].weight <= knapsack0_1.capacity)
            {
                items_bool[curr_index] = 1;
                if (curr_index < knapsack0_1.items.Count)
                    backtracking(knapsack0_1, curr_best_value + knapsack0_1.items[curr_index].value, curr_weight + knapsack0_1.items[curr_index].weight, curr_index + 1);
                if (curr_best_value + knapsack0_1.items[curr_index].value > best_solution && curr_index == knapsack0_1.items.Count -1)
                {
                    items_bool.CopyTo(sol_bool,0);
                    best_solution = curr_best_value + knapsack0_1.items[curr_index].value;
                    best_weight = curr_weight + knapsack0_1.items[curr_index].weight;
                }
            }
            if (curr_index < knapsack0_1.items.Count && bounding_func(knapsack0_1, curr_best_value, curr_weight, curr_index) >= best_solution)
            {
                items_bool[curr_index] = 0;
                if (curr_index < knapsack0_1.items.Count)
                    backtracking(knapsack0_1, curr_best_value , curr_weight , curr_index + 1);
                if (curr_best_value > best_solution && curr_index == knapsack0_1.items.Count -1)
                {
                    items_bool.CopyTo(sol_bool, 0);
                    best_solution = curr_best_value;
                    best_weight = curr_weight;
                }
            }
            return best_solution;
        }















    }
}
