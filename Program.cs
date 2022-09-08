using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aglomera;
using Aglomera.Evaluation.Internal;
using Aglomera.Linkage;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;


namespace classical_genetic
{
    class Program
    {
        private GeneticAlgorithm<double> ga;
        private Random random;
        private Dataset data;
        private Dataset data1;
        private Dataset data2;
        private Dataset testing;
        private int[][] bestActivations;
        private double[] bestFitnesses;
        private int ind = 1;
        private string pathNa = @"C:\pobrane\deskryptory\22\descriptors_user_";
    void Start()
        {
            int populationSize = 50;
            float mutationRate = 0.1f;

            //int ind = 3;
            random = new System.Random();
            Person[] persons = new Person[27];
            for (int l = 3; l < 30; l++)
            {
                string pathName = pathNa + l + ".csv";
                Person pers = new Person(pathName);
                persons[l - 3] = pers;
            }
            data = new Dataset(random, ind, persons);
            while (data.testing_ones < 8)
            {
                data = new Dataset(random, ind, persons);
            }
            data1 = new Dataset(random, ind, persons);
            data2 = new Dataset(random, ind, persons);
            testing = new Dataset(random, ind, persons);
            while (testing.testing_ones < 8)
            {
                testing = new Dataset(random, ind, persons);
            }
            double[][] features = new double[][]
            {
            new double[] { 1,0,0,1,1},
            new double[] { 0,0,1,0,1},
            new double[] { 1,1,1,1,0},
            new double[] { 0,1,1,0,0},
            new double[] { 0,0,0,0,0},
            };
            
            ga = new GeneticAlgorithm<double>(populationSize, 16, random, getRandomBit, FitnessFunction, features, 16, mutationRate);
            ga.CalculateFitness();

        }
        void Update()
        {
            int epochs = 20;
            bestActivations = new int[epochs+2][];
            bestFitnesses = new double[epochs+2];
            for (int e = 0; e < epochs; e++)
            {
                ga.NewGeneration();
                ga.CalculateFitness();
                Console.WriteLine("\"" + e + "\": {");
                bestFitnesses[e] = ga.BestFitness;
                Console.WriteLine("\"best\": " + ga.BestFitness +",");
                Console.WriteLine("\"sum\": " + ga.fitnessSum +",");
                Console.Write("\"activ\": [");
                bestActivations[e] = ga.BestGenes.Select(x => (int)x).ToArray();
                ga.BestGenes.ToList().ForEach(element => Console.Write($",{element}"));
                Console.Write("] }, ");
                
            }
            bestActivations[epochs] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            bestFitnesses[epochs] = 2.0;
            bestActivations[epochs + 1] = new int[] { 0, 0, 1, 1, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 };
            bestFitnesses[epochs + 1] = 2.0;
        }
        void Test()
        {
            for (int c=0;c<bestActivations.GetLength(0);c++)
            {
                int correct_ones = 0;
                int all_correct = 0;
                int numClasses = 2;
                double recall = 0;
                double accu = 0;
                int k = 9;
                testing.activate(bestActivations[c]);
                int total_pred = testing.activatedTesting.GetLength(0);
                foreach (double[] samp in testing.activatedTesting)
                {
                    int predicted = KNN.Classify(samp, testing.activatedTraining,
                      numClasses, k);
                    if (predicted == samp.Last())
                    {
                        if (predicted == 1) { correct_ones += 1; }
                        all_correct += 1;
                    }
                }
                recall = (double)correct_ones / (double)testing.testing_ones;
                accu = (double)all_correct / (double)total_pred;
                Console.WriteLine("Fitness: " + bestFitnesses[c] + ", recall: " + recall + ", accu: " + accu + ", total: " + (accu + recall));
            }
        }
        private double getRandomBit()
        {
            return random.Next() % 2;
        }
        public static double[] getFixedArray(Random random)
        {
            int limit = 8;
            int size = 16;
            double[] res = new double[size];
            for (int i = 0; i < size; i++)
            {
                if (i < limit)
                {
                    res[i] = 1.0;
                }
                else { res[i] = 0; }

            }
            Extensions.Shuffle(random, res);
            return res;
        }
        /*private double FitnessFunction(int i)
        {
            double score = 0;
            DNA<double> dna = ga.Population[i];
            for (int k = 0; k < dna.features.Length; k++)
            {
                double[] dna_sec = new double[dna.Genes.Length];
                for (int l = 0; l < dna.Genes.Length; l++)
                {
                    if (dna.Genes[l] > 0)
                    {
                        dna_sec[l] = dna.features[k][l];
                    }
                    else { dna_sec[l] = 0; }
                }
                score += Vector_Similarity.Cosine(dna.Genes, dna_sec);
            }
            return score;
        }*/
        /*static int ProporSelect(double[] vals, Random rnd)
        {
            // vals[] can't be all 0.0s
            int n = vals.Length;

            double sum = 0.0;
            for (int i = 0; i < n; ++i)
                sum += vals[i];

            double accum = 0.0;
            double p = rnd.NextDouble();

            for (int i = 0; i < n; ++i)
            {
                accum += (vals[i] / sum);
                if (p < accum) return i;
            }
            return n - 1;  // last index
        }*/
        private double FitnessFunction(int i)
        {
            /*double score = 0;
            DNA<double> dna = ga.Population[i];
            for (int k =0; k<dna.Genes.Length; k++)
            {
                if (dna.Genes[k] == 1) { score += 1; }
                else {; }
            }
            return score;*/
            double score = 0;
            double score1 = 0;
            double score2 = 0;
            DNA<double> dna = ga.Population[i];
            int[] activations = dna.Genes.Select(x => (int)x).ToArray();
            
            SignatureFitness sig_fit = new SignatureFitness(random, activations, data, pathNa, ind);
            //SignatureFitness sig_fit1 = new SignatureFitness(random, activations, data1);
            //SignatureFitness sig_fit2 = new SignatureFitness(random, activations, data2);
            //List<double[][]> activated = sig_fit.activate_features(activations);
            score = sig_fit.totalFitness();
            //score1 = sig_fit1.totalFitness();
            //score2 = sig_fit2.totalFitness();
            return score*100;

        }

        static void Main(string[] args)
        {
            
            
            Program prop = new Program();
            prop.Start();
            prop.Update();
            Console.WriteLine("Test results: ");
            prop.Test();
            Console.ReadLine();


        }

        

    }
}
