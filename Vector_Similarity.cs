using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classical_genetic
{
    public class Vector_Similarity
    {
        public static double Cosine(double[] v1, double[] v2)
        {
            double result = 0;
            result = InnerProduct(v1, v2) / (VectorSize(v1) * VectorSize(v2));
            return result;
        }
        public static double InnerProduct(double[] v1, double[] v2)
        {
            double Inner = 0;
            for (int i = 0; i < v1.Length; i++)
            {
                Inner += v1[i] * v2[i];
            }
            return Inner;
        }
        public static double VectorSize(double[] vector)
        {
            double vector_size = 0;

            for (int i = 0; i < vector.Length; i++)
            {
                vector_size += Math.Pow(vector[i], 2);
            }
            vector_size = Math.Sqrt(vector_size);

            return vector_size;
        }
    }
}
