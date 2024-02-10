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
            string folderPath = Path.Combine(solutionDirectory, "training_files");

            // Initialize JsonFolderReader to read JSON files
            var jsonReader = new JsonFolderReader(folderPath);

            // Access the sequences data directly
            var sequencesContainers = jsonReader.AllSequences;

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
        }
    }
}
