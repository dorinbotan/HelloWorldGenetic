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
            // создай популяцию
            Population population = new Population();
                                             
            for(int i = 0; i < 8192; i++)
            {
                // измерь пригодность
                population.CalculateFitness();

                // отсортируй по пригодности                                       
                population.Sort();

                // покажи лучшего
                Console.WriteLine(population.citizens[0]);

                // если лучший найден, break
                if (!population.citizens[0].Equals("Hello world!"))
                    break;

                // спаривай популяцию
                
            }

            Console.ReadLine();
        }
    }

    class Population
    {
        const int POP_SIZE = 2048;
        const string TEXT = "Hello world!";

        Random random = new Random();
                                       
        public List<Citizen> citizens { get; set; }

        public Population()
        {
            citizens = new List<Citizen>(POP_SIZE);               

            for (int i = 0; i < POP_SIZE; i++)
            {
                Citizen citizen = new Citizen();

                for (int j = 0; j < TEXT.Length; j++)
                    citizen.str += (char)random.Next(33, 127);

                citizens.Add(citizen);
            }

            citizens = citizens.OrderByDescending(o => o.fitness).ToList();   
        }               

        public void CalculateFitness()
        {
            int fitness;

            for(int i = 0; i < citizens.Count; i++)
            {
                fitness = 0;

                for (int j = 0; j < TEXT.Length; j++)
                    fitness += Math.Abs(citizens[i].str[j] - TEXT[j]);

                citizens[i].fitness = fitness;
            }
        }          

        public void Sort()
        {
            citizens = citizens.OrderBy(o => o.fitness).ToList();
        }
        
        private List<Citizen> Elitism(List<Citizen> population, int size)
        {
            List<Citizen> toReturn = new List<Citizen>(size);

            for (int i = 0; i < size; i++)
                toReturn[i] = population[i];

            return toReturn;
        } 

        private void Mutate(Citizen citizen)
        {
            int pos = random.Next(0, TEXT.Length);
            int delta = random.Next(33, 127);

            StringBuilder sb = new StringBuilder(citizen.str);
            sb[pos] = (char)((citizen.str[pos] + delta) % 94 + 33);
            citizen.str = sb.ToString();
        }
    }

    class Citizen
    {
        public string str { get; set; } = "";
        public int fitness { get; set; } = 0; 

        public override string ToString()
        {
            return str + " (" + fitness + ")";
        }
    }
}
