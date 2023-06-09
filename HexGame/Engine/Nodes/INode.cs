using HexGame.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Engine.Nodes
{
    internal interface INode
    {
        public INode? Parent { get; set; }
        public List<INode> Children { get; }
        public int Visits { get; set; }
        public double Wins { get; set; }
        public GameState State { get; }

        public bool IsFullyExpanded();
        public double Q(double explorationConstant);
        public INode AddChild(GameState childState);
    }
}
