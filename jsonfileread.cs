using Newtonsoft.Json; // Importing the Newtonsoft.Json
using System; // Importing the System namespace for basic .NET framework functionalities.
using System.Collections.Generic; // Importing the System.Collections.Generic namespace for utilizing generic collection classes.
using System.IO; // Importing the System.IO namespace for working with input/output operations like file reading and writing.


public class SequencesContainer
// Declaration of a public class named SequencesContainer.
{
    public List<List<int>> Sequences { get; set; }
    // Declaration of a public property named Sequences of type List<List<int>>, representing a container for sequences of integers.
    // The property has a getter and a setter.
}

public class JsonFolderReader 
    // Declaration of a public class named JsonFolderReader.
{
    public List<SequencesContainer> AllSequences { get; private set; }
    // Declaration of a public property named AllSequences of type List<SequencesContainer>, which will hold all sequences read from JSON files.
    // The property has a getter and a private setter.

    public JsonFolderReader(string folderPath)
    // Declaration of a constructor method for the JsonFolderReader class, which takes a string parameter named folderPath representing the path of the folder containing JSON files.
    {
        AllSequences = new List<SequencesContainer>();

        // Get all JSON files in the folder
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
        // Retrieving an array of file paths for all JSON files in the specified folderPath.

        foreach (var filePath in jsonFiles)
        {
            // Read and deserialize each JSON file

            string jsonContent = File.ReadAllText(filePath);

            // Reading the content of the JSON file specified by filePath and storing it as a string in the jsonContent variable.

            SequencesContainer sequencesContainer = JsonConvert.DeserializeObject<SequencesContainer>(jsonContent);

            // Deserializing the JSON content into an instance of the SequencesContainer class and storing it in the sequencesContainer variable.

            AllSequences.Add(sequencesContainer);
            // Adding the deserialized sequencesContainer to the AllSequences list.
        }
    }
}
