namespace hw_2_server;

public static class Logger
{
    private const string _path = "logs.txt"; 
    public static void Write(string str)
    {
        Console.WriteLine(str);

        string strWithDate = DateTime.Now + " " + str;
        File.AppendAllText(_path, strWithDate + Environment.NewLine);
    }
}