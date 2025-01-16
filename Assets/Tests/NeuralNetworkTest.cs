using NUnit.Framework;

public class NeuralNetworkTest {
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
}
