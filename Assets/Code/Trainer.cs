using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Trainer : MonoBehaviour {
    // Cube variables
    private CubeScript Cube;

    // User Control
    private bool IsTraining = false;

    // Path variables
    private string WeightsPath;

    // Neural Network
    // public bool UseSavedWeights = true;
    private NeuralNetwork Network;
    private static readonly int InputSize = 3 * 3 * 6 * 6;
    public int[] NeuralNetHiddenLayers = new int[0];
    private static readonly int OutputSize = 6 * 2;
    public float LearningRate = 0.01f;
    public int MaxIterations = 500;
    private int CurrentIteration = 1;

    void Start() {
        Cube = GameObject.FindWithTag("Cube").GetComponent<CubeScript>();

        WeightsPath = Application.dataPath + @"\Saves\Weights.txt";

        List<int> layers = new()
        {
            InputSize
        };
        for (int i = 0; i < NeuralNetHiddenLayers.Length; i++) {
            layers.Add(NeuralNetHiddenLayers[i]);
        }
        layers.Add(OutputSize);
        var activationTypes = new List<ActivationType>() { 
            ActivationType.RELU,
            ActivationType.SIGMOID
        };
        Network = new(layers, activationTypes, LearningRate);
    }

    void Update() {
        if (!IsTraining) {
            UserInput();
        } else {
            TrainingStep();
        }
    }

    private void UserInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // var weights = Network.GetWeights();
            // SaveWeights(weights, Network.Type);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }


        if (Input.GetKeyDown(KeyCode.Space)) {
            IsTraining = true;
            Debug.Log("Training started");
            return;
        }

        var userDirection = Input.GetKey(KeyCode.LeftShift) ? -1 : 1;
        if (Input.GetKeyDown(KeyCode.Q))
            Cube.BottomLayer(userDirection);
        if (Input.GetKeyDown(KeyCode.W))
            Cube.BackLayer(userDirection);
        if (Input.GetKeyDown(KeyCode.E))
            Cube.TopLayer(userDirection);
        if (Input.GetKeyDown(KeyCode.A))
            Cube.LeftLayer(userDirection);
        if (Input.GetKeyDown(KeyCode.S))
            Cube.FrontLayer(userDirection);
        if (Input.GetKeyDown(KeyCode.D))
            Cube.RightLayer(userDirection);
        if (Input.GetKeyDown(KeyCode.X))
            Cube.Scramble();
    }

    private void TrainingStep() {
        if (CurrentIteration > MaxIterations) {
            IsTraining = false;
            return;
        }

        var state = Cube.GetState();
        var input = CubeStateToInput(state);
        var output = Network.FeedForward(input);
        var action = Functions.ArgMax(output);
        PerformAction(action);

        var expectedAction = CalculateExpectedAction();
        var expectedOutput = Functions.InverseArgMax(expectedAction, OutputSize);
        Network.BackProp(expectedOutput);

        CurrentIteration++;
    }

    private float[] CubeStateToInput(Color[,,] colors) {
        var input = new float[InputSize];
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 3; j++) {
                for (int k = 0; k < 3; k++) {
                    var index = (i * 3 * 3 + j * 3 + k) * 6;
                    var color = colors[i, j, k];
                    for (int l = 0; l < 6; l++) {
                        input[index + l] = l == (int)color ? 1 : 0;
                    }
                }
            }
        }
        return input;
    }

    private void PerformAction(int action) {
        switch (action) {
            case 0:
                Cube.BottomLayer(1);
                break;
            case 1:
                Cube.BottomLayer(-1);
                break;
            case 2:
                Cube.BackLayer(1);
                break;
            case 3:
                Cube.BackLayer(-1);
                break;
            case 4:
                Cube.TopLayer(1);
                break;
            case 5:
                Cube.TopLayer(-1);
                break;
            case 6:
                Cube.LeftLayer(1);
                break;
            case 7:
                Cube.LeftLayer(-1);
                break;
            case 8:
                Cube.FrontLayer(1);
                break;
            case 9:
                Cube.FrontLayer(-1);
                break;
            case 10:
                Cube.RightLayer(1);
                break;
            case 11:
                Cube.RightLayer(-1);
                break;
        }
    }

    private int CalculateExpectedAction() {
        // TODO
        return 0;
    }

    // File I/O

    public (float[][,], ActivationType) LoadWeights() {
        var lines = File.ReadAllLines(WeightsPath);

        var type = (ActivationType)System.Enum.Parse(typeof(ActivationType), lines[1]);
        var layers = lines[2].Split(' ');
        var layerSizes = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++) {
            layerSizes[i] = int.Parse(layers[i]);
        }
        
        var weights = new float[layers.Length - 1][,];
        int line = 4;
        for (int i = 0; i < weights.Length; i++) {
            var width = layerSizes[i];
            var height = layerSizes[i + 1];
            weights[i] = new float[width, height];
            for (int j = 0; j < width; j++) {
                var values = lines[line].Split(' ');
                for (int k = 0; k < height; k++) {
                    weights[i][j, k] = float.Parse(values[k]);
                }
                line++;
            }
            line++;
        }

        return (weights, type);
    }

    public void SaveWeights(float[][,] weights, ActivationType type) {
        StringBuilder buffer = new();
        buffer.Append("####### ");
        var time = System.DateTime.Now.ToLocalTime();
        buffer.Append(time);
        buffer.AppendLine(" #######");

        buffer.AppendLine(type.ToString());

        buffer.Append(weights[0].GetLength(0));
        buffer.Append(' ');
        for (int i = 0; i < weights.Length; i++) {
            buffer.Append(weights[i].GetLength(1));
            buffer.Append(' ');
        }
        buffer.AppendLine();
        buffer.AppendLine();

        for (int i = 0; i < weights.Length; i++) {
            for (int j = 0; j < weights[i].GetLength(0); j++) {
                for (int k = 0; k < weights[i].GetLength(1); k++) {
                    buffer.Append(weights[i][j,k]);
                    buffer.Append(' ');
                }
                buffer.AppendLine();
            }
            buffer.AppendLine();
        }

        File.WriteAllText(WeightsPath, buffer.ToString());
    }
}