using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationSystem.ClusteringAlgorithm
{
    public class Kohonen : IClusteringAlgorithm
    {
        private int MapDimensions;          /* Size of the map */
        private int DataDimensions;         /* Size of the data */
        private int Epochs;                 /* Number of iterations to learn over. */


        /* Learning parameters */
        private double PrefetchThreshold;   /* Preset threshold for which to recognize */
        private double InitialLearningRate; /* Learning rate upon starting to learn. */


        /* This represents the Clusters. Each cluster contains a prototype and a memberlist
         * with all ID's (Integer objects) of the datapoints that are member of that cluster.*/
        private Cluster[][] clusters;

        /* Vectors/Lists of training data. */
        private List<double[]> TrainingData;
        private List<double[]> TestData;


        /* Results of Test() */
        private double Hitrate;
        private double Accuracy;




        public Kohonen(int mapDim, int dataDim, int epochs, List<double[]> trainData, List<double[]> testData)
        {

            // Data dimensions is Variable though..
            // => Depends on the number of Variables
            // ==> Can change due to Dynamic Groups...
            DataDimensions = dataDim;
            MapDimensions = mapDim;
            Epochs = epochs;
            TrainingData = trainData;
            TestData = testData;
            PrefetchThreshold = 0.5;
            InitialLearningRate = 0.8;


            var random = new Random();

            clusters = new Cluster[MapDimensions][];


            /* Init all Clusters (their prototypes) with random values */
            for (int i = 0; i < MapDimensions; i++)
            {
                clusters[i] = new Cluster[MapDimensions];

                for (int j = 0; j < MapDimensions; j++)
                {
                    clusters[i][j] = new Cluster();
                    clusters[i][j].Prototype = new double[dataDim];
                    for (int k = 0; k < dataDim; k++)
                    {
                        clusters[i][j].Prototype[k] = random.NextDouble();
                    }
                }
            }
        }

        public void SetPrefetchThreshold(double pfThreshold) => PrefetchThreshold = pfThreshold;
        
        public bool Train()
        {
            throw new NotImplementedException();
        }

        public bool Test()
        {
            throw new NotImplementedException();
        }

        #region Printing

        public void ShowTest()
        {
            Console.WriteLine("");
            Console.WriteLine($"Initial learning Rate = {InitialLearningRate}");
            Console.WriteLine($"Prefetch threshold = {PrefetchThreshold}");
            Console.WriteLine($"Hitrate = {Hitrate}");
            Console.WriteLine($"Accuracy = {Accuracy}");
            Console.WriteLine($"Hitrate + Accuracy = {Hitrate + Accuracy}");
        }

        public void ShowMembers()
        {
            Console.WriteLine("\n\n -----Members-----\n");
            for (int i = 0; i < MapDimensions; i++)
            {
                for (int j = 0; j < MapDimensions; j++)
                {
                    Console.WriteLine($"Members cluster [{i}][{j}] : {clusters[i][j].CurrentMembers.Count}");
                }
            }
        }

        public void ShowPrototypes()
        {
            Console.WriteLine("\n\n -----Prototypes-----\n");
            for (int i = 0; i < MapDimensions; i++)
            {
                for (int j = 0; j < MapDimensions; j++)
                {
                    Console.Write($"Prototype Cluster [{i}][{j}] :");

                    for (int k = 0; k < DataDimensions; k++)
                    {
                        Console.Write($" {clusters[i][j].Prototype[k]}");
                    }
                    Console.WriteLine();
                }
            }
        }

        #endregion
    }

    public class Cluster
    {
        public double[] Prototype;
        public HashSet<Node> CurrentMembers; /* Indexes of the current members. */

        public Cluster()
        {
            CurrentMembers = new HashSet<Node>();
        }
    }
}
