# Implement anomaly detection sample (WS2023-24)

SE project repository for WS2023-24

# Introduction: 
Our project would therefore use the NeoCortex API's multisequencelearning class to implement an anomaly detection system. Specifically, 
we train our HTM Engine by reading numerical sequences from multiple JSON files inside a folder, then use the trained engine to identify patterns and identify anomalies.

# Required tools for the project:

To run this project, we need:

1. .NET 8.0 SDK
2. Nuget package: NeoCortexApi Version = 1.1.4
3. Nuget package: XPlot.Plotly = 4.0.6
4. For code debugging, we are using IDE Visual Studio Community 2022/ Visual Studio Code.

# Usage:
to run this project, We need to follow the following steps:

1. Install .NET SDK. 
2. Then use the code editor/IDE of your choice Such as Visual Studio Community 2022.
3. Add/reference nuget packages to this project.
4. Place numerical sequence JSON Files (datasets) under relevant folders respectively. All the folders should be inside the project folder.
5. Enter dotnet run. It will ask you to enter tolerance value ratio. Give a ratio and then press enter.
6. Output will be persisted on your terminal. Also, graph with anomalies will pop-up and displayed on your default browser.

Our project is based on NeoCortex API. 

# Details of the project: 
Our HTM Engine has been trained using the MultiSequenceLearning class in the NeoCortex API. The first step in training the HTM Engine will be 
reading and utilizing data from our  `training_files` (learning) and `predicting_files` (predictive) folders, which are both present as numerical sequences in 
JSON files in the `predicting_files` and `training_files` folders inside the project directory. 

# Data format:
For this project, we used two datasets. 

1. We used real-time data from the Numenta Anomaly Benchmark (NAB). We have taken the tweet count of Google per hour as numerical sequences, which are stored inside the JSON files. Example of a JSON file within the `training_files` and `predicting_files` folder.

According to the dataset, we used a total of 32 hours (16 hours for training and 16 hours for predicting) of data where the beginning time was 01.03.2015 at 12 am and the end time was 01.03.2015 at 4 pm for training data and also the beginning time was 02.03.2015 at 12 am and the end time was 02.03.2015 at 4 pm for predicting data. 

Below we give our data sequences where the sequences are in JSON files. We keep our dataset in two individual folders which are `training_files` (for training data where 4 files) and `predicting_files` (for predicting data where also 4 files).  

For example, an hourly sequence has a list of 12 numerical values per hour: [14, 14, 9, 13, 7, 7, 5, 13, 11, 7, 9, 9]. Our JSON structure is like the data given below: 

