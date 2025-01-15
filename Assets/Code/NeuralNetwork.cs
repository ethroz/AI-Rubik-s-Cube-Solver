#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

public class NeuralNetwork : IComparable<NeuralNetwork> {
    private readonly Layer[] Layers;
    public float Fitness;

    /// <summary>
    /// Initializes a new instance of the <see cref="NeuralNetwork"/> class.
    /// </summary>
    /// <param name="layers">The layers of the neural network.</param>
    /// <param name="activationTypes">The activation types for each layer.</param>
    public NeuralNetwork(List<int> layers, List<ActivationType> activationTypes, float learningRate = 0.01f) {
        if (activationTypes.Count >= layers.Count)
            throw new ArgumentException("Number of activation types must be less than the number of layers.");

        for (int i = 1; i < layers.Count - activationTypes.Count; ++i) {
            activationTypes.Insert(0, activationTypes[0]);
        }

        Layers = new Layer[layers.Count - 1];
        for (int i = 0; i < Layers.Length; ++i) {
            Layers[i] = new Layer(layers[i], layers[i + 1], activationTypes[i], learningRate);
        }

        Fitness = 0.0f;
    }

    /// <summary>
    /// Copy a neural networks from a parent network.
    /// </summary>
    /// <param name="parent">The parent.</param>
    public NeuralNetwork(NeuralNetwork parent) {
        // Copy everything.
        Layers = new Layer[parent.Layers.Length];
        for (int i = 0; i < Layers.Length; ++i) {
            Layers[i] = new Layer(parent.Layers[i]);
        }

        // Reset the fitness.
        Fitness = 0.0f;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NeuralNetwork"/> class.
    /// </summary>
    /// <param name="weights">The weights to use for the neural network.</param>
    /// <param name="activationTypes">The activation types for each layer.</param>
    public NeuralNetwork(float[][,] weights, ActivationType[] activationTypes, float learningRate) {
        // Create each layer of the network.
        Layers = new Layer[weights.Length];
        for (int i = 0; i < Layers.Length; ++i) {
            Layers[i] = new Layer(weights[i].GetLength(1), weights[i].GetLength(0), activationTypes[i], learningRate);
            Array.Copy(weights[i], Layers[i].Weights, weights[i].Length);
        }

        Fitness = 0.0f;
    }

    /// <summary>
    /// Feeds forward the inputs through all the layers of the neural network
    /// and returns the output.
    /// </summary>
    /// <param name="inputs">The inputs to the neural network.</param>
    public float[] FeedForward(float[] inputs) {
        for (int i = 0; i < Layers.Length; ++i) {
            inputs = Layers[i].FeedForward(inputs);
        }
        return inputs;
    }

    /// <summary>
    /// Mutates the weights of the neural network.
    /// </summary>
    public void Mutate() {
        for (int i = 0; i < Layers.Length; i++) {
            Layers[i].Mutate();
        }
    }

    public void BackProp(float[] expected) {
        Layers[^1].BackPropOutput(expected);

        for (int i = Layers.Length - 2; i >= 0; --i) {
            Layers[i].BackPropHidden(Layers[i + 1].Gamma, Layers[i + 1].Weights);
        }

        for (int i = 0; i < Layers.Length; ++i) {
            Layers[i].UpdateWeights();
        }
    }

    public void BatchTrain(float[][] inputs, float[][] expected) {
        for (int j = 0; j < inputs.Length; ++j) {
            var prevOutputs = inputs[j];
            for (int i = 0; i < Layers.Length; ++i) {
                prevOutputs = Layers[i].FeedForward(prevOutputs);
            }

            Layers[^1].BackPropOutput(expected[j]);

            for (int i = Layers.Length - 2; i >= 0; --i) {
                Layers[i].BackPropHidden(Layers[i + 1].Gamma, Layers[i + 1].Weights);
            }
        }

        for (int i = 0; i < Layers.Length; ++i) {
            Layers[i].UpdateWeights();
        }
    }

    public static NeuralNetwork LoadFromFile(string path) {
        var lines = File.ReadAllLines(path);
        List<int> layers = new();
        for (int i = 0; i < lines[0].Length; ++i) {
            // Is this a digit?
            if (char.IsDigit(lines[0][i])) {
                // Get the number of digits in the number.
                int j = i + 1;
                for (; j < lines[0].Length; ++j) {
                    // Is this not a digit?
                    if (!char.IsDigit(lines[0][j])) {
                        break;
                    }
                }

                // Parse the number.
                var num = int.Parse(lines[0].Substring(i, j - i));
                layers.Add(num);
                i = j;
            }
        }

        // Setup the weight array.
        float[][,] weights = new float[layers.Count - 1][,];
        float[][] biases = new float[layers.Count - 1][];
        for (int i = 0; i < weights.Length; ++i) {
            weights[i] = new float[layers[i + 1], layers[i]];
            biases[i] = new float[layers[i + 1]];
        }

        // Get the weights and biases.
        int w = 0, y = 0;
        for (int i = 1; i < lines.Length; ++i) {
            // # lines indicate the start of a new weight matrix.
            if (lines[i] == "#") {
                Trace.Assert(y == weights[w].GetLength(0));
                y = 0;
                ++w;
                continue;
            }

            // Go through all the characters in each line.
            int x = 0;
            for (int j = 0; j < lines[i].Length; ++j) {
                // Is this the start of a weight?
                if (char.IsDigit(lines[i][j]) || lines[i][j] == '-') {
                    int k = j + 1;
                    for (; k < lines[i].Length; ++k) {
                        // Is this the end of a weight?
                        if (lines[i][k] == ' ') {
                            break;
                        }
                    }

                    // Parse the number.
                    var subst = lines[i].Substring(j, k - j);
                    var weight = float.Parse(subst);
                    if (x < weights[w].GetLength(1)) {
                        weights[w][y, x] = weight;
                        ++x;
                    }
                    else {
                        biases[w][y] = weight;
                    }
                    j = k;
                }
            }
            Trace.Assert(x == weights[w].GetLength(1) + 1);
            ++y;
        }

        // Get the activation types.
        ActivationType[] activationTypes = new ActivationType[layers.Count - 1];
        for (int i = 0; i < activationTypes.Length; ++i) {
            activationTypes[i] = (ActivationType)Enum.Parse(typeof(ActivationType), lines[lines.Length - activationTypes.Length + i]);
        }

        // Get the learning rate.
        float learningRate = float.Parse(lines[^1]);

        return new NeuralNetwork(weights, activationTypes, learningRate);
    }

    public void SaveToFile(string path) {
        // Specify the layer sizes.
        StringBuilder sb = new("{");
        for (int i = 0; i < Layers.Length; i++) {
            sb.Append(Layers[i].numberOfInputs);
            sb.Append(',');
        }
        sb.Append(Layers[^1].numberOfOutputs);
        sb.Append('}');
        sb.AppendLine();

        // Get the weights and biases.
        for (int i = 0; i < Layers.Length; i++) {
            for (int j = 0; j < Layers[i].numberOfOutputs; j++) {
                for (int k = 0; k < Layers[i].numberOfInputs; k++) {
                    sb.Append(Layers[i].Weights[j, k]);
                    sb.Append(' ');
                }
                sb.Append(Layers[i].Biases[j]);
                sb.AppendLine();
            }
            sb.Append('#');
            sb.AppendLine();
        }

        // Write the activation types.
        for (int i = 0; i < Layers.Length; i++) {
            sb.AppendLine(Layers[i].ActivationType.ToString());
        }

        // Write the learning rate.
        sb.AppendLine(Layers[0].LearningRate.ToString());

        // Write the weights to the file at the given path.
        using StreamWriter file = new(path);
        file.Write(sb.ToString());
    }

    /// <summary>
    /// Compares the fitness of two neural networks.
    /// </summary>
    /// <param name="other">The other neural network.</param>
    /// <returns>An integer indicating the comparison result.</returns>
    public int CompareTo(NeuralNetwork? other) {
        if (other == null)
            throw new NullReferenceException();

        if (Fitness > other.Fitness)
            return 1;
        else if (Fitness < other.Fitness)
            return -1;
        else
            return 0;
    }
}
