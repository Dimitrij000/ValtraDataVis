using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public static class ValtraParser
{
    public static List<ValtraRecord> Parse(string path)
    {
        var records = new List<ValtraRecord>();
        var ci = CultureInfo.InvariantCulture;
        var lines = File.ReadAllLines(path);

        bool headerFound = false;

        foreach (var line in lines)
        {
            // Ищем строку с заголовками
            if (!headerFound)
            {
                if (line.Contains("GPSTime") && line.Contains("Easting") && line.Contains("Northing"))
                {
                    headerFound = true;
                }
                continue;
            }

            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Данные разделены символом |
            var cols = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (cols.Length < 7)
                continue;

            try
            {
                var rec = new ValtraRecord
                {
                    gpsTime = double.Parse(cols[1].Trim(), ci),
                    easting = double.Parse(cols[5].Trim(), ci),
                    northing = double.Parse(cols[6].Trim(), ci)
                };

                records.Add(rec);
            }
            catch
            {
                // пропускаем строки, которые не парсятся
                continue;
            }
        }

        return records;
    }
}
