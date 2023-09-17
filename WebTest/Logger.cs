namespace WebTest
{
    public class Logger
    {
        public static void LogToFile(string path,string message)
        {
            File.AppendAllText(path, message);
        }
    }
}
