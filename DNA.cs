using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classical_genetic
{
    class DNA<T> 
    {
        public T[] Genes { get; private set; }
        public double Fitness { get; private set; }
        public double[][] features { get; private set; }
        private Random random;
        private Func<T> getRandomGene;
        private Func<int, double> fitnessFunction;

        public DNA(int size, Random random, Func<T> getRandomGene, Func<int,double> fitnessFunction, double[][] Features, bool shouldInitGenes = true)
        {
            Genes = new T[size];
            this.random = random;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;
            this.features = Features;
            if (shouldInitGenes)
            {
                for (int i = 0; i < Genes.Length; i++)
                {
                    Genes[i] = getRandomGene();
                }
                /*T[] tempg = getRandomGene(random);
                Genes = tempg;*/
            }
            
        }
        public double CalculateFitness(int index)
        {
            Fitness = fitnessFunction(index);
            return Fitness;
        }
        
        public DNA<T> Crossover(DNA<T> otherParent)
        {
            DNA<T> child = new DNA<T>(Genes.Length, random, getRandomGene, fitnessFunction, features, shouldInitGenes: false);
            double crossoverProb = 0.75;
            double tempProb = random.NextDouble();
            if (tempProb < crossoverProb)
            {
                int crosspoint = random.Next(0, Genes.Length - 1);
                for (int i = 0; i < Genes.Length; i++)
                {
                    if (i < crosspoint)
                    {
                        child.Genes[i] = Genes[i];
                    }
                    else { child.Genes[i] = otherParent.Genes[i]; }
                }
            }
            else
            {
                if(Fitness > otherParent.Fitness)
                {
                    child.Genes = Genes;
                }
                else { child.Genes = otherParent.Genes; }
            }

            /*for (int i = 0; i<Genes.Length; i++)
            {
                child.Genes[i] = random.NextDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];
            }*/
            return child;
        }

        public void Mutate(float MutationRate)
        {
            /*T[] temp = getRandomGene(random);
            if (random.NextDouble() < MutationRate)
            {
                Genes = temp;
            }*/
            for (int i=0; i< Genes.Length; i++)
            {
                if (random.NextDouble() < MutationRate)
                {
                    Genes[i] = getRandomGene();
                }
            }

        }

    }
}
