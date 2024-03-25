# Implement anomaly detection sample (WS2023-24)

SE project repository for WS2023-24


# Team members:
Md Zahid Hasan(1396470)
Md Mosharraf Hossain(1386448)

# Introduction:

Hierarchical Temporal Memory (HTM) technology is a machine learning framework inspired by the human neocortex's structure and function.
It emulates the brain's ability to learn, recognize patterns, and make predictions from sensory data. HTM models the neocortex with layers
of neurons arranged hierarchically, processing information in a sequence and learning temporal patterns dynamically. Key components include 
spatial pooling, which creates sparse distributed representations of input data, and temporal memory, enabling learning sequences of patterns over time. 
HTM systems use these learned models for real-time inference, making predictions, and detecting anomalies in streaming data. 
Its applications span anomaly detection, time-series prediction, natural language processing, and sensor data analysis. 
With continuous learning capabilities, HTM adapts to changing environments and dynamic datasets effectively. 
As a result, it offers promising avenues for understanding intelligence and building intelligent systems. 
Ongoing research in neuroscience and machine learning fuels HTM's evolution, 
driving advancements in artificial intelligence and cognitive computing. 

Our project would therefore use the NeoCortex API's multisequencelearning class to implement an anomaly detection system. Specifically, 
we train our HTM Engine by reading numerical sequences from multiple JSON files inside a folder, then use the trained engine to identify patterns and identify anomalies.

# Required tools for the project:

To run this project, we need:

.NET 8.0 SDK
Nuget package: NeoCortexApi Version= 1.1.4
For code debugging, we are using IDE Visual Studio Community 2022.

# Usage:
to run this project, WE need to follow the following steps:

1. Install .NET SDK. 
2. Then use code editor/IDE of your choice Such as Visual Studio Community 2022 or Visual Studio Code.
3. Create a new console project and place all the C# codes inside your project folder.
4. Add/reference Nuget package NeoCortexApi v1.1.4 to this project.
5. Place numerical sequence JSON Files (datasets) under relevant folders respectively. All the folders should be inside the project folder.

Our project is based on NeoCortex API. 

# Details of the project: 
Our HTM Engine has been trained using the MultiSequenceLearning class in the NeoCortex API. The first step in training the HTM Engine will be 
reading and utilizing data from our training (learning) and predicting (predictive) folders, which are both present as numerical sequences in 
JSON files in the "predicting" and "training" folders inside the project directory. 

# Data format:
For this project, we used real-time data from Numenta Anomaly Benchmark (NAB). We have taken the tweet count of Google per hour as numerical sequences, 
which are stored inside the JSON files. Example of a JSON file within the training and predicting folder.

According to the dataset, we used a total of 32 hours (16 hours for training and 16 hours for predicting) of data where the beginning time was 01.03.2015 at 12 am and the end time was 01.03.2015 at 4 pm for training data and also the beginning time was 02.03.2015 at 12 am and the end time was 02.03.2015 at 4 pm for predicting data. 



Below we give our data sequences where the sequences are in JSON files. We keep our dataset in two individual folders which are training_files (for training data where 4 files) and predicting_files (for predicting data where also 4 files).  

For example, an hourly sequence has a list of 12 numerical values per hour: [14, 14, 9, 13, 7, 7, 5, 13, 11, 7, 9, 9]. Our JSON structure is like the data given below:

```

{
  "sequences": [
    [13, 16, 9, 5,10, 7, 5, 9, 13, 14, 9, 8],
    [12, 4, 11, 11, 11, 18, 6, 5, 8, 9, 5, 15],
    [13, 6, 14, 7, 1, 8, 9, 5, 6, 10, 6, 6],
    [10, 16, 8, 9, 5, 16, 6, 10, 11, 6,  16, 14]
  ]
}

--------------------------------------------------------------
--------------------------------------------------------------

{
  "sequences": [
    [14, 14, 9, 13, 7, 7, 5, 13, 11, 7, 9, 9],
    [15, 6, 9, 5, 12, 10, 10, 10, 15, 12, 17, 14],
    [9, 9, 7, 11, 11, 7, 7, 12, 6, 12, 6, 12],
    [10, 6, 12, 11, 8, 8, 10, 11, 5, 3, 8, 7]
  ]
}
```


# Encoding Process:
Our input data must be encoded so that our HTM Engine can process it.

We are using the following settings because we will be training and testing data that falls between the range of integer values 
between 0-100 without any periodicity. Since we only expect values to fall within this range, the minimum and maximum values 
are set to 0 and 100, respectively. It is necessary to modify these values in other use cases.
```

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
# Complete process for Encoding:

```
 public Predictor Run(Dictionary<string, List<double>> sequences)
        {
            Console.WriteLine($"Hello NeocortexApi! Experiment {nameof(MultiSequenceLearning)}");

            int inputBits = 100;
            int numColumns = 1024;

            HtmConfig cfg = new HtmConfig(new int[] { inputBits }, new int[] { numColumns })
            {
                Random = new ThreadSafeRandom(42),

                CellsPerColumn = 25,
                GlobalInhibition = true,
                LocalAreaDensity = -1,
                NumActiveColumnsPerInhArea = 0.02 * numColumns,
                PotentialRadius = (int)(0.15 * inputBits),
                //InhibitionRadius = 15,

                MaxBoost = 10.0,
                DutyCyclePeriod = 25,
                MinPctOverlapDutyCycles = 0.75,
                MaxSynapsesPerSegment = (int)(0.02 * numColumns),

                ActivationThreshold = 15,
                ConnectedPermanence = 0.5,

                // Learning is slower than forgetting in this case.
                PermanenceDecrement = 0.25,
                PermanenceIncrement = 0.15,

                // Used by punishing of segments.
                PredictedSegmentDecrement = 0.1
            };

            double max = 100;

            Dictionary<string, object> settings = new Dictionary<string, object>()
            {
                { "W", 15},
                { "N", inputBits},
                { "Radius", -1.0},
                { "MinVal", 0.0},
                { "Periodic", false},
                { "Name", "scalar"},
                { "ClipInput", false},
                { "MaxVal", max}
            };

            EncoderBase encoder = new ScalarEncoder(settings);

            return RunExperiment(inputBits, cfg, encoder, sequences);
        }
```
#  Execution Process of the project

We carry out our project in the manner described below:

In our project, we read all the JSON files from the `training_files` and `predicting_folder` using the `JSONFolderreader` class. 
We then convert the read data into sequences. 

Sequences from the training and predicting folder will used for training our model while the predicting folder will only be used for predicting. Model training will be done using multisequencelearning class.

After that, we use the `AnomalyDetection` class to detect anomalies. We can pass the tolerance value from outside to `AnomalyDetectMethod` method.  

  
         




