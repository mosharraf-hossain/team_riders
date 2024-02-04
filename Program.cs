using System.Xml.Linq;

namespace anomalydetectionapp;

class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, List<double>> mysequences = new Dictionary<string, List<double>>();
        
        // Assuming the JSON file is in a folder named "Data" within the same directory as the .sln file
        string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        string filePath = Path.Combine(solutionDirectory, "training_files", "file.json");

        var jsonReader = new JsonFileReader(filePath);

        // Access the sequences data directly
        var sequences = jsonReader.SequencesContainer.Sequences;

        for (int i = 0; i < sequences.Count; i++)
        {
            var sequence = sequences[i];
            List<double> convertedSequence = sequence.Select(x => (double)x).ToList();
            string sequenceKey = "S" + (i + 1);
            mysequences.Add(sequenceKey, convertedSequence);
        }

        MultiSequenceLearning myexperiment = new MultiSequenceLearning();
        var predictor = myexperiment.Run(mysequences);
    }
}
