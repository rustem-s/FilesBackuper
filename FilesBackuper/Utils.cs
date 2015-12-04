using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FilesBackuper
{
    class Utils
    {

        /*
         * readParams
         * 
        */
        public static void readIni()
        {


            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);

            string folderName = new FileInfo(location.AbsolutePath).Directory.FullName;

            IniFile ini = new IniFile(folderName + "\\FilesBackuper.ini");

            // reading
            String sourcePath = ini.IniReadValue("initParams", "sourcePath");

            String destinationPath = ini.IniReadValue("initParams", "destinationPath");

            String excludeSourceFolderNames = ini.IniReadValue("initParams", "excludeSourceFolderNames");

            // validation
            if (String.IsNullOrEmpty(sourcePath))
            {
                throw new System.Exception("Parameter 'sourcePath' not exists in BackupFiles.ini");
            }

            if (String.IsNullOrEmpty(destinationPath))
            {
                throw new System.Exception("Parameter 'destinationPath' not exists in BackupFiles.ini");
            }

            if (String.IsNullOrEmpty(excludeSourceFolderNames))
            {
                throw new System.Exception("Parameter 'excludeSourceFolderNames' not exists in BackupFiles.ini");
            }

            // assign values
            Params.sourcePath = sourcePath;

            Params.destinationPath = destinationPath;

            Params.excludeSourceFolderNames = excludeSourceFolderNames.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

        }

        public static void directoryCopy(string sourceDirName, string destDirName)
        {

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                try
                {
                    file.CopyTo(temppath, false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss") + " " + e.Message);
                }
            }

            foreach (DirectoryInfo subdir in dirs)
            {

                if (!(Array.Find(Params.excludeSourceFolderNames, s => s.Equals(subdir.Name)) == null))
                    continue;

                string temppath = Path.Combine(destDirName, subdir.Name);
                directoryCopy(subdir.FullName, temppath);
            }
        }

    }
}
