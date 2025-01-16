using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

static class CubeScriptExtensions {
    public static string[] GetNames(this CubeScript cube) {
        string[] names = new string[27];
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                for (int k = 0; k < 3; ++k) {
                    names[i * 9 + j * 3 + k] = cube.Cubelets[k, j, i].name;
                }
            }
        }
        return names;
    }
}

public class CubsScriptTest {
    [SetUp]
    public void Setup() {
        SceneManager.LoadScene("Scenes/Scene");
    }

    [UnityTest]
    public IEnumerator DefaultCube() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        var expected = new string[] {
            "BottomFrontLeft",
            "BottomFront",
            "BottomFrontRight",
            "FrontLeft",
            "Front",
            "FrontRight",
            "TopFrontLeft",
            "TopFront",
            "TopFrontRight",
            "BottomLeft",
            "Bottom",
            "BottomRight",
            "Left",
            "Center",
            "Right",
            "TopLeft",
            "Top",
            "TopRight",
            "BottomBackLeft",
            "BottomBack",
            "BottomBackRight",
            "BackLeft",
            "Back",
            "BackRight",
            "TopBackLeft",
            "TopBack",
            "TopBackRight",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TopLayerClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.TopLayer(true);
        var expected = new string[] {
            "BottomFrontLeft",
            "BottomFront",
            "BottomFrontRight",
            "FrontLeft",
            "Front",
            "FrontRight",
            "TopFrontRight",
            "TopRight",
            "TopBackRight",
            "BottomLeft",
            "Bottom",
            "BottomRight",
            "Left",
            "Center",
            "Right",
            "TopFront",
            "Top",
            "TopBack",
            "BottomBackLeft",
            "BottomBack",
            "BottomBackRight",
            "BackLeft",
            "Back",
            "BackRight",
            "TopFrontLeft",
            "TopLeft",
            "TopBackLeft",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TopLayerCounterClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.TopLayer(false);
        var expected = new string[] {
            "BottomFrontLeft",
            "BottomFront",
            "BottomFrontRight",
            "FrontLeft",
            "Front",
            "FrontRight",
            "TopBackLeft",
            "TopLeft",
            "TopFrontLeft",
            "BottomLeft",
            "Bottom",
            "BottomRight",
            "Left",
            "Center",
            "Right",
            "TopBack",
            "Top",
            "TopFront",
            "BottomBackLeft",
            "BottomBack",
            "BottomBackRight",
            "BackLeft",
            "Back",
            "BackRight",
            "TopBackRight",
            "TopRight",
            "TopFrontRight",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator LeftLayerClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.LeftLayer(true);
        var expected = new string[] {
            "TopFrontLeft",
            "BottomFront",
            "BottomFrontRight",
            "TopLeft",
            "Front",
            "FrontRight",
            "TopBackLeft",
            "TopFront",
            "TopFrontRight",
            "FrontLeft",
            "Bottom",
            "BottomRight",
            "Left",
            "Center",
            "Right",
            "BackLeft",
            "Top",
            "TopRight",
            "BottomFrontLeft",
            "BottomBack",
            "BottomBackRight",
            "BottomLeft",
            "Back",
            "BackRight",
            "BottomBackLeft",
            "TopBack",
            "TopBackRight",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator LeftLayerCounterClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.LeftLayer(false);
        var expected = new string[] {
            "BottomBackLeft",
            "BottomFront",
            "BottomFrontRight",
            "BottomLeft",
            "Front",
            "FrontRight",
            "BottomFrontLeft",
            "TopFront",
            "TopFrontRight",
            "BackLeft",
            "Bottom",
            "BottomRight",
            "Left",
            "Center",
            "Right",
            "FrontLeft",
            "Top",
            "TopRight",
            "TopBackLeft",
            "BottomBack",
            "BottomBackRight",
            "TopLeft",
            "Back",
            "BackRight",
            "TopFrontLeft",
            "TopBack",
            "TopBackRight",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator FrontLayerClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.FrontLayer(true);
        var expected = new string[] {
            "BottomFrontRight",
            "FrontRight",
            "TopFrontRight",
            "BottomFront",
            "Front",
            "TopFront",
            "BottomFrontLeft",
            "FrontLeft",
            "TopFrontLeft",
            "BottomLeft",
            "Bottom",
            "BottomRight",
            "Left",
            "Center",
            "Right",
            "TopLeft",
            "Top",
            "TopRight",
            "BottomBackLeft",
            "BottomBack",
            "BottomBackRight",
            "BackLeft",
            "Back",
            "BackRight",
            "TopBackLeft",
            "TopBack",
            "TopBackRight",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator FrontLayerCounterClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.FrontLayer(false);
        var expected = new string[] {
            "TopFrontLeft",
            "FrontLeft",
            "BottomFrontLeft",
            "TopFront",
            "Front",
            "BottomFront",
            "TopFrontRight",
            "FrontRight",
            "BottomFrontRight",
            "BottomLeft",
            "Bottom",
            "BottomRight",
            "Left",
            "Center",
            "Right",
            "TopLeft",
            "Top",
            "TopRight",
            "BottomBackLeft",
            "BottomBack",
            "BottomBackRight",
            "BackLeft",
            "Back",
            "BackRight",
            "TopBackLeft",
            "TopBack",
            "TopBackRight",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator RightLayerClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.RightLayer(true);
        var expected = new string[] {
            "BottomFrontLeft",
            "BottomFront",
            "BottomBackRight",
            "FrontLeft",
            "Front",
            "BottomRight",
            "TopFrontLeft",
            "TopFront",
            "BottomFrontRight",
            "BottomLeft",
            "Bottom",
            "BackRight",
            "Left",
            "Center",
            "Right",
            "TopLeft",
            "Top",
            "FrontRight",
            "BottomBackLeft",
            "BottomBack",
            "TopBackRight",
            "BackLeft",
            "Back",
            "TopRight",
            "TopBackLeft",
            "TopBack",
            "TopFrontRight",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator RightLayerCounterClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.RightLayer(false);
        var expected = new string[] {
            "BottomFrontLeft",
            "BottomFront",
            "TopFrontRight",
            "FrontLeft",
            "Front",
            "TopRight",
            "TopFrontLeft",
            "TopFront",
            "TopBackRight",
            "BottomLeft",
            "Bottom",
            "FrontRight",
            "Left",
            "Center",
            "Right",
            "TopLeft",
            "Top",
            "BackRight",
            "BottomBackLeft",
            "BottomBack",
            "BottomFrontRight",
            "BackLeft",
            "Back",
            "BottomRight",
            "TopBackLeft",
            "TopBack",
            "BottomBackRight",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator BackLayerClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.BackLayer(true);
        var expected = new string[] {
            "BottomFrontLeft",
            "BottomFront",
            "BottomFrontRight",
            "FrontLeft",
            "Front",
            "FrontRight",
            "TopFrontLeft",
            "TopFront",
            "TopFrontRight",
            "BottomLeft",
            "Bottom",
            "BottomRight",
            "Left",
            "Center",
            "Right",
            "TopLeft",
            "Top",
            "TopRight",
            "BottomBackRight",
            "BackRight",
            "TopBackRight",
            "BottomBack",
            "Back",
            "TopBack",
            "BottomBackLeft",
            "BackLeft",
            "TopBackLeft",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator BackLayerCounterClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.BackLayer(false);
        var expected = new string[] {
            "BottomFrontLeft",
            "BottomFront",
            "BottomFrontRight",
            "FrontLeft",
            "Front",
            "FrontRight",
            "TopFrontLeft",
            "TopFront",
            "TopFrontRight",
            "BottomLeft",
            "Bottom",
            "BottomRight",
            "Left",
            "Center",
            "Right",
            "TopLeft",
            "Top",
            "TopRight",
            "TopBackLeft",
            "BackLeft",
            "BottomBackLeft",
            "TopBack",
            "Back",
            "BottomBack",
            "TopBackRight",
            "BackRight",
            "BottomBackRight",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator BottomLayerClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.BottomLayer(true);
        var expected = new string[] {
            "BottomFrontRight",
            "BottomRight",
            "BottomBackRight",
            "FrontLeft",
            "Front",
            "FrontRight",
            "TopFrontLeft",
            "TopFront",
            "TopFrontRight",
            "BottomFront",
            "Bottom",
            "BottomBack",
            "Left",
            "Center",
            "Right",
            "TopLeft",
            "Top",
            "TopRight",
            "BottomFrontLeft",
            "BottomLeft",
            "BottomBackLeft",
            "BackLeft",
            "Back",
            "BackRight",
            "TopBackLeft",
            "TopBack",
            "TopBackRight",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator BottomLayerCounterClockwise() {
        var cube = GameObject.Find("/Cube").GetComponent<CubeScript>();
        cube.BottomLayer(false);
        var expected = new string[] {
            "BottomBackLeft",
            "BottomLeft",
            "BottomFrontLeft",
            "FrontLeft",
            "Front",
            "FrontRight",
            "TopFrontLeft",
            "TopFront",
            "TopFrontRight",
            "BottomBack",
            "Bottom",
            "BottomFront",
            "Left",
            "Center",
            "Right",
            "TopLeft",
            "Top",
            "TopRight",
            "BottomBackRight",
            "BottomRight",
            "BottomFrontRight",
            "BackLeft",
            "Back",
            "BackRight",
            "TopBackLeft",
            "TopBack",
            "TopBackRight",
        };
        var actual = cube.GetNames();
        CollectionAssert.AreEqual(expected, actual);

        yield return null;
    }

    [UnityTest]
    public IEnumerator KnownScramblePositions() {
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
