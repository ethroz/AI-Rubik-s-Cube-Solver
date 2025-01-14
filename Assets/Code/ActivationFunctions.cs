using System;
using UnityEngine;

public enum ActivationType {
    SIGMOID,
    RELU,
    ELU
}

public static class Activation {
    public static Func<float, float> GetFunc(ActivationType type) {
        return type switch {
            ActivationType.SIGMOID => Sigmoid,
            ActivationType.RELU => ReLU,
            ActivationType.ELU => ELU,
            _ => throw new ArgumentException("Unknown activation type"),
        };
    }

    public static Func<float, float> GetFuncDer(ActivationType type) {
        return type switch {
            ActivationType.SIGMOID => SigmoidDer,
            ActivationType.RELU => ReLUDer,
            ActivationType.ELU => ELUDer,
            _ => throw new ArgumentException("Unknown activation type"),
        };
    }

    public static float Sigmoid(float value) {
        return 1 / (1 + Mathf.Pow((float)Math.E, -value));
    }

    public static float SigmoidDer(float value) {
        return value * (1 - value);
    }

    public static float ReLU(float value) {
        if (value >= 0)
            return value;
        else
            return 0;
    }

    public static float ReLUDer(float value) {
        if (value >= 0)
            return 1;
        else
            return 0;
    }

    public static float ELU(float value) {
        if (value >= 0)
            return value;
        else
            return Mathf.Pow((float)Math.E, value) + 1;
    }

    public static float ELUDer(float value) {
        if (value >= 0)
            return 1;
        else
            return value + 1;
    }
}
