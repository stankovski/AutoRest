using System;
using HelloWorld;

public class Program
{
    public static void Main()
    {
        var myClient = new MyClient();
        var salutation = myClient.GetGreeting();
        Console.WriteLine(salutation);
    }
}