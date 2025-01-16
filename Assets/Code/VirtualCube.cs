using System;
using System.Collections.Generic;
using System.Text;

public enum Color {
    YELLOW = 0,
    BLUE = 1,
    RED = 2,
    GREEN = 3,
    ORANGE = 4,
    WHITE = 5,
}

public enum Face {
    TOP = 0,
    LEFT = 1,
    FRONT = 2,
    RIGHT = 3,
    BACK = 4,
    BOTTOM = 5,
}

public enum Edge {
    TOP = 0,
    RIGHT = 1,
    BOTTOM = 2,
    LEFT = 3,
}

public class VirtualCube {
    // Cube layout:
    //
    //  0 1 2  |  (0,0) (0,1) (0,2)
    //  3 4 5  |  (1,0) (1,1) (1,2)
    //  6 7 8  |  (2,0) (2,1) (2,2)
    //
    //   TOP   |   LEFT  |  FRONT  |  RIGHT  |  BACK   | BOTTOM
    //    O    |    Y    |    Y    |    Y    |    Y    |    R
    //   YYY   |   BBB   |   RRR   |   GGG   |   OOO   |   WWW
    // B YYY G | O BBB R | B RRR G | R GGG O | G OOO B | B WWW G
    //   YYY   |   BBB   |   RRR   |   GGG   |   OOO   |   WWW
    //    R    |    W    |    W    |    W    |    W    |    O
    //
    // All sides are oriented according to the above diagram^
    // They are oriented as if viewing from outside the cube.
    // This simplifies the rotational logic.

    private readonly Color[,,] state = new Color[6,3,3];

    public VirtualCube() {
        for (int i = 0; i < 6; ++i) {
            for (int j = 0; j < 3; ++j) {
                for (int k = 0; k < 3; ++k) {
                    state[i,j,k] = (Color)i;
                }
            }
        }
    }

    public VirtualCube(string stateString) {
        var lines = stateString.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length != 3) {
            throw new ArgumentException("Invalid state string");
        }

