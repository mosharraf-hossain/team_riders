/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace anomalydetectionapp
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<double>> mysequences = new Dictionary<string, List<double>>();

            // Assuming the JSON file is in a folder named "Data" within the same directory as the .sln file
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string trainingfolderPath = Path.Combine(solutionDirectory, "training_files");
            string predictingfolderPath = Path.Combine(solutionDirectory, "predicting_files");

            // Initialize JsonFolderReader to read JSON files
            var jsonReader = new JsonFolderReader(trainingfolderPath);

            var jsonreader1 = new JsonFolderReader(predictingfolderPath);

            // Access the sequences data directly
            var sequencesContainers = jsonReader.AllSequences;

            var sequencesContainers1 = jsonreader1.AllSequences;

            int sequenceIndex = 1;
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

            // Assuming MultiSequenceLearning is defined and instantiated correctly
            MultiSequenceLearning myexperiment = new MultiSequenceLearning();
            var predictor = myexperiment.Run(mysequences);

            predictor.Reset();


            foreach (var sequencesContainer1 in sequencesContainers1)
            {
                //PredictNextElement(predictor, sequencesContainer1.Sequences);
            }

            /*
            
            private static void PredictNextElement(Predictor predictor, double[] list)
            {
                Debug.WriteLine("------------------------------");

                foreach (var item in list)
                {
                    var res = predictor.Predict(item);

                    if (res.Count > 0)
                    {
                        foreach (var pred in res)
                        {
                            Debug.WriteLine($"{pred.PredictedInput} - {pred.Similarity}");
                        }

                        var tokens = res.First().PredictedInput.Split('_');
                        var tokens2 = res.First().PredictedInput.Split('-');
                        Debug.WriteLine($"Predicted Sequence: {tokens[0]}, predicted next element {tokens2.Last()}");
                    }
                    else
                        Debug.WriteLine("Nothing predicted :(");
                }

                Debug.WriteLine("------------------------------");
            }

            */


        }
    }
}
*/


// Path: MultiSequenceLearning.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace anomalydetectionapp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load training sequences
            Dictionary<string, List<double>> trainingSequences = LoadSequences("training_files");

            // Load predicting sequences
            Dictionary<string, List<double>> predictingSequences = LoadSequences("predicting_files");

            // Train the model
            MultiSequenceLearning myexperiment = new MultiSequenceLearning();
            var predictor = myexperiment.Run(trainingSequences);

            // Reset the predictor before making predictions
            predictor.Reset();

            // Make predictions
            foreach (var sequenceEntry in predictingSequences)
            {
                var sequenceKey = sequenceEntry.Key;
                var sequence = sequenceEntry.Value;

                // Predict next elements in the sequence
                List<double> predictedSequence = new List<double>();
                foreach (var element in sequence)
                {
                    double predictedValue = predictor.Predict(element);
                    predictedSequence.Add(predictedValue);
                }

                // Do something with the predicted sequence
                Console.WriteLine($"Predicted sequence for {sequenceKey}: {string.Join(", ", predictedSequence)}");
            }
        }

        static Dictionary<string, List<double>> LoadSequences(string folderName)
        {
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            // Get folder path
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string folderPath = Path.Combine(solutionDirectory, folderName);

            // Initialize JsonFolderReader to read JSON files
            var jsonReader = new JsonFolderReader(folderPath);
            var sequencesContainers = jsonReader.AllSequences;

            int sequenceIndex = 1;
            foreach (var sequencesContainer in sequencesContainers)
            {
                var sequencesList = sequencesContainer.Sequences;
                foreach (var sequence in sequencesList)
                {
                    List<double> convertedSequence = sequence.Select(x => (double)x).ToList();
                    string sequenceKey = "S" + sequenceIndex;
                    sequences.Add(sequenceKey, convertedSequence);
                    sequenceIndex++;
                }
            }

            return sequences;
        }
    }
}