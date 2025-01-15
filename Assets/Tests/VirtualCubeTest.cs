using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class VirtualCubeTest {
    [Test]
    public void VirtualCubeGetStringState() {
        VirtualCube cube = new();
        var expected =
            "YYY BBB RRR GGG OOO WWW\n" +
            "YYY BBB RRR GGG OOO WWW\n" +
            "YYY BBB RRR GGG OOO WWW\n";
        Assert.AreEqual(expected, cube.GetStateString());
    }

    [Test]
    public void VirtualCubeTestTopLayer() {
        VirtualCube cube = new();
        cube.TopLayer(true);
        var expected =
            "YYY RRR GGG OOO BBB WWW\n" +
            "YYY BBB RRR GGG OOO WWW\n" +
            "YYY BBB RRR GGG OOO WWW\n";
        Assert.AreEqual(expected, cube.GetStateString());
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    // [UnityTest]
    // public IEnumerator VirtualCubeTestWithEnumeratorPasses()
    // {
    //     // Use the Assert class to test conditions.
    //     // Use yield to skip a frame.
    //     yield return null;
    // }
}
