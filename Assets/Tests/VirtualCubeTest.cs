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
    public void VirtualCubeTestTopLayerClockwise() {
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
    public void VirtualCubeTestTopLayerCounterClockwise() {
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
    public void VirtualCubeTestLeftLayerClockwise() {
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
    public void VirtualCubeTestLeftLayerCounterClockwise() {
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
    public void VirtualCubeTestFrontLayerClockwise() {
        VirtualCube cube = new();
        cube.FrontLayer(true);
        var expected = new string[] {
            "YYY BBW RRR YGG OOO GGG",
            "YYY BBW RRR YGG OOO WWW",
            "BBB BBW RRR YGG OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void VirtualCubeTestFrontLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.FrontLayer(false);
        var expected = new string[] {
            "YYY BBY RRR WGG OOO BBB",
            "YYY BBY RRR WGG OOO WWW",
            "GGG BBY RRR WGG OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine).SkipLast(1).ToArray();
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void VirtualCubeTestRightLayerClockwise() {
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
    public void VirtualCubeTestRightLayerCounterClockwise() {
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
    public void VirtualCubeTestBackLayerClockwise() {
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
    public void VirtualCubeTestBackLayerCounterClockwise() {
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
    public void VirtualCubeTestBottomLayerClockwise() {
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
    public void VirtualCubeTestBottomLayerCounterClockwise() {
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
}
