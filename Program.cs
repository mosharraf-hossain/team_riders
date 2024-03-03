using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NeoCortexApi;
using System.Diagnostics;

namespace anomalydetectionapp
{
    class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<string, List<double>> mysequences = new Dictionary<string, List<double>>();

            // Assuming the JSON file is in a folder named "Data" within the same directory as the .sln file
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string trainingfolderPath = Path.Combine(solutionDirectory, "training_files");
            string predictingfolderPath = Path.Combine(solutionDirectory, "predicting_files");

            // Initialize JsonFolderReader to read JSON files
            var trainingjsonReader = new JsonFolderReader(trainingfolderPath);

            var predictingjsonreader = new JsonFolderReader(predictingfolderPath);

            // Access the sequences data directly
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

            // Assuming MultiSequenceLearning is defined and instantiated correctly
            MultiSequenceLearning myexperiment = new MultiSequenceLearning();
            var predictor = myexperiment.Run(mysequences);

            predictor.Reset();

            foreach (var sequencesContainer in sequencesContainers1)
            {
                var sequences = sequencesContainer.Sequences;

                foreach (var list in sequences)
                {
                    double[] lst = list.Select(x => (double)x).ToArray();
                    PredictNextElement(predictor, lst);
                }
            }

            static void PredictNextElement(Predictor predictor, double[] list)
            {
                Console.WriteLine("------------------------------");

                foreach (var item in list)
                {
                    var res = predictor.Predict(item);

                    if (res.Count > 0)
                    {
                        foreach (var pred in res)
                        {
                            Console.WriteLine($"{pred.PredictedInput} - {pred.Similarity}");
                        }

                        var value = res.First().PredictedInput.Split('_');
                        var value1 = res.First().PredictedInput.Split('-');
                        Console.WriteLine($"Predicted Sequence: {value[0]}, predicted next element {value1.Last()}");
                    }
                    else
                        Console.WriteLine("Nothing predicted :(");
                }

                Console.WriteLine("------------------------------");

                
            }

        private static List<string> DetectAnomaly(Predictor predictor,double[] list, double tolerance = 0.1);
            {

                // Here we traversed the input list one by one
                for (int i = 0; i < list.Length; i++)
                {

                    //Values for the rest of the list will be iteratively referred to, in the following variable.
                    var item = list[i];

                    // We use our trained model in the predictor to predict the next value.
                    var res = predictor.Predict(item);

                    resultOutputStringList.Add("Present element from the input list in the testing sequence: " + item);

                    if (res.Count > 0)
                    {
                        // Here we extracting predicted data and the accuracy level from the predicting output values.
                        var value = res.First().PredictedInput.Split('_');
                        var value1 = res.First().PredictedInput.Split('_');
                        var value2 = res.First().Similarity;

                        // We use exclude for the last element of the list because there is no element after that to detect anomaly.

                        if (i < list.Length - 1)
                        {
                            double nextIndex = i + 1;
                            double nextItem = list[nextIndex];
                            double predictedNextItem = double.Parse(value1.Last());

                            // Anomalyscore variable will be used to check the deviation from predicted item
                            var AnomalyScore = Math.Abs(predictedNextItem - nextItem);
                            var deviation = AnomalyScore / nextItem;

                            if (deviation <= tolerance)
                            {

                                resultOutputStringList.Add("Anomaly not detected in the next value!! and found similar value: " + value2 + "%.");
                                currentAccuracy += value2;

                            }
                            else
                            {
                                resultOutputStringList.Add($" Wow Anomaly detected !!! in the next value. System predict it {predictedNextItem} with similarity: {value2}%, where the actual value is {nextItem}.");
                                i++;

                                // skip to the next element for checking, as we cannot use anomalous element for prediction
                                resultOutputStringList.Add("Anomaly was detected, because of these we skip to the next value in our testing sequence values.");
                                currentAccuracy += value2;
                            }

                        }

                        else
                        {
                            resultOutputStringList.Add("The list is finished because of that we are not go through the further anomaly testing.");
                        }
                    }
                    else
                    {
                        resultOutputStringList.Add("There is nothing to predict !!! So Anomaly cannot be detected.");
                    }

                }


            }



        }
       

    }
}
