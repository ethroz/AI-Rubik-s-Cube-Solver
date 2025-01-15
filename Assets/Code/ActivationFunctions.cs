using System;
using System.Linq;

public enum ActivationType {
    NONE,
    SIGMOID,
    RELU,
    SOFTMAX
}

public static class Functions {
    public static Func<float[], float[]> GetActivation(ActivationType type) {
        return type switch {
            ActivationType.SIGMOID => Sigmoid,
            ActivationType.RELU => ReLU,
            ActivationType.SOFTMAX => Softmax,
            _ => (input) => input,
        };
    }

    public static Func<float[], float[]> GetActivationDerivative(ActivationType type) {
        return type switch {
            ActivationType.SIGMOID => SigmoidDerivative,
            ActivationType.RELU => ReLUDerivative,
            ActivationType.SOFTMAX => SoftmaxDerivative,
            _ => (inputs) => Enumerable.Repeat(1.0f, inputs.Length).ToArray(),
        };
    }

    public static float[] Softmax(float[] inputs) {
        float[] exps = inputs.Select(i => (float)Math.Exp(i)).ToArray();
        float sum = exps.Sum();
        return exps.Select(i => i / sum).ToArray();
    }

    public static float[] SoftmaxDerivative(float[] outputs) {
        // The derivative is the same as the sigmoid derivative apparently.
        return outputs.Select(SigmoidDerivative).ToArray();
    }

    public static int ArgMax(float[] input) {
        int maxIndex = 0;
        for (int i = 1; i < input.Length; i++) {
            if (input[i] > input[maxIndex]) {
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    public static float[] InverseArgMax(int index, int length) {
        float[] result = new float[length];
        result[index] = 1;
        return result;
    }

    public static float Sigmoid(float input) {
        return 1 / (1 + (float)Math.Exp(-input));
    }

    public static float[] Sigmoid(float[] inputs) {
        return inputs.Select(Sigmoid).ToArray();
    }

    public static float SigmoidDerivative(float output) {
        return output * (1 - output);
    }

    public static float[] SigmoidDerivative(float[] outputs) {
        return outputs.Select(SigmoidDerivative).ToArray();
    }

    public static float ReLU(float input) {
        return Math.Max(0, input);
    }

    public static float[] ReLU(float[] inputs) {
        return inputs.Select(ReLU).ToArray();
    }

    public static float ReLUDerivative(float output) {
        return output > 0 ? 1 : 0;
    }

    public static float[] ReLUDerivative(float[] outputs) {
        return outputs.Select(ReLUDerivative).ToArray();
    }
}
