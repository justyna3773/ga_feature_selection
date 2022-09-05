using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classical_genetic
{
    class GeneticAlgorithm<T>
    {
        public List<DNA<T>> Population { get; private set; }
        public int Generation { get; private set; }
        public int Elitism;
        public double[][] Features;
        public double BestFitness { get; private set; }
        private List<DNA<T>> newPopulation;
        public T[] BestGenes { get; private set; }
        public float MutationRate;
        private Random random;
        public double fitnessSum;
        private int dnaSize;
        private double[] fitnesses;
        private Func<T> getRandomGene;
        private Func<int, double> fitnessFunction;
        public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomGene, Func<int,double> fitnessFunction, double[][] Features, int elitism, float mutationRate = 0.01f )
        {
            Generation = 1;
            Elitism = elitism;
            MutationRate = mutationRate;
            Population = new List<DNA<T>>();
            newPopulation = new List<DNA<T>>(populationSize);
            this.random = random;
            this.dnaSize = dnaSize;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;
            this.fitnesses = new double[populationSize];
            BestGenes = new T[dnaSize];
            for (int i = 0; i< populationSize; i++)
            {
                Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, Features,shouldInitGenes: true));
            }
            
        }
        public void NewGeneration()
        {

            if (Population.Count <= 0)
            {
                return;
            }
            for (int k = 0; k < Population.Count; k++)
            {
                this.fitnesses[k] = Population[k].CalculateFitness(k);
            }
            CalculateFitness();
            List<DNA<T>> newPopulation = new List<DNA<T>>();

            for (int i = 0; i < Population.Count; i++)
            {
                DNA<T> parent1 = ChooseParent();
                DNA<T> parent2 = ChooseParent();

                DNA<T> child = parent1.Crossover(parent2);
                child.Mutate(MutationRate);
                newPopulation.Add(child);
            }
            Population = newPopulation;

            Generation++;
        }
        


        private int CompareDNA(DNA<T> a, DNA<T> b)
        {
            if (a.Fitness > b.Fitness)
            {
                return -1;
            }
            else if (a.Fitness < b.Fitness)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void CalculateFitness()
        {
            fitnessSum = 0;
            DNA<T> best = Population[0];
            for (int i = 0; i < Population.Count; i++)
            {
                fitnessSum += Population[i].CalculateFitness(i);
                if(Population[i].Fitness > best.Fitness)
                {
                    best = Population[i];
                }
            }
            BestFitness = best.Fitness;
            best.Genes.CopyTo(BestGenes, 0);
        }

        private DNA<T> ChooseParent()
        {
            double randomNumber = random.NextDouble();
            for (int i = 0; i < Population.Count; i++)
            {
                /*if(randomNumber<Population[i].Fitness)
                {
                    return Population[i];
                }
                randomNumber -= Population[i].Fitness;*/
                int ind = ProporSelect(this.fitnesses, random);
                return Population[ind];

            }
            return null;
        }

        static int ProporSelect(double[] vals, Random rnd)
        {
            // vals[] can't be all 0.0s
            int n = vals.Length;

            double sum = 0.0;
            for (int i = 0; i < n; ++i)
            { sum += vals[i]; }

            double accum = 0.0;
            double p = rnd.NextDouble();

            for (int i = 0; i< n; ++i) {
                accum += (vals[i] / sum);
                if (p<accum) return i;
            }
            return n - 1;  // last index

        }
    }
}
