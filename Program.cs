using System.Xml.Linq;

namespace anomalydetectionapp;

class Program
{
    static void Main(string[] args)
    {
        /*Dictionary<string, List<double>> mysequences = new Dictionary<string, List<double>>();
        
        //have to make class for reading data values

        mysequences.Add("S1", new List<double>(new double[] { 0.0, 1.0, 2.0, 3.0, 4.0, 2.0, 5.0, }));
        mysequences.Add("S2", new List<double>(new double[] { 8.0, 1.0, 2.0, 9.0, 10.0, 7.0, 11.00 }));
        MultiSequenceLearning myexperiment = new MultiSequenceLearning();
        var predictor = myexperiment.Run(mysequences);*/

        // Assuming the JSON file is in a folder named "Data" within the same directory as the .sln file
        string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        string filePath = Path.Combine(solutionDirectory, "training_files", "file.json");

        var jsonReader = new JsonFileReader(filePath);

        // Access the sequences data directly
        var sequences = jsonReader.SequencesContainer.Sequences;

        // Now 'sequences' variable contains the data from the JSON file

        // Example: Print the sequences
        foreach (var sequence in sequences)
        {
            Console.WriteLine(string.Join(", ", sequence));
        }


    }
}
