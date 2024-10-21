namespace hw_4_server;

public static class Log
{
    private const string _path = "logs.txt"; 
    public static void Write(string str)
    {
        Console.WriteLine(str);

        string strWithDate = DateTime.Now + " " + str;
        File.AppendAllText(_path, strWithDate + Environment.NewLine);
    }
}