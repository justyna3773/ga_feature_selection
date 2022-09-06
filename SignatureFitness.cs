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
        public double[][][] data { get; private set; }
        private int index;
        public Random random { get; private set; }
        public double[][] trainingData { get; private set; }
        public double[][] testingData { get; private set; }
        public int testing_ones { get; private set; }
        public int training_ones { get; private set;}
        public List<double[][]> activatedFeatures { get; private set; }
        public List<double[]> meansFeatures { get; private set; }
        public List<double[]> stdsFeatures { get; private set; }
        public int[] currActivations { get; private set; }
        public double[] totalSimilarity { get; private set; }
        public double[] totalDissimilarity { get; private set; }
        public string pathN { get; private set; }
        public SignatureFitness(Random rand,int[] activations, Dataset dataset, string pathNa, int ind)
        {
            Persons = new Person[27];
            data = new double[27][][];
            activatedFeatures = new List<double[][]>();
            random = rand;
            index = ind;
            for(int i=3; i<30; i++)
            {
                string pathName = pathNa + i + ".csv";
                Person pers = new Person(pathName);
                Persons[i - 3] = pers;
                activatedFeatures.Add(pers.choose_features(activations));
                data[i - 3] = pers.currFeatures;
            }
            //this.DataToTraining();
            //ten zestaw daje 254.
            this.currActivations = activations;
            dataset.activate(activations);
            this.trainingData = dataset.activatedTraining;
            this.testingData = dataset.activatedTesting;
            this.testing_ones = dataset.testing_ones;
            this.training_ones = dataset.training_ones;
            
        }
        /*private void DataDownsampled()
        {
            List<double> temp;
            List<double[]> temp_nested;
            List<List<double>> total_data = new List<List<double>>();
            //use lists to add labels to samples
            // all samples from the recognized person and twice as many other samples, around 150 in total
            int limit1 = Persons[index].currFeatures.GetLength(0) * 2;
            int limit2 = Persons[index].currFeatures.GetLength(0);
            double[][] arr = new double[limit1+limit2][];
            double[][] final_arr;
            for (int i =0;i<limit1;i++)
            {
                //choose random person and choose random signature of a person
                int randomPerson = random.Next(0, Persons.GetLength(0));
                int randomSig = random.Next(0, Persons[randomPerson].currFeatures.GetLength(0));
                if (randomPerson != index)
                {
                    temp = Persons[randomPerson].currFeatures[randomSig].ToList();
                    temp.Add(0.0);
                    arr[i] = temp.ToArray();//tutaj tymczasowa lista od razu zamieniana na array
                }
                else {
                    temp = Persons[randomPerson].currFeatures[randomSig].ToList();
                    temp.Add(1.0);
                    arr[i] = temp.ToArray();//tutaj tymczasowa lista od razu zamieniana na array
                }
            }
            //split the dataset into training and testing 70:30

            for (int k = limit1;k<limit1+limit2; k++)
            {
                temp = Persons[index].currFeatures[k-limit1].ToList();
                temp.Add(1.0);
                arr[k] = temp.ToArray();
                
            }
            int[] indexes = Enumerable.Range(0, limit1+limit2).ToArray();
            Extensions.Shuffle(random, indexes);
            for (int m = 0; m < indexes.Length; m++)
            {
               arr[m] = arr[indexes[m]];
            }
            int total_data_len = indexes.Length;
            int split_point = Convert.ToInt32(total_data_len * 0.7);
            this.trainingData = new double[split_point][];
            this.testingData = new double[total_data_len - split_point][];
            for (int l = 0; l < indexes.Length; l++)
            {
                if (l < split_point)
                {
                    this.trainingData[l] = arr[l];
                }
                else { this.testingData[l - split_point] = arr[l]; }
            }
            this.testing_ones = 0;
            this.training_ones = 0;
            foreach (double[] tempor in this.testingData)
            {
                if (tempor.Last() != 0) { this.testing_ones += 1; }

            }
            foreach (double[] tempor1 in this.trainingData)
            {
                if (tempor1.Last() != 0) { this.training_ones += 1; }
            }

        }*/

        private void DataToTraining()
        {
            //this will be done once, to obtain better results on comparable dataset
            List<double[]> temp;
            List<double> temp_nested;
            List<List<double>> total_data = new List<List<double>>();
            double[][] arr;
            double[][] final_arr;
            int limit = data.GetLength(0);
            for (int i=0;i<limit;i++)
            {
                arr = data[i];
                temp = arr.ToList();
                for (int k=0;k<temp.Count;k++)
                {
                    temp_nested = temp[k].ToList();
                    if (index != i)
                    { temp_nested.Add(0); }
                    else
                    {
                        temp_nested.Add(1.0);
                    }
                    total_data.Add(temp_nested);
                }
            }
            final_arr = total_data.Select(a => a.ToArray()).ToArray();
            //this.trainingData = final_arr;
            //shuffle data before splitting
            int samples_num = final_arr.GetLength(0);
            int[] indexes = Enumerable.Range(0, samples_num).ToArray();
            Extensions.Shuffle(random, indexes);
            // apply shuffling to final_arr
            for (int m=0;m<indexes.Length;m++)
            {
                final_arr[m] = final_arr[indexes[m]];
            }
            // split data with 70:30 ratio
            int total_data_len = indexes.Length;
            int split_point = Convert.ToInt32(total_data_len * 0.7);
            this.trainingData = new double[split_point][];
            this.testingData = new double[total_data_len - split_point][];
            for(int l=0;l<indexes.Length;l++)
            {
                if (l<split_point)
                {
                    this.trainingData[l] = final_arr[l];
                }
                else { this.testingData[l - split_point] = final_arr[l]; }
            }
            this.testing_ones = 0;
            this.training_ones = 0;
            foreach (double[] tempor in this.testingData)
            {
                if (tempor.Last() != 0) { this.testing_ones += 1; }
            
            }
            foreach (double[] tempor1 in this.trainingData)
            {
                if (tempor1.Last() != 0) { this.training_ones += 1; }
            }

            //return final_arr;
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

            //double[][] trainData;
            //this.DataToTraining();
            int ones = 0;
            foreach (int x in currActivations)
            {
                if (x == 1) { ones += 1; }
            }
            int sum_correct = 0;
            int total_pred = this.testingData.GetLength(0);
            int numFeatures = 16;//może trzeba zrestrukturyzować kod
            int numClasses = 2;
            int sum_correct_ones = 0;
            //double[] unknown = new double[] { 5.25, 1.75 };
            int k = 9;
            foreach (double[] samp in this.testingData)
            {
                int predicted = KNNProgram.Classify(samp, this.trainingData,
                  numClasses, k);
                if (predicted == samp.Last()) {
                    if (predicted == 1) { sum_correct_ones += 1; }
                    sum_correct += 1; }
            }
            //Console.WriteLine("Accuracy " + sum_correct / total_pred);
            double recall = (double)sum_correct_ones / (double)testing_ones;
            double accuracy = (double)sum_correct / (double)total_pred;
            return (recall + accuracy);
        }

    }
}
