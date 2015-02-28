using System;

namespace DeleteFiles
{
internal class Program
{
    private static void Main(string[] args)
    {

        Console.WriteLine("West Wind DeleteFiles Utility\r\n" +
                          "(c)  West Wind Technologies - www.west-wind.com\r\n");

        if (args == null || args.Length == 0 || args[0] == "HELP" || args[0] == "/?")
        {
            string options =
                @"
DeleteFiles <filespec> -r -f -y -l -d10 -s3600

Commands:
---------
HELP || /?      This help display           

Options:
--------
pathSpec    FilePath and File Spec. Make sure to add a filespec
-r          Delete files [R]ecursively     
-f          Remove empty [F]olders
-y          Delete to Rec[Y]le Bin (can be slow!)
-l          Disp[L]ays items that would be deleted
-dXX        Number of [D]ays before the current date to delete            
-sXX        Number of [S]econds before the current time to delete
        (seconds override days)
        if neither -d or -s no date filter is applied

Examples:
---------
DeleteFiles c:\temp\*.* -r -f        - deletes all files in temp folder recursively 
                                       and deletes empty folders
DeleteFiles c:\temp\*.* -r -f -d10   - delete files 10 days or older 
DeleteFiles c:\temp\*.* -r -f -s3600 - delete files older than an hour
DeleteFiles ""c:\My Files\*.*"" -r   - deletes all files in temp folder recursively

";

            Console.WriteLine(options);
            return;

        }

        DeleteFilesCommandLineParser cmdLine = new DeleteFilesCommandLineParser();
        cmdLine.Parse();

        if (cmdLine.DisplayOnly)
            Console.WriteLine(DeleteFiles.Properties.Resources.DisplayOnlyModeNoFilesFoldersAreDeletedRN);

        DeleteFilesProcessor del = new DeleteFilesProcessor();
        del.ProcessFiles(cmdLine);
            

#if DEBUG
        Console.WriteLine("Done. Press any key to exit...");
        Console.ReadKey();
#endif
    }
        
}
}