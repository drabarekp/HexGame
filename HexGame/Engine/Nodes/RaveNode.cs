using HexGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HexGame.Engine.Nodes
{
    internal class RaveNode : Node
    {
        public Dictionary<GameMove, double> RaveWins;

        public RaveNode(GameState state) : base(state)
        {
            RaveWins = new Dictionary<GameMove, double>();
        }

        public override double Q(double explorationCostant)
        {
            double beta = 0.5;

            double raveValue = 0;

            if (Parent != null)
            {
                if (RaveWins.ContainsKey(Parent.State.LastMove))
                    raveValue = RaveWins[Parent.State.LastMove] / Visits;
            }

            return beta * raveValue + (1 - beta) * base.Q(explorationCostant);
        }

        public override INode AddChild(GameState childState)
        {
            var childNode = new RaveNode(childState)
            {
                Parent = this
            };

            Children.Add(childNode);
            return childNode;
        }
    }
}
