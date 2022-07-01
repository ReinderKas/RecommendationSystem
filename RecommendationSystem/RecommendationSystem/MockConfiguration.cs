using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationSystem
{
    public class MockConfiguration
    {

        public Guid FeatureModelId;
        public List<Requirement> Requirements;

        public List<Variable> Nodes;
        public List<Constraint> Constraints;


        public MockConfiguration(Guid featureModelId)
        {
            FeatureModelId = featureModelId;
            Requirements = new List<Requirement>();
        }


        public void AddSelectedRequirement(Guid NodeId, double value)
        {
            Requirements.Add(new Requirement(GetNodeById(NodeId), true, value));
        }

        public void AddDeselectedRequirement(Guid NodeId)
        {
            Requirements.Add(new Requirement(GetNodeById(NodeId), false, 0));
        }

        public Variable AddNode()
        {
            var newNode = new Variable();
            Nodes.Add(newNode);
            return newNode;
        }

        public Variable GetNodeById(Guid NodeId) => Nodes.FirstOrDefault(n => n.Id == NodeId);
    }

    public class Requirement
    {
        public Variable Node;
        public bool Selected;
        public double Value;

        public Requirement(Variable node, bool selected, double value)
        {
            if (node == null)
                throw new Exception("Node does not exist in the Configuration.");

            Node = node;
            Selected = selected;
            Value = value;
        }
    }

    public class Variable
    {
        public Guid Id;

        public bool Selected;
        public double Value;
        private double SomDistance; // TODO: How do you design this? (correctly at least.)

        public Requirement Requirement;

        private static Random random;


        public Variable()
        {
            if (random == null)
                random = new Random(1);

            Id = Guid.NewGuid();
            Selected = false;
            Value = 0;
            SomDistance = random.NextDouble();
        }

        public void AddRequirement(Requirement requirement)
        {
            Requirement = requirement;
        }

        // Distance within a Self Organizing Map.
        public double SOMDistance()
        {
            //TODO: How do you design this?
            return SomDistance;
        }
    }

    public class Constraint
    {
        public Guid Id;
        public ConstraintType Type;
        public List<Variable> Variables;

        public Constraint(ConstraintType type = ConstraintType.Optional, params Variable[] Nodes)
        {
            Id = Guid.NewGuid();
            Type = type;
            Variables = Nodes.ToList();        
        }

        public enum ConstraintType
        {
            Mandatory,
            Optional,
            Alternative,
            Or,

            Exclude,
            Required,
        }
    }
}
