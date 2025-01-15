using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {
    void Start() {
        // 0 0 0    => 0
        // 0 0 1    => 1
        // 0 1 0    => 1
        // 0 1 1    => 0
        // 1 0 0    => 1
        // 1 0 1    => 0
        // 1 1 0    => 0
        // 1 1 1    => 1

        NeuralNetwork net = new(
            new List<int>() { 3, 10, 1 },
            new() { ActivationType.RELU, ActivationType.SIGMOID },
            0.5f
        );

        for (int i = 0; i < 5000; i++) {
            net.BatchTrain(new float[][] {
                new float[] { 0, 0, 0 },
                new float[] { 0, 0, 1 },
                new float[] { 0, 1, 0 },
                new float[] { 0, 1, 1 },
                new float[] { 1, 0, 0 },
                new float[] { 1, 0, 1 },
                new float[] { 1, 1, 0 },
                new float[] { 1, 1, 1 }
            }, new float[][] {
                new float[] { 0 },
                new float[] { 1 },
                new float[] { 1 },
                new float[] { 0 },
                new float[] { 1 },
                new float[] { 0 },
                new float[] { 0 },
                new float[] { 1 }
            });
        }

        Debug.Log(net.FeedForward(new float[] { 0, 0, 0 })[0]);
        Debug.Log(net.FeedForward(new float[] { 0, 0, 1 })[0]);
        Debug.Log(net.FeedForward(new float[] { 0, 1, 0 })[0]);
        Debug.Log(net.FeedForward(new float[] { 0, 1, 1 })[0]);
        Debug.Log(net.FeedForward(new float[] { 1, 0, 0 })[0]);
        Debug.Log(net.FeedForward(new float[] { 1, 0, 1 })[0]);
        Debug.Log(net.FeedForward(new float[] { 1, 1, 0 })[0]);
        Debug.Log(net.FeedForward(new float[] { 1, 1, 1 })[0]);
    }
}
