
namespace CoreOPerations
{
    public static class GenericHelper
    {
        public static string GetCustomFileName(string prName)
        {
            var tickToNearestSeconds = DateTime.UtcNow.Round(TimeSpan.TicksPerSecond).Ticks;
           return $"{prName}_{tickToNearestSeconds}.json";
        }

        // Assumption  fileName is in the format fileName_ToDateFile();
        public static (Dictionary<string, List<long>>, Dictionary<long, List<string>>) GetFileNameHashMap(string[] files)
        {
            var dicNameAskey = new Dictionary<string, List<long>>();
            var dicDateAskey = new Dictionary<long, List<string>>();
            foreach (var file in files)
            {
                // assumption filename should be in the format fileName_ToDateFile();
                var fileNameWithoutExtension = file.Replace(".json","");
                var fileNameAndDate = fileNameWithoutExtension.Split('_');
                var fileName = fileNameAndDate[0].Trim().ToLower();
                long ticksNearestSecond = long.Parse(fileNameAndDate[1]);

                if (dicNameAskey.ContainsKey(fileName))
                {
                    var list = dicNameAskey[fileName];
                    list.Add(ticksNearestSecond);
                }
                else
                {
                    var list = new List<long> ();
                    list.Add(ticksNearestSecond);
                    dicNameAskey.Add(fileName, list);
                }

                // second dictionary
                if (dicDateAskey.ContainsKey(ticksNearestSecond))
                {
                    var list = dicDateAskey[ticksNearestSecond];
                    list.Add(fileName);
                }
                else
                {
                    var list = new List<string>();
                    list.Add(fileName);
                    dicDateAskey.Add(ticksNearestSecond, list);
                }
            }
            return (dicNameAskey, dicDateAskey);
        }

        public static DateTime Round(this DateTime date, long ticks = TimeSpan.TicksPerSecond)
        {
            if (ticks > 1)
            {
                var frac = date.Ticks % ticks;
                if (frac != 0)
                {
                    // Rounding is needed..
                    if (frac * 2 >= ticks)
                    {
                        // round up
                        return new DateTime(date.Ticks + ticks - frac);
                    }
                    else
                    {
                        // round down
                        return new DateTime(date.Ticks - frac);
                    }
                }
            }
            return date;
        }
    }    
}
