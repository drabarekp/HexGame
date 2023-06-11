using HexGame.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HexGame.Engine.Nodes
{
    internal class Node : INode
    {
        public INode? Parent { get; set; }
        public List<INode> Children { get; }
        public int Visits { get; set; }
        public double Wins { get; set; }
        public GameState State { get; }

        public Node(GameState state)
        {
            State = state;
            Parent = null;
            Children = new List<INode>();
            Visits = 0;
            Wins = 0;
        }

        public bool IsFullyExpanded()
        {
            return Children.Count == State.GetPossibleMoves().Count;
        }

        public virtual double Q(double explorationConstant) => Wins / Visits + explorationConstant * Math.Sqrt(Math.Log(Parent!.Visits) / Visits);

        public virtual INode AddChild(GameState childState)
        {
            var childNode = new Node(childState)
            {
                Parent = this
            };

            Children.Add(childNode);
            return childNode;
        }
    }
}
