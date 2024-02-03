using Newtonsoft.Json;
public class SequencesContainer
{
    public List<List<int>> Sequences;
}

public class JsonFileReader
{
    public SequencesContainer SequencesContainer;

    public JsonFileReader(string filePath)
    {
        // Read the JSON file content
        string jsonContent = File.ReadAllText(filePath);

        // Deserialize the JSON content into SequencesContainer
        SequencesContainer = JsonConvert.DeserializeObject<SequencesContainer>(jsonContent);
    }
}