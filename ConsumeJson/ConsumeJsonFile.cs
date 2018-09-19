using System;

namespace ConsumeJson
{
    using Newtonsoft.Json;

    public class ConsumeJsonFile
    {
        public static TimeDetails ReturnTimeOffsets(string entityName ,dynamic annotationResults)
        {

        
            double startTimeInSec = 0.00;
            double endTimeInSec = 0.00;
            bool entityFound = false;
 
            foreach (var annotationResult in annotationResults)
            {

              var shotLabelAnnotations = annotationResult.ShotLabelAnnotations;


                foreach (var shotLabelAnnotation in shotLabelAnnotations)
                {
                    var categoryEntities = shotLabelAnnotation.CategoryEntities;
                    var entity = shotLabelAnnotation.Entity;
                    var segments = shotLabelAnnotation.Segments;

                    if (categoryEntities != null)
                    {

                        foreach (var categoryEntity in categoryEntities)
                        {
                            if (categoryEntity.Description == entityName)
                            {
                                foreach (var segment in segments)
                                {
                                    var start = segment.Segment.StartTimeOffset.Seconds;
                                    var end = segment.Segment.EndTimeOffset.Seconds;

                                    if (!entityFound)
                                    {
                                        startTimeInSec = start;
                                        endTimeInSec = end;
                                        entityFound = true;
                                    }
                                    else
                                    {
                                        if (startTimeInSec > start)
                                            startTimeInSec = start;
                                        if (endTimeInSec < end)
                                            endTimeInSec = end;
                                    }
                                }
                            }
                        }
                    }
                    else if (entity.Description == entityName || entity.Description == "human")
                    {
                        foreach (var segment in segments)
                        {
                            var start = segment.Segment.StartTimeOffset.Seconds;
                            var end = segment.Segment.EndTimeOffset.Seconds;


                            if (!entityFound)
                            {
                                startTimeInSec = start;
                                endTimeInSec = end;
                                entityFound = true;
                            }
                            else
                            {
                                if (startTimeInSec > start)
                                    startTimeInSec = start;
                                if (endTimeInSec < end)
                                    endTimeInSec = end;
                            }
                        }

                    }
                }
            }

            var startTime = TimeSpan.FromSeconds(startTimeInSec);
            var endTime = TimeSpan.FromSeconds(endTimeInSec);

            return new TimeDetails { StartTime = startTime, EndTime = endTime };
        }

        private static double ConvertSecondsStringToDouble(string value)
        {
            var time = value.Replace("s", "");
           
            return Convert.ToDouble(value.Replace("s", ""));
        }


        private static double ConvertDurationToDouble(dynamic value)
        {
            var time = value.Replace("s", "");

            return value.Seconds + value.Nanos;
        }
    }
    public class TimeDetails
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
