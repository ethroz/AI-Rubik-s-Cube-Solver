using NUnit.Framework;
using System.IO;
using UnityEngine;


public class NeuralNetworkTest {
    public static void AreEqual(NeuralNetwork net1, NeuralNetwork net2) {
        Assert.AreEqual(net1.Layers.Length, net1.Layers.Length);
        for (int i = 0; i < net1.Layers.Length; i++) {
            CollectionAssert.AreEqual(net1.Layers[i].Weights, net2.Layers[i].Weights);
            CollectionAssert.AreEqual(net1.Layers[i].Biases, net2.Layers[i].Biases);
            Assert.AreEqual(net1.Layers[i].ActivationType, net2.Layers[i].ActivationType);
            Assert.AreEqual(net1.Layers[i].LearningRate, net2.Layers[i].LearningRate);
        }
    }
    
    [Test]
    public void SimpleXOR() {
        // 0 0 0    => 0
        // 0 0 1    => 1
        // 0 1 0    => 1
        // 0 1 1    => 0
        // 1 0 0    => 1
        // 1 0 1    => 0
        // 1 1 0    => 0
        // 1 1 1    => 1

        NeuralNetwork net = new(
            new() { 3, 25, 1 },
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

        Assert2.AreNear(0, net.FeedForward(new float[] { 0, 0, 0 })[0], 0.1f);
        Assert2.AreNear(1, net.FeedForward(new float[] { 0, 0, 1 })[0], 0.1f);
        Assert2.AreNear(1, net.FeedForward(new float[] { 0, 1, 0 })[0], 0.1f);
        Assert2.AreNear(0, net.FeedForward(new float[] { 0, 1, 1 })[0], 0.1f);
        Assert2.AreNear(1, net.FeedForward(new float[] { 1, 0, 0 })[0], 0.1f);
        Assert2.AreNear(0, net.FeedForward(new float[] { 1, 0, 1 })[0], 0.1f);
        Assert2.AreNear(0, net.FeedForward(new float[] { 1, 1, 0 })[0], 0.1f);
        Assert2.AreNear(1, net.FeedForward(new float[] { 1, 1, 1 })[0], 0.1f);
    }

    [Test]
    public void SaveLoadWeights() {
        NeuralNetwork net = new(
            new() { 3, 10, 1 },
            new() { ActivationType.RELU, ActivationType.SIGMOID },
            0.5f
        );
        var weightsPath = Application.dataPath + @"\Tests\Weights.txt";
        net.SaveToFile(weightsPath);

        NeuralNetwork loadedNet = NeuralNetwork.LoadFromFile(weightsPath);

        AreEqual(net, loadedNet);

        File.Delete(weightsPath);
    }
}
