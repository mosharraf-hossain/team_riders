using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace anomalydetectionapp
{
    /// <summary>
    /// Class containing methods to read sequences from JSON files, train a model, and detect anomalies.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
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
                    List<double> inputlist = sequence.Select(x => (double)x).ToList();
                    double[] inputArray = inputlist.ToArray();
                    AnomalyDetection.AnomalyDetectMethod(predictor, inputArray, 0.2);
                }
            }
        }
    }
}
