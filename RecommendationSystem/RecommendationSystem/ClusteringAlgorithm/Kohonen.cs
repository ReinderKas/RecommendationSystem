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
        private List<Variable[]> TrainingData;
        private List<Variable[]> TestData;


        /* Results of Test() */
        private double Hitrate;
        private double Accuracy;




        public Kohonen(int mapDim, int dataDim, int epochs, List<Variable[]> trainData, List<Variable[]> testData)
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


            InitializeRandomClusters();
        }

        public void SetPrefetchThreshold(double pfThreshold) => PrefetchThreshold = pfThreshold;

        private void InitializeRandomClusters()
        {
            var random = new Random();
            clusters = new Cluster[MapDimensions][];


            /* Init all Clusters (their prototypes) with random values */
            for (int i = 0; i < MapDimensions; i++)
            {
                clusters[i] = new Cluster[MapDimensions];

                for (int j = 0; j < MapDimensions; j++)
                {
                    clusters[i][j] = new Cluster();
                    clusters[i][j].Prototype = new double[DataDimensions];
                    for (int k = 0; k < DataDimensions; k++)
                    {
                        clusters[i][j].Prototype[k] = random.NextDouble();
                    }
                }
            }
        }


        public bool Train()
        {
            double learningRate = 0;
            double squareSize = 0;
            int radius = 0;

            /* Step 1:
             *  Initialize map with random vectors
             *  (As done in the constructor) */


            /*  Repeat 'Epoch' times :*/
            for (int currentEpoch = 0; currentEpoch < Epochs; currentEpoch++)
            {
                Console.WriteLine($"Epoch {currentEpoch}. \r");

                /* Step 2 : 
                 *  Calculate the squareSize and the learning Rate.
                 *  They decrease lineary with the number of Epochs. */
                learningRate = InitialLearningRate * (1 - ((double)currentEpoch / Epochs));
                squareSize = ((double)MapDimensions / 2) * (1 - ((double)currentEpoch / Epochs));
                radius = (int)squareSize;

                /* Step 3 : 
                 *  Every input vector is presented to the map (always in the same order => TODO: Room for improvement?)
                 *  For each vector its Best Maching Unit is found, and :      */

                foreach (var input in TrainingData)
                {
                    double bestDistance = DataDimensions;
                    int bestClusterX = 0;
                    int bestClusterY = 0;


                    // TODO: Double loop => Performance?
                    for (int x = 0; x < MapDimensions; x++)
                    {
                        for (int y = 0; y < MapDimensions; y++)
                        {
                            double currentDistance = 0;
                            Variable[] prototype = clusters[x][y].Prototype;

                            for (int datIndex = 0; datIndex < DataDimensions; datIndex++)
                            {
                                currentDistance += Math.Pow(input[datIndex].SOMDistance() - prototype[datIndex].SOMDistance(), 2);
                            }

                            currentDistance = Math.Sqrt(currentDistance);

                            /* Does nothing if the distance is the same 
                             * => Is an unlikely case anyway */
                            if (currentDistance < bestDistance)
                            {
                                bestClusterX = x;
                                bestClusterY = y;
                                bestDistance = currentDistance;
                                // TODO: Can the performance be improved here?
                            }
                        }
                    }

                    /* Security for index out of bounds for the clusters. */
                    int xBegin = Math.Max(bestClusterX - radius, 0);
                    int xEnd = Math.Min(bestClusterX + radius, MapDimensions - 1);

                    int yBegin = Math.Max(bestClusterY - radius, 0);
                    int yEnd = Math.Min(bestClusterY + radius, MapDimensions - 1);


                    /* Step 4 :
                     *  All nodes within the neighbourhood of the Best Matching Unit are changed,
                     *  This algorithm does NOT implement distance relative learning */

                    for (int x = xBegin; x <= xEnd; x++)
                    {
                        for (int y = yBegin; y <= yEnd; y++)
                        {
                            Variable[] prototype = clusters[x][y].Prototype;

                            for (int index = 0; index < DataDimensions; index++)
                            {
                                var curr = ((1 - learningRate) * prototype[index].SOMDistance()) + (learningRate * input[index].SOMDistance());
                                prototype[index].SOMDistance() = curr;
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Completed {Epochs} training Epochs.\r");

            /* Assing all members to their closest cluster. */
            for (int member = 0; member < TrainingData.Count; member++)
            {

                Variable[] memberData = TrainingData[member];

                double bestDistance = DataDimensions;
                int bestClusterDim1 = 0;
                int bestClusterDim2 = 0;

                for (int i = 0; i < MapDimensions; i++)
                {
                    for (int i2 = 0; i2 < MapDimensions; i2++)
                    {

                        double currentDistance = 0;
                        Variable[] prototype = clusters[i][i2].Prototype;

                        for (int index = 0; index < DataDimensions; index++)
                        {
                            currentDistance += Math.Pow(memberData[index].SOMDistance() - prototype[index].SOMDistance(), 2);
                        }

                        currentDistance = Math.Sqrt(currentDistance);

                        if (currentDistance < bestDistance)
                        {
                            bestClusterDim1 = i;
                            bestClusterDim2 = i2;
                            bestDistance = currentDistance;
                        }
                    }
                }
                clusters[bestClusterDim1][bestClusterDim2].CurrentMembers.Add(memberData[member]);
            }



            /* Kohonen SOM can take quite a while, so we could present the user with a progress bar. */
            return true;
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
        public Variable[] Prototype;
        public HashSet<Variable> CurrentMembers; /* Indexes of the current members. */

        public Cluster()
        {
            CurrentMembers = new HashSet<Variable>();
        }
    }
}
