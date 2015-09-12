using System;
using MyNamespace;
using Microsoft.Rest.Azure.Authentication;

public class Program
{
    public static void Main()
    {
        var client = new ResourceManagementClient(new ApplicationTokenCredentials("a3603e38-ccc6-4104-b643-a03a5abd3441", "9148c3a5-1e1b-4e0a-87c2-302229534991" , "fxrs7ga0XU4GHrJ78HmQSke64Ps2q/QTsUDK3dB/XYg="));
        client.SubscriptionId = "cbbdaed0-fea9-4693-bf0c-d446ac93c030";
        var resourceGroups = client.ResourceGroups.List();
        foreach (var resourceGroup in resourceGroups)
        {
            System.Console.WriteLine(resourceGroup.Name);            
        }
    }
}