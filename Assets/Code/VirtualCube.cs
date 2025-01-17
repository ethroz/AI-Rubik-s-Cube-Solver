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
    BOTTOM = 5
}

public static class FaceExtensions {
    public static Face Opposite(this Face face) {
        return face switch {
            Face.TOP => Face.BOTTOM,
            Face.LEFT => Face.RIGHT,
            Face.FRONT => Face.BACK,
            Face.RIGHT => Face.LEFT,
            Face.BACK => Face.FRONT,
            Face.BOTTOM => Face.TOP,
            _ => throw new ArgumentException("Invalid face"),
        };
    }
}

public enum Edge {
    TOP = 0,
    RIGHT = 1,
    BOTTOM = 2,
    LEFT = 3,
}

public class CubeMove {
    public Face Face;
    public bool Clockwise;

    public CubeMove(Face face, bool clockwise) {
        Face = face;
        Clockwise = clockwise;
    }

    public CubeMove(int action) {
        if (action < 0 || action > 11) {
            throw new ArgumentException("Action must be in the range [0, 11].");
        }
        Face = (Face)(action / 2);
        Clockwise = action % 2 == 0;
    }

    public int ToInt() {
        return (int)Face * 2 + (Clockwise ? 0 : 1);
    }

    public CubeMove Opposite() {
        return new(Face, !Clockwise);
    }

    public override string ToString() {
        return $"{Face} {(Clockwise ? "CW" : "CCW")}";
    }

    public override bool Equals(object obj) {
        if (obj is CubeMove other) {
            return Face == other.Face && Clockwise == other.Clockwise;
        }
        return false;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Face, Clockwise);
    }

    public static bool operator ==(CubeMove left, CubeMove right) {
        return left.Equals(right);
    }

    public static bool operator !=(CubeMove left, CubeMove right) {
        return !(left == right);
    }
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

    public VirtualCube Copy() {
        var copy = new VirtualCube();
        for (int i = 0; i < 6; ++i) {
            for (int j = 0; j < 3; ++j) {
                for (int k = 0; k < 3; ++k) {
                    copy.state[i, j, k] = state[i, j, k];
                }
            }
        }
        return copy;
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

    public void RotateTop(bool clockwise) {
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

    public void RotateLeft(bool clockwise) {
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

    public void RotateFront(bool clockwise) {
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

    public void RotateRight(bool clockwise) {
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

    public void RotateBack(bool clockwise) {
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

    public void RotateBottom(bool clockwise) {
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

    public void Rotate(CubeMove move) {
        switch (move.Face) {
            case Face.TOP:
                RotateTop(move.Clockwise);
                break;
            case Face.LEFT:
                RotateLeft(move.Clockwise);
                break;
            case Face.FRONT:
                RotateFront(move.Clockwise);
                break;
            case Face.RIGHT:
                RotateRight(move.Clockwise);
                break;
            case Face.BACK:
                RotateBack(move.Clockwise);
                break;
            case Face.BOTTOM:
                RotateBottom(move.Clockwise);
                break;
        }
    }

    public void Apply(IEnumerable<CubeMove> moves) {
        foreach (var move in moves) {
            Rotate(move);
        }
    }

    public static CubeMove[] GenerateScramble(int moves = 25) {
        if (moves < 1) {
            throw new ArgumentException("Number of moves must be positive");
        }

        var movesArray = new CubeMove[moves];
        for (int i = 0; i < moves; i++) {
            List<CubeMove> choices = new() {
                new(Face.TOP, true),
                new(Face.TOP, false),
                new(Face.LEFT, true),
                new(Face.LEFT, false),
                new(Face.FRONT, true),
                new(Face.FRONT, false),
                new(Face.RIGHT, true),
                new(Face.RIGHT, false),
                new(Face.BACK, true),
                new(Face.BACK, false),
                new(Face.BOTTOM, true),
                new(Face.BOTTOM, false),
            };
            if (i >= 1) {
                // Avoid placing opposite moves next to each other.
                var prevMove = movesArray[i - 1];
                var oppositePrevMove = prevMove.Opposite();
                choices.Remove(oppositePrevMove);
            }
            else if (i >= 2 && movesArray[i - 2] == movesArray[i - 1]) {
                // Ensure there are no more than 2 of the same move in a row.
                var prevNum = movesArray[i - 1];
                choices.Remove(prevNum);
            }

            var index = UnityEngine.Random.Range(0, choices.Count);
            movesArray[i] = choices[index];
        }

        return movesArray;
    }

    public void Scramble(int moves = 25) {
        var movesArray = GenerateScramble(moves);
        foreach (var move in movesArray) {
            Rotate(move);
        }
    }

    public int Score() {
        int score = 0;
        for (int i = 0; i < 6; ++i) {
            for (int j = 0; j < 3; ++j) {
                for (int k = 0; k < 3; ++k) {
                    if (state[i, j, k] == (Color)i) {
                        score++;
                    }
                }
            }
        }
        return score;
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

    public static bool operator ==(VirtualCube left, VirtualCube right) {
        for (int i = 0; i < 6; ++i) {
            for (int j = 0; j < 3; ++j) {
                for (int k = 0; k < 3; ++k) {
                    if (left.state[i, j, k] != right.state[i, j, k]) {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public static bool operator !=(VirtualCube left, VirtualCube right) {
        return !(left == right);
    }

    public override bool Equals(object obj) {
        if (obj is VirtualCube other) {
            return this == other;
        }
        return false;
    }

    public override int GetHashCode() {
        int hc = state.Length;
        foreach (var color in state) {
            hc = unchecked(hc * 161803 + (int)color);
        }
        return hc;
    }
}