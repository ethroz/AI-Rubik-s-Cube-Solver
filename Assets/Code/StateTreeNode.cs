using System;
using System.Collections.Generic;

public class StateTreeNode {
    public VirtualCube State;
    public CubeMove Move;
    public int Score;
    private readonly List<StateTreeNode> Children;

    public StateTreeNode(VirtualCube state, CubeMove move) {
        State = state;
        Move = move;
        Score = state.Score();
        Children = new();
    }

    private void CreateChildren() {
        if (Children.Count > 0) {
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
                Children.Add(new(newState, move));
            }
        }
    }

    public void CreateChildren(int depth) {
        if (depth == 0) {
            return;
        }
        CreateChildren();
        foreach (var child in Children) {
            child.CreateChildren(depth - 1);
        }
    }

    public int CalculateBestScore() {
        if (Children.Count == 0) {
            return Score;
        }
        int bestScore = int.MinValue;
        foreach (var child in Children) {
            bestScore = Math.Max(bestScore, child.CalculateBestScore());
        }
        return bestScore;
    }

    public StateTreeNode GetBestChild() {
        if (Children.Count == 0) {
            return this;
        }
        int bestScore = int.MinValue;
        StateTreeNode bestChild = null;
        foreach (var child in Children) {
            int score = child.CalculateBestScore();
            if (score > bestScore) {
                bestScore = score;
                bestChild = child;
            }
        }
        return bestChild;
    }
}
