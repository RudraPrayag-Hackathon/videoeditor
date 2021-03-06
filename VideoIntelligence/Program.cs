﻿namespace VideoIntelligence
{
    using System;
    using System.IO;
    using ConsumeJson;
    using Google.Cloud.VideoIntelligence.V1;
    using VideoCutterApp;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Service is running.");

            String envName = "GOOGLE_APPLICATION_CREDENTIALS";
            String envValue = @"..\..\..\Credentials\json1.json";

            if (Environment.GetEnvironmentVariable(envName) == null)

                Environment.SetEnvironmentVariable(envName, envValue);

            var client = VideoIntelligenceServiceClient.Create();
            var request = new AnnotateVideoRequest()
            {
                InputContent = Google.Protobuf.ByteString.CopyFrom(File.ReadAllBytes(@"..\..\..\Videos\InputVideo\testInput.mp4")),
                Features = { Feature.LabelDetection }
            };

            var op = client.AnnotateVideo(request).PollUntilCompleted();

            TimeDetails timeDetails = ConsumeJsonFile.ReturnTimeOffsets("person", op.Result.AnnotationResults);

            string duration = (timeDetails.EndTime - timeDetails.StartTime).ToString();

            VideoEditor.CutVideo(@"..\..\..\Videos\InputVideo\testInput.mp4", timeDetails.StartTime.ToString(), duration , @"..\..\..\Videos\OutputVideo\Output.mp4");

            Console.WriteLine("Video is edited. Press key to exit.");

            Console.ReadKey();

        }
    }
}
