using System;
using System.Collections.Generic;

public class RewardCalculator {
    public static readonly int MAX_SCORE = 9 * 6;
    private readonly float DiscountRate;
    private readonly int Lookahead;
    private readonly Dictionary<VirtualCube, int> Occurrences = new();

    public RewardCalculator(int lookahead, float discountRate = 1.0f) {
        Lookahead = lookahead;
        DiscountRate = discountRate;
    }

    public void AddOccurrence(VirtualCube state) {
        if (Occurrences.ContainsKey(state)) {
            Occurrences[state]++;
        }
        else {
            Occurrences.Add(state, 1);
        }
    }

    public float Calculate(float score, StateTreeNode node) {
        return score * DiscountRate - Occurrences.GetValueOrDefault(node.State, 0) + (node.Score == MAX_SCORE ? Lookahead * MAX_SCORE : 0);
    }
}

public class StateTreeNode {
    public readonly VirtualCube State;
    public readonly CubeMove Move;
    public readonly int Score;
    private readonly RewardCalculator calculator;
    private readonly List<StateTreeNode> children;

    public StateTreeNode(VirtualCube state, CubeMove move, RewardCalculator rewardParameters) {
        State = state;
        Move = move;
        calculator = rewardParameters;
        Score = state.Score();
        children = new();
    }

    public int Children {
        get {
            return children.Count;
        }
    }

    public int TotalChildren() {
        int total = 0;
        foreach (var child in children) {
            total += child.TotalChildren();
        }
        return total + children.Count;
    }

    private void CreateChildren() {
        if (children.Count > 0) {
            return;
        }
        if (State.IsSolved()) {
            return;
        }
        // Create all possible children.
        for (int i = 0; i < 6; ++i) {
            for (int j = 0; j < 2; ++j) {
                VirtualCube newState = State.Copy();
                var move = new CubeMove((Face)i, j == 0);
                newState.Rotate(move);
                children.Add(new(newState, move, calculator));
            }
        }
    }

    // If this is run twice back to back, with x and x + 1 as arguments,
    // the second run will add 12 ^ (x + 1) more children.
    public void CreateChildren(int depth) {
        if (depth == 0) {
            return;
        }
        CreateChildren();
        foreach (var child in children) {
            child.CreateChildren(depth - 1);
        }
    }

    public float CalculateBestScore() {
        if (children.Count == 0) {
            return calculator.Calculate(Score, this);
        }
        var bestScore = float.NegativeInfinity;
        foreach (var child in children) {
            var score = child.CalculateBestScore();
            bestScore = Math.Max(bestScore, score);
        }
        return calculator.Calculate(bestScore + Score, this);
    }

    public StateTreeNode GetBestChild() {
        if (children.Count == 0) {
            return this;
        }
        var bestScore = float.NegativeInfinity;
        StateTreeNode bestChild = null;
        foreach (var child in children) {
            var score = child.CalculateBestScore();
            if (score > bestScore) {
                bestScore = score;
                bestChild = child;
            }
        }
        return bestChild;
    }
}