If you want to visit the dataset then just click the [link](https://github.com/numenta/NAB/blob/master/data/realTweets/Twitter_volume_GOOG.csv)

```json

{
  "sequences": [
    [13, 16, 9, 5,10, 7, 5, 9, 13, 14, 9, 8],
    [12, 4, 11, 11, 11, 18, 6, 5, 8, 9, 5, 15],
    [13, 6, 14, 7, 1, 8, 9, 5, 6, 10, 6, 6],
    [10, 16, 8, 9, 5, 16, 6, 10, 11, 6,  16, 14]
  ]
}
```

2. We also use another kind of dataset as JSON files (which is fabricated) to predict anomaly detection, where the full process is similar to the above. We did this to test our HTM process and the accuracy level test for various types of data. The fabricated dataset is based on the temperature of Dhaka, Bangladesh, in the year 2023. Firstly, we keep both training and predicting datasets in reserved dataset folders then when we need to, we take them from these folders and insert the data into the `training_files` and `predicting_files` folders.  

There are also 4 files each file has 5 sequences where each sequence has 10 numerical data (means 10 days temperature value) for training, which is located in the `training_files` folder, and also 4 files each file has 5 sequences where each sequence has 10 numerical data (means 10 days temperature value) for predicting which is located in the `predicting_files` folder. We took a total of 400 days of data. 

Our Dataset structure is like the data given below:

If you want to visit the dataset then just click the [link](https://www.accuweather.com/en/bd/dhaka/28143/march-weather/28143?year=2023)

```json
{ 
"sequences": [ 
[35, 26, 38, 28, 33, 27, 32, 29, 36, 25], 
[34, 26, 33, 21, 37, 26, 31, 25, 39, 31], 
[37, 29, 36, 28, 39, 33, 32, 25, 33, 27], 
[23, 17, 24, 16, 22, 15, 23, 14, 21, 14], 
[18, 14, 22, 15, 17, 13, 22, 15, 24, 17] 
] 
}
```

# Encoding Process:

Our input data must be encoded so that our HTM Engine can process it.

We are using the following settings, because we will be training and testing data that falls between the range of integer values 
between 0-100 without any periodicity. Since we only expect values to fall within this range, the minimum and maximum values 
are set to 0 and 100, respectively. It is necessary to modify these values in other use cases.

"W": 21 - The width of the output vector. 
"N": 1024 - The number of bits to use for encoding the input range. 
“Radius": -1.0 - The radius of the output vector. 
"MinVal": 0.0 - The minimum value of the input range. 
"MaxVal ": 100.0 - The maximum value of the input range. 
"Periodic": false - Whether the encoder should wrap values around the ends of the input range.
"Name": integer  -  A descriptive name for the settings.
"ClipInput": false - Whether to clip input values to the input range.
"MaxVal": max - The maximum value of the input range.

If you want to see the full code you can view it by clicking the [link](https://github.com/mosharraf-hossain/team_riders/blob/main/Multisequencelearning.cs)

```csharp

            int inputBits = 121;
            int numColumns = 1210;
            ---------------------
            ---------------------
            double max = 100;


             Dictionary<string, object> settings = new Dictionary<string, object>()
               {
                   { "W", 21},
                   { "N", inputBits},
                   { "Radius", -1.0},
                   { "MinVal", 0.0},
                   { "Periodic", false},
                   { "Name", "integer"},
                   { "ClipInput", false},
                   { "MaxVal", max}
               };
```

# HTM Configuration:

We set up parameters where the number of bits is used for encoding the input range (121) and the number of columns in the HTM network (1210). We also created the encoder "EncoderBase encoder = new ScalarEncoder(settings);" and then run the experiment with the configured parameters, where the encoding process was given previously. If you want to see the full code you can view it by clicking the [link](https://github.com/mosharraf-hossain/team_riders/blob/main/Multisequencelearning.cs)

```csharp

  public Predictor Run(Dictionary<string, List<double>> sequences)
 {

     Console.WriteLine($"Hello NeocortexApi! Experiment {nameof(MultiSequenceLearning)}");
     
     int inputBits = 121;
     int numColumns = 1210;

     HtmConfig cfg = new HtmConfig(new int[] { inputBits }, new int[] { numColumns })
     {
         Random = new ThreadSafeRandom(42),

         CellsPerColumn = 21,
         GlobalInhibition = true,
         LocalAreaDensity = -1,
         
     };
    ................................................
    ................................................
```
#  Execution Process of the project

We carry out our project in the manner described below:

Keep all the JSON files inside the folder `training_files` and `predicting_files`, present in project directory. To execute our project, from terminal, write dotnet run and press enter.  After that, add tolerance value ratio which you would like to use, for instance: 0.2.

````terminal
mosharraf@Mds-MacBook-Air team_riders % dotnet run

Enter the tolerance value for anomaly detection experiment (Example: 0.1):

0.2
````

We have used JSON format over other formats like CSV or XML because it can handle complex and large amounts of data. We use JsonFolderReader class under [jsonfileread.cs](https://github.com/mosharraf-hossain/team_riders/blob/main/jsonfileread.cs) to read the json files from the folders.

````csharp
public JsonFolderReader(string folderPath)
    {
        AllSequences = new List<SequencesContainer>();

        // Get all JSON files in the specified folder
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
.......

````

We extract the numerical sequences from JSON files present inside both the training and predicting folders and use them to train the HTM model using the multisequencelearning class. Later, data extracted from the predicting folder is used for anomaly detection. Sequences from the training and predicting folder will used for training our model while the predicting folder will only be used for predicting. We use dictionary to store sequences. The folders are in the same project directory:

````csharp
            // Create a dictionary to store sequences
            Dictionary<string, List<double>> mysequences = new Dictionary<string, List<double>>();

            ................
            .............

            // Get the solution directory path
            string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

            // Construct paths for training and predicting folders
            string trainingfolderPath = Path.Combine(solutionDirectory, "training_files");
            string predictingfolderPath = Path.Combine(solutionDirectory, "predicting_files");
````

After extracting data from JSON files, we converted it into a format suitable for HTM training.

````csharp  

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

````
We use multisequencelearning classes for training our HTM model like below:

````csharp
           // Train the model using MultiSequenceLearning
            MultiSequenceLearning myexperiment = new MultiSequenceLearning();
            var predictor = myexperiment.Run(mysequences);
            predictor.Reset();
````

We used the predictor object as a trained HTM model to predict anomalies from our extracted data from predicting_files.

We use list of lists to store numerical sequences which we are using for anomaly detection, and anomaly indices: indices where we can find anomalies. This is important for plotting. More about this later.

````csharp
            // Create lists to store all data and anomaly indices
            List<double[]> allData = new List<double[]>();
            List<List<int>> allAnomalyIndices = new List<List<int>>();
````

Using the `AnomalyDetectMethod´ method of the [AnomalyDetection](https://github.com/mosharraf-hossain/team_riders/blob/main/AnomalyDetection.cs) class, we pass the numerical sequences one by one to our predictor model to detect anomalies. Please note that before passing list of numerical sequences, we are trimming a few values in the beginning randomly.

````csharp

                    // Convert the sequence to a list of double values
                    List<double> inputlist = sequence.Select(x => (double)x).ToList();
                    double[] inputArray = inputlist.ToArray();

                    // Trim some values randomly in the beginning of the sequence
                    Random random = new Random();
                    int trimCount = random.Next(1, 4);
                    double[] inputTestArray = inputArray.Skip(trimCount).ToArray();

                    // Get the anomaly indices from the AnomalyDetection class
                    List<int> anomalyIndices = AnomalyDetection.AnomalyDetectMethod(predictor, inputTestArray, tValue);
````

In the end, we are going to use AnomalyPlotter class under [PlotAnomaly.cs](https://github.com/mosharraf-hossain/team_riders/blob/main/PlotAnomaly.cs) to plot our anomalies.

````csharp

AnomalyPlotter.PlotGraphWithAnomalies(allData, allAnomalyIndices);

````

We are going to iterate through each value of a numerical sequence which is passed through the inputTestArray parameter to the `AnomalyDetectMethod` method. The trained model output: predictor is used to predict the next element for comparison. We use an anomalyscore ratio to calculate and compare to detect anomalies If the prediction crosses a certain tolerance level, it is taken as an anomaly. We can pass the tolerance value from outside to the method mentioned above.

```csharp
var res = predictor.Predict(item);
```
The prediction derived from the predictor model is in the format of, "NeoCortexApi.Classifiers.ClassifierResult`1[System.String]". We use string operations to extract data from it. 

```csharp
var value1 = res.First().PredictedInput.ToString().Split('-');
var value2 = res.First().Similarity;
```

Normally output from HTM is in the following format when we pass a numerical value 14 for example:
```csharp
S3_11-5-12-10-14-13 - 100
S1_5-6-16-10-4-11-7 - 5
.....
```

The first line has the best prediction which the HTM model predicts, with accuracy. We can easily derive the predicted value, which will come after 14 (in this case, it is 13). The string operations are used to get these values. Later we are going to use this to determine anomalies.

````csharp

                ..............

                        int nextIndex = i + 1;
                        double nextItem = list[nextIndex];
                        double predictedNextItem = double.Parse(value1.Last());

                        // Calculating the anomaly score and deviation
                        var AnomalyScore = Math.Abs(predictedNextItem - nextItem);
                        var deviation = AnomalyScore / nextItem;


                ..........
````

We are using AnomalyScore, which is nothing, but the absolute value of the ratio of differences between HTM´s predicted number and actual number. If the ratio exceeds tolerancevalue, we mark it as an anomaly, otherwise, it is not. When an anomaly is detected, we skip that element in the list (we did not pass that value to HTM in the loop).

We use accuracyPerList to record accuracy per numerical sequence tested. recordAccuracy is collected from inside each loop run, which indicates HTM model´s accuracy. We also add index positions of anomalies to anomalyIndices, when we encounter indices of anomalies in loop. totalAccuracy and listCount is used to calculate average accuracy of the whole experiment.

````csharp

// Calculating the accuracy of the HTM model for each list
double accuracyPerList = (recordAccuracy / list.Length);
````

We use static variables to access these from outside, i.e: in Program.cs.

````csharp

// Static variables to store total accuracy and list count
public static double totalAccuracy { get; set; }
public static double listCount { get; set; }

````

The AnomalyPlotter class is used to plot graphs of sequences of data and their anomalies. The class contains one static method, PlotGraphWithAnomalies, which takes two parameters: allData: a list of arrays of doubles, where each array represents a sequence of data, and allAnomalyIndices: a list of lists of integers, where each list represents the indices of the anomalies in a sequence.

````csharp
public static void PlotGraphWithAnomalies(List<double[]> allData, List<List<int>> allAnomalyIndices)
````

The method works by creating a line graph for each sequence and a scatter plot for its anomalies. It then combines all the graphs into a chart and displays it. This method uses the XPlot.Plotly library to create the graphs and the chart.

Two lists, allGraphs and allAnomalies, are initialized to store the graphs for the data sequences and their anomalies respectively.

````csharp
List<Scatter> allGraphs = new List<Scatter>();
List<Scatter> allAnomalies = new List<Scatter>();
````
For each sequence in allData and its corresponding anomaly indices in allAnomalyIndices, a graph for the sequence and a graph for the anomalies are created.

````csharp
for (int i = 0; i < allData.Count; i++)
{
  double[] data = allData[i];
  List<int> anomalyIndices = allAnomalyIndices[i];
  ...

}
````

A Scatter object graph is created for the sequence. The x-values are the indices of the data points, and the y-values are the data points themselves. The graph is a line graph and is named "Sequence" followed by the index of the sequence.

````csharp
var graph = new Scatter
{
    x = Enumerable.Range(0, data.Length).ToArray(),
    y = data,
    mode = "lines",
    name = "Sequence" + i
};
````

Another Scatter object anomalies is created for the anomalies in the sequence. The x-values are the indices of the anomalies, and the y-values are the values of the anomalies. The graph is a scatter plot and is named "Anomalies in sequence" followed by the index of the sequence. The color of the markers is set to red.

````csharp
var anomalies = new Scatter
{
    x = anomalyIndices.ToArray(),
    y = anomalyIndices.Select(index => data[index]).ToArray(),
    mode = "markers",
    name = "Anomalies in sequence" + i,
    marker = new Marker { color = "red" }
};

````

The graph and anomalies are added to allGraphs and allAnomalies respectively.

````csharp

allGraphs.Add(graph);
allAnomalies.Add(anomalies);

````

A Chart object chart is created by plotting all the graphs in allGraphs and allAnomalies. The title of the chart and the labels of the x-axis and y-axis are set. Finally, the chart is displayed.

````csharp

var chart = Chart.Plot(allGraphs.Concat(allAnomalies));
chart.WithTitle("Graph with Anomalies");
chart.WithXTitle("X-axis(Index of data point)");
chart.WithYTitle("Y-axis(Value of data point)");
chart.Show();

````

# Results:

Once the experiment concludes, the anomaly detection results are persisted to the screen, along with HTM accuracy for each individual number sequences and overall HTM accuracy for the whole experiment, like [this](https://github.com/mosharraf-hossain/team_riders/blob/main/output/exp_outputs/Fabricated%20data%20output/run1_log.txt). After experiment is completed, the plotted graph pops up in the default browser like this. Red dots mark the anomalies in our numerical sequence data from predicting folder.

![Plot](https://github.com/mosharraf-hossain/team_riders/blob/main/output/exp_outputs/Fabricated%20data%20output/run2_jpg.png)

Overall, we conducted anomaly detection experiments on two distinct [datasets](https://github.com/mosharraf-hossain/team_riders/tree/main/output/used_datasets). 

We executed three experiments with varying tolerance values for the NAB dataset (Real data) and two experiments for the Weather Fabricated dataset (Fabricated). Our results indicated an average accuracy of *22.58%* for the NAB dataset and *25.75%* for the weather dataset. All the output logs are stored in this [folder](https://github.com/mosharraf-hossain/team_riders/tree/main/output/exp_outputs).

Overall, in conclusion, we can say that the HTM accuracy can be improved by using more data. We were not able to train large amounts of data due to limitations of our local machine. However, we can try using more number of numerical sequences in cloud.


