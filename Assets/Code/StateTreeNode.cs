using System;
using System.Collections.Generic;

public class RewardParameters {
    public readonly float DiscountRate;

    public RewardParameters(float discountRate = 1.0f) {
        DiscountRate = discountRate;
    }
}

public class StateTreeNode {
    public readonly VirtualCube State;
    public readonly CubeMove Move;
    public readonly float Score;
    private readonly RewardParameters parameters;
    private readonly List<StateTreeNode> children;

    public StateTreeNode(VirtualCube state, CubeMove move) {
        State = state;
        Move = move;
        parameters = new();
        Score = state.Score();
        children = new();
    }

    public StateTreeNode(VirtualCube state, CubeMove move, RewardParameters rewardParameters) {
        State = state;
        Move = move;
        parameters = rewardParameters;
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
                children.Add(new(newState, move, parameters));
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
            return Score;
        }
        var bestScore = float.NegativeInfinity;
        foreach (var child in children) {
            bestScore = Math.Max(bestScore, child.CalculateBestScore() * parameters.DiscountRate);
        }
        return bestScore;
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
