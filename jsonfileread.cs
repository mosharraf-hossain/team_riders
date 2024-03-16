using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class SequencesContainer
{
    public List<List<int>> Sequences { get; set; }
}

public class JsonFolderReader
{
    public List<SequencesContainer> AllSequences { get; private set; }

    public JsonFolderReader(string folderPath)
    {
        AllSequences = new List<SequencesContainer>();

        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

        foreach (var filePath in jsonFiles)
        {
            string jsonContent = File.ReadAllText(filePath);
            SequencesContainer sequencesContainer = JsonConvert.DeserializeObject<SequencesContainer>(jsonContent);
            AllSequences.Add(sequencesContainer);
        }
    }
}
