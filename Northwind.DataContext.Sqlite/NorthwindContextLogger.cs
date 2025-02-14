using static System.Environment;

namespace Northwind.EntityModels;

public class NorthwindContextLogger
{
    public const string logFolder = "book-logs";

    public static void WriteLine(string message)
    {
        if(!Directory.Exists(logFolder))
        {
            Directory.CreateDirectory(logFolder);
        }
        string folder = Path.Combine(GetFolderPath(SpecialFolder.DesktopDirectory), logFolder);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string path = Path.Combine(folder, $"northwindlog-{dateTimeStamp}.txt");


        using(StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(message);
        }



    }
}