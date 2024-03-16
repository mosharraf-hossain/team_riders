namespace anomalydetectionapp
{
    class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<string, List<double>> mysequences = new Dictionary<string, List<double>>();

            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

            string trainingfolderPath = Path.Combine(solutionDirectory, "training_files");

            string predictingfolderPath = Path.Combine(solutionDirectory, "predicting_files");

            var trainingjsonReader = new JsonFolderReader(trainingfolderPath);

            var predictingjsonreader = new JsonFolderReader(predictingfolderPath);

            var sequencesContainers = trainingjsonReader.AllSequences;

            var sequencesContainers1 = predictingjsonreader.AllSequences;

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

            MultiSequenceLearning myexperiment = new MultiSequenceLearning();
            var predictor = myexperiment.Run(mysequences);
            predictor.Reset();

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
