using System;
using System.Text;
using UnityEngine;

public enum Color {
    YELLOW = 0,
    BLUE = 1,
    RED = 2,
    GREEN = 3,
    ORANGE = 4,
    WHITE = 5,
}

public class CubeScript : MonoBehaviour {
    // Cube layout:
    //
    //  6 7 8  |  (0,2) (1,2) (2,2)
    //  3 4 5  |  (0,1) (1,1) (2,1)
    //  0 1 2  |  (0,0) (1,0) (2,0)
    //
    //    O    |    Y    |    Y    |    Y    |    Y    |    R
    //   YYY   |   BBB   |   RRR   |   GGG   |   OOO   |   WWW
    // B YYY G | O BBB R | B RRR G | R GGG O | G OOO B | B WWW G
    //   YYY   |   BBB   |   RRR   |   GGG   |   OOO   |   WWW
    //    R    |    W    |    W    |    W    |    W    |    O
    //

    // Cube variables
    private GameObject[,,] Cubes = new GameObject[3, 3, 3];
    private Color[,,] CurrentColors = new Color[6, 3, 3];

    void Awake() {
        Cubes[0, 2, 0] = GameObject.Find("/Cube/TopFrontLeft");
        Cubes[1, 2, 0] = GameObject.Find("/Cube/TopFront");
        Cubes[2, 2, 0] = GameObject.Find("/Cube/TopFrontRight");
        Cubes[0, 2, 1] = GameObject.Find("/Cube/TopLeft");
        Cubes[1, 2, 1] = GameObject.Find("/Cube/Top");
        Cubes[2, 2, 1] = GameObject.Find("/Cube/TopRight");
        Cubes[0, 2, 2] = GameObject.Find("/Cube/TopBackLeft");
        Cubes[1, 2, 2] = GameObject.Find("/Cube/TopBack");
        Cubes[2, 2, 2] = GameObject.Find("/Cube/TopBackRight");
        Cubes[0, 1, 0] = GameObject.Find("/Cube/FrontLeft");
        Cubes[1, 1, 0] = GameObject.Find("/Cube/Front");
        Cubes[2, 1, 0] = GameObject.Find("/Cube/FrontRight");
        Cubes[0, 1, 1] = GameObject.Find("/Cube/Left");
        Cubes[1, 1, 1] = GameObject.Find("/Cube/Center");
        Cubes[2, 1, 1] = GameObject.Find("/Cube/Right");
        Cubes[0, 1, 2] = GameObject.Find("/Cube/BackLeft");
        Cubes[1, 1, 2] = GameObject.Find("/Cube/Back");
        Cubes[2, 1, 2] = GameObject.Find("/Cube/BackRight");
        Cubes[0, 0, 0] = GameObject.Find("/Cube/BottomFrontLeft");
        Cubes[1, 0, 0] = GameObject.Find("/Cube/BottomFront");
        Cubes[2, 0, 0] = GameObject.Find("/Cube/BottomFrontRight");
        Cubes[0, 0, 1] = GameObject.Find("/Cube/BottomLeft");
        Cubes[1, 0, 1] = GameObject.Find("/Cube/Bottom");
        Cubes[2, 0, 1] = GameObject.Find("/Cube/BottomRight");
        Cubes[0, 0, 2] = GameObject.Find("/Cube/BottomBackLeft");
        Cubes[1, 0, 2] = GameObject.Find("/Cube/BottomBack");
        Cubes[2, 0, 2] = GameObject.Find("/Cube/BottomBackRight");

        // Color Assignments
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 3; j++) {
                for (int k = 0; k < 3; k++) {
                    CurrentColors[i, j, k] = (Color)i;
                }
            }
        }
    }

    public Color[,,] GetState() {
        return CurrentColors;
    }

    private static char GetColorChar(Color color) {
        return color switch {
            Color.YELLOW => 'Y',
            Color.BLUE => 'B',
            Color.RED => 'R',
            Color.GREEN => 'G',
            Color.ORANGE => 'O',
            Color.WHITE => 'W',
            _ => throw new ArgumentException("Invalid color"),
        };
    }

    public string GetStateString() {
        StringBuilder sb = new();
        for (int j = 2; j >= 0; j--) {
            for (int i = 0; i < 6; i++) {
                for (int k = 0; k < 3; k++) {
                    sb.Append(GetColorChar(CurrentColors[i, j, k]));
                }
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public void TopLayer(float direction) {
        var temp = new GameObject[3, 3];
        var colTemp = new Color[3, 3];
        var colSidesTemp = new Color[12];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubes[j, 2, i].transform.RotateAround(Vector3.up, Vector3.up, direction * 90);
                temp[j, i] = Cubes[j, 2, i];
                colTemp[j, i] = CurrentColors[0, i, j];
            }
        }
        for (int i = 0; i < 12; i++) {
            colSidesTemp[i] = CurrentColors[Mathf.FloorToInt(i / 3) + 1, 2, (int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3];
        }

        if (direction == 1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[i, 2, Mathf.Abs(j - 2)] = temp[j, i];
                    CurrentColors[0, Mathf.Abs(j - 2), i] = colTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++) {
                CurrentColors[Mathf.FloorToInt(i / 3) + 1, 2, (int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3] = colSidesTemp[(i + 3) % 12];
            }
        }
        else if (direction == -1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[Mathf.Abs(i - 2), 2, j] = temp[j, i];
                    CurrentColors[0, j, Mathf.Abs(i - 2)] = colTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--) {
                CurrentColors[Mathf.FloorToInt(i / 3) + 1, 2, (int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3] = colSidesTemp[(i + 9) % 12];
            }
        }
    }

    public void LeftLayer(float direction) {
        var temp = new GameObject[3, 3];
        var colTemp = new Color[3, 3];
        var colSidesTemp = new Color[12];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubes[0, i, j].transform.RotateAround(Vector3.back, Vector3.back, direction * 90);
                temp[j, i] = Cubes[0, i, j];
                colTemp[j, i] = CurrentColors[1, i, j];
            }
        }
        for (int i = 0; i < 12; i++) {
            colSidesTemp[i] = CurrentColors[Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4), (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, 0];
        }

        if (direction == 1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[0, j, Mathf.Abs(i - 2)] = temp[j, i];
                    CurrentColors[1, Mathf.Abs(j - 2), i] = colTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--) {
                CurrentColors[Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4), (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, 0] = colSidesTemp[(i + 9) % 12];
            }
        }
        else if (direction == -1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[0, Mathf.Abs(j - 2), i] = temp[j, i];
                    CurrentColors[1, j, Mathf.Abs(i - 2)] = colTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++) {
                CurrentColors[Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4), (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, 0] = colSidesTemp[(i + 3) % 12];
            }
        }
    }

    public void FrontLayer(float direction) {
        var temp = new GameObject[3, 3];
        var colTemp = new Color[3, 3];
        var colSidesTemp = new Color[12];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubes[j, i, 0].transform.RotateAround(Vector3.right, Vector3.right, direction * 90);
                temp[j, i] = Cubes[j, i, 0];
                colTemp[j, i] = CurrentColors[2, i, j];
            }
        }
        for (int i = 0; i < 12; i++) {
            colSidesTemp[i] = CurrentColors[Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)(-Mathf.Abs(i - 4) + 4)), Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)) * Mathf.RoundToInt((float)Math.Tanh(((i + 3) % 6) - 1) + Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)))];
        }

        if (direction == 1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[i, Mathf.Abs(j - 2), 0] = temp[j, i];
                    CurrentColors[2, Mathf.Abs(j - 2), i] = colTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--) {
                CurrentColors[Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)(-Mathf.Abs(i - 4) + 4)), Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)) * Mathf.RoundToInt((float)Math.Tanh(((i + 3) % 6) - 1) + Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)))] = colSidesTemp[(i + 9) % 12];
            }
        }
        else if (direction == -1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[Mathf.Abs(i - 2), j, 0] = temp[j, i];
                    CurrentColors[2, j, Mathf.Abs(i - 2)] = colTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++) {
                CurrentColors[Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)(-Mathf.Abs(i - 4) + 4)), Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)) * Mathf.RoundToInt((float)Math.Tanh(((i + 3) % 6) - 1) + Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)))] = colSidesTemp[(i + 3) % 12];
            }
        }
    }

    public void RightLayer(float direction) {
        var temp = new GameObject[3, 3];
        var colTemp = new Color[3, 3];
        var colSidesTemp = new Color[12];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubes[2, i, j].transform.RotateAround(Vector3.forward, Vector3.forward, direction * 90);
                temp[j, i] = Cubes[2, i, j];
                colTemp[j, i] = CurrentColors[3, i, j];
            }
        }
        for (int i = 0; i < 12; i++) {
            colSidesTemp[i] = CurrentColors[Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4), (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, 2];
        }

        if (direction == 1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[2, Mathf.Abs(j - 2), i] = temp[j, i];
                    CurrentColors[3, Mathf.Abs(j - 2), i] = colTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++) {
                CurrentColors[Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4), (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, 2] = colSidesTemp[(i + 3) % 12];
            }
        }
        else if (direction == -1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[2, j, Mathf.Abs(i - 2)] = temp[j, i];
                    CurrentColors[3, j, Mathf.Abs(i - 2)] = colTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--) {
                CurrentColors[Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4), (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, 2] = colSidesTemp[(i + 9) % 12];
            }
        }
    }

    public void BackLayer(float direction) {
        var temp = new GameObject[3, 3];
        var colTemp = new Color[3, 3];
        var colSidesTemp = new Color[12];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubes[j, i, 2].transform.RotateAround(Vector3.left, Vector3.right, direction * 90);
                temp[j, i] = Cubes[j, i, 2];
                colTemp[j, i] = CurrentColors[4, i, j];
            }
        }
        for (int i = 0; i < 12; i++) {
            colSidesTemp[i] = CurrentColors[Mathf.RoundToInt((float)2.49 * Mathf.Pow((float)Math.E, (-1) * ((float)(i - 7)) / (float)3 * ((float)(i - 7)) / (float)3)), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)-Mathf.Abs(i - 4) + Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) + 3), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)];
        }

        if (direction == 1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[i, Mathf.Abs(j - 2), 2] = temp[j, i];
                    CurrentColors[4, Mathf.Abs(j - 2), i] = colTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--) {
                CurrentColors[Mathf.RoundToInt((float)2.49 * Mathf.Pow((float)Math.E, (-1) * ((float)(i - 7)) / (float)3 * ((float)(i - 7)) / (float)3)), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)-Mathf.Abs(i - 4) + Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) + 3), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)] = colSidesTemp[(i + 9) % 12];
            }
        }
        else if (direction == -1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[Mathf.Abs(i - 2), j, 2] = temp[j, i];
                    CurrentColors[4, j, Mathf.Abs(i - 2)] = colTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++) {
                CurrentColors[Mathf.RoundToInt((float)2.49 * Mathf.Pow((float)Math.E, (-1) * ((float)(i - 7)) / (float)3 * ((float)(i - 7)) / (float)3)), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)-Mathf.Abs(i - 4) + Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) + 3), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)] = colSidesTemp[(i + 3) % 12];
            }
        }
    }

    public void BottomLayer(float direction) {
        var temp = new GameObject[3, 3];
        var colTemp = new Color[3, 3];
        var colSidesTemp = new Color[12];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubes[j, 0, i].transform.RotateAround(Vector3.down, Vector3.up, direction * 90);
                temp[j, i] = Cubes[j, 0, i];
                colTemp[j, i] = CurrentColors[5, i, j];
            }
        }
        for (int i = 0; i < 12; i++) {
            colSidesTemp[i] = CurrentColors[Mathf.FloorToInt(i / 3) + 1, 0, (int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3];
        }

        if (direction == 1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[i, 0, Mathf.Abs(j - 2)] = temp[j, i];
                    CurrentColors[5, Mathf.Abs(j - 2), i] = colTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++) {
                CurrentColors[Mathf.FloorToInt(i / 3) + 1, 0, (int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3] = colSidesTemp[(i + 3) % 12];
            }
        }
        else if (direction == -1) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Cubes[Mathf.Abs(i - 2), 0, j] = temp[j, i];
                    CurrentColors[5, j, Mathf.Abs(i - 2)] = colTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--) {
                CurrentColors[Mathf.FloorToInt(i / 3) + 1, 0, (int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3] = colSidesTemp[(i + 9) % 12];
            }
        }
    }

    public void Scramble() {
        for (int i = 0; i < 25; i++) {
            int temp = UnityEngine.Random.Range(0, 2) * 2 - 1;
            int side = UnityEngine.Random.Range(1, 7);

            switch (side) {
                case 1:
                    TopLayer(temp);
                    break;
                case 2:
                    LeftLayer(temp);
                    break;
                case 3:
                    FrontLayer(temp);
                    break;
                case 4:
                    RightLayer(temp);
                    break;
                case 5:
                    BackLayer(temp);
                    break;
                case 6:
                    BottomLayer(temp);
                    break;
            }
        }
    }

    public bool IsSolved() {
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 3; j++) {
                for (int k = 0; k < 3; k++) {
                    if (CurrentColors[i, j, k] != (Color)i) {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}