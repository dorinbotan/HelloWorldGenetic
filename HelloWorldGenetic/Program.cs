using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloWorldGenetic
{
    class Program
    {                
        static void Main(string[] args)
        {
            Population population = new Population();

            while (true)
            {
                Console.WriteLine(population.GetBest());

                if (population.GetBest().Fitness == 0)
                    break;                           

                population.Progress();
            }

            Console.ReadLine();
        }
    }

    class Population
    {
        private int pop_size = 2048;
        private int elite_size = 100;
        private double mutation_rate = 0.2;
        public string text = "Hello world!";

        private Random randomizer;

        public List<Citizen> citizens { get; set; }
                                      
        public Population()
        {
            randomizer = new Random();

            CreateInitPopulation();    
        }                                  

        private void CreateInitPopulation()
        {
            citizens = new List<Citizen>(pop_size);
            
            for(int i = 0; i < pop_size; i++)
            {
                Citizen c = new Citizen();

                for(int j = 0; j < text.Length; j++)
                    c.Str += (char)(randomizer.Next(32, 127));

                citizens.Add(c);
            }

            CalculateFitness();
            Sort();
        }

        private void CalculateFitness()
        {
            int fitness;
                
            for(int i = 0; i < citizens.Count; i++)
            {
                fitness = 0;

                for(int j = 0; j < text.Length; j++)           
                    fitness += Math.Abs(citizens[i].Str[j] - text[j]);

                citizens[i].Fitness = fitness;
            }
        }

        private void Sort()
        {
            citizens = citizens.OrderBy(o => o.Fitness).ToList();
        }

        public Citizen GetBest()
        {
            return citizens[0];
        }

        public void Progress()
        {
            for(int i = elite_size; i < pop_size; i++)
            {
                Citizen c1 = citizens[i];
                Citizen c2 = citizens[randomizer.Next(0, elite_size)];
                                           
                Cross(c1, c2);             
            }

            CalculateFitness();
            Sort();
        }

        private void Mutate(Citizen citizen)
        {
            int pos = randomizer.Next(0, text.Length);
            char mut = (char)randomizer.Next(33, 127);
            
            StringBuilder sb = new StringBuilder(citizen.Str);
            sb[pos] = mut;
            citizen.Str = sb.ToString();                  
        }

        private void Cross(Citizen c1, Citizen c2)
        {
            int pos = randomizer.Next(0, text.Length);   

            if (pos % 2 == 0)                                                      
                c1.Str = c1.Str.Substring(0, pos) + c2.Str.Substring(pos, text.Length - pos);
            else
            {                     
                string tmp = c1.Str.Substring(pos, text.Length - pos);
                c1.Str = c2.Str.Substring(0, pos) + tmp;
            }

            if(randomizer.Next(0, 100) < 100 * mutation_rate)
                Mutate(c1);                                  
        }
    }

    class Citizen
    {
        public string Str { get; set; } = "";
        public int Fitness { get; set; } = 0;                   

        public override string ToString()
        {
            return String.Format("{0} ({1})", Str, Fitness);
        }
    }
}
