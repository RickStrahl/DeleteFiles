using Westwind.Utilities.System;

namespace DeleteFiles
{
public class DeleteFilesCommandLineParser : CommandLineParser
{
    public string FullPath { get; set; }
    public bool Recursive { get; set; }
    public bool DisplayOnly { get; set; }
    public bool UseRecycleBin { get; set; }
    public bool RemoveEmptyFolders { get; set; }
    public int Days { get; set; }
    public int Seconds { get; set; }

    public string Path { get; set; }
    public string FileSpec { get; set; }


    public DeleteFilesCommandLineParser(string[] args = null,string cmdLine = null) 
        : base(args,cmdLine)
    {}

    public override void Parse()
    {
        // first argument is path
        FullPath = Args[0];
        if (!string.IsNullOrEmpty(FullPath))
        {
            //FullPath = System.IO.Path.GetFullPath(FullPath);

            Path = System.IO.Path.GetDirectoryName(FullPath);
            FileSpec = System.IO.Path.GetFileName(FullPath);
        }

        DisplayOnly = ParseParameterSwitch("-l");
        Recursive = ParseParameterSwitch("-r");
        RemoveEmptyFolders = ParseParameterSwitch("-f");
        UseRecycleBin = ParseParameterSwitch("-y");
        Days = ParseIntParameterSwitch("-d",-1);
        Seconds = ParseIntParameterSwitch("-s",-1);
    }
}
}
