using System;
using System.Collections.Generic;
using UnityEngine;
using Extras;

public class CubeScript : MonoBehaviour
{
    // Cube layout:
    //
    //  6 7 8  |  (0,2) (1,2) (2,2)
    //  3 4 5  |  (0,1) (1,1) (2,1)
    //  0 1 2  |  (0,0) (1,0) (2,0)
    //
    //  Y -> 0
    //  B -> 1
    //  R -> 2
    //  G -> 3
    //  O -> 4
    //  W -> 5  
    //
    //    O    |    Y    |    Y    |    Y    |    Y    |    R
    //   YYY   |   BBB   |   RRR   |   GGG   |   OOO   |   WWW
    // B YYY G | O BBB R | B RRR G | R GGG O | G OOO B | B WWW G
    //   YYY   |   BBB   |   RRR   |   GGG   |   OOO   |   WWW
    //    R    |    W    |    W    |    W    |    W    |    O
    //

    // Cube variables
    private GameObject MainCamera;
    public float CameraDistance = 5.0f;
    private float Sensitivity = 8;
    private float Pitch = 45;
    private float Yaw = -45;
    private float UserDirection = 1;
    private bool LimitedInput = false;
    private GameObject[,,] Cubes = new GameObject[3, 3, 3];

    // Save document variables
    public static string NL = Environment.NewLine;
    private string WeightsSavePath;
    public static string path;

    // Cube solver variables
    private CubeSolver Solver = new CubeSolver();
    private bool StartSolve = false;
    private float[] Solution = new float[0];
    private int CurrentSolutionStep = 0;
    private static char[,,] CurrentColors = new char[3, 3, 6];
    private float CurrentMove;
    // history function
    private int Count = 0;
    private int CurrentSide = 0;
    private int MoveIndex = 0;
    private List<float> History = new List<float>();
    private float[] HistoryArray = new float[0];

    //Neural Network variables
    private static NeuralNetwork Network = new NeuralNetwork(new int[] { 324, 274, 366, 12 });
    private float[] CurrentNeuralOutputs = new float[12];
    private bool Done;
    private bool Solved = true;
    private float Average = 0;
    private float Best = 0;
    private int CorrectCount = 0;
    private int CurrentItteration = 1;
    private int MaxItterations = 500;
    private bool Finished = true;
    private bool NeuralStart = false;
    private float NeuralChoice;

    //declaration of arrays
    private float[] CurrentState = new float[324];
    private float[] ExpectedOutput = new float[12];

