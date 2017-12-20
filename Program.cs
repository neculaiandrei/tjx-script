using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace txj_script
{
    class EncoreObj
    {
        
    } 
    class Program
    {
        static bool isHoliday(string category)
        {
            return category.ToLower().Contains("holiday")
                || category.ToLower().Contains("xmas");
        }

        static bool isEarlierThan2018(DateTime date)
        {
            return date < new DateTime(2017, 12, 31);
        }

        static bool isAfter2ndOfChristmas(DateTime date)
        {
            return date > new DateTime(2017, 12, 26);
        }

        static dynamic findMostRecentPlaylist(JArray playlists)
        {
            return
                playlists
                .ToObject<IList<dynamic>>()
                .Aggregate((max, current) =>
                {
                    var currentDateTime = (DateTime)current.Sequence[0].StartDateTime;
                    var maxDateTime = (DateTime)max.Sequence[0].StartDateTime;

                    if (currentDateTime > maxDateTime
                        && isEarlierThan2018(currentDateTime))
                    {
                        return current;
                    }

                    return max;
                });
        }

        static dynamic filterSequence(dynamic encore)
        {
            var filtered = 
                ((JArray)encore.Sequence)
                .ToObject<IList<dynamic>>()
                .Where(s =>
                {
                    var startDateTime = (DateTime) s.StartDateTime;
                    var category = ((JToken)s.Category).Value<string>();
                    return isAfter2ndOfChristmas(startDateTime) && !isHoliday(category);
                });

            return JArray.FromObject(filtered);
        }

        static void ProcessFile(string path)
        {
            var text = File.ReadAllText(path, Encoding.UTF8);
            var playlists = JsonConvert.DeserializeObject<JArray>(text);

            var mostRecentPlaylist = findMostRecentPlaylist(playlists);
            var newSequence = filterSequence(mostRecentPlaylist);

            mostRecentPlaylist.Sequence = newSequence;
            mostRecentPlaylist.CreatedDate = "2017-12-13T00:00:00.1957943Z";
            mostRecentPlaylist.Sequence[0].StartDateTime = "2017-12-26T00:00:00";

            playlists.Add(mostRecentPlaylist);

            var json = playlists.ToString();
            File.WriteAllText(path, json, Encoding.UTF8);
        }

        static void Main(string[] args)
        {
            foreach (var path in args)
            {
                ProcessFile(path);
            }
        }
    }
}
