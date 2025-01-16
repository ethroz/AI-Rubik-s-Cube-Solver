using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class NeuralNetwork : IComparable<NeuralNetwork> {
    public readonly Layer[] Layers;
    public float Fitness;

    /// <summary>
    /// Initializes a new instance of the <see cref="NeuralNetwork"/> class.
    /// </summary>
    /// <param name="layers">The layers of the neural network.</param>
    /// <param name="activationTypes">The activation types for each layer.</param>
    public NeuralNetwork(List<int> layers, List<ActivationType> activationTypes, float learningRate = 0.01f) {
        if (layers.Count < 2) {
            throw new ArgumentException("There must be at least two layers in the neural network.");
        }
        
        while (activationTypes.Count >= layers.Count) {
            activationTypes.RemoveAt(activationTypes.Count - 2);
        }

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
    /// Initializes a new instance of the <see cref="NeuralNetwork"/> class.
    /// </summary>
    /// <param name="layers">The layers of the neural network.</param>
    private NeuralNetwork(Layer[] layers) {
        Layers = layers;
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

    /// <summary>
    /// Train the network on the last inputs given the expected outputs.
    /// </summary>
    /// <param name="expected">The expected outputs.</param>
    public void BackProp(float[] expected) {
        Layers[^1].BackPropOutput(expected);

        for (int i = Layers.Length - 2; i >= 0; --i) {
            Layers[i].BackPropHidden(Layers[i + 1].Gamma, Layers[i + 1].Weights);
        }

        for (int i = 0; i < Layers.Length; ++i) {
            Layers[i].UpdateWeights();
        }
    }

    /// <summary>
    /// Train the network on a batch of inputs given the expected outputs.
    /// </summary>
    /// <param name="inputs">The inputs to the neural network.</param>
    /// <param name="expected">The expected outputs.</param>
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

    /// <summary>
    /// Save the neural network to a file.
    /// </summary>
    /// <param name="path">The path to save the neural network to.</param>
    public void SaveToFile(string path) {
        using var writer = new StreamWriter(path);
        foreach (var layer in Layers) {
            layer.SaveToFile(writer);
        }
    }

    /// <summary>
    /// Load a neural network from a file.
    /// </summary>
    /// <param name="path">The path to load the neural network from.</param>
    public static NeuralNetwork LoadFromFile(string path) {
        using var reader = new StreamReader(path);
        List<Layer> layers = new();

        Layer next = null;
        do {
            next = Layer.LoadFromFile(reader);
            if (next != null) {
                layers.Add(next);
            }
        }
        while (next != null);

        if (layers.Count < 2) {
            throw new InvalidDataException("Invalid NeuralNetwork Weights file: There must be at least two layers in the neural network.");
        }

        return new NeuralNetwork(layers.ToArray());
    }

    /// <summary>
    /// Compares the fitness of two neural networks.
    /// </summary>
    /// <param name="other">The other neural network.</param>
    /// <returns>An integer indicating the comparison result.</returns>
    public int CompareTo(NeuralNetwork other) {
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
