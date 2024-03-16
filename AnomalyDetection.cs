using NeoCortexApi;   // Importing namespace for classes from NeoCortexApi

namespace anomalydetectionapp
// Declaration of a namespace named anomalydetectionapp, which encapsulates the classes and other types defined within it.

{
    /// <summary>
    /// Implements an experiment that demonstrates how to learn sequences.
    /// </summary>


    public class AnomalyDetection     // Method for anomaly detection
    {
        public static void AnomalyDetectMethod(Predictor predictor, double[] list, double tolerance = 0.1)
        {
            Console.WriteLine("Testing the sequence for anomaly detection: " + string.Join(", ", list) + ".");
            // Print the sequence being tested for anomaly detection

            for (int i = 0; i < list.Length; i++)   // Loop through each item in the list
            {
                var item = list[i];

                // Predict the next item in the sequence

                var res = predictor.Predict(item);

                if (res.Count > 0)    // If prediction result is available
                {
                    var value1 = res.First().PredictedInput.ToString().Split('-');   // Extract predicted value and similarity percentage
                    var value2 = res.First().Similarity;

                    if (i < list.Length - 1)  // If not at the end of the list
                    {
                        int nextIndex = i + 1;
                        double nextItem = list[nextIndex];
                        double predictedNextItem = double.Parse(value1.Last());
                        var AnomalyScore = Math.Abs(predictedNextItem - nextItem);
                        var deviation = AnomalyScore / nextItem;

                        if (deviation <= tolerance)  // If deviation is within tolerance
                        {
                            Console.WriteLine("Anomaly not detected in the next value!! and found similar value: " + value2 + "%.");
                            // No anomaly detected in the next value
                        }
                        else
                        {
                            Console.WriteLine($"Anomaly detected!!! in the next value. System predicts it {predictedNextItem} with similarity: {value2}%, where the actual value is {nextItem}.");
                            i++;
                            // Anomaly detected in the next value
                            Console.WriteLine("Anomaly was detected, because of this we skip to the next value in our testing sequence values.");
                            // Skip to the next value in testing sequence
                        }
                    }
                    else
                    {
                        Console.WriteLine("The list is finished, so we do not go through further anomaly testing.");
                        // End of the list reached
                    }
                }
                else
                {
                    Console.WriteLine("There is nothing to predict!!! So Anomaly cannot be detected.");
                    // No prediction available

                }
            }
        }
    }
}
