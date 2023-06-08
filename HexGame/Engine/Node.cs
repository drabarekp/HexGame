﻿using HexGame.Models;
using System;
using System.Collections.Generic;

namespace HexGame.Engine
{
    internal class Node
    {
        public Node? Parent;
        public List<Node> Children;
        public int Visits;
        public double Wins;
        public GameState State;

        public Node(GameState state)
        {
            this.State = state;
            Parent = null;
            Children = new List<Node>();
            Visits = 0;
            Wins = 0;
        }

        public bool IsFullyExpanded()
        {
            return Children.Count == State.GetPossibleMoves().Count;
        }

        public Node? SelectChild(double explorationConstant)
        {
            Node? bestChild = null;
            double bestScore = double.MinValue;

            foreach (var child in Children)
            {
                double score = child.Wins / child.Visits + explorationConstant * Math.Sqrt(Math.Log(Visits) / child.Visits);
                if (score > bestScore)
                {
                    bestChild = child;
                    bestScore = score;
                }
            }

            return bestChild;
        }

        public Node AddChild(GameState childState)
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