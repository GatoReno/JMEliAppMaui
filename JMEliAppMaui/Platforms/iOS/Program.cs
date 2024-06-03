using ObjCRuntime;
using UIKit;
using static CoreFoundation.DispatchSource;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JMEliAppMaui;

public class Program
{
	// This is the main entry point of the application.
	static void Main(string[] args)
	{
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
	}
}

 