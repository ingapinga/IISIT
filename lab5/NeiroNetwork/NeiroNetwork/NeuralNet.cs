using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeiroNetwork
{
    class NeuralNet
    {
        List<Layer> layers = new List<Layer>();
        int current_guess;

        public NeuralNet(List<int> layers_neuron_numbers, int number_of_inputs) // принимает список количеств нейронов в слоях и количество входных связей (нейронов)
        {
            for (int i = 0; i < layers_neuron_numbers.Count; i++)
                if (i == 0)
                    layers.Add(new Layer(layers_neuron_numbers[i], number_of_inputs));
                else
                    layers.Add(new Layer(layers_neuron_numbers[i], layers[i - 1].Neurons.Count));
        }

        public int GuessNumber(List<double> input_neurons)
        {
            current_guess = 0;
            for (int i = 0; i < layers.Count; i++)
                for (int j = 0; j < layers[i].Neurons.Count; j++)
                    if (i == 0)
                        layers[i].Neurons[j].CalculateValue(input_neurons);
                    else
                        layers[i].Neurons[j].CalculateValue(layers[i - 1].GetOutputsOfNeurons());

            double max_value = layers[layers.Count - 1].Neurons[0].Value;
            for (int i = 1; i < layers[layers.Count - 1].Neurons.Count; i++)
                if (max_value < layers[layers.Count - 1].Neurons[i].Value)
                {
                    current_guess = i;
                    max_value = layers[layers.Count - 1].Neurons[i].Value;
                }
            return current_guess;
        }

        public void BackPropagation(int correct_answer, List<byte> pixels)
        {
            if (current_guess != correct_answer)
            {
                int error = Math.Abs(correct_answer - current_guess);
                layers[layers.Count - 1].Neurons[current_guess].FixWeights(-error, pixels);
                layers[layers.Count - 1].Neurons[correct_answer].FixWeights(error, pixels);
            }
        }
    }

    class Layer
    {
        public List<Neuron> Neurons { get; }    // нейроны в слое

        public Layer(int number_of_neurons, int number_of_connections)  // принимает количество нейронов в слое и количество нейронов в предыдущем слое для соединений
        {
            Neurons = new List<Neuron>();
            List<Connection> connections = new List<Connection>();
            for (int i = 0; i < number_of_connections; i++)
                connections.Add(new Connection(0.5));

            for (int i = 0; i < number_of_neurons; i++)
                Neurons.Add(new Neuron(connections));
        }
        public List<double> GetOutputsOfNeurons()       // вернуть список всех значение нейронов со слоя
        {
            List<double> outputs = new List<double>();
            for (int i = 0; i < Neurons.Count; i++)
                outputs.Add(Neurons[i].Value);
            return outputs;
        }
    }

    class Neuron
    {
        List<Connection> Inputs { get; }
        public double Value { get; private set; }

        public Neuron(List<Connection> inputs)  // принимает все связи из прошлого слоя
        {
            Inputs = new List<Connection>();
            foreach (Connection input in inputs)
                Inputs.Add(new Connection(input.Weight));
        }

        public void CalculateValue(List<double> input_neurons)
        {
            Value = 0;
            for (int i = 0; i < Inputs.Count; i++)
                Value += Inputs[i].Weight * input_neurons[i];
        }

        public void FixWeights(int error, List<byte> pixels)
        {
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (pixels[i] == 1)
                    Inputs[i].Weight += (double)error / Inputs.Count;
            }
        }
    }

    class Connection
    {
        public double Weight { get; set; }

        public Connection(double weight)
        {
            Weight = weight;
        }
    }
}
