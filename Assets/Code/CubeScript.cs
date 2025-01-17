using UnityEngine;

public class CubeScript : MonoBehaviour {
    public readonly GameObject[,,] Cubelets = new GameObject[3, 3, 3];
    private readonly VirtualCube virtualCube = new();

    void Start() {
        Cubelets[0, 2, 0] = GameObject.Find("/Cube/TopFrontLeft");
        Cubelets[1, 2, 0] = GameObject.Find("/Cube/TopFront");
        Cubelets[2, 2, 0] = GameObject.Find("/Cube/TopFrontRight");
        Cubelets[0, 2, 1] = GameObject.Find("/Cube/TopLeft");
        Cubelets[1, 2, 1] = GameObject.Find("/Cube/Top");
        Cubelets[2, 2, 1] = GameObject.Find("/Cube/TopRight");
        Cubelets[0, 2, 2] = GameObject.Find("/Cube/TopBackLeft");
        Cubelets[1, 2, 2] = GameObject.Find("/Cube/TopBack");
        Cubelets[2, 2, 2] = GameObject.Find("/Cube/TopBackRight");
        Cubelets[0, 1, 0] = GameObject.Find("/Cube/FrontLeft");
        Cubelets[1, 1, 0] = GameObject.Find("/Cube/Front");
        Cubelets[2, 1, 0] = GameObject.Find("/Cube/FrontRight");
        Cubelets[0, 1, 1] = GameObject.Find("/Cube/Left");
        Cubelets[1, 1, 1] = GameObject.Find("/Cube/Center");
        Cubelets[2, 1, 1] = GameObject.Find("/Cube/Right");
        Cubelets[0, 1, 2] = GameObject.Find("/Cube/BackLeft");
        Cubelets[1, 1, 2] = GameObject.Find("/Cube/Back");
        Cubelets[2, 1, 2] = GameObject.Find("/Cube/BackRight");
        Cubelets[0, 0, 0] = GameObject.Find("/Cube/BottomFrontLeft");
        Cubelets[1, 0, 0] = GameObject.Find("/Cube/BottomFront");
        Cubelets[2, 0, 0] = GameObject.Find("/Cube/BottomFrontRight");
        Cubelets[0, 0, 1] = GameObject.Find("/Cube/BottomLeft");
        Cubelets[1, 0, 1] = GameObject.Find("/Cube/Bottom");
        Cubelets[2, 0, 1] = GameObject.Find("/Cube/BottomRight");
        Cubelets[0, 0, 2] = GameObject.Find("/Cube/BottomBackLeft");
        Cubelets[1, 0, 2] = GameObject.Find("/Cube/BottomBack");
        Cubelets[2, 0, 2] = GameObject.Find("/Cube/BottomBackRight");
    }

    public Color[,,] GetState() {
        return virtualCube.GetState();
    }

    public void RotateTop(bool clockwise) {
        virtualCube.RotateTop(clockwise);
        var angle = clockwise ? 90 : -90;
        var face = new GameObject[3, 3];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[i, 2, j].transform.RotateAround(gameObject.transform.position, gameObject.transform.up, angle);
                face[i, j] = Cubelets[i, 2, j];
            }
        }

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[i, 2, j] = clockwise ? face[2 - j, i] : face[j, 2 - i];
            }
        }
    }

    public void RotateLeft(bool clockwise) {
        virtualCube.RotateLeft(clockwise);
        var angle = clockwise ? 90 : -90;
        var face = new GameObject[3, 3];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[0, i, j].transform.RotateAround(gameObject.transform.position, -gameObject.transform.right, angle);
                face[i, j] = Cubelets[0, i, j];
            }
        }

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[0, i, j] = clockwise ? face[2 - j, i] : face[j, 2 - i]; 
            }
        }
    }

    public void RotateFront(bool clockwise) {
        virtualCube.RotateFront(clockwise);
        var angle = clockwise ? 90 : -90;
        var face = new GameObject[3, 3];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[i, j, 0].transform.RotateAround(gameObject.transform.position, -gameObject.transform.forward, angle);
                face[i, j] = Cubelets[i, j, 0];
            }
        }

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[i, j, 0] = clockwise ? face[2 - j, i] : face[j, 2 - i];
            }
        }
    }

    public void RotateRight(bool clockwise) {
        virtualCube.RotateRight(clockwise);
        var angle = clockwise ? 90 : -90;
        var face = new GameObject[3, 3];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[2, i, j].transform.RotateAround(gameObject.transform.position, gameObject.transform.right, angle);
                face[i, j] = Cubelets[2, i, j];
            }
        }

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[2, i, j] = clockwise ? face[j, 2 - i] : face[2 - j, i];
            }
        }
    }

    public void RotateBack(bool clockwise) {
        virtualCube.RotateBack(clockwise);
        var angle = clockwise ? 90 : -90;
        var face = new GameObject[3, 3];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[i, j, 2].transform.RotateAround(gameObject.transform.position, -gameObject.transform.forward, angle);
                face[i, j] = Cubelets[i, j, 2];
            }
        }

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[i, j, 2] = clockwise ? face[2 - j, i] : face[j, 2 - i];
            }
        }
    }

    public void RotateBottom(bool clockwise) {
        virtualCube.RotateBottom(clockwise);
        var angle = clockwise ? 90 : -90;
        var face = new GameObject[3, 3];

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[i, 0, j].transform.RotateAround(gameObject.transform.position, gameObject.transform.up, angle);
                face[i, j] = Cubelets[i, 0, j];
            }
        }

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Cubelets[i, 0, j] = clockwise ? face[2 - j, i] : face[j, 2 - i];
            }
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

    public void Scramble(int moves = 25) {
        var movesArray = VirtualCube.GenerateScramble(moves);
        foreach (var move in movesArray) {
            Rotate(move);
        }
    }

    public bool IsSolved() {
        return virtualCube.IsSolved();
    }
}