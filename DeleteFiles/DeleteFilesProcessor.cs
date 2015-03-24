using DeleteFiles.Properties;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DeleteFiles
{
    public class DeleteFilesProcessor
    {

        public int FileCount { get; set; }
        public long FileSizeCount { get; set; }
        public int FolderCount { get; set; }
        public int LockedFileCount { get; set; }
        DeleteFilesCommandLineParser Parser { get; set; }

        public DeleteFilesProcessor(DeleteFilesCommandLineParser parser = null)
        {
            Parser = parser;
        }

        public bool ProcessFiles()
        {
            FileCount = 0;
            FolderCount = 0;
            LockedFileCount = 0;
            LockedFileCount = 0;

            if (!Directory.Exists(Parser.FilePath))
            {
                OnShowMessage(Resources.StartFolderDoesnTExist + Parser.FilePath);
                return false;
            }

            bool result = ProcessFolder(Parser.FilePath, Parser);
            
            if (Parser.QuietMode < 2)
            {
                Parser.QuietMode = 0;

                OnShowMessage("\r\n\r\nSummary:");

                if (FileCount == 0 && LockedFileCount == 0)
                    OnShowMessage("  No files found to delete.");

                if (FileCount > 0)
                {
                    string size;
                    if (FileSizeCount < 1000)
                        size = FileSizeCount.ToString("N0") + " bytes";
                    else if (FileSizeCount < 15000)
                        size = (FileSizeCount / 1000).ToString("N2") + "kb";
                    else if (FileSizeCount < 1000000)
                        size = (FileSizeCount/1000).ToString("N0") + "kb";
                    else
                        size = ((decimal) FileSizeCount/1000000M).ToString("N1") + "mb";

                    OnShowMessage(string.Format("  {0} files and {1} deleted.", FileCount, size));
                }
                if (FolderCount > 0)
                    OnShowMessage(string.Format("  {0} folders deleted.", FolderCount));
                if (LockedFileCount > 0)
                    OnShowMessage(string.Format("  {0} locked files not deleted.", LockedFileCount));
            }

            return result;
        }

        protected bool ProcessFolder(string activeFolder, DeleteFilesCommandLineParser parser)
        {
            ZetaLongPaths.ZlpFileInfo[] files;

            try
            {
                var folder = new ZetaLongPaths.ZlpDirectoryInfo(activeFolder);
                files = folder.GetFiles(parser.FileSpec);
            }
            catch (Exception e)
            {
                OnShowMessage(Resources.ErrorOpening + activeFolder + ". " + e.GetBaseException().Message);
                return false;
            }
            bool success = true;
            foreach (var fi in files)
            {
                //var fi = new FileInfo(file);
                string file = fi.FullName;
                if (!fi.Exists)
                    continue;
                
                try
                {
                    if (IsFileToBeDeleted(fi.Name))
                    {
                        long fsize = fi.Length;

                        if (!parser.DisplayOnly)
                        {
                            if (parser.UseRecycleBin)
                                fi.MoveToRecycleBin();
                                //FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            else
                                try
                                {
                                    File.Delete(fi.FullName);
                                }                            
                                catch (PathTooLongException ex)
                                {
                                    // try again with long path routines
                                    fi.Delete();
                                }
                                catch (UnauthorizedAccessException ex)
                                {
                                    fi.Attributes =  ZetaLongPaths.Native.FileAttributes.Normal;
                                    fi.Delete();
                                }                            
                        }
                        OnShowMessage(Resources.Deleting + file);
                        FileCount++;
                        
                        FileSizeCount += fsize;
                    }
                }
                catch(Exception ex)
                {                    
                    OnShowMessage(Resources.FailedToDelete + file);
                    LockedFileCount++; 
                    success = false;
                }
            }

            if (parser.Recursive)
            {
                var folders = new ZetaLongPaths.ZlpDirectoryInfo(activeFolder);
                var dirs = folders.GetDirectories();
                foreach (var directory in dirs)
                {
                    string dir = directory.FullName;

                    success = ProcessFolder(dir, parser);
                    if (success && parser.RemoveEmptyFolders)
                    {
                        if (!directory.GetFiles().Any() && !directory.GetDirectories().Any())
                            try
                            {
                                if (!parser.DisplayOnly)
                                {
                                    if (parser.UseRecycleBin)
                                        directory.MoveToRecycleBin();
                                        //FileSystem.DeleteDirectory(dir, UIOption.OnlyErrorDialogs,
                                        //    RecycleOption.SendToRecycleBin);
                                    else
                                    {
                                        try
                                        {
                                            Directory.Delete(directory.FullName);
                                        }
                                        catch (PathTooLongException ex)
                                        {
                                            directory.Delete(false);
                                        }
                                        catch (UnauthorizedAccessException ex)
                                        {
                                            if (parser.DeleteReadOnly)
                                            {
                                                directory.Attributes = ZetaLongPaths.Native.FileAttributes.Normal;
                                                directory.Delete(false);
                                            }
                                        }
                                    }
                                }
                                FolderCount++;
                                OnShowMessage(Resources.DeletingDirectory + dir);
                            }
                            catch
                            {                                
                                OnShowMessage(Resources.FailedToDeleteDirectory + dir);
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
        /// Parser.Seconds from current time
        /// Parser.Days from current time
        /// if neither is set file can be deleted
        /// </summary>
        /// <param name="file"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        private bool IsFileToBeDeleted(string file)
        {
            if (Parser.Seconds > -1)
            {
                var ftime = File.GetLastWriteTimeUtc(file);
                if (DateTime.UtcNow > ftime.AddSeconds(Parser.Seconds))
                    return true;
                return false;
            }
            if (Parser.Days > -1)
            {
                var ftime = File.GetLastWriteTime(file);
                if (DateTime.Now.Date >= ftime.Date.AddDays(Parser.Days))
                    return true;
            }

            // if neither days or seconds were provided delete all files
            return true;
        }

        /// <summary>
        /// Event that allows you to override the output that is sent 
        /// by this class. If not set output is sent to the Console.
        /// </summary>
        public event Action<string> ShowMessage;

        public virtual void OnShowMessage(string message)
        {
            if (Parser.QuietMode > 0 )
                return;

            if (ShowMessage != null)
                ShowMessage(message);
            else
                Console.WriteLine(message);
        }

      
    }

}
