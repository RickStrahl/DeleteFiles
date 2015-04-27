using System;
using System.Reflection;

namespace DeleteFiles
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            string version = v.Major + "." + v.Minor;

            Console.WriteLine("West Wind DeleteFiles [Version {0}]\r\n", version);

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
-f          Remove empty [F]olders (start folder is not deleted)
-y          Delete to Rec[Y]le Bin (can be slow!)
-l          Disp[L]ays items that would be deleted
-q0..2      Quiet mode: -q0 - all (default)  -q1 - No file detail
                        -q2 - No file detail, no summary
-dXX        Number of [D]ays before the current date to delete            
-sXX        Number of [S]econds before the current time to delete
            (seconds override days)
            if neither -d or -s no date filter is applied (default)

Examples:
---------
DeleteFiles c:\temp\*.* -r -f        - deletes all files in temp folder recursively 
                                       and deletes empty folders
DeleteFiles *.tmp -r                 - delete .tmp files recursively from current folder down
DeleteFiles ""c:\My Files\*.*"" -r   - deletes all files leaves folders
DeleteFiles c:\temp\*.* -r -f -d10   - delete files 10 days or older 
DeleteFiles c:\temp\*.* -r -f -s3600 - delete files older than an hour
DeleteFiles c:\thumbs.db -r          - delete thumbs.db files on entire drive

";

                Console.WriteLine(options);
                return;
            }

            DeleteFilesCommandLineParser cmdLine = new DeleteFilesCommandLineParser();
            cmdLine.Parse();

            if (cmdLine.DisplayOnly)
                Console.WriteLine(DeleteFiles.Properties.Resources.DisplayOnlyModeNoFilesFoldersAreDeletedRN);

            DeleteFilesProcessor del = new DeleteFilesProcessor(cmdLine);
            del.ProcessFiles();


#if DEBUG
            Console.WriteLine("Done. Press any key to exit...");
            Console.ReadKey();
#endif
        }

    }
}