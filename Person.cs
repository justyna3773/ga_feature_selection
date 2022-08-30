using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classical_genetic
{
    class Person
    {
        public string path { get; private set; }
        public List<List<double>> fullSignatures { get; private set; }
        public double[][] currFeatures { get; private set; }
        public Person(string path)
        {
            this.path = path;
            fullSignatures = ReadingCSV.Parse(path);
        }

        public double[][] choose_features(int[] activations)
        {
            double[][] arr = new double[fullSignatures.Count][];
            for(int k =0;k<fullSignatures.Count;k++)
            {
                double[] temp_arr = new double[activations.Length];
                arr[k] = temp_arr;
                for (int i=0;i<activations.Length; i++)
                {
                    if (activations[i] == 1) { arr[k][i] = fullSignatures[k][i]; }
                    else { arr[k][i]= 0; }
                }
            }
            this.currFeatures = arr;
            return arr;
        }
        public (double[], double[]) getMeansStds()
        {
            int len = currFeatures[0].Length;
            double[] means = new double[len];
            double[] diffs = new double[len];
            double[] stds = new double[len];
            for (int i = 0; i < fullSignatures[0].Count; i++)
            {
                double[] temp_mean = new double[fullSignatures.Count];
                for (int k = 0; k < fullSignatures.Count; k++)
                {
                    temp_mean[k] = currFeatures[k][i];

                }
                means[i] = temp_mean.Average();
                stds[i] = Extensions.StdDev(temp_mean);
                

            }
            return (means, stds);
        }

        public double similarity_score(double[] sample)
        {
            //calculates similarity score of sample's features, score is added if difference between features and means of features is smaller than standard deviation of a feature
            //double[][] arrays = fullSignatures.Select(a => a.ToArray()).ToArray();
            int len = sample.Length;
            double[] means = new double[len];
            double[] diffs = new double[len];
            double[] stds = new double[len];
            for (int i =0;i<fullSignatures[0].Count;i++)
            {
                double[] temp_mean = new double[fullSignatures.Count];
                for (int k = 0; k < fullSignatures.Count; k++)
                {
                    temp_mean[k] = currFeatures[k][i];

                }
                means[i] = temp_mean.Average();
                stds[i] = Extensions.StdDev(temp_mean);
                //
                double diff = Math.Abs(sample[i] - means[i]);
                if (diff > stds[i])
                {
                    diffs[i] = 0;
                }
                else { diffs[i] = 1; }
                
            }
            return diffs.Sum();
        }

    }
}
