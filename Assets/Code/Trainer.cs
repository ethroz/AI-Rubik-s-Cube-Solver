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
    public int[] NeuralNetHiddenLayers = new int[] { 100, 100 };
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
            // IsTraining = true;
            // Debug.Log("Training started");
            // return;
            Debug.Log(Cube.GetStateString());
        }

        var clockwise = !Input.GetKey(KeyCode.LeftShift);
        if (Input.GetKeyDown(KeyCode.Q))
            Cube.BottomLayer(clockwise);
        if (Input.GetKeyDown(KeyCode.W))
            Cube.BackLayer(clockwise);
        if (Input.GetKeyDown(KeyCode.E))
            Cube.TopLayer(clockwise);
        if (Input.GetKeyDown(KeyCode.A))
            Cube.LeftLayer(clockwise);
        if (Input.GetKeyDown(KeyCode.S))
            Cube.FrontLayer(clockwise);
        if (Input.GetKeyDown(KeyCode.D))
            Cube.RightLayer(clockwise);
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
        var clockwise = action % 2 == 1;
        var face = action / 2 + 1;
        switch (face) {
            case 1:
                Cube.TopLayer(clockwise);
                break;
            case 2:
                Cube.LeftLayer(clockwise);
                break;
            case 3:
                Cube.FrontLayer(clockwise);
                break;
            case 4:
                Cube.RightLayer(clockwise);
                break;
            case 5:
                Cube.BackLayer(clockwise);
                break;
            case 6:
                Cube.BottomLayer(clockwise);
                break;
        }
    }

    private int CalculateExpectedAction() {
        // TODO
        return 0;
    }
}