using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classical_genetic
{
    class SignatureFitness
    {
        public Person[] Persons { get; private set; }
        public List<double[][]> activatedFeatures { get; private set; }
        public List<double[]> meansFeatures { get; private set; }
        public List<double[]> stdsFeatures { get; private set; }
        public int[] currActivations { get; private set; }
        public double[] totalSimilarity { get; private set; }
        public double[] totalDissimilarity { get; private set; }
        public SignatureFitness()
        {
            Persons = new Person[27];
            for(int i=3; i<30; i++)
            {
                string pathName = @"C:\pobrane\deskryptory\22\descriptors_user_" + i + ".csv";
                Person pers = new Person(pathName);
                Persons[i - 3] = pers;
            }
        }

        public List<double[][]> activate_features(int[] activations)
        {
            this.currActivations = activations;
            activatedFeatures = new List<double[][]>();
            foreach( Person pers in Persons)
            {
                activatedFeatures.Add(pers.choose_features(activations));
            }
            return activatedFeatures;
        }

        double dissimilarity_score(double[] sample, int ind)
        {
            //this.meansStds();
            double[] scores = new double[sample.Length];
            for (int n = 0; n < sample.Length; n++)
            {

                double score = 0;
                for (int i = 0; i < Persons.GetLength(0); i++)
                {
                    if (ind != n)
                    {
                        double diff = Math.Abs(sample[n] - this.meansFeatures[i][n]);
                        if (diff > this.stdsFeatures[i][n])
                        {
                            score += 1;
                        }
                        else { score += 0; }
                    }
                }
                scores[n] = score;

            }
            return scores.Sum();
        }

        public void meansStds()
        {
            this.meansFeatures = new List<double[]>(new double[Persons.Length][]);
            this.stdsFeatures = new List<double[]>(new double[Persons.Length][]);
            for (int i=0;i<Persons.Length;i++)
            {
                (this.meansFeatures[i], this.stdsFeatures[i]) = Persons[i].getMeansStds();

            }
        }
        public double totalFitness(int n)
        {
            this.meansStds();
            double[] totalFitness = new double[currActivations.Length];
            double[] totaldissimilarity = new double[currActivations.Length];
            int ones = 0;
            foreach (int m in currActivations)
            {
                if (m == 1)
                {
                    ones += 1;
                }
            }
            //int n = 3;
            double similarity_score_accu = 0;
            double dissimilarity_score_accu = 0;
                for (int k = 0; k < Persons[n].currFeatures.GetLength(0); k++)
                {
                    
                    double similarity_score = Persons[n].similarity_score(Persons[n].currFeatures[k]);
                    double dissimilarity_score = this.dissimilarity_score(Persons[n].currFeatures[k], n);
                similarity_score_accu += similarity_score;
                dissimilarity_score_accu += dissimilarity_score;


                
                }
                
            
            if (ones == 0) { ones = 1; }
            //return ((this.totalDissimilarity.Sum() - 10 * (this.totalSimilarity.Sum())))
            return ((10/ones)*(dissimilarity_score_accu - similarity_score_accu));
            //if (ones == 8) { ones = 1; }
            //return ((16-ones)*(this.totalDissimilarity.Sum() - this.totalSimilarity.Sum()));
        }

    }
}
