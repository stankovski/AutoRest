using System;
using MyNamespace;

public class Program
{
    public static void Main()
    {
        var myClient = new MyClient();
        var salutation = myClient.GetGreeting();
        Console.WriteLine(salutation);
    }
}