using DeleteFiles.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace DeleteFiles
{
    public class DeleteFilesProcessor
    {
        public string ErrorMessage { get; set; }

        protected void SetError()
        {
            SetError(null);
        }

        protected void SetError(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                ErrorMessage = string.Empty;
                return;
            }
            ErrorMessage += message;
        }

        protected void SetError(Exception ex, bool checkInner = false)
        {
            if (ex == null)
                this.ErrorMessage = string.Empty;

            Exception e = ex;
            if (checkInner)
                e = e.GetBaseException();

            ErrorMessage = e.Message;
        }
 

        public bool ProcessFiles(DeleteFilesCommandLineParser parser)
        {
            if (!Directory.Exists(parser.Path))
            {
                this.SetError(Resources.StartFolderDoesnTExist + parser.Path);
                return false;
            }
            

            return ProcessFolder(parser.Path, parser);            
        }

        protected bool ProcessFolder(string activeFolder, DeleteFilesCommandLineParser parser)
        {
            var files = Directory.GetFiles(activeFolder, parser.FileSpec);
            bool success = true;
            foreach (var file in files)
            {
                try
                {
                    if(IsFileToBeDeleted(file, parser))
                    {
                        if (parser.UseRecycleBin)
                            FileSystem.DeleteFile(file,UIOption.OnlyErrorDialogs,RecycleOption.SendToRecycleBin);
                        else
                            File.Delete(file);

                        Console.WriteLine(Properties.Resources.Deleting + file);
                    }
                }
                catch
                {
                    Console.WriteLine(Properties.Resources.FailedToDelete + file);
                    success = false;
                }                
            }
            
            if (parser.Recursive)
            {
                var dirs = Directory.GetDirectories(activeFolder);
                foreach (var dir in dirs)
                {
                    success = ProcessFolder(dir, parser);
                    if (success)
                    {
                        if (Directory.GetFiles(dir).Count() == 0 && Directory.GetDirectories(dir).Count() == 0)
                            try
                            {
                                if (parser.UseRecycleBin)
                                    FileSystem.DeleteDirectory(dir,UIOption.OnlyErrorDialogs,RecycleOption.SendToRecycleBin);
                                else
                                    Directory.Delete(dir);

                                Console.WriteLine(Properties.Resources.DeletingDirectory + dir);
                            }
                            catch
                            {
                                Console.WriteLine(Properties.Resources.FailedToDeleteDirectory + dir);
                            }
                    }
                }
            }

            return success;
        }
        
        /// <summary>
        /// Determines if file is to be deleted from disk
        /// 
        /// Checks:
        /// parser.Seconds from current time
        /// parser.Days from current time
        /// if neither is set file can be deleted
        /// </summary>
        /// <param name="file"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        bool IsFileToBeDeleted(string file, DeleteFilesCommandLineParser parser)
        {
            if (parser.Seconds > -1)
            {
                var ftime = File.GetLastWriteTimeUtc(file);                
                if (DateTime.UtcNow > ftime.AddSeconds(parser.Seconds))
                    return true;
                return false;
            }
            if (parser.Days > -1)
            {
                var ftime = File.GetLastWriteTime(file);                
                if (DateTime.Now.Date >= ftime.Date.AddDays(parser.Days))
                    return true;
                return false;
            }

            // if neither days or seconds were provided delete all files
            return true;
        }
    }
}
