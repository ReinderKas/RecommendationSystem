using RecommendationSystem.ClusteringAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationSystem
{
    public class Clustering
    {
        public Guid FeatureModelId;
        public IClusteringAlgorithm Algorithm;


        /* Dimensionality of the data/clusters. 
         * This would be the number of features (that can be selected?). */
        private int MapDimensions;
        private int DataDimensions;

        private int maxMapDim = 1000;


        // Data will be a Configuration.
        private List<MockConfiguration> trainData; // Quotation configurations
        private List<MockConfiguration> testData;  // Is there a real need for Testing Data?


        public Clustering(Guid featureModelId, IClusteringAlgorithm algorithm, int mapDimensions = 10, int dataDimensions = 10)
        {
            FeatureModelId = featureModelId;
            Algorithm = algorithm;
            MapDimensions = mapDimensions;
            DataDimensions = dataDimensions;
            DataDimensions = dataDimensions;
        }

        public bool Test() => Algorithm.Test();
        public bool Train() => Algorithm.Train();

        public void InitKohonen()
        {
            int epochs = 0;
            while (true
                 && (MapDimensions > 0
                    && MapDimensions < maxMapDim))
            {
                Console.WriteLine($"\nMap size? (N*N, 0 < N < {maxMapDim}) ");
                try
                {
                    MapDimensions = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}\n @{e.StackTrace}");
                    Console.WriteLine($"Please try again.");
                }
            }

            while (true)
            {
                Console.WriteLine("\nNumber of training epochs?");
                try
                {
                    epochs = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}\n @{e.StackTrace}");
                    Console.WriteLine($"Please try again.");
                }
            }

            /* From this the Kohonen Model can be created. */
            Algorithm = new Kohonen(MapDimensions, DataDimensions, epochs, trainData, testData);
        }






    }
}
