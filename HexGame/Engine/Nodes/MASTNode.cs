﻿using HexGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Engine.Nodes
{
    internal class MASTNode : Node
    {
        public MASTNode(GameState state) : base(state)
        {
        }

        public override INode AddChild(GameState childState)
        {
            var childNode = new MASTNode(childState)
            {
                Parent = this
            };

            Children.Add(childNode);
            return childNode;
        }

    }
}
