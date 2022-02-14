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
        private double error;

        public NeuralNetwork(Neuron[][] neurons)
        {
            this.neurons = neurons;
            deltas = new double[neurons.Length][];
            for (int i = 0; i < deltas.Length; ++i)
            {
                deltas[i] = new double[neurons[i].Length];
            }
        }

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
            double[][] result = new double[neurons.Length][];
            for (int i = 0; i < neurons.Length; i++)
            {
                result[i] = new double[neurons[i].Length];
            }

            for (int i = 0; i < result[0].Length; ++i)
            {
                result[0][i] = (expectedResults[i] - results[i])
            }
        }

        private double[] GetOutput(double[] networkInput)
        {
            double[] layerInput = networkInput;
            foreach (var layer in neurons)
            {
                var layerOutput = new double[layer.Length];
                for (int i = 0; i < layer.Length; ++i)
                {
                    layerOutput[i] = layer[i].GetOutput(layerInput);
                }

                layerInput = layerOutput;
            }

            return layerInput;
        }
    }

    internal class Neuron
    {
        private Func<double, double> fActivation;
        public Neuron()
        {
            fActivation = x => 1 / (1 + Math.Exp(-x));
        }

        public Neuron(Func<double, double> fActivation)
        {
            this.fActivation = fActivation;
        }

        public double[] Weights { get; set; }

        public double GetOutput(double[] input)
        {
            return fActivation(input.Select((e, i) => e * Weights[i]).Sum());
        }
    }
}
