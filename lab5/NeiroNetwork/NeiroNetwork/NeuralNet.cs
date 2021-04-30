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
		
		public NeuralNet(List<int> layers_neuron_numbers, int number_of_inputs)	// принимает список количество нейронов в слоях и количество входных связей
		{
			for(int i = 0; i < layers_neuron_numbers.Count; i++)
				if (i == 0)
					layers.Add(new Layer(layers_neuron_numbers[i], number_of_inputs));
				else
					layers.Add(new Layer(layers_neuron_numbers[i], layers[i - 1].Neurons.Count));
		}

		public int GuessNumber(List<double> input_neurons)
		{
			int result = 0;
			for (int i = 0; i < layers.Count; i++)
				for (int j = 0; j < layers[i].Neurons.Count; j++)
					if(i == 0)
						layers[i].Neurons[j].CalculateValue(input_neurons);
					else
						layers[i].Neurons[j].CalculateValue(layers[i - 1].GetOutputsOfNeurons());

			double max_value = -1;
			for(int i = 0; i < layers[layers.Count-1].Neurons.Count; i++)
				if(max_value < layers[layers.Count - 1].Neurons[i].Value)
				{
					result = i;
					max_value = layers[layers.Count - 1].Neurons[i].Value;
				}
			return result;
		}
	}

	class Layer
	{
		public List<Neuron> Neurons { get; }	// нейроны в слое

		public Layer(int number_of_neurons, int number_of_connections)	// принимает количество нейронов в слое и количество нейронов в предыдущем слое для соединений
		{
			Neurons = new List<Neuron>();
			List<Connection> connections = new List<Connection>();
			for (int i = 0; i < number_of_connections; i++)
				connections.Add(new Connection(0.5));

			for (int i = 0; i < number_of_neurons; i++)
				Neurons.Add(new Neuron(connections));
		}
		public List<double> GetOutputsOfNeurons()		// вернуть список всех значение нейронов со слоя
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

		public Neuron(List<Connection> inputs)	// принимает все связи из прошлого слоя
		{
			Inputs = new List<Connection>();
			foreach (Connection input in inputs)
				Inputs.Add(input);
		}

		public void CalculateValue(List<double> input_neurons)
		{
			Value = 0;
			for(int i = 0; i < Inputs.Count; i++)
				Value += Inputs[i].Weight * input_neurons[i];
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
