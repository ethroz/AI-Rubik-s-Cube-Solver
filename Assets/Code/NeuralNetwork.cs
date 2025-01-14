using System;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork {
    public float Time;
    public string Level;
    public ActivationType Type;
    public int[] Size;
    public Layer[] Layers;

    public NeuralNetwork(int[] size, ActivationType type = ActivationType.SIGMOID) {
        if (size.Length < 2) {
            Debug.LogError("Neural Network must have at least 2 layers");
            return;
        }
        Type = type;
        Size = new int[size.Length];
        for (int i = 0; i < size.Length; i++)
            Size[i] = size[i];

        Layers = new Layer[Size.Length - 1];

        for (int i = 0; i < Layers.Length; i++) {
            Layers[i] = new Layer(Size[i], Size[i + 1], Type);
        }
    }

    public NeuralNetwork(float[][,] weights, ActivationType type = ActivationType.SIGMOID) {
        Type = type;
        Size = new int[weights.Length + 1];
        Size[0] = weights[0].GetLength(0);
        for (int i = 1; i < Size.Length; i++)
            Size[i] = weights[i - 1].GetLength(1);

        Layers = new Layer[weights.Length];

        for (int i = 0; i < Layers.Length; i++) {
            Layers[i] = new Layer(weights[i], Type);
        }
    }

    public float[][,] GetWeights() {
        List<float[,]> arrArrWeights = new();

        for (int i = 0; i < Layers.Length; i++) {
            arrArrWeights.Add(Layers[i].Weights);
        }

        return arrArrWeights.ToArray();
    }

    public float[] FeedForward(float[] inputs) {
        Layers[0].FeedForward(inputs);

        for (int i = 1; i < Layers.Length; i++)
            Layers[i].FeedForward(Layers[i - 1].Outputs);

        return Layers[Layers.Length - 1].Outputs;
    }

    public void BackProp(float[] expected) {
        Layers[Layers.Length - 1].BackPropOutput(expected);
        for (int i = Layers.Length - 2; i > -1; i--)
            Layers[i].BackPropHidden(Layers[i + 1].Gamma, Layers[i + 1].Weights);

        for (int i = 0; i < Layers.Length; i++)
            Layers[i].UpdateWeights();
    }

    public class Layer {
        public int NumberOfInputs;
        public int NumberOfOutputs;

        public float[] Inputs;
        public float[] Outputs;
        public float[,] Weights;
        public float[,] WeightsDelta;
        public float[] Gamma;
        public float[] Error;

        public Func<float, float> ActivationFunc;
        public Func<float, float> ActivationFuncDer;

        public static System.Random random = new System.Random();
        public static float LearningRate = 0.01f;

        public Layer(int numberOfInputs, int numberOfOutputs, ActivationType type) {
            NumberOfInputs = numberOfInputs;
            NumberOfOutputs = numberOfOutputs;

            Inputs = new float[NumberOfInputs];
            Outputs = new float[NumberOfOutputs];
            Weights = new float[NumberOfOutputs, NumberOfInputs];
            WeightsDelta = new float[NumberOfOutputs, NumberOfInputs];
            Gamma = new float[NumberOfOutputs];
            Error = new float[NumberOfOutputs];

            ActivationFunc = Activation.GetFunc(type);
            ActivationFuncDer = Activation.GetFuncDer(type);

            InitializeWeights();
        }

        public Layer(float[,] weights, ActivationType type) {
            NumberOfInputs = weights.GetLength(0);
            NumberOfOutputs = weights.GetLength(1);

            Inputs = new float[NumberOfInputs];
            Outputs = new float[NumberOfOutputs];
            Weights = weights;
            WeightsDelta = new float[NumberOfOutputs, NumberOfInputs];
            Gamma = new float[NumberOfOutputs];
            Error = new float[NumberOfOutputs];

            ActivationFunc = Activation.GetFunc(type);
            ActivationFuncDer = Activation.GetFuncDer(type);
        }

        private void InitializeWeights() {
            for (int i = 0; i < NumberOfOutputs; i++) {
                for (int j = 0; j < NumberOfInputs; j++) {
                    Weights[i, j] = (float)random.NextDouble() - 0.5f;
                }
            }
        }

        public float[] FeedForward(float[] inputs) {
            Inputs = inputs;

            for (int i = 0; i < NumberOfOutputs; i++) {
                Outputs[i] = 0;
                for (int j = 0; j < NumberOfInputs; j++)
                    Outputs[i] += Inputs[j] * Weights[i, j];

                Outputs[i] = ActivationFunc(Outputs[i]);
            }
            return Outputs;
        }

        public void BackProp(float[] expected) {
            for (int i = 0; i < NumberOfOutputs; i++)
                Error[i] = expected[i] - Outputs[i];

            for (int i = 0; i < NumberOfInputs; i++)
                Gamma[i] = 0;

            for (int i = 0; i < NumberOfOutputs; i++) {
                for (int j = 0; j < NumberOfInputs; j++) {
                    Gamma[j] += Weights[i, j] * Error[i];
                    WeightsDelta[i, j] = Error[i] * ActivationFuncDer(Outputs[i]);
                }
            }
        }

        public void BackPropOutput(float[] expected) {
            for (int i = 0; i < NumberOfOutputs; i++)
                Error[i] = Outputs[i] - expected[i];

            for (int i = 0; i < NumberOfOutputs; i++)
                Gamma[i] = Error[i] * ActivationFuncDer(Outputs[i]);

            for (int i = 0; i < NumberOfOutputs; i++)
                for (int j = 0; j < NumberOfInputs; j++)
                    WeightsDelta[i, j] = Gamma[i] * Inputs[j];
        }

        public void BackPropHidden(float[] gammaForward, float[,] weightsForward) {
            for (int i = 0; i < NumberOfOutputs; i++) {
                Gamma[i] = 0;

                for (int j = 0; j < gammaForward.Length; j++)
                    Gamma[i] += gammaForward[j] * weightsForward[j, i];

                Gamma[i] *= Outputs[i];
            }

            for (int i = 0; i < NumberOfOutputs; i++)
                for (int j = 0; j < NumberOfInputs; j++)
                    WeightsDelta[i, j] = Gamma[i] * Inputs[j];
        }

        public void UpdateWeights() {
            for (int i = 0; i < NumberOfOutputs; i++) {
                for (int j = 0; j < NumberOfInputs; j++) {
                    Weights[i, j] -= WeightsDelta[i, j] * LearningRate;
                }
            }
        }
    }
}