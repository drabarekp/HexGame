using HexGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Engine.Nodes
{
    internal class HeuristicNode : Node
    {
        public HeuristicNode(GameState state) : base(state)
        {
        }

        public double Alpha { get; set; }
        public double Beta { get; set; }
        public double Heuristic { get; set; }

        public override INode AddChild(GameState childState)
        {
            var childNode = new HeuristicNode(childState)
            {
                Parent = this
            };

            Children.Add(childNode);
            return childNode;
        }
    }
}
