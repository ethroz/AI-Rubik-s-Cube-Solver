using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// A single layer of the neural network.
/// </summary>
public class Layer {
    public int numberOfInputs;
    public int numberOfOutputs;
    public float[] Outputs;
    public float[] Inputs;
    public float[,] Weights;
    public float[,] WeightsDelta;
    public float[] Gamma;
    public float[] Error;
    public float[] Biases;
    public float[] BiasesDelta;
    public float LearningRate;
    public ActivationType ActivationType;

    private static readonly Random random = new();
    private readonly Func<float[], float[]> activationFunction;
    private readonly Func<float[], float[]> activationDerivativeFunction;

    /// <summary>
    /// Initializes a new instance of the <see cref="Layer"/> class.
    /// </summary>
    public Layer(int numberOfInputs, int numberOfOutputs, ActivationType activationType, float learningRate) {
        this.numberOfInputs = numberOfInputs;
        this.numberOfOutputs = numberOfOutputs;

        Inputs = new float[numberOfInputs];
        Outputs = new float[numberOfOutputs];
        Weights = new float[numberOfOutputs, numberOfInputs];
        WeightsDelta = new float[numberOfOutputs, numberOfInputs];
        Gamma = new float[numberOfOutputs];
        Error = new float[numberOfOutputs];
        Biases = new float[numberOfOutputs];
        BiasesDelta = new float[numberOfOutputs];
        LearningRate = learningRate;

        ActivationType = activationType;
        activationFunction = Functions.GetActivation(activationType);
        activationDerivativeFunction = Functions.GetActivationDerivative(activationType);

        InitializeWeights();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Layer"/> class.
    /// </summary>
    private Layer(float[,] weights, float[] biases, ActivationType activationType, float learningRate) {
        numberOfInputs = weights.GetLength(1);
        numberOfOutputs = weights.GetLength(0);

        Inputs = new float[numberOfInputs];
        Outputs = new float[numberOfOutputs];
        Weights = weights;
        WeightsDelta = new float[numberOfOutputs, numberOfInputs];
        Gamma = new float[numberOfOutputs];
        Error = new float[numberOfOutputs];
        Biases = biases;
        BiasesDelta = new float[numberOfOutputs];
        LearningRate = learningRate;

        ActivationType = activationType;
        activationFunction = Functions.GetActivation(activationType);
        activationDerivativeFunction = Functions.GetActivationDerivative(activationType);
    }

    /// <summary>
    /// Copy a layer from a parent layer.
    /// </summary>
    /// <param name="parent">The parent.</param>
    public Layer(Layer parent) {
        // Copy everything.
        numberOfInputs = parent.numberOfInputs;
        numberOfOutputs = parent.numberOfOutputs;

        Inputs = new float[numberOfInputs];
        Outputs = new float[numberOfOutputs];
        Weights = new float[numberOfOutputs, numberOfInputs];
        WeightsDelta = new float[numberOfOutputs, numberOfInputs];
        Gamma = new float[numberOfOutputs];
        Error = new float[numberOfOutputs];
        Biases = new float[numberOfOutputs];
        BiasesDelta = new float[numberOfOutputs];

        // Copy the weights.
        Array.Copy(parent.Weights, Weights, Weights.Length);
        Array.Copy(parent.Biases, Biases, Biases.Length);

        ActivationType = parent.ActivationType;
        activationFunction = parent.activationFunction;
        activationDerivativeFunction = parent.activationDerivativeFunction;
    }

    /// <summary>
    /// Initializes the weights for this layer.
    /// </summary>
    public void InitializeWeights() {
        for (int i = 0; i < numberOfOutputs; ++i) {
            for (int j = 0; j < numberOfInputs; ++j) {
                Weights[i, j] = (float)random.NextDouble() - 0.5f;
            }
            Biases[i] = (float)random.NextDouble() - 0.5f;
        }
    }

    public void SaveToFile(StreamWriter stream) {
        stream.WriteLine("##");
        stream.WriteLine(ActivationType);
        stream.WriteLine("#");
        stream.WriteLine(LearningRate.ToString("G9"));
        stream.WriteLine("#");
        for (int i = 0; i < numberOfOutputs; ++i) {
            for (int j = 0; j < numberOfInputs; ++j) {
                stream.Write(Weights[i, j].ToString("G9"));
                stream.Write(' ');
            }
            stream.WriteLine();
        }
        stream.WriteLine("#");
        for (int i = 0; i < numberOfOutputs; ++i) {
            stream.Write(Biases[i].ToString("G9"));
            stream.Write(' ');
        }
        stream.WriteLine();
    }

    public static Layer LoadFromFile(StreamReader stream) {
        // Read and validate the header
        if (stream.ReadLine() != "##") {
            return null;
        }

        // Read the activation type
        ActivationType activationType = (ActivationType)Enum.Parse(typeof(ActivationType), stream.ReadLine());

        // Read and validate the separator
        if (stream.ReadLine() != "#") {
            throw new InvalidDataException("Invalid file format");
        }

        // Read the learning rate
        float learningRate = float.Parse(stream.ReadLine());

        // Read and validate the separator
        if (stream.ReadLine() != "#") {
            throw new InvalidDataException("Invalid file format");
        }

        // Read the weights
        var weights = new List<float[]>();
        string line;
        while ((line = stream.ReadLine()) != "#") {
            var weightRow = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(float.Parse).ToArray();
            weights.Add(weightRow);
        }

        // Convert weights to 2D array
        int numberOfOutputs = weights.Count;
        int numberOfInputs = weights[0].Length;
        float[,] weightsArray = new float[numberOfOutputs, numberOfInputs];
        for (int i = 0; i < numberOfOutputs; i++) {
            for (int j = 0; j < numberOfInputs; j++) {
                weightsArray[i, j] = weights[i][j];
            }
        }

        // Read the biases
        var biases = stream.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(float.Parse).ToArray();

        // Create and return the layer
        return new Layer(weightsArray, biases, activationType, learningRate);
    }

    /// <summary>
    /// Feeds forward the inputs through the layer.
    /// </summary>
    public float[] FeedForward(float[] inputs) {
        Inputs = inputs;

        for (int i = 0; i < numberOfOutputs; ++i) {
            Outputs[i] = Biases[i];

            for (int j = 0; j < numberOfInputs; ++j) {
                Outputs[i] += inputs[j] * Weights[i, j];
            }
        }

        Outputs = activationFunction(Outputs);

        return Outputs;
    }

    /// <summary>
    /// Mutates the weights for this layer.
    /// </summary>
    public void Mutate() {
        for (int i = 0; i < numberOfOutputs; i++) {
            for (int j = 0; j < numberOfInputs; j++) {
                float weight = Weights[i, j];

                // Chances of weight mutation.
                float randomNumber = (float)random.NextDouble() * 1000.0f;
                if (randomNumber <= 2.0f) {
                    // Flip sign of weight.
                    weight *= -1f;
                }
                else if (randomNumber <= 4.0f) {
                    // Pick a random weight between -1 and 1.
                    weight = ((float)random.NextDouble() - 0.5f) * 2.0f;
                }
                else if (randomNumber <= 6.0f) {
                    // Randomly increase current weight.
                    float factor = (float)random.NextDouble() + 1.0f;
                    weight *= factor;
                }
                else if (randomNumber <= 8.0f) {
                    // Randomly decrease current weight.
                    float factor = (float)random.NextDouble();
                    weight *= factor;
                }

                Weights[i, j] = weight;
            }
            float bias = Biases[i];

            // Chances of bias mutation.
            float randomBiasNumber = (float)random.NextDouble() * 1000.0f;
            if (randomBiasNumber <= 2.0f) {
                // Flip sign of bias.
                bias *= -1f;
            }
            else if (randomBiasNumber <= 4.0f) {
                // Pick a random bias between -1 and 1.
                bias = ((float)random.NextDouble() - 0.5f) * 2.0f;
            }
            else if (randomBiasNumber <= 6.0f) {
                // Randomly increase current bias.
                float factor = (float)random.NextDouble() + 1.0f;
                bias *= factor;
            }
            else if (randomBiasNumber <= 8.0f) {
                // Randomly decrease current bias.
                float factor = (float)random.NextDouble();
                bias *= factor;
            }

            Biases[i] = bias;
        }
    }

    /// <summary>
    /// Performs a back propagation on the output layer.
    /// </summary>
    public void BackPropOutput(float[] expected) {
        for (int i = 0; i < numberOfOutputs; ++i)
            Error[i] = Outputs[i] - expected[i];

        var derivatives = activationDerivativeFunction(Outputs);
        for (int i = 0; i < numberOfOutputs; ++i)
            Gamma[i] = Error[i] * derivatives[i];

        for (int i = 0; i < numberOfOutputs; ++i) {
            for (int j = 0; j < numberOfInputs; ++j) {
                WeightsDelta[i, j] += Gamma[i] * Inputs[j];
            }
            BiasesDelta[i] += Gamma[i];
        }
    }

    /// <summary>
    /// Performs a back propagation on a hidden layer.
    /// </summary>
    public void BackPropHidden(float[] gammaForward, float[,] weightsForward) {
        var derivatives = activationDerivativeFunction(Outputs);
        for (int i = 0; i < numberOfOutputs; ++i) {
            Gamma[i] = 0;

            for (int j = 0; j < gammaForward.Length; ++j) {
                Gamma[i] += gammaForward[j] * weightsForward[j, i];
            }

            Gamma[i] *= derivatives[i];
        }

        for (int i = 0; i < numberOfOutputs; ++i) {
            for (int j = 0; j < numberOfInputs; ++j) {
                WeightsDelta[i, j] += Gamma[i] * Inputs[j];
            }
            BiasesDelta[i] += Gamma[i];
        }
    }

    /// <summary>
    /// Updates the weights of the layer.
    /// </summary>
    public void UpdateWeights() {
        for (int i = 0; i < numberOfOutputs; ++i) {
            for (int j = 0; j < numberOfInputs; ++j) {
                Weights[i, j] -= WeightsDelta[i, j] * LearningRate;
                WeightsDelta[i, j] = 0;
            }
            Biases[i] -= BiasesDelta[i] * LearningRate;
            BiasesDelta[i] = 0;
        }
    }
}
