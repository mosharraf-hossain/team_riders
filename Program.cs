using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace anomalydetectionapp
{
    /// <summary>
    /// Class containing methods to read sequences from JSON files, trim input sequences, train a model, and detect anomalies.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            #pragma warning disable // Disable all warnings
            
            
            //Taking user input for tolerance value of the experiment
            Console.WriteLine("");
            Console.WriteLine("Enter the tolerance value for anomaly detection experiment(Example: 0.1):");
            Console.WriteLine("");
            double tValue = double.Parse(Console.ReadLine());
            
            Console.WriteLine("");
            Console.WriteLine("***************************************************");
            Console.WriteLine("");
            Console.WriteLine($"Hello! Beginning our anomaly detection experiment.");
            Console.WriteLine("");
            Console.WriteLine("***************************************************");

            // Create a dictionary to store sequences
            Dictionary<string, List<double>> mysequences = new Dictionary<string, List<double>>();

            // Get the solution directory path
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

            // Construct paths for training and predicting folders
            string trainingfolderPath = Path.Combine(solutionDirectory, "training_files");
            string predictingfolderPath = Path.Combine(solutionDirectory, "predicting_files");

            // Read sequences from training and predicting folders
            var trainingjsonReader = new JsonFolderReader(trainingfolderPath);
            var predictingjsonreader = new JsonFolderReader(predictingfolderPath);

            var sequencesContainers = trainingjsonReader.AllSequences;
            var sequencesContainers1 = predictingjsonreader.AllSequences;

            int sequenceIndex = 1;

            // Process sequences from the training folder
            // Convert the sequences to a dictionary
            // Key: Sequence ID, Value: List of values in the sequence
            // Example: "S1": [1, 2, 3, 4, 5]
            // This is required for training the model
            foreach (var sequencesContainer in sequencesContainers)
            {
                var sequences = sequencesContainer.Sequences;

                foreach (var sequence in sequences)
                {
                    List<double> convertedSequence = sequence.Select(x => (double)x).ToList();

                    string sequenceKey = "S" + sequenceIndex;
                    mysequences.Add(sequenceKey, convertedSequence);
                    sequenceIndex++;
                }
            }

            // Train the model using MultiSequenceLearning
            MultiSequenceLearning myexperiment = new MultiSequenceLearning();
            var predictor = myexperiment.Run(mysequences);
            predictor.Reset();

            // Detect anomalies in sequences from the predicting folder
            foreach (var sequencesContainer in sequencesContainers1)
            {
                var sequences = sequencesContainer.Sequences;

                foreach (var sequence in sequences)
                {
                    // Convert the sequence to a list of double values
                    List<double> inputlist = sequence.Select(x => (double)x).ToList();
                    double[] inputArray = inputlist.ToArray();

                    // Trim some values randomly in the beginning of the sequence
                    Random random = new Random();
                    int trimCount = random.Next(1, 4);
                    double[] inputTestArray = inputArray.Skip(trimCount).ToArray();

                    // Detect anomalies in the sequence using the trained model
                    AnomalyDetection.AnomalyDetectMethod(predictor, inputTestArray, tValue);
                }
            }

            // Calculate the final experiment accuracy
            double finalExpAccuracy = AnomalyDetection.totalAccuracy / AnomalyDetection.listCount;
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Final experiment accuracy: " + finalExpAccuracy + "%.");
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------");
        }
    }
}
