using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class CubsScriptTest {
    [SetUp]
    public void Setup() {
        SceneManager.LoadScene("Scenes/Scene");
    }

    [UnityTest]
    public IEnumerator KnownScramble() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();

        cube.LeftLayer(false);
        cube.TopLayer(true);
        cube.FrontLayer(true);
        cube.BackLayer(true);
        cube.FrontLayer(true);
        cube.BackLayer(false);
        cube.BackLayer(false);
        cube.LeftLayer(false);
        cube.RightLayer(true);
        cube.FrontLayer(false);

        List<Vector3> positions = new();
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                for (int k = 0; k < 3; ++k) {
                    var newPosition = cube.Cubelets[i, j, k].transform.position;
                    foreach (var position in positions) {
                        Assert2.AreNotNear(position, newPosition);
                    }
                    positions.Add(newPosition);
                }
            }
        }

        yield return null;
    }
}
