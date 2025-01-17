using System.Diagnostics;
using NUnit.Framework;

public class StateTreeTest {
    [Test]
    public void OneRotateOneDepth() {
        RewardCalculator calculator = new(1);
        for (int i = 0; i < 12; ++i) {
            var state = new VirtualCube();
            var move = new CubeMove(i);
            state.Rotate(move);

            var root = new StateTreeNode(state, null, calculator);
            root.CreateChildren(1);
            Assert.AreEqual(RewardCalculator.MAX_SCORE - 12, root.Score);
            Assert.AreEqual(12, root.Children);
            Assert.AreEqual(12, root.TotalChildren());
            Assert.AreEqual(3 * RewardCalculator.MAX_SCORE - 12, root.CalculateBestScore());

            var best = root.GetBestChild();
            Assert.AreEqual(RewardCalculator.MAX_SCORE, best.Score);
            Assert.AreEqual(move.Opposite(), best.Move);
        }
    }

    [Test]
    public void OneRotateTwoDepth() {
        RewardCalculator calculator = new(2);
        for (int i = 0; i < 12; ++i) {
            var state = new VirtualCube();
            var move = new CubeMove(i);
            state.Rotate(move);

            var root = new StateTreeNode(state, null, calculator);
            root.CreateChildren(2);
            Assert.AreEqual(RewardCalculator.MAX_SCORE - 12, root.Score);
            Assert.AreEqual(12, root.Children);
            Assert.AreEqual(12 + 144 - 12, root.TotalChildren());
            Assert.AreEqual(4 * RewardCalculator.MAX_SCORE - 12, root.CalculateBestScore());

            var best = root.GetBestChild();
            Assert.AreEqual(RewardCalculator.MAX_SCORE, best.Score);
            Assert.AreEqual(move.Opposite(), best.Move);
            Assert.AreEqual(0, best.Children);
        }
    }

    [Test]
    public void TwoRotateTwoDepth() {
        RewardCalculator calculator = new(2);
        for (int i = 0; i < 12; ++i) {
            var move1 = new CubeMove(i);
            for (int j = 0; j < 12; ++j) {
                var move2 = new CubeMove(j);
                // Skip repeated, redundant, and opposite moves.
                if (move1.Face == move2.Face || move1.Face == move2.Face.Opposite()) {
                    continue;
                }

                var state = new VirtualCube();
                state.Rotate(move1);
                state.Rotate(move2);

                var root = new StateTreeNode(state, null, calculator);
                root.CreateChildren(2);
                Assert.AreEqual(RewardCalculator.MAX_SCORE - 12 - 10, root.Score);
                Assert.AreEqual(12, root.Children);
                Assert.AreEqual(12 + 144, root.TotalChildren());

                var firstBest = root.GetBestChild();
                var secondBest = firstBest.GetBestChild();
                Assert.AreEqual(RewardCalculator.MAX_SCORE - 12, firstBest.Score);
                Assert.AreEqual(move2.Opposite(), firstBest.Move);
                Assert.AreEqual(12, firstBest.Children);
                Assert.AreEqual(RewardCalculator.MAX_SCORE, secondBest.Score);
                Assert.AreEqual(move1.Opposite(), secondBest.Move);
                Assert.AreEqual(0, secondBest.Children);
            }
        }
    }

    [Test]
    public void TwoRotateOneDepth() {
        RewardCalculator calculator = new(1);
        for (int i = 0; i < 12; ++i) {
            var move1 = new CubeMove(i);
            for (int j = 0; j < 12; ++j) {
                var move2 = new CubeMove(j);
                // Skip repeated, redundant, and opposite moves.
                if (move1.Face == move2.Face || move1.Face == move2.Face.Opposite()) {
                    continue;
                }

                var state = new VirtualCube();
                state.Rotate(move1);
                state.Rotate(move2);

                var root = new StateTreeNode(state, null, calculator);
                root.CreateChildren(1);
                Assert.AreEqual(RewardCalculator.MAX_SCORE - 12 - 10, root.Score);
                Assert.AreEqual(12, root.Children);
                Assert.AreEqual(12, root.TotalChildren());

                var best = root.GetBestChild();
                Assert.AreEqual(RewardCalculator.MAX_SCORE - 12, best.Score);
                Assert.AreEqual(move2.Opposite(), best.Move);
                Assert.AreEqual(0, best.Children);

                root.CreateChildren(2);
                Assert.AreEqual(12, root.Children);
                Assert.AreEqual(12 + 144, root.TotalChildren());

                best = best.GetBestChild();
                Assert.AreEqual(RewardCalculator.MAX_SCORE, best.Score);
                Assert.AreEqual(move1.Opposite(), best.Move);
                Assert.AreEqual(0, best.Children);
            }
        }
    }
}