        for (int j = 0; j < 3; ++j) {
            var line = lines[j];
            if (line.Length != 18) {
                throw new ArgumentException("Invalid state string");
            }

            for (int i = 0; i < 6; ++i) {
                for (int k = 0; k < 3; ++k) {
                    state[i, j, k] = line[i * 3 + k] switch {
                        'Y' => Color.YELLOW,
                        'B' => Color.BLUE,
                        'R' => Color.RED,
                        'G' => Color.GREEN,
                        'O' => Color.ORANGE,
                        'W' => Color.WHITE,
                        _ => throw new ArgumentException("Invalid color"),
                    };
                }
            }
        }
    }

    public Color[,,] GetState() {
        return state;
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
        for (int j = 0; j < 3; ++j) {
            for (int i = 0; i < 6; ++i) {
                for (int k = 0; k < 3; ++k) {
                    sb.Append(GetColorChar(state[i, j, k]));
                }
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    private (Func<int, Color>, Action<int, Color>) GetEdgeAccessors(Face face, Edge edge) {
        return edge switch {
            Edge.TOP => ((i) => state[(int)face, 0, i], (i, c) => state[(int)face, 0, i] = c),
            Edge.RIGHT => ((i) => state[(int)face, i, 2], (i, c) => state[(int)face, i, 2] = c),
            Edge.BOTTOM => ((i) => state[(int)face, 2, 2 - i], (i, c) => state[(int)face, 2, 2 - i] = c),
            Edge.LEFT => ((i) => state[(int)face, 2 - i, 0], (i, c) => state[(int)face, 2 - i, 0] = c),
            _ => throw new ArgumentException("Invalid edge"),
        };
    }

    private void SwapEdges(Face face1, Edge edge1, Face face2, Edge edge2) {
        var (edge1Read, edge1Write) = GetEdgeAccessors(face1, edge1);
        var (edge2Read, edge2Write) = GetEdgeAccessors(face2, edge2);
        
        for (int i = 0; i < 3; ++i) {
            var temp = edge1Read(i);
            edge1Write(i, edge2Read(i));
            edge2Write(i, temp);
        }
    }

    private void RotateFace(Face face, bool clockwise) {
        var temp = new Color[3, 3];

        // Copy the face
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                temp[i, j] = state[(int)face, i, j];
            }
        }

        // Rotate the face
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                state[(int)face, i, j] = clockwise ? temp[2 - j, i] : temp[j, 2 - i];
            }
        }
    }

    public void TopLayer(bool clockwise) {
        RotateFace(Face.TOP, clockwise);

        var order = new (Face, Edge)[] {
            (Face.BACK, Edge.TOP),
            (Face.RIGHT, Edge.TOP),
            (Face.FRONT, Edge.TOP),
            (Face.LEFT, Edge.TOP),
        };
        if (clockwise) {
            Array.Reverse(order);
        }
        for (int i = 0; i < 3; ++i) {
            SwapEdges(order[i].Item1, order[i].Item2, order[i + 1].Item1, order[i + 1].Item2);
        }
    }

    public void LeftLayer(bool clockwise) {
        RotateFace(Face.LEFT, clockwise);

        var order = new (Face, Edge)[] {
            (Face.TOP, Edge.LEFT),
            (Face.FRONT, Edge.LEFT),
            (Face.BOTTOM, Edge.LEFT),
            (Face.BACK, Edge.RIGHT),
        };
        if (clockwise) {
            Array.Reverse(order);
        }
        for (int i = 0; i < 3; ++i) {
            SwapEdges(order[i].Item1, order[i].Item2, order[i + 1].Item1, order[i + 1].Item2);
        }
    }

    public void FrontLayer(bool clockwise) {
        RotateFace(Face.FRONT, clockwise);

        var order = new (Face, Edge)[] {
            (Face.TOP, Edge.BOTTOM),
            (Face.RIGHT, Edge.LEFT),
            (Face.BOTTOM, Edge.TOP),
            (Face.LEFT, Edge.RIGHT),
        };
        if (clockwise) {
            Array.Reverse(order);
        }
        for (int i = 0; i < 3; ++i) {
            SwapEdges(order[i].Item1, order[i].Item2, order[i + 1].Item1, order[i + 1].Item2);
        }
    }

    public void RightLayer(bool clockwise) {
        RotateFace(Face.RIGHT, clockwise);

        var order = new (Face, Edge)[] {
            (Face.TOP, Edge.RIGHT),
            (Face.BACK, Edge.LEFT),
            (Face.BOTTOM, Edge.RIGHT),
            (Face.FRONT, Edge.RIGHT),
        };
        if (clockwise) {
            Array.Reverse(order);
        }
        for (int i = 0; i < 3; ++i) {
            SwapEdges(order[i].Item1, order[i].Item2, order[i + 1].Item1, order[i + 1].Item2);
        }
    }

    public void BackLayer(bool clockwise) {
        RotateFace(Face.BACK, !clockwise);

        var order = new (Face, Edge)[] {
            (Face.TOP, Edge.TOP),
            (Face.RIGHT, Edge.RIGHT),
            (Face.BOTTOM, Edge.BOTTOM),
            (Face.LEFT, Edge.LEFT),
        };
        if (clockwise) {
            Array.Reverse(order);
        }
        for (int i = 0; i < 3; ++i) {
            SwapEdges(order[i].Item1, order[i].Item2, order[i + 1].Item1, order[i + 1].Item2);
        }
    }

    public void BottomLayer(bool clockwise) {
        RotateFace(Face.BOTTOM, !clockwise);

        var order = new (Face, Edge)[] {
            (Face.BACK, Edge.BOTTOM),
            (Face.RIGHT, Edge.BOTTOM),
            (Face.FRONT, Edge.BOTTOM),
            (Face.LEFT, Edge.BOTTOM),
        };
        if (clockwise) {
            Array.Reverse(order);
        }
        for (int i = 0; i < 3; ++i) {
            SwapEdges(order[i].Item1, order[i].Item2, order[i + 1].Item1, order[i + 1].Item2);
        }
    }

    public int[] Scramble(int moves = 25) {
        if (moves < 1) {
            throw new ArgumentException("Number of moves must be positive");
        }

        var movesArray = new int[moves];
        for (int i = 0; i < moves; i++) {
            List<int> choices = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            if (i >= 1) {
                // Avoid placing opposite moves next to each other.
                var prevNum = movesArray[i - 1];
                var oppositePrevNum = prevNum + (prevNum % 2 == 0 ? 1 : -1);
                choices.Remove(oppositePrevNum);
            }
            else if (i >= 2 && movesArray[i - 2] == movesArray[i - 1]) {
                // Ensure there are no more than 2 of the same move in a row.
                var prevNum = movesArray[i - 1];
                choices.Remove(prevNum);
            }

            var index = UnityEngine.Random.Range(0, choices.Count);
            movesArray[i] = choices[index];
        }

        foreach (var move in movesArray) {
            var clockwise = move % 2 == 0;
            var side = move / 2 + 1;

            switch (side) {
                case 1:
                    TopLayer(clockwise);
                    break;
                case 2:
                    LeftLayer(clockwise);
                    break;
                case 3:
                    FrontLayer(clockwise);
                    break;
                case 4:
                    RightLayer(clockwise);
                    break;
                case 5:
                    BackLayer(clockwise);
                    break;
                case 6:
                    BottomLayer(clockwise);
                    break;
            }
        }

        return movesArray;
    }

    public bool IsSolved() {
        for (int i = 0; i < 6; ++i) {
            for (int j = 0; j < 3; ++j) {
                for (int k = 0; k < 3; ++k) {
                    if (state[i, j, k] != (Color)i) {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}