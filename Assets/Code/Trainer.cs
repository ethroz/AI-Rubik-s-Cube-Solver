using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour {
    // Cube variables
    private CubeScript Cube;

    // User Control
    private bool IsTraining = false;

    // Path variables
    private readonly string WeightsPath = Application.dataPath + @"\Saves\Weights.txt";

    // Neural Network
    // public bool UseSavedWeights = true;
    private NeuralNetwork Network;
    private StateTreeNode Root;
    private static readonly int InputSize = 3 * 3 * 6 * 6;
    private static readonly int OutputSize = 6 * 2;
    public int[] NeuralNetHiddenLayers = new int[] { 100, 100 };
    public float LearningRate = 0.01f;
    public int MaxIterations = 100;
    public int Lookahead = 1;
    public float DiscountRate = 0.99f;
    private int CurrentIteration = 1;

    void Start() {
        Cube = GameObject.FindWithTag("Cube").GetComponent<CubeScript>();
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


        if (Input.GetKeyDown(KeyCode.Return)) {
            CreateNet();
            CreatePredictionTree();
            IsTraining = true;
            return;
        }

        var clockwise = !Input.GetKey(KeyCode.LeftShift);
        if (Input.GetKeyDown(KeyCode.Q))
            Cube.RotateBottom(clockwise);
        if (Input.GetKeyDown(KeyCode.W))
            Cube.RotateBack(clockwise);
        if (Input.GetKeyDown(KeyCode.E))
            Cube.RotateTop(clockwise);
        if (Input.GetKeyDown(KeyCode.A))
            Cube.RotateLeft(clockwise);
        if (Input.GetKeyDown(KeyCode.S))
            Cube.RotateFront(clockwise);
        if (Input.GetKeyDown(KeyCode.D))
            Cube.RotateRight(clockwise);
        if (Input.GetKeyDown(KeyCode.X))
            Cube.Scramble();
    }

    private void CreateNet() {
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

    private void CreatePredictionTree() {
        var parameters = new RewardParameters(DiscountRate);
        Root = new StateTreeNode(Cube.GetCube(), null, parameters);
        Root.CreateChildren(Lookahead);
    }

    private void TrainingStep() {
        if (CurrentIteration > MaxIterations) {
            IsTraining = false;
            return;
        }

        var state = Cube.GetCube().GetState();
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
                Cube.RotateTop(clockwise);
                break;
            case 2:
                Cube.RotateLeft(clockwise);
                break;
            case 3:
                Cube.RotateFront(clockwise);
                break;
            case 4:
                Cube.RotateRight(clockwise);
                break;
            case 5:
                Cube.RotateBack(clockwise);
                break;
            case 6:
                Cube.RotateBottom(clockwise);
                break;
        }
    }

    private int CalculateExpectedAction() {
        // TODO
        return 0;
    }
}