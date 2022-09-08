using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classical_genetic
{
    class KNN
    {
        public class CompareDist : IComparable<CompareDist>
        {

            public int idx;  // index of a training item
            public double dist;  // distance to unknown
            public int CompareTo(CompareDist other)
            {
                if (this.dist < other.dist) return -1;
                else if (this.dist > other.dist) return +1;
                else return 0;
            }

        }
        public static double Distance(double[] predicted, double[] data)
        {
            double sum = 0.0;
            for (int i = 0; i < (predicted.Length -1); ++i)
                sum += (predicted[i] - data[i]) * (predicted[i] - data[i]);
            return Math.Sqrt(sum);
        }
        public static int Classify(double[] predicted,
        double[][] trainData, int numClasses, int k)
        {
            int n = trainData.Length;
            CompareDist[] inddist = new CompareDist[n];
            for (int i = 0; i < n; ++i)
            {
                CompareDist curr = new CompareDist();
                double dist = Distance(predicted, trainData[i]);
                curr.idx = i;
                curr.dist = dist;
                inddist[i] = curr;
            }
            Array.Sort(inddist); 
            int result = Voting(inddist, trainData, numClasses, k);
            return result;

    }
        public static int Voting(CompareDist[] inddist, double[][] trainData,
        int numClasses, int k)
        {
            int[] Votings = new int[numClasses];  // One cell per class
            for (int i = 0; i < k; i++)//++i
            {       
                int idx = inddist[i].idx;            
                int c = (int)trainData[idx].Last();   
                ++Votings[c];
            }
            int mostVotings = 0;
            int classWithMostVotings = 0;
            for (int j = 0; j < numClasses; ++j)
            {
                if (Votings[j] > mostVotings)
                {
                    mostVotings = Votings[j];
                    classWithMostVotings = j;
                }
            }
            return classWithMostVotings;
        }
    }
}
