using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETEditor
{

    public class BindleGameFile
    {
        public BindleGameFile(string bundleType)
        {
            mBundleType = bundleType;
        }
        public string mBundleType;

        private string dirPath;

        private string lowerBundleType;
        public void Init(string dir)
        {
            
            lowerBundleType = mBundleType.ToString().ToLower();
            dirPath = dir + "/" + lowerBundleType;
            if (false == Directory.Exists(dirPath))
            {
                //创建pic文件夹
                Directory.CreateDirectory(dirPath);
            }
        }

        public void MoveFile(string filePath, string fileName, string fileNameSign)
        {
            if (fileNameSign.Contains(lowerBundleType))
            {
                File.Move(filePath, dirPath + "/" + fileName);
            }
        }

        public void GenerateVersionInfo()
        {
            string versionPath = $"{dirPath}/{lowerBundleType}Version.txt";
            VersionConfig versionProto = new VersionConfig();
            GenerateVersionProto(dirPath, versionProto);

            using (FileStream fileStream = new FileStream(versionPath, FileMode.Create))
            {
                byte[] bytes = JsonHelper.ToJson(versionProto).ToByteArray();
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }



        private static void GenerateVersionProto(string dir, VersionConfig versionProto)
        {
            foreach (string file in Directory.GetFiles(dir))
            {
                string md5 = MD5Helper.FileMD5(file);
                FileInfo fi = new FileInfo(file);
                long size = fi.Length;

                versionProto.FileInfoDict.Add(fi.Name, new FileVersionInfo
                {
                    File = fi.Name,
                    MD5 = md5,
                    Size = size,
                });
            }
        }
    }
  public static class BuildBundleClassify
    {
        public static List<BindleGameFile> BindleGameFiles = new List<BindleGameFile>() {new BindleGameFile("11")};

        public static void Init(string dir)
        {
            foreach (var gameFile in BindleGameFiles)
            {
                gameFile.Init(dir);
            }
        }

        public static void ClassifyMoveFile(string dir)
        {
            Init(dir);
            string[] dirs = Directory.GetFiles(dir);
            foreach (string file in dirs)
            {
                FileInfo fi = new FileInfo(file);
                string[] fileNames = fi.Name.Split('.');
                if (fileNames.Length < 2)
                {
                    continue;
                }
                string fileNameSign = fileNames[1];
                foreach (var gameFile in BindleGameFiles)
                {
                    gameFile.MoveFile(file, fi.Name, fileNameSign);
                }
            }
            GenerateVersionInfo();
        }

        public static void GenerateVersionInfo()
        {
            foreach (var gameFile in BindleGameFiles)
            {
                gameFile.GenerateVersionInfo();
            }

        }
    }
}
