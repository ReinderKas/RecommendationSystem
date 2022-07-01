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

        public List<Node> Nodes;
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

        public Node AddNode()
        {
            var newNode = new Node();
            Nodes.Add(newNode);
            return newNode;
        }

        public Node GetNodeById(Guid NodeId) => Nodes.FirstOrDefault(n => n.Id == NodeId);
    }

    public class Requirement
    {
        public Node Node;
        public bool Selected;
        public double Value;

        public Requirement(Node node, bool selected, double value)
        {
            if (node == null)
                throw new Exception("Node does not exist in the Configuration.");

            Node = node;
            Selected = selected;
            Value = value;
        }
    }

    public class Node
    {
        public Guid Id;


        public Requirement Requirement;

        public bool Selected;
        public double Value;


        public Node()
        {
            Id = Guid.NewGuid();
            Selected = false;
            Value = 0;
        }

        public void AddRequirement(Requirement requirement)
        {
            Requirement = requirement;
        }
    }

    public class Constraint
    {
        public Guid Id;
        public ConstraintType Type;
        public List<Node> Variables;

        public Constraint(ConstraintType type = ConstraintType.Optional, params Node[] Nodes)
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
