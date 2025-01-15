using NUnit.Framework;
using System;
using System.Linq;
public class VirtualCubeTest {
    
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    // [UnityTest]
    // public IEnumerator VirtualCubeTestWithEnumeratorPasses()
    // {
    //     // Use the Assert class to test conditions.
    //     // Use yield to skip a frame.
    //     yield return null;
    // }

    [Test]
    public void VirtualCubeGetStringState() {
        VirtualCube cube = new();
        var expected = new string[] {
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void TopLayerClockwise() {
        VirtualCube cube = new();
        cube.TopLayer(true);
        var expected = new string[] {
            "YYY RRR GGG OOO BBB WWW",
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void TopLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.TopLayer(false);
        var expected = new string[] {
            "YYY OOO BBB RRR GGG WWW",
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void LeftLayerClockwise() {
        VirtualCube cube = new();
        cube.LeftLayer(true);
        var expected = new string[] {
            "OYY BBB YRR GGG WOO RWW",
            "OYY BBB YRR GGG WOO RWW",
            "OYY BBB YRR GGG WOO RWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void LeftLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.LeftLayer(false);
        var expected = new string[] {
            "RYY BBB WRR GGG YOO OWW",
            "RYY BBB WRR GGG YOO OWW",
            "RYY BBB WRR GGG YOO OWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void FrontLayerClockwise() {
        VirtualCube cube = new();
        cube.FrontLayer(true);
        var expected = new string[] {
            "YYY BBW RRR YGG OOO WWW",
            "YYY BBW RRR YGG OOO WWW",
            "BBB BBW RRR YGG OOO GGG",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void FrontLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.FrontLayer(false);
        var expected = new string[] {
            "YYY BBY RRR WGG OOO WWW",
            "YYY BBY RRR WGG OOO WWW",
            "GGG BBY RRR WGG OOO BBB",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void RightLayerClockwise() {
        VirtualCube cube = new();
        cube.RightLayer(true);
        var expected = new string[] {
            "YYR BBB RRW GGG OOY WWO",
            "YYR BBB RRW GGG OOY WWO",
            "YYR BBB RRW GGG OOY WWO",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void RightLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.RightLayer(false);
        var expected = new string[] {
            "YYO BBB RRY GGG OOW WWR",
            "YYO BBB RRY GGG OOW WWR",
            "YYO BBB RRY GGG OOW WWR",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void BackLayerClockwise() {
        VirtualCube cube = new();
        cube.BackLayer(true);
        var expected = new string[] {
            "BBB WBB RRR GGY OOO GGG",
            "YYY WBB RRR GGY OOO WWW",
            "YYY WBB RRR GGY OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void BackLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.BackLayer(false);
        var expected = new string[] {
            "GGG YBB RRR GGW OOO BBB",
            "YYY YBB RRR GGW OOO WWW",
            "YYY YBB RRR GGW OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void BottomLayerClockwise() {
        VirtualCube cube = new();
        cube.BottomLayer(true);
        var expected = new string[] {
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
            "YYY RRR GGG OOO BBB WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void BottomLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.BottomLayer(false);
        var expected = new string[] {
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
            "YYY OOO BBB RRR GGG WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void TopThenLeft() {
        VirtualCube cube = new();
        cube.TopLayer(true);
        cube.LeftLayer(true);
        var expected = new string[] {
            "YYY RRR GGG OOO BBB WWW",
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }
}
