namespace anomalydetectionapp;

class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, List<double>> mysequences = new Dictionary<string, List<double>>();
        
        //have to make class for reading data values

        mysequences.Add("S1", new List<double>(new double[] { 0.0, 1.0, 2.0, 3.0, 4.0, 2.0, 5.0, }));
        mysequences.Add("S2", new List<double>(new double[] { 8.0, 1.0, 2.0, 9.0, 10.0, 7.0, 11.00 }));
        MultiSequenceLearning myexperiment = new MultiSequenceLearning();
        var predictor = myexperiment.Run(mysequences);


    }
}
