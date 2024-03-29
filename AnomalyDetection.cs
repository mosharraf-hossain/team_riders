using NeoCortexApi;

namespace anomalydetectionapp

{
    /// <summary>
    /// Class which contains method for anomaly detection from a sequence of input values.
    /// </summary>
    public class AnomalyDetection
    {
        /// <summary>
        /// Detects anomalies in a sequence of values using trained model predictor.
        /// </summary>
        /// <param name="predictor">The predictor model used for prediction.</param>
        /// <param name="list">The sequence of values to test for anomalies.</param>
        /// <param name="tolerance">The tolerance level ratio for anomaly detection (default is 0.1).</param>
        public static void AnomalyDetectMethod(Predictor predictor, double[] list, double tolerance = 0.1)
        {
            Console.WriteLine("Testing the sequence for anomaly detection: " + string.Join(", ", list) + ".");

            for (int i = 0; i < list.Length; i++)
            {
                var item = list[i];

                var res = predictor.Predict(item);

                if (res.Count > 0)
                {
                    var value1 = res.First().PredictedInput.ToString().Split('-');
                    var value2 = res.First().Similarity;

                    if (i < list.Length - 1)
                    {
                        int nextIndex = i + 1;
                        double nextItem = list[nextIndex];
                        double predictedNextItem = double.Parse(value1.Last());
                        var AnomalyScore = Math.Abs(predictedNextItem - nextItem);
                        var deviation = AnomalyScore / nextItem;

                        if (deviation <= tolerance)
                        {
                            Console.WriteLine("Anomaly not detected in the next value!! and found similar value: " + value2 + "%.");
                        }
                        else
                        {
                            Console.WriteLine($"Anomaly detected!!! in the next value. System predicts it {predictedNextItem} with similarity: {value2}%, where the actual value is {nextItem}.");
                            i++;
                            Console.WriteLine("Anomaly was detected, because of this we skip to the next value in our testing sequence values.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The list is finished, so we do not go through further anomaly testing.");
                    }
                }
                else
                {
                    Console.WriteLine("There is nothing to predict!!! So Anomaly cannot be detected.");
                }
            }
        }
    }
}
