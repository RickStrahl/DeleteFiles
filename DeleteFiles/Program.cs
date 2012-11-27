using DeleteFiles.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteFiles
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            Console.WriteLine("West Wind DeleteFiles Utility\r\n" +
                              "(c) 2012, West Wind Technologies - www.west-wind.com\r\n");

            if (args == null || args.Length == 0 || args[0] == "HELP" || args[0] == "/?")
            {
                string options =
                    @"
DeleteFiles <filespec> -r -d10 -t12/1/2012

Commands:
---------
HELP || /?      This help display           

Options:
--------
pathSpec    Path and File Spec. Make sure to add a filespec
-r          Delete files recursively     
-f          Remove empty Folders
-y          Delete to Recyle Bin (can be slow!)
-dXX        Number of days before the current date to delete            
-sXX        Number of seconds before the current time to delete
            (seconds override days)
            if neither -d or -s no date filter is applied

Examples:
---------
DeleteFiles c:\temp\*.* -r         - deletes all files in temp folder recursively
DeleteFiles c:\temp\*.* -r -d10    - delete files 10 days or older
DeleteFiles c:\temp\*.* -r -s3600  - delete files older than an hour
DeleteFiles ""c:\My Files\*.*"" -r - deletes all files in temp folder recursively

";

                Console.WriteLine(options);
                return;

            }

            DeleteFilesCommandLineParser cmdLine = new DeleteFilesCommandLineParser();
            cmdLine.Parse();

            DeleteFilesProcessor del = new DeleteFilesProcessor();
            if (!del.ProcessFiles(cmdLine))
                Console.WriteLine(Resources.Error  + del.ErrorMessage);

#if DEBUG
            Console.ReadKey();
#endif
        }
        
    }
}