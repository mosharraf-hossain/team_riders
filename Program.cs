namespace anomalydetectionapp
// Declaration of a namespace named anomalydetectionapp, which encapsulates the classes and other types defined within it.

{
    class Program // Declaration of a class named Program.
    {
        public static void Main(string[] args)
            // Declaration of a public method named Main, which serves as the entry point of the program.
        {
            Dictionary<string, List<double>> mysequences = new Dictionary<string, List<double>>();

            //Declaration and initialization of a Dictionary to hold sequences, where the key is a string identifier and the value is a list of double values.

            // Assuming the JSON file is in a folder named "Data" within the same directory as the .sln file

            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

            // Retrieving the solution directory path by navigating up from the current directory.

            string trainingfolderPath = Path.Combine(solutionDirectory, "training_files");

            // Combining the solution directory path with the name of the folder containing training JSON files to get the full path.

            string predictingfolderPath = Path.Combine(solutionDirectory, "predicting_files");

            // Combining the solution directory path with the name of the folder containing predicting JSON files to get the full path.

            // Initialize JsonFolderReader to read JSON files

            var trainingjsonReader = new JsonFolderReader(trainingfolderPath);

            // Creating an instance of JsonFolderReader to read training JSON files.


            var predictingjsonreader = new JsonFolderReader(predictingfolderPath);

            // Creating an instance of JsonFolderReader to read predicting JSON files.


            // Access the sequences data directly
            var sequencesContainers = trainingjsonReader.AllSequences;

            // Retrieving the list of SequencesContainer objects containing training sequences.

            var sequencesContainers1 = predictingjsonreader.AllSequences;

            // Retrieving the list of SequencesContainer objects containing predicting sequences.


            int sequenceIndex = 1;
            // Initializing a variable to keep track of sequence index.

            foreach (var sequencesContainer in sequencesContainers)      // Iterating over each SequencesContainer object in the training sequences.
            {
                var sequences = sequencesContainer.Sequences;      // Retrieving the list of sequences from the current SequencesContainer.

                foreach (var sequence in sequences)     // Iterating over each sequence in the list of sequences.
                {
                    List<double> convertedSequence = sequence.Select(x => (double)x).ToList();

                    // Converting each integer value in the sequence to double and storing it in a list.

                    string sequenceKey = "S" + sequenceIndex;         // Generating a unique key for the sequence.
                    mysequences.Add(sequenceKey, convertedSequence);  // Adding the converted sequence to the dictionary with its corresponding key.
                    sequenceIndex++;       // Incrementing the sequence index for the next sequence.


                }
            }

            // Assuming MultiSequenceLearning is defined and instantiated correctly
            MultiSequenceLearning myexperiment = new MultiSequenceLearning();      // Creating an instance of the MultiSequenceLearning class.
            var predictor = myexperiment.Run(mysequences);    // Running the experiment with the provided sequences to obtain a predictor.
            predictor.Reset();  // Resetting the predictor for predicting new sequences.

            foreach (var sequencesContainer in sequencesContainers1)    // Iterating over each SequencesContainer object in the predicting sequences.
            {
                var sequences = sequencesContainer.Sequences;     // Retrieving the list of sequences from the current SequencesContainer.

                foreach (var sequence in sequences)
                {
                    List<double> inputlist = sequence.Select(x => (double)x).ToList();    // Converting each integer value in the sequence to double and storing it in a list.
                    double[] inputArray = inputlist.ToArray();    // Converting the list of double values to an array.
                    AnomalyDetection.AnomalyDetectMethod(predictor, inputArray, 0.2);  // Calling the AnomalyDetectMethod to detect anomalies in the current sequence using the predictor.
                }
            }
        }
    }
}
