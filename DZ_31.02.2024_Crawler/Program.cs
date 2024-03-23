//Написать класс CrawlerPathLogger, который выполняет логирование директорий и файлов этих директорий на заданном диске.

using static System.Console;

public class CrawlerPathLogger
{
    private string[] DirsArr;
    private string FilePathLog;
    private string LogicDiskLetter;

    public string[] GetDirsArr()
    {
        return DirsArr;
    }

    public void SetDirsArr(string[] dirs)
    {
        DirsArr = dirs;
    }

    public string GetFilePathLog()
    {
        return FilePathLog;
    }

    public void SetFilePathLog(string filePath)
    {
        FilePathLog = filePath;
    }

    public string GetLogicDiskLetter()
    {
        return LogicDiskLetter;
    }

    public void SetLogicDiskLetter(string diskLetter)
    {
        LogicDiskLetter = diskLetter;
    }

    public void RecursiveDirectoryTraversal(string path)
    {
        try
        {
            DirsArr = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
        }
        catch (UnauthorizedAccessException ex)
        {
            WriteLine($"Ошибка доступа к каталогу: {ex.Message}");
        }
    }

    public void WriteToLogFile()
    {
        using (StreamWriter sw = File.CreateText(FilePathLog))
        {
            foreach (string dir in DirsArr)
            {
                sw.WriteLine($"{DateTime.Now}, {dir}");

                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    sw.WriteLine($"    -> {fileInfo.CreationTime}, {fileInfo.Name}, {fileInfo.Extension}");
                }

                string[] subdirs = Directory.GetDirectories(dir);
                foreach (string subdir in subdirs)
                {
                    sw.WriteLine($"    -> {DateTime.Now}, {subdir}");

                    string[] subFiles = Directory.GetFiles(subdir);
                    foreach (string subFile in subFiles)
                    {
                        FileInfo subFileInfo = new FileInfo(subFile);
                        sw.WriteLine($"        -> {subFileInfo.CreationTime}, {subFileInfo.Name}, {subFileInfo.Extension}");
                    }
                }
            }
        }
    }

    public void ReadFromFile()
    {
        try
        {
            using (StreamReader sr = new StreamReader(FilePathLog))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            WriteLine($"Произошла ошибка при чтении файла: {ex.Message}");
        }
    }
}

class Program
{
    static void Main()
    {
        CrawlerPathLogger crawler = new CrawlerPathLogger();
        crawler.SetDirsArr(new string[] { "dir1", "dir2" });
        crawler.SetFilePathLog("logFile.txt");
        crawler.SetLogicDiskLetter("C://");

        string directoryPath = "C:\\AMD"; 

        crawler.RecursiveDirectoryTraversal(directoryPath);
        crawler.WriteToLogFile();
        crawler.ReadFromFile();
    }
}
