using System;
using System.Collections.Generic;
using UnityEngine;
using Extras;

public class ModularNetwork
{
    public static int[][] layout;
    public static List<int> modIndex = new List<int>();
    public static NeuralNetwork[] modNetwork;

    public ModularNetwork(int[][] layout)
    {
        ModularNetwork.layout = new int[layout.Length][];

        for (int i = 0; i < layout.Length; i++)
        {
            ModularNetwork.layout[i] = layout[i];
            if (i == 0)
                continue;
            if (layout[i - 1].Length != layout[i].Length)
                modIndex.Add(i);
        }

        modNetwork = new NeuralNetwork[1 + layout[modIndex[0]].Length + 1];

        int[] currentNetwork = new int[modIndex[0]];
        for (int i = 0; i < modIndex[0]; i++)
        {
            currentNetwork[i] = layout[i][0];
        }
        modNetwork[0] = new NeuralNetwork(currentNetwork);

        for (int i = 1; i < modNetwork.Length - 1; i++)
        {
            currentNetwork = new int[modIndex[1] - modIndex[0]];
            for (int j = modIndex[0]; j < modIndex[1]; j++)
            {
                currentNetwork[j - modIndex[0]] = layout[j][i - 1];
            }
            modNetwork[i] = new NeuralNetwork(currentNetwork);
        }

        currentNetwork = new int[layout.Length - modIndex[1]];
        for (int i = modIndex[1]; i < layout.Length; i++)
        {
            currentNetwork[i - modIndex[1]] = layout[i][0];
        }
        modNetwork[modNetwork.Length - 1] = new NeuralNetwork(currentNetwork);
    }

    public float[] FeedForward(float[] inputs)
    {
        float[] input = modNetwork[0].FeedForward(inputs);
        float[] output = new float[0];

        for (int i = 1; i < modNetwork.Length - 1; i++)
        {
            float[] old = output;
            float[] knew = new float[layout[modIndex[1] - 1][i - 1]];
            output = new float[old.Length + knew.Length];
            int index = 0;
            for (int j = 0; j < i; j++)
            {
                index += layout[modIndex[0]][j];
            }
            knew = modNetwork[i].FeedForward(input);
            for (int j = 0; j < old.Length; j++)
            {
                output[j] = old[j];
            }
            for (int j = 0; j < knew.Length; j++)
            {
                output[j + old.Length] = knew[j];
            }
        }

        input = new float[layout[layout.Length - 1][0]];
        input = modNetwork[modNetwork.Length  - 1].FeedForward(output);

        return input;
    }

    public void BackProp(float[] expected)
    {
        modNetwork[modNetwork.Length - 1].BackProp(expected);
        float[] gammaForward = modNetwork[modNetwork.Length - 1].layers[0].gamma;
        float[,] weightsForward = modNetwork[modNetwork.Length - 1].layers[0].weights;

        for (int i = 1; i < modNetwork.Length - 1; i++)
        {
            float[] newGammaForward;
        }
    }
}

public class NeuralNetwork
{
    public int[] layer;
    public Layer[] layers;

    public NeuralNetwork(int[] layer)
    {
        this.layer = new int[layer.Length];
        for (int i = 0; i < layer.Length; i++)
        {
            this.layer[i] = layer[i];
        }

        layers = new Layer[layer.Length - 1];

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new Layer(layer[i], layer[i + 1]);
        }
    }

    public float[] FeedForward(float[] inputs)
    {
        layers[0].FeedForward(inputs, false);

        for (int i = 1; i < layers.Length - 1; i++)
        {
            layers[i].FeedForward(layers[i - 1].outputs, false);
        }

        layers[layers.Length - 1].FeedForward(layers[layers.Length - 2].outputs, true); 

        return layers[layers.Length - 1].outputs;
    }

    public void BackProp(float[] expected)
    {
        for (int i = layers.Length - 1; i > -1; i--)
        {
            if (i == layers.Length - 1)
            {
                layers[i].BackPropOutput(expected);
            }
            else
            {
                layers[i].BackPropHidden(layers[i + 1].gamma, layers[i + 1].weights);
            }
        }

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].UpdateWeights();
        }
    }

    public void BackPropHidden(float[] gammaForward, float[,] weightsForward)
    {
        for (int i = layers.Length - 1; i > -1; i--)
        {
            if (i == layers.Length - 1)
            {
                layers[i].BackPropHidden(gammaForward, weightsForward);
            }
            else
            {
                layers[i].BackPropHidden(layers[i + 1].gamma, layers[i + 1].weights);
            }
        }

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].UpdateWeights();
        }
    }

    public class Layer
    {
        public int numberOfInputs; //# of neurons in the previous layer
        public int numberOfOutputs; //# of neurons in the current 

        public float[] inputs;
        public float[] outputs;
        public float[,] weights;
        public float[,] weightsDelta;
        public float[] gamma;
        public float[] error;
        public static System.Random random = new System.Random();
        public static float LearningRate = 0.01f;

        public Layer(int numberOfInputs, int numberOfOutputs)
        {
            this.numberOfInputs = numberOfInputs;
            this.numberOfOutputs = numberOfOutputs;

            outputs = new float[numberOfOutputs];
            inputs = new float[numberOfInputs];
            weights = new float[numberOfOutputs, numberOfInputs];
            weightsDelta = new float[numberOfOutputs, numberOfInputs];
            gamma = new float[numberOfOutputs];
            error = new float[numberOfOutputs];

            InitializeWeights();
        }

        public void InitializeWeights()
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weights[i, j] = (float)random.NextDouble() - 0.5f;
                }
            }
        }

        public float[] FeedForward(float[] inputs, bool lastLayer)
        {
            this.inputs = inputs;

            for (int i = 0; i < numberOfOutputs; i++)
            {
                outputs[i] = 0;
                for (int j = 0; j < numberOfInputs; j++)
                {
                    outputs[i] += inputs[j] * weights[i, j];
                }

                if (lastLayer)
                    outputs[i] = Swish(outputs[i]);
                else
                    outputs[i] = TanH(outputs[i]);
            }

            return outputs;
        }

        public static float TanH(float value)
        {
            return (float)Math.Tanh(value);
        }

        public static float TanHDer(float value)
        {
            return 1 - (value * value);
        }

        public static float Swish(float value)
        {
            return ((value) / (Mathf.Pow(D.E, value) + 1));
        }

        public static float SwishDer(float value)
        {
            return value + 0;
        }

        public static float Sigmoid(float value)
        {
            return value;
        }

        public static float SigmoidDer(float value)
        {
            return value;
        }

        public void LearningCertainty(float[] expected)
        {

        }

        public void BackPropOutput(float[] expected)
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                error[i] = outputs[i] - expected[i];
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                gamma[i] = error[i] * TanHDer(outputs[i]);
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];
                }
            }
        }

        public void BackPropHidden(float[] gammaForward, float[,] weightsForward)
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                gamma[i] = 0;

                for (int j = 0; j < gammaForward.Length; j++)
                {
                    gamma[i] += gammaForward[j] * weightsForward[j, i];
                }

                gamma[i] *= SwishDer(outputs[i]);
            }
        }

        public void UpdateWeights()
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weights[i, j] -= weightsDelta[i, j] * LearningRate;
                }
            }
        }
    }
}