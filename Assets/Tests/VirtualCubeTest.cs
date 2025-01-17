using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

public class VirtualCubeTest {
    [Test]
    public void GetStringState() {
        VirtualCube cube = new();
        var expected = new string[] {
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void TopLayerClockwise() {
        VirtualCube cube = new();
        cube.RotateTop(true);
        var expected = new string[] {
            "YYY RRR GGG OOO BBB WWW",
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void TopLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.RotateTop(false);
        var expected = new string[] {
            "YYY OOO BBB RRR GGG WWW",
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void LeftLayerClockwise() {
        VirtualCube cube = new();
        cube.RotateLeft(true);
        var expected = new string[] {
            "OYY BBB YRR GGG OOW RWW",
            "OYY BBB YRR GGG OOW RWW",
            "OYY BBB YRR GGG OOW RWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void LeftLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.RotateLeft(false);
        var expected = new string[] {
            "RYY BBB WRR GGG OOY OWW",
            "RYY BBB WRR GGG OOY OWW",
            "RYY BBB WRR GGG OOY OWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void FrontLayerClockwise() {
        VirtualCube cube = new();
        cube.RotateFront(true);
        var expected = new string[] {
            "YYY BBW RRR YGG OOO GGG",
            "YYY BBW RRR YGG OOO WWW",
            "BBB BBW RRR YGG OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void FrontLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.RotateFront(false);
        var expected = new string[] {
            "YYY BBY RRR WGG OOO BBB",
            "YYY BBY RRR WGG OOO WWW",
            "GGG BBY RRR WGG OOO WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void RightLayerClockwise() {
        VirtualCube cube = new();
        cube.RotateRight(true);
        var expected = new string[] {
            "YYR BBB RRW GGG YOO WWO",
            "YYR BBB RRW GGG YOO WWO",
            "YYR BBB RRW GGG YOO WWO",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void RightLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.RotateRight(false);
        var expected = new string[] {
            "YYO BBB RRY GGG WOO WWR",
            "YYO BBB RRY GGG WOO WWR",
            "YYO BBB RRY GGG WOO WWR",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void BackLayerClockwise() {
        VirtualCube cube = new();
        cube.RotateBack(true);
        var expected = new string[] {
            "BBB WBB RRR GGY OOO WWW",
            "YYY WBB RRR GGY OOO WWW",
            "YYY WBB RRR GGY OOO GGG",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void BackLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.RotateBack(false);
        var expected = new string[] {
            "GGG YBB RRR GGW OOO WWW",
            "YYY YBB RRR GGW OOO WWW",
            "YYY YBB RRR GGW OOO BBB",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void BottomLayerClockwise() {
        VirtualCube cube = new();
        cube.RotateBottom(true);
        var expected = new string[] {
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
            "YYY RRR GGG OOO BBB WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void BottomLayerCounterClockwise() {
        VirtualCube cube = new();
        cube.RotateBottom(false);
        var expected = new string[] {
            "YYY BBB RRR GGG OOO WWW",
            "YYY BBB RRR GGG OOO WWW",
            "YYY OOO BBB RRR GGG WWW",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void TopThenLeftThenBack() {
        VirtualCube cube = new();
        cube.RotateTop(true);
        cube.RotateLeft(true);
        cube.RotateBack(true);
        var expected = new string[] {
            "BBB RBR YGG OOO WWW GWW",
            "OYY WBR YRR GGY BOO RWW",
            "BYY WBR YRR GGY BOO GGO",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void RightThenFrontThenBottom() {
        VirtualCube cube = new();
        cube.RotateRight(false);
        cube.RotateFront(false);
        cube.RotateBottom(false);
        var expected = new string[] {
            "YYO BBO YYY RGG WOO WWB",
            "YYO BBY RRR WGG WOO WWB",
            "GGG WOO BBY RRR WGG RRB",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void KnownScramble() {
        VirtualCube cube = new();
        cube.RotateLeft(false);
        cube.RotateTop(true);
        cube.RotateFront(true);
        cube.RotateBack(true);
        cube.RotateFront(true);
        cube.RotateBack(false);
        cube.RotateBack(false);
        cube.RotateLeft(false);
        cube.RotateRight(true);
        cube.RotateFront(false);
        var expected = new string[] {
            "RGW GGG YWB YBB OOW OBR",
            "RYW RBW RRG YGO YOY BWO",
            "RGO RRG YOW BWW GYY BBO",
        };
        var actual = cube.GetStateString().Split(" " + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void Equals() {
        VirtualCube original = new();
        var copy = original.Copy();
        Assert.AreEqual(original, copy);
    }

    [Test]
    public void HashCode() {
        VirtualCube original = new();
        var copy = original.Copy();
        HashSet<VirtualCube> set = new() {
            original,
            copy
        };
        Assert.AreEqual(1, set.Count);
    }
}