    void Start()
    {
        //Cube variable assignments
        MainCamera = Camera.main.gameObject;

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

        //Solver assignments
        for (int i = 0; i < 6; i++)
        {
            char color = IndexToColor(i);
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    CurrentColors[k, j, i] = color;
                }
            }
        }

        //Assign Save Document Variables
        WeightsSavePath = Application.dataPath + @"\Saves\Weight Saves.txt";
        path = Application.dataPath + @"\Saves\TestTextDocument.txt";
        System.IO.File.WriteAllText(path, "");

        //Neural and Modular Network Assignments
    }

    void Update()
    {
        UserInput();
    }

    private void Test1()
    {
        //Write Code Here
        Scramble();
        Solver = new CubeSolver();
        Solution = Solver.FindSolution(CurrentColors);

        AssignInputs();
        GetModularChoice();
        NeuralOutput();
        ModularBackPropCalculations();
    }

    private void Test2()
    {
        //Write Code Here
        double rate = 1;

        double input = 1;
        double weight = NeuralNetwork.Layer.random.NextDouble() - 0.5f;
        double expected = 1;
        double output = input * weight;
        output = Math.Tanh(output);
        double error = output - expected;
        double gamma = error * NeuralNetwork.Layer.TanHDer((float)output);
        double weightDelta = gamma * input;
        double newWeight = weight;
        newWeight -= weightDelta * rate;
        D.L("weight: " + weight + "   new weight: " + newWeight + "   change: " + (newWeight - weight));
    }
    
    private void Test3()
    {
        //Write Code Here
        SaveWeightsToDoc(new float[0][][]);
    }

    private void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        if (Input.GetMouseButton(0))
        {
            Yaw += Sensitivity * Input.GetAxis("Mouse X");
            Pitch -= Sensitivity * Input.GetAxis("Mouse Y");
            if (Pitch > 90)
                Pitch = 90;
            if (Pitch < -90)
                Pitch = -90;

            // Orbit the camera around the origin.
            MainCamera.transform.position = Vector3.back * CameraDistance;
            MainCamera.transform.eulerAngles = Vector3.zero;
            MainCamera.transform.RotateAround(Vector3.zero, MainCamera.transform.up, Yaw);
            MainCamera.transform.RotateAround(Vector3.zero, MainCamera.transform.right, Pitch);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            RunTest(Test1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            RunTest(Test2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            RunTest(Test3);

        if (LimitedInput)
            return;

        if (Input.GetKey(KeyCode.LeftShift))
            UserDirection = -1;
        else
            UserDirection = 1;
        if (Input.GetKeyDown(KeyCode.Q))
            BottomLayer(UserDirection);
        if (Input.GetKeyDown(KeyCode.W))
            BackLayer(UserDirection);
        if (Input.GetKeyDown(KeyCode.E))
            TopLayer(UserDirection);
        if (Input.GetKeyDown(KeyCode.A))
            LeftLayer(UserDirection);
        if (Input.GetKeyDown(KeyCode.S))
            FrontLayer(UserDirection);
        if (Input.GetKeyDown(KeyCode.D))
            RightLayer(UserDirection);
        if (Input.GetKeyDown(KeyCode.X))
            Scramble();
        if (Input.GetKeyDown(KeyCode.C))
            System.IO.File.WriteAllText(path, "");
        if (Input.GetKeyDown(KeyCode.V))
            SaveCubeToDoc();
        if (Input.GetKeyDown(KeyCode.B))
            System.Diagnostics.Process.Start(path);
        if (Input.GetKeyDown(KeyCode.N))
            System.Diagnostics.Process.Start(WeightsSavePath);
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            System.IO.File.WriteAllText(WeightsSavePath, "");
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Solver = new CubeSolver();
            Solution = Solver.FindSolution(CurrentColors);
            CurrentSolutionStep = 0;
            StartSolve = true;
        }
        if (StartSolve)
            Solve();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Scramble();
            Solver = new CubeSolver();
            Solution = Solver.FindSolution(CurrentColors);
            Solved = false;
            CurrentSolutionStep = 0;
            CorrectCount = 0;
        }
        if (!Solved)
            NeuralTraining();
        if (Input.GetKeyDown(KeyCode.RightShift))
            Finished = false;
        if (!Finished)
            MassTraining();
    }

    public static char IndexToColor(int index)
    {
        return index switch
        {
            0 => 'Y',
            1 => 'B',
            2 => 'R',
            3 => 'G',
            4 => 'O',
            5 => 'W',
            _ => throw new NotImplementedException("A cube cannot have more than 6 sides"),
        };
    }

    public static int ColorToIndex(char color)
    {
        return color switch
        {
            'Y' => 0,
            'B' => 1,
            'R' => 2,
            'G' => 3,
            'O' => 4,
            'W' => 5,
            _ => throw new NotImplementedException("A cube does not have a " + color + " side"),
        };
    }

    private static void RunTest(Action test)
    {
        Debug.LogWarning("Running Test");
        test();
        Debug.LogWarning("Ending Test");
    }

    public static void SaveCubeToDoc()
    {
        System.IO.File.AppendAllText(path, "################# Cube #################" + NL);

        for (int i = 2; i > -1; i--)
        {
            for (int j = 0; j < 6; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    System.IO.File.AppendAllText(path, CurrentColors[k, i, j] + " ");
                }
                System.IO.File.AppendAllText(path, " ");
            }
            System.IO.File.AppendAllText(path, NL);
        }

        System.IO.File.AppendAllText(path, "################# Cube #################" + NL);
    }

    public static void SaveStringArrayToDoc(string[,,] array)
    {
        System.IO.File.AppendAllText(path, "############## Test Array ##############" + NL);

        for (int i = 2; i > -1; i--)
        {
            for (int j = 0; j < 6; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    System.IO.File.AppendAllText(path, array[k, i, j] + " ");
                }
                System.IO.File.AppendAllText(path, " ");
            }
            System.IO.File.AppendAllText(path, NL);
        }

        System.IO.File.AppendAllText(path, "############## Test Array ##############" + NL);
    }

    public static void SaveCustomToDoc(string customText, bool newLine, bool barrier)
    {
        System.IO.File.AppendAllText(path, customText);
        if (newLine)
            System.IO.File.AppendAllText(path, NL);
        if (barrier)
            System.IO.File.AppendAllText(path, "########################################" + NL);
    }

    public void SaveWeightsToDoc(float[][][] weights)
    {
        string time = System.DateTime.Now.ToLocalTime().Month + "/" + System.DateTime.Now.ToLocalTime().Day + "/" + System.DateTime.Now.ToLocalTime().Year + " " + System.DateTime.Now.ToLocalTime().Hour + ":" + System.DateTime.Now.ToLocalTime().Minute + ":" + System.DateTime.Now.ToLocalTime().Second;
        System.IO.File.AppendAllText(WeightsSavePath, "####### " + time + " #######" + NL);

        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    System.IO.File.AppendAllText(WeightsSavePath, weights[i][j][k] + NL);
                }
            }
        }

        System.IO.File.AppendAllText(WeightsSavePath, "####### " + time + " #######" + NL);
    }

    private void TopLayer(float direction)
    {
        GameObject[,] temp = new GameObject[3, 3];
        char[,] charTemp = new char[3, 3];
        char[] charSidesTemp = new char[12];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Cubes[j, 2, i].transform.RotateAround(Vector3.up, Vector3.up, direction * 90);
                temp[j, i] = Cubes[j, 2, i];
                charTemp[j, i] = CurrentColors[j, i, 0];
            }
        }
        for (int i = 0; i < 12; i++)
        {
            charSidesTemp[i] = CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 2, Mathf.FloorToInt(i / 3) + 1];
        }

        if (direction == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[i, 2, Mathf.Abs(j - 2)] = temp[j, i];
                    CurrentColors[i, Mathf.Abs(j - 2), 0] = charTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++)
            {
                CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 2, Mathf.FloorToInt(i / 3) + 1] = charSidesTemp[(i + 3) % 12];
            }
        }
        else if (direction == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[Mathf.Abs(i - 2), 2, j] = temp[j, i];
                    CurrentColors[Mathf.Abs(i - 2), j, 0] = charTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--)
            {
                CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 2, Mathf.FloorToInt(i / 3) + 1] = charSidesTemp[(i + 9) % 12];
            }
        }
    }

    private void LeftLayer(float direction)
    {
        GameObject[,] temp = new GameObject[3, 3];
        char[,] charTemp = new char[3, 3];
        char[] charSidesTemp = new char[12];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Cubes[0, i, j].transform.RotateAround(Vector3.back, Vector3.back, direction * 90);
                temp[j, i] = Cubes[0, i, j];
                charTemp[j, i] = CurrentColors[j, i, 1];
            }
        }
        for (int i = 0; i < 12; i++)
        {
            charSidesTemp[i] = CurrentColors[0, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)];
        }

        if (direction == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[0, j, Mathf.Abs(i - 2)] = temp[j, i];
                    CurrentColors[i, Mathf.Abs(j - 2), 1] = charTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--)
            {
                CurrentColors[0, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)] = charSidesTemp[(i + 9) % 12];
            }
        }
        else if (direction == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[0, Mathf.Abs(j - 2), i] = temp[j, i];
                    CurrentColors[Mathf.Abs(i - 2), j, 1] = charTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++)
            {
                CurrentColors[0, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)] = charSidesTemp[(i + 3) % 12];
            }
        }
    }

    private void FrontLayer(float direction)
    {
        GameObject[,] temp = new GameObject[3, 3];
        char[,] charTemp = new char[3, 3];
        char[] charSidesTemp = new char[12];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Cubes[j, i, 0].transform.RotateAround(Vector3.right, Vector3.right, direction * 90);
                temp[j, i] = Cubes[j, i, 0];
                charTemp[j, i] = CurrentColors[j, i, 2];
            }
        }
        for (int i = 0; i < 12; i++)
        {
            charSidesTemp[i] = CurrentColors[Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)) * Mathf.RoundToInt((float)Math.Tanh(((i + 3) % 6) - 1) + Math.Sign((i + 1) * (i - 5.5) * (i - 11.5))), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)(-Mathf.Abs(i - 4) + 4)), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)];
        }

        if (direction == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[i, Mathf.Abs(j - 2), 0] = temp[j, i];
                    CurrentColors[i, Mathf.Abs(j - 2), 2] = charTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--)
            {
                CurrentColors[Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)) * Mathf.RoundToInt((float)Math.Tanh(((i + 3) % 6) - 1) + Math.Sign((i + 1) * (i - 5.5) * (i - 11.5))), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)(-Mathf.Abs(i - 4) + 4)), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)] = charSidesTemp[(i + 9) % 12];
            }
        }
        else if (direction == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[Mathf.Abs(i - 2), j, 0] = temp[j, i];
                    CurrentColors[Mathf.Abs(i - 2), j, 2] = charTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++)
            {
                CurrentColors[Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)) * Mathf.RoundToInt((float)Math.Tanh(((i + 3) % 6) - 1) + Math.Sign((i + 1) * (i - 5.5) * (i - 11.5))), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)(-Mathf.Abs(i - 4) + 4)), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)] = charSidesTemp[(i + 3) % 12];
            }
        }
    }

    private void RightLayer(float direction)
    {
        GameObject[,] temp = new GameObject[3, 3];
        char[,] charTemp = new char[3, 3];
        char[] charSidesTemp = new char[12];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Cubes[2, i, j].transform.RotateAround(Vector3.forward, Vector3.forward, direction * 90);
                temp[j, i] = Cubes[2, i, j];
                charTemp[j, i] = CurrentColors[j, i, 3];
            }
        }
        for (int i = 0; i < 12; i++)
        {
            charSidesTemp[i] = CurrentColors[2, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)];
        }

        if (direction == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[2, Mathf.Abs(j - 2), i] = temp[j, i];
                    CurrentColors[i, Mathf.Abs(j - 2), 3] = charTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++)
            {
                CurrentColors[2, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)] = charSidesTemp[(i + 3) % 12];
            }
        }
        else if (direction == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[2, j, Mathf.Abs(i - 2)] = temp[j, i];
                    CurrentColors[Mathf.Abs(i - 2), j, 3] = charTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--)
            {
                CurrentColors[2, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)] = charSidesTemp[(i + 9) % 12];
            }
        }
    }

    private void BackLayer(float direction)
    {
        GameObject[,] temp = new GameObject[3, 3];
        char[,] charTemp = new char[3, 3];
        char[] charSidesTemp = new char[12];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Cubes[j, i, 2].transform.RotateAround(Vector3.left, Vector3.right, direction * 90);
                temp[j, i] = Cubes[j, i, 2];
                charTemp[j, i] = CurrentColors[j, i, 4];
            }
        }
        for (int i = 0; i < 12; i++)
        {
            charSidesTemp[i] = CurrentColors[Mathf.RoundToInt((float)2.49 * Mathf.Pow((float)Math.E, (-1) * ((float)(i - 7)) / (float)3 * ((float)(i - 7)) / (float)3)), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)-Mathf.Abs(i - 4) + Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) + 3), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)];
        }

        if (direction == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[i, Mathf.Abs(j - 2), 2] = temp[j, i];
                    CurrentColors[i, Mathf.Abs(j - 2), 4] = charTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--)
            {
                CurrentColors[Mathf.RoundToInt((float)2.49 * Mathf.Pow((float)Math.E, (-1) * ((float)(i - 7)) / (float)3 * ((float)(i - 7)) / (float)3)), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)-Mathf.Abs(i - 4) + Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) + 3), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)] = charSidesTemp[(i + 9) % 12];
            }
        }
        else if (direction == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[Mathf.Abs(i - 2), j, 2] = temp[j, i];
                    CurrentColors[Mathf.Abs(i - 2), j, 4] = charTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++)
            {
                CurrentColors[Mathf.RoundToInt((float)2.49 * Mathf.Pow((float)Math.E, (-1) * ((float)(i - 7)) / (float)3 * ((float)(i - 7)) / (float)3)), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)-Mathf.Abs(i - 4) + Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) + 3), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)] = charSidesTemp[(i + 3) % 12];
            }
        }
    }

    private void BottomLayer(float direction)
    {
        GameObject[,] temp = new GameObject[3, 3];
        char[,] charTemp = new char[3, 3];
        char[] charSidesTemp = new char[12];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Cubes[j, 0, i].transform.RotateAround(Vector3.down, Vector3.up, direction * 90);
                temp[j, i] = Cubes[j, 0, i];
                charTemp[j, i] = CurrentColors[j, i, 5];
            }
        }
        for (int i = 0; i < 12; i++)
        {
            charSidesTemp[i] = CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 0, Mathf.FloorToInt(i / 3) + 1];
        }

        if (direction == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[i, 0, Mathf.Abs(j - 2)] = temp[j, i];
                    CurrentColors[i, Mathf.Abs(j - 2), 5] = charTemp[j, i];
                }
            }
            for (int i = 0; i < 12; i++)
            {
                CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 0, Mathf.FloorToInt(i / 3) + 1] = charSidesTemp[(i + 3) % 12];
            }
        }
        else if (direction == -1)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cubes[Mathf.Abs(i - 2), 0, j] = temp[j, i];
                    CurrentColors[Mathf.Abs(i - 2), j, 5] = charTemp[j, i];
                }
            }
            for (int i = 11; i > -1; i--)
            {
                CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 0, Mathf.FloorToInt(i / 3) + 1] = charSidesTemp[(i + 9) % 12];
            }
        }
    }

    private void Scramble()
    {
        for (int i = 0; i < 25; i++)
        {
            int temp = UnityEngine.Random.Range(0, 2) * 2 - 1;
            int side = UnityEngine.Random.Range(1, 7);

            switch (side)
            {
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

    private void Solve()
    {
        if (CurrentSolutionStep == Solution.Length)
        {
            Debug.Log("Solved!");
            Debug.Log(Solution.Length);
            StartSolve = false;
            return;
        }

        switch ((int)Mathf.Abs(Solution[CurrentSolutionStep]))
        {
            case 1:
                TopLayer(Solution[CurrentSolutionStep] / Mathf.Abs(Solution[CurrentSolutionStep]));
                break;
            case 2:
                LeftLayer(Solution[CurrentSolutionStep] / Mathf.Abs(Solution[CurrentSolutionStep]));
                break;
            case 3:
                FrontLayer(Solution[CurrentSolutionStep] / Mathf.Abs(Solution[CurrentSolutionStep]));
                break;
            case 4:
                RightLayer(Solution[CurrentSolutionStep] / Mathf.Abs(Solution[CurrentSolutionStep]));
                break;
            case 5:
                BackLayer(Solution[CurrentSolutionStep] / Mathf.Abs(Solution[CurrentSolutionStep]));
                break;
            case 6:
                BottomLayer(Solution[CurrentSolutionStep] / Mathf.Abs(Solution[CurrentSolutionStep]));
                break;
        }

        CurrentSolutionStep++;
    }

    //################### Both Types Of Networks ###################

    private void AssignInputs()
    {
        float[] temp;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    switch (CurrentColors[k, j, i])
                    {
                        case 'Y':
                            temp = new float[] { 1, 0, 0, 0, 0, 0 };
                            for (int m = 0; m < 6; m++)
                            {
                                CurrentState[(54 * i) + (18 * j) + (6 * k) + m] = temp[m];
                            }
                            break;
                        case 'B':
                            temp = new float[] { 0, 1, 0, 0, 0, 0 };
                            for (int m = 0; m < 6; m++)
                            {
                                CurrentState[(54 * i) + (18 * j) + (6 * k) + m] = temp[m];
                            }
                            break;
                        case 'R':
                            temp = new float[] { 0, 0, 1, 0, 0, 0 };
                            for (int m = 0; m < 6; m++)
                            {
                                CurrentState[(54 * i) + (18 * j) + (6 * k) + m] = temp[m];
                            }
                            break;
                        case 'G':
                            temp = new float[] { 0, 0, 0, 1, 0, 0 };
                            for (int m = 0; m < 6; m++)
                            {
                                CurrentState[(54 * i) + (18 * j) + (6 * k) + m] = temp[m];
                            }
                            break;
                        case 'O':
                            temp = new float[] { 0, 0, 0, 0, 1, 0 };
                            for (int m = 0; m < 6; m++)
                            {
                                CurrentState[(54 * i) + (18 * j) + (6 * k) + m] = temp[m];
                            }
                            break;
                        case 'W':
                            temp = new float[] { 0, 0, 0, 0, 0, 1 };
                            for (int m = 0; m < 6; m++)
                            {
                                CurrentState[(54 * i) + (18 * j) + (6 * k) + m] = temp[m];
                            }
                            break;
                    }
                }
            }
        }
    }

    private float[] CorrectOutput()
    {
        float[] temp = new float[12] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        int offset = 0;

        if (Solution[CorrectCount] / Mathf.Abs(Solution[CorrectCount]) == -1)
            offset = 6;

        temp[(int)Mathf.Abs(Solution[CorrectCount]) - 1 + offset] = 1;

        return temp;
    }

    private void NeuralOutput()
    {
        float direction = NeuralChoice / Mathf.Abs(NeuralChoice);

        //Rotate face side of cube
        switch ((int)Mathf.Abs(NeuralChoice))
        {
            case 1:
                TopLayer(direction);
                break;
            case 2:
                LeftLayer(direction);
                break;
            case 3:
                RightLayer(direction);
                break;
            case 4:
                FrontLayer(direction);
                break;
            case 5:
                BackLayer(direction);
                break;
            case 6:
                BottomLayer(direction);
                break;
        }
    }

    //################### Modular Neural Network ###################

    private void ModularTraining()
    {

    }

    private void GetModularChoice()
    {
        CurrentNeuralOutputs = new float[12];
        CurrentNeuralOutputs = ModNet.FeedForward(CurrentState);
        float biggest = CurrentNeuralOutputs[0];
        float direction;
        int side = 0;

        for (int i = 1; i < CurrentNeuralOutputs.Length; i++)
        {
            if (biggest < CurrentNeuralOutputs[i])
            {
                biggest = CurrentNeuralOutputs[i];
                side = i;
            }
        }

        for (int i = 0; i < CurrentNeuralOutputs.Length; i++)
        {
            if (i == side)
                CurrentNeuralOutputs[i] = 1;
            else
                CurrentNeuralOutputs[i] = Mathf.Round(CurrentNeuralOutputs[i]);
        }

        if (side <= 5)
            direction = 1;
        else
            direction = -1;

        side = (side % 6) + 1;

        NeuralChoice = side * direction;
    }

    private void ModularBackPropCalculations()
    {
        ExpectedOutput = CorrectOutput();
        ModNet.BackProp(ExpectedOutput);
    }

    //####################### Neural Network #######################

    private void MassTraining()
    {
        if (CurrentItteration == MaxItterations + 1)
        {
            Debug.Log("Done");
            Debug.Log("Average: " + Average * 100 + "%   Best: " + Best * 100 + "%");
            Finished = true;
            return;
        }

        if (NeuralStart)
            NeuralTraining(true);
        else
        {
            //Debug.Log("Run#" + CurrentItteration + " had " + CorrectCount + " out of " + 100 + " correct   " + "Average: " + Average + "%   Best: " + Best + "%");
            Scramble();
            Solver = new CubeSolver();
            Solution = Solver.FindSolution(CurrentColors);
            NeuralStart = true;
            CurrentSolutionStep = 0;
            CorrectCount = 0;
            CurrentItteration++;
        }
    }

    private void NeuralTraining(bool useless)
    {
        if (CurrentSolutionStep == Solution.Length)
        {
            Debug.Log(CorrectCount + " out of " + CurrentSolutionStep + " correct");
            NeuralStart = false;
            return;
        }

        AssignInputs();
        GetNeuralChoice();
        NeuralOutput();
        NeuralBackPropCalculations();

        if (NeuralChoice == Solution[CurrentSolutionStep])
            CorrectCount++;

        CurrentSolutionStep++;
    }

    private void NeuralTraining()
    {
        for (int i = 0; i < 6; i++)
        {
            char color = IndexToColor(i);
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (CurrentColors[k, j, i] != color)
                        goto Skip;
                }
            }
        }
        Debug.Log("Solved! Took " + CurrentSolutionStep + " steps to get " + CorrectCount + " correct");
        Solved = true;
        return;
        Skip:

        AssignInputs();
        GetNeuralChoice();
        NeuralOutput();

        Debug.Log("Neural Choice: " + NeuralChoice + "   Expected: " + Solution[CorrectCount]);
        if (NeuralChoice == Solution[CorrectCount])
        {
            CorrectCount++;
        }

        NeuralBackPropCalculations();

        string temp = "";
        for (int i = 0; i < 12; i++)
        {
            temp += CurrentNeuralOutputs[i] + " ";
        }
        Debug.Log("Neural Outputs: " + temp);
        temp = "";
        for (int i = 0; i < 12; i++)
        {
            temp += ExpectedOutput[i] + " ";
        }
        Debug.Log("Expected Output: " + temp);

        CurrentSolutionStep++;
    }

    private void GetNeuralChoice()
    {
        CurrentNeuralOutputs = new float[12];
        CurrentNeuralOutputs = Network.FeedForward(CurrentState);
        float biggest = CurrentNeuralOutputs[0];
        float direction;
        int side = 0;

        for (int i = 1; i < CurrentNeuralOutputs.Length; i++)
        {
            if (biggest < CurrentNeuralOutputs[i])
            {
                biggest = CurrentNeuralOutputs[i];
                side = i;
            }
        }

        for (int i = 0; i < CurrentNeuralOutputs.Length; i++)
        {
            if (i == side)
                CurrentNeuralOutputs[i] = 2;
            else
                CurrentNeuralOutputs[i] = Mathf.Round(CurrentNeuralOutputs[i]);
        }

        if (side <= 5)
            direction = 1;
        else
            direction = -1;

        side = (side % 6) + 1;

        NeuralChoice =  side * direction;
    }

    private void NeuralBackPropCalculations()
    {
        ExpectedOutput = CorrectOutput();
        Network.BackProp(ExpectedOutput);
    }
}