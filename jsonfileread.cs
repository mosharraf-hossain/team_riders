using Newtonsoft.Json;

namespace anomalydetectionapp
{
    public class JsonFileReader
    {
        private string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "file.json");
        private List<List<int>> sequences;

        public JsonFileReader()
        {
            ReadJsonFile();
        }

        private void ReadJsonFile()
        {
            // Read the JSON file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON into a list of lists of integers
            sequences = JsonConvert.DeserializeObject<Dictionary<string, List<List<int>>>>(json)["sequences"];
        }

        // Property to expose the 'sequences' data
        public List<List<int>> Sequences
        {
            get { return sequences; }
        }
    }
}