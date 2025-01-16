using UnityEngine;

namespace NUnit.Framework {
    public static class Assert2 {
        public static void AreNear(float expected, float actual, float tolerance = 1e-6f) {
            if (Mathf.Abs(expected - actual) > tolerance) {
                throw new AssertionException($"Expected {expected} to be near {actual}");
            }
        }

        public static void AreNotNear(float expected, float actual, float tolerance = 1e-6f) {
            if (Mathf.Abs(expected - actual) <= tolerance) {
                throw new AssertionException($"Expected {expected} to not be near {actual}");
            }
        }

        public static void AreNear(Vector3 expected, Vector3 actual, float tolerance = 1e-6f) {
            if ((expected - actual).sqrMagnitude > tolerance * tolerance) {
                throw new AssertionException($"Expected {expected} to be near {actual}");
            }
        }

        public static void AreNotNear(Vector3 expected, Vector3 actual, float tolerance = 1e-6f) {
            if ((expected - actual).sqrMagnitude <= tolerance * tolerance) {
                throw new AssertionException($"Expected {expected} to not be near {actual}");
            }
        }
    }
}
