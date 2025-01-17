using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trainer : MonoBehaviour {
    // Cube variables
    private CubeScript Cube;

    // Configurable
    public List<int> HiddenLayers = new() { 100, 100 };
    public List<ActivationType> Activations = new() { ActivationType.RELU, ActivationType.SOFTMAX };
    public float LearningRate = 0.01f;
    public int DataSize = 1000;
    [Range(0.0f, 1.0f)]
    public float TestSplit = 0.2f;
    public int BatchSize = 10;
    public int Iterations = 1;
    public int ScrambleMoves = 3;
    // public bool UseSavedWeights = true;
    
    // Neural Net stuff
    private readonly string WeightsPath = Application.dataPath + @"\Saves\Weights.txt";
    private NeuralNetwork Network;
    private static readonly int InputSize = 3 * 3 * 6 * 6;
    private static readonly int OutputSize = 6 * 2;
    
    private bool IsSolving = false;
    private Task TrainTask;
    private bool IsTraining { get {
        return TrainTask.Status == TaskStatus.Running;
    }}

    void Start() {
        Cube = GameObject.FindWithTag("Cube").GetComponent<CubeScript>();
    }

    void Update() {
        UserInput();
        if (IsSolving) {
            InferenceStep();
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

        if (IsSolving || IsTraining) {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Network == null) {
                Debug.LogError("Cannot perform model inference without a model");
            }
            else {
                IsSolving = true;
            }
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartTraining();
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
        List<int> layers = new() {
            InputSize
        };
        layers.AddRange(HiddenLayers);
        layers.Add(OutputSize);
        Network = new(layers, Activations, LearningRate);
    }

    private void StartTraining() {
        if (Network == null) {
            CreateNet();
        }
        TrainTask = Task.Run(Train);
    }

    private void Train() {
        // Copy the inputs.
        var dataSize = DataSize;
        var testSplit = TestSplit;
        var batchSize = BatchSize;
        var iterations = Iterations;
        var scrambleMoves = ScrambleMoves;

        Debug.Log("Generating data");
        var totalSize = dataSize * scrambleMoves;
        var inputs = new float[totalSize][];
        var outputs = new float[totalSize][];
        for (int i = 0; i < dataSize; ++i) {
            var moves = VirtualCube.GenerateScramble(scrambleMoves);
            var cube = new VirtualCube();
            for (int j = 0; j < scrambleMoves; ++j) {
                cube.Rotate(moves[j]);
                var input = CubeStateToInput(cube.GetState());
                inputs[i * scrambleMoves + j] = input;
                var output = MoveToOutput(moves[i]);
                outputs[i * scrambleMoves + j] = output;
            }
        }

        Debug.Log("Splitting the data");
        var testSize = (int)(totalSize * testSplit);
        var trainSize = totalSize - testSize;
        var trainInputs = new ReadOnlySpan<float[]>(inputs, 0, trainSize);
        var trainOutputs = new ReadOnlySpan<float[]>(outputs, 0, trainSize);
        var testInputs = new ReadOnlySpan<float[]>(inputs, trainSize, testSize);
        var testOutputs = new ReadOnlySpan<float[]>(outputs, trainSize, testSize);

        Debug.Log("Starting Training");
        var numBatches = trainSize / batchSize;
        for (int i = 0; i < iterations; ++i) {
            int index = 0;
            for (int j = 0; j < numBatches; ++j) {
                var inputBatch = trainInputs.Slice(index, batchSize);
                var outputBatch = trainOutputs.Slice(index, batchSize);
                index += batchSize;
                Network.BatchTrain(inputBatch, outputBatch);
            }
        }

        Debug.Log("Starting Tests");
        int numCorrect = 0;
        for (int i = 0; i < testSize; ++i) {
            var predictedOutputs = Network.FeedForward(testInputs[i]);
            var predictedOutput = Functions.ArgMax(predictedOutputs);
            var expectedOutput = Functions.ArgMax(testOutputs[i]);
            if (predictedOutput == expectedOutput) {
                ++numCorrect;
            }
        }
        var score = (float)numCorrect / testSize;
        Debug.Log($"Test score: {score}");
    }

    private void InferenceStep() {
        var state = Cube.GetState();
        var inputs = CubeStateToInput(state);
        var outputs = Network.FeedForward(inputs);
        var action = Functions.ArgMax(outputs);
        var move = new CubeMove(action);
        Cube.Rotate(move);
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

    private float[] MoveToOutput(CubeMove move) {
        var opposite = move.Opposite();
        var action = opposite.ToInt();
        var output = Functions.InverseArgMax(action, OutputSize);
        return output;
    }
}