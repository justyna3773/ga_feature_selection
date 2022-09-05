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
        
    void Start()
        {
            int populationSize = 10;
            float mutationRate = 0.1f;

            int ind = 3;
            random = new System.Random();
            Person[] persons = new Person[27];
            for (int l = 3; l < 30; l++)
            {
                string pathName = @"C:\pobrane\deskryptory\22\descriptors_user_" + l + ".csv";
                Person pers = new Person(pathName);
                persons[l - 3] = pers;
            }
            data = new Dataset(random, ind, persons);
            data1 = new Dataset(random, ind, persons);
            data2 = new Dataset(random, ind, persons);
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
            int epochs = 50;
            for (int e = 0; e < epochs; e++)
            {
                ga.NewGeneration();
                ga.CalculateFitness();
                Console.WriteLine("\"" + e + "\": {");
                Console.WriteLine("\"best\": " + ga.BestFitness +",");
                Console.WriteLine("\"sum\": " + ga.fitnessSum +",");
                Console.Write("\"activ\": [");
                ga.BestGenes.ToList().ForEach(element => Console.Write($",{element}"));
                Console.Write("] }, ");
                
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
            
            SignatureFitness sig_fit = new SignatureFitness(random, activations, data);
            SignatureFitness sig_fit1 = new SignatureFitness(random, activations, data1);
            SignatureFitness sig_fit2 = new SignatureFitness(random, activations, data2);
            //List<double[][]> activated = sig_fit.activate_features(activations);
            score = sig_fit.totalFitness();
            score1 = sig_fit1.totalFitness();
            score2 = sig_fit2.totalFitness();
            return (score+score1+score2)*10;

        }

        static void Main(string[] args)
        {
            /*var metric = new DataPoint();
            double[] v1 = { 1, 2, 3 , 4, 5, 6};
            double[] v2 = { 2, 8, 6 };

            Console.WriteLine(Vector_Similarity.Cosine(v1, v2));
            Console.ReadLine();
            */
            /*List<List<double>> csv = ReadingCSV.Parse(@"C:\pobrane\deskryptory\22\descriptors_user_3.csv");
            Person person1 = new Person(@"C:\pobrane\deskryptory\22\descriptors_user_3.csv");
            int[] activ = { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0};
            //double[,] full_signatures = person1.choose_features(activ);
            SignatureFitness sig = new SignatureFitness();
            List<double[][]> res = sig.activate_features(activ);
            sig.totalFitness();
            double[][] chosen = person1.choose_features(activ);
            double[] scores = person1.similarity_score(person1.fullSignatures[0].ToArray());
            Console.ReadLine();*/
            Program prop = new Program();
            prop.Start();
            prop.Update();
            Console.ReadLine();


        }

        /*void Parse() {
            var path = @"C:\descriptors_user_3.csv";
            //var features_csv = new List<List<double>>();
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    var values = new List<double>();
                    string[] fields = csvParser.ReadFields();
                    
                    foreach (string field in fields){

                        values.Add(Convert.ToDouble(field));
                    }
                    //features_csv.Add(values);

                   
                }
            }*/

    }
}
