using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var neurons = new Neuron[2][] {
                new Neuron[4] {
                    new Neuron(new double[] { -0.5, -0.5, 1 }),
                    new Neuron(new double[] { -0.5, 0.5, 0.5 }),
                    new Neuron(new double[] { 0.5, -0.5, 0.5 }),
                    new Neuron(new double[] { 0.5, 0.5, -1 })
                },
                new Neuron[2] {
                    new Neuron(new double[] { 1, -1, -1, 1}),
                    new Neuron(new double[] { -1, 1, 1, -1})
                }
            };
            var neuralNetwork = new NeuralNetwork(neurons);
            var trainingObjects = new Tuple<double[], double[]>[] {
                new Tuple<double[], double[]>(new double[] { 0, 0, 1 }, new double[] { 1, 0 }),
                new Tuple<double[], double[]>(new double[] { 0, 1, 1 }, new double[] { 0, 1 }),
                new Tuple<double[], double[]>(new double[] { 1, 0, 1 }, new double[] { 0, 1 }),
                new Tuple<double[], double[]>(new double[] { 1, 1, 1 }, new double[] { 1, 0 }),
            };
            neuralNetwork.Learn(trainingObjects);
        }
    }
}
