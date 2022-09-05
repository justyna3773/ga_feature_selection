using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classical_genetic
{
    class Dataset
    {
        public double[][] TrainingData { get; private set; }
        public double[][] TestingData { get; private set; }
        public double[][] activatedTraining { get; private set; }
        public double[][] activatedTesting { get; private set; }
        public int[] activations { get; set; }
        public Random random { get; private set; }
        public int index { get; private set; }
        public Person[] persons { get; private set; }
        public int training_ones { get; private set; }
        public int testing_ones { get; private set; }
        public Dataset(Random rand, int ind, Person[] persns)
            //dodawać aktywacje póżniej przy funkcji fitness
        {
            //this.activations = activ;
            random = rand;
            index = ind;
            persons = persns;
            this.DataDownsampled();
        }
        private void DataDownsampled()
        {
            List<double> temp = new List<double>() ;
            List<double[]> temp_nested;
            List<List<double>> total_data = new List<List<double>>();
            //use lists to add labels to samples
            // all samples from the recognized person and twice as many other samples, around 150 in total
            int limit1 = persons[index].fullSignatures.Count() * 2;
            int limit2 = persons[index].fullSignatures.Count();
            double[][] arr = new double[limit1 + limit2][];
            double[][] final_arr;
            for (int i = 0; i < limit1; i++)
            {
                //choose random person and choose random signature of a person
                int randomPerson = random.Next(0, persons.GetLength(0));
                int randomSig = random.Next(0, persons[randomPerson].fullSignatures.Count());
                if (randomPerson != index)
                {
                    temp = persons[randomPerson].fullSignatures[randomSig];
                    if (temp.Count <= 16)
                    {
                        temp.Add(0.0);
                    }
                    arr[i] = temp.ToArray();//tutaj tymczasowa lista od razu zamieniana na array
                }
                else
                {
                    temp = persons[randomPerson].fullSignatures[randomSig];
                    if (temp.Count <= 16)
                    {
                        temp.Add(1.0);
                    }
                    arr[i] = temp.ToArray();//tutaj tymczasowa lista od razu zamieniana na array
                }
            }
            //activating features in dataset
            
            //split the dataset into training and testing 70:30

            for (int k = limit1; k < limit1 + limit2; k++)
            {
                temp = persons[index].fullSignatures[k - limit1];
                if (temp.Count <= 16)
                {
                    temp.Add(1.0);
                }
                arr[k] = temp.ToArray();

            }
            
            int[] indexes = Enumerable.Range(0, limit1 + limit2).ToArray();
            Extensions.Shuffle(random, indexes);
            for (int m = 0; m < indexes.Length; m++)
            {
                arr[m] = arr[indexes[m]];
            }
            int total_data_len = indexes.Length;
            int split_point = Convert.ToInt32(total_data_len * 0.7);
            this.TrainingData = new double[split_point][];
            this.TestingData = new double[total_data_len - split_point][];
            for (int l = 0; l < indexes.Length; l++)
            {
                if (l < split_point)
                {
                    this.TrainingData[l] = arr[l];
                }
                else { this.TestingData[l - split_point] = arr[l]; }
            }
            this.testing_ones = 0;
            this.training_ones = 0;
            foreach (double[] tempor in this.TestingData)
            {
                if (tempor.Last() != 0) { this.testing_ones += 1; }

            }
            foreach (double[] tempor1 in this.TrainingData)
            {
                if (tempor1.Last() != 0) { this.training_ones += 1; }
            }

        }
        public void activate(int[] activ)
        {
            this.activations = activ;
            activatedTesting = TestingData.Select(a => a.ToArray()).ToArray();
            activatedTraining = TrainingData.Select(a => a.ToArray()).ToArray();
            for (int a = 0; a < this.TrainingData.GetLength(0); a++)
            {
                for (int b = 0; b < activations.Length; b++)
                {
                    if (activations[b] == 0)
                    {
                        activatedTraining[a][b] = 0;
                    }

                }
            }
            for (int a = 0; a < this.TestingData.GetLength(0); a++)
            {
                for (int b = 0; b < activations.Length; b++)
                {
                    if (activations[b] == 0)
                    {
                        activatedTesting[a][b] = 0;
                    }

                }
            }
        }
    }
}
