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

        double[] dissimilarity_score(double[] sample, int ind)
        {
            this.meansStds();
            double[] scores = new double[sample.Length];
            for (int n = 0; n < sample.Length; n++)
            {

                double score = 0;
                for (int i = 0; i < Persons.Length; i++)
                {
                    if (ind != n)
                    {
                        double diff = Math.Abs(sample[n] - this.meansFeatures[i][n]);
                        if (diff > this.stdsFeatures[i][n])
                        {
                            score += diff;
                        }
                        else { score += 0; }
                    }
                }
                scores[n] = score;

            }
            return scores;
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
        public double totalFitness()
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
            for (int n=0;n<Persons.Length;n++)
            {
                double[] similarity_score_accu = new double[currActivations.Length];
                double[] dissimilarity_score_accu = new double[currActivations.Length];
                for (int k = 0; k < Persons[n].currFeatures.Length; k++)
                {
                    double[] similarity_score = Persons[n].similarity_score(Persons[n].currFeatures[k]);
                    double[] dissimilarity_score = this.dissimilarity_score(Persons[n].currFeatures[k], n);



                    for (int i = 0; i < currActivations.Length; i++)
                    {
                        similarity_score_accu[i] += similarity_score[i];
                        dissimilarity_score_accu[i] += dissimilarity_score[i];
                        totalFitness[i] += similarity_score[i];
                        totaldissimilarity[i] += dissimilarity_score[i];
                    }
                }
                
            }
            this.totalDissimilarity = totaldissimilarity;
            this.totalSimilarity = totalFitness;
            //return ((this.totalDissimilarity.Sum() - 10 * (this.totalSimilarity.Sum())))
            return (this.totalDissimilarity.Sum() - 8*this.totalSimilarity.Sum() - 20000*ones);
        }

    }
}
