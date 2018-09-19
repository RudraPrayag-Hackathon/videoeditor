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
            String envName = "GOOGLE_APPLICATION_CREDENTIALS";
            String envValue = @"C:\Users\rekha.kulkarni\Downloads\json1.json";

            if (Environment.GetEnvironmentVariable(envName) == null)

                Environment.SetEnvironmentVariable(envName, envValue);

            var client = VideoIntelligenceServiceClient.Create();
            var request = new AnnotateVideoRequest()
            {
                InputContent = Google.Protobuf.ByteString.CopyFrom(File.ReadAllBytes(@"C:\Videos\testInput.mp4")),
                Features = { Feature.LabelDetection }
            };

            var op = client.AnnotateVideo(request).PollUntilCompleted();

            TimeDetails timeDetails = ConsumeJsonFile.ReturnTimeOffsets("person", op.Result.AnnotationResults);

            string duration = (timeDetails.EndTime - timeDetails.StartTime).ToString();

            VideoEditor.CutVideo(@"C:\Videos\testInput.mp4", timeDetails.StartTime.ToString(), duration , @"C:\Videos\vpOp.mp4");

            Console.ReadKey();

        }
    }
}