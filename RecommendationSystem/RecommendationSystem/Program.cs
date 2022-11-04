using System;

namespace RecommendationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var clusteringAlgorithm = new Kohonen()

            var Clustering = new (Guid.NewGuid());

            Clustering.InitKohonen();
            Clustering.Train();
            Clustering.Test();



            //Clustering.ShowPrototypes();
            Clustering.ShowMembers();
            Clustering.ShowTestResults();

            var pause = true;
        }
    }
}