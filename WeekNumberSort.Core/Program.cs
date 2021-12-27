using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;


var cultureInfo = new CultureInfo("fr-FR");
var calendar = cultureInfo.Calendar;
var calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
var sourceDirectory = args.Length == 0 ? "." : args[0];
var movedFilesNumber = 0;
var failedMovedFilesNumber = 0;
var availablePatternExtensions = new List<string>() { "*.jpg", "*.png" };
foreach (var patternExtension in availablePatternExtensions)
{
    foreach (var fullFileName in Directory.EnumerateFiles(sourceDirectory, patternExtension))
    {
        var createdDate = File.GetCreationTime(fullFileName);
        var modifiedDate = File.GetLastWriteTime(fullFileName);
        var date = createdDate < modifiedDate ? createdDate : modifiedDate;
        var weekOfYear = calendar.GetWeekOfYear(date, calendarWeekRule, firstDayOfWeek);
        var formattedWeekOfYear = weekOfYear > 9 ? weekOfYear.ToString() : "0" + weekOfYear;
        var year = date.Year;
        var oldDirectoryName = Path.GetDirectoryName(fullFileName);
        var fileName = Path.GetFileName(fullFileName);
        var newDirectoryName = Path.Combine(oldDirectoryName, year + "-" + formattedWeekOfYear);
        var newFileName = Path.Combine(newDirectoryName, fileName);
        Directory.CreateDirectory(newDirectoryName);
        try
        {
            Directory.Move(fullFileName, newFileName);
            movedFilesNumber++;
        }
        catch
        {
            failedMovedFilesNumber++;
        }
    }
}
Console.WriteLine(movedFilesNumber + " files moved");
Console.WriteLine(failedMovedFilesNumber + " files not moved");

