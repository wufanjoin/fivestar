using UnityEngine;
using System.IO;
using System;

namespace ETModel
{

    public class UteCopyAllFileToKHD
    {

        private static string formPath; //原路径
        private static string targetPath; //目标路径 
        private static bool isNull = false;

        public static void Copy(string pformPaht, string ptargetPaht)
        {
            formPath = pformPaht;
            targetPath = ptargetPaht;
            Log.Debug("源文件夹"+formPath);
            Log.Debug("目标文件夹"+targetPath);
            //targetPath = @"D:\test";
            Copy();
        }

        private static void Copy()
        {
            isNull = false;
            if (!Directory.Exists(targetPath))
            {
                Log.Error("未找到文件夹");
            }
            CleanDirectory(targetPath);
            CopyDirectory(formPath, targetPath);
            if (!isNull)
            {
                Debug.Log("Arts\\Scenes目录文件导入成功！！");
            }
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="srcDir">起始文件夹</param>
        /// <param name="tgtDir">目标文件夹</param>
        private static void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();
            DirectoryInfo[] dirs = source.GetDirectories();
            if (files.Length == 0 && dirs.Length == 0)
            {
                Log.Error("当前项目中Arts\\Scenes文件夹为空");
                isNull = true;
                return;
            }
            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
            }
            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
            }
        }

        //删除目标文件夹下面所有文件
        private static void CleanDirectory(string dir)
        {
            foreach (string subdir in Directory.GetDirectories(dir))
            {
                Directory.Delete(subdir, true);
            }

            foreach (string subFile in Directory.GetFiles(dir))
            {
                File.Delete(subFile);
            }
        }
    }
}
