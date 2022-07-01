using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationSystem.ClusteringAlgorithm
{
    public interface IClusteringAlgorithm
    {
        public bool Train();
        public bool Test();
        public void SetPrefetchThreshold(double pfThreshold);
        public void ShowTest();
        public void ShowMembers();
        public void ShowPrototypes();

    }
}
