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
        private Neuron[][] neurons;
        private double[][] deltas;
        private double[][] inputs;
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
            } while (error > errorThreshold);
        }

        private double ProcessIteration(Tuple<double[], double[]> trainingObject)
        {
            double[] xs = trainingObject.Item1;
            double[] expectedResults = trainingObject.Item2;
            double[] results = GetOutput(xs);

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
                    deltas[i][j] = 
                }
            }
        }

        private double[] GetOutput(double[] networkInput)
        {
            double[] layerInput = networkInput;
            int j = 0;
            foreach (var layer in neurons)
            {
                var layerOutput = new double[layer.Length];
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
