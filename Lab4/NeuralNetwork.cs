using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    internal class NeuralNetwork
    {
        private const double errorThreshold = 0.05;
        private const double learningRate = 0.6;
        private const double momentum = 0.5;
        private Neuron[][] neurons;
        private double[][] deltas;
        private double[][] inputs;
        private double[][] prevLayerOutputs;
        private double error;

        public NeuralNetwork(Neuron[][] neurons)
        {
            this.neurons = neurons;
            deltas = new double[neurons.Length][];
            inputs = new double[neurons.Length][];
            for (int i = 0; i < deltas.Length; ++i)
            {
                deltas[i] = new double[neurons[i].Length];
                inputs[i] = new double[neurons[i].Length];
            }
        }

        public static Func<double, double> FActivation { get; } = x => 1 / (1 + Math.Exp(-x));
        public static Func<double, double> FActivationDerivative { get; } = x => Math.Exp(-x) / Math.Pow((1 + Math.Exp(-x)), 2);

        public void Learn(Tuple<double[], double[]>[] trainingObjects)
        {
            do
            {
                error = 0;
                List<double> errors = new List<double>();
                for (int i = 0; i < trainingObjects.Length; ++i)
                {
                    errors.Add(ProcessIteration(trainingObjects[i]));
                }
                error = errors.Sum() / errors.Count;
            } while (error > errorThreshold);
        }

        private double ProcessIteration(Tuple<double[], double[]> trainingObject)
        {
            double[] xs = trainingObject.Item1;
            double[] expectedResults = trainingObject.Item2;
            double[] results = GetOutput(xs);
            deltas = GetDeltas(expectedResults, results);
            ApplyWeightsChange();
            var error = expectedResults.Select((e, i) => Math.Pow(e - results[i], 2)).Sum() / results.Length;
            return error;
        }

        private void ApplyWeightsChange()
        {
            for (int i = 0; i < neurons.Length; ++i)
            {
                for (int j = 0; j < neurons[i].Length; ++j)
                {
                    for (int k = 0; k < neurons[i][j].Weights.Length; ++k)
                    {
                        var gradient = deltas[i][j] * prevLayerOutputs[i][k];
                        var weightChange = learningRate * gradient + momentum * neurons[i][j].WeightsChanges[k];
                        neurons[i][j].Weights[k] += weightChange;
                        neurons[i][j].WeightsChanges[k] = weightChange;
                    }
                }
            }
        }

        private double[][] GetDeltas(double[] expectedResults, double[] results)
        {
            double[][] deltas = new double[neurons.Length][];
            for (int i = 0; i < neurons.Length; i++)
            {
                deltas[i] = new double[neurons[i].Length];
            }

            var lastLayerIndex = deltas.Length - 1;
            for (int i = 0; i < deltas[lastLayerIndex].Length; ++i)
            {
                deltas[lastLayerIndex][i] = (expectedResults[i] - results[i]) * FActivationDerivative(inputs[lastLayerIndex][i]);
            }

            for (int i = lastLayerIndex - 1; i >= 0; --i)
            {
                for (int j = 0; j < deltas[i].Length; ++j)
                {
                    deltas[i][j] = deltas[i + 1].Select((d, k) => d * neurons[i + 1][k].Weights[j]).Sum() * FActivationDerivative(inputs[i][j]);
                }
            }

            return deltas;
        }

        private double[] GetOutput(double[] networkInput)
        {
            double[] layerInput = networkInput;
            int j = 0;
            foreach (var layer in neurons)
            {
                var layerOutput = new double[layer.Length];
                prevLayerOutputs[j] = layerInput;
                for (int i = 0; i < layer.Length; ++i)
                {
                    inputs[j][i] = layer[i].GetInput(layerInput);
                    layerOutput[i] = layer[i].GetOutput(inputs[j][i]);
                }

                layerInput = layerOutput;
                ++j;
            }

            return layerInput;
        }
    }

    internal class Neuron
    {
        public double[] Weights { get; set; }
        public double[] WeightsChanges { get; set; }

        public Neuron(double[] weights)
        {
            Weights = weights;
            WeightsChanges = new double[Weights.Length];
        }

        public double GetInput(double[] input)
        {
            return input.Select((e, i) => e * Weights[i]).Sum();
        }

        public double GetOutput(double input)
        {
            return NeuralNetwork.FActivation(input);
        }
    }
}
