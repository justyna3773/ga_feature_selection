using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classical_genetic
{
    class KNNProgram
    {
        public static double Distance(double[] unknown, double[] data)
        {
            double sum = 0.0;
            for (int i = 0; i < (unknown.Length -1); ++i)
                sum += (unknown[i] - data[i]) * (unknown[i] - data[i]);
            return Math.Sqrt(sum);
        }
        public static int Classify(double[] unknown,
        double[][] trainData, int numClasses, int k)
        {
            int n = trainData.Length;
            IndexAndDistance[] info = new IndexAndDistance[n];
            for (int i = 0; i < n; ++i)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(unknown, trainData[i]);
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }
            Array.Sort(info);  // sort by distance
            //Console.WriteLine("Nearest / Distance / Class");
            //Console.WriteLine("==========================");
            /*for (int i = 0; i < k; ++i)
            {
                int c = (int)trainData[info[i].idx][2];
                string dist = info[i].dist.ToString("F3");
                Console.WriteLine("( " + trainData[info[i].idx][0] +
                  "," + trainData[info[i].idx][1] + " )  :  " +
                  dist + "        " + c);
            }*/
            int result = Vote(info, trainData, numClasses, k);
            return result;
        // Classify
    }
        public static int Vote(IndexAndDistance[] info, double[][] trainData,
        int numClasses, int k)
        {
            int[] votes = new int[numClasses];  // One cell per class
            for (int i = 0; i < k; i++)//++i
            {       // Just first k
                int idx = info[i].idx;            // Which train item
                int c = (int)trainData[idx].Last();   // Class in last cell
                ++votes[c];
            }
            int mostVotes = 0;
            int classWithMostVotes = 0;
            for (int j = 0; j < numClasses; ++j)
            {
                if (votes[j] > mostVotes)
                {
                    mostVotes = votes[j];
                    classWithMostVotes = j;
                }
            }
            return classWithMostVotes;
        }
    }
}
