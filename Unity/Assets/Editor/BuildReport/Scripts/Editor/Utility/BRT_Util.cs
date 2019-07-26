#if (UNITY_4 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
	#define UNITY_4_AND_GREATER
#endif

#if (UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
	#define UNITY_4_1_AND_GREATER
#endif

#if (UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
	#define UNITY_4_2_AND_GREATER
#endif

#if (UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5)
	#define UNITY_4_3_AND_GREATER
#endif

#if UNITY_4 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
#define UNITY_5_3_AND_LESSER
#endif

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace BuildReportTool
{


public static class Util
{
	public static int GetUnityMajorVersion(string unityVersion)
	{
		string majorVersion = string.Empty;
		
		int len = 0;
		
		int majorIdx = 0;
		for (majorIdx = 0, len = unityVersion.Length; majorIdx < len; ++majorIdx)
		{
			if (System.Char.IsDigit(unityVersion[majorIdx]) && unityVersion[majorIdx] != '.')
			{
				majorVersion += unityVersion[majorIdx];
			}
			if (unityVersion[majorIdx] == '.')
			{
				break;
			}
		}
		
		return int.Parse(majorVersion);
	}
	
	public static bool UnityMajorVersionUsedIsAtMost(int versionAtMost, string unityVersionName)
	{
		int majorVersion = GetUnityMajorVersion(unityVersionName);
		
		return (majorVersion <= versionAtMost);
	}
	
	public static bool UnityMajorVersionUsedIsAtLeast(int versionAtLeast, string unityVersionName)
	{
		int majorVersion = GetUnityMajorVersion(unityVersionName);
		
		return (majorVersion >= versionAtLeast);
	}
	
	// care should be taken when using ShouldGetBuildReportNow and ShouldSaveGottenBuildReportNow
	// as they are effectively global variables
	// unfortunately this is the only way I can ensure persistence of bool variables in between recompilations
	// SerializeField seems to fail at times

	public static BuildPlatform GetBuildPlatformBasedOnUnityBuildTarget(BuildTarget b)
	{
		switch (b)
		{
#if UNITY_5_3_AND_LESSER
			case BuildTarget.WebPlayer:
				return BuildPlatform.Web;
			case BuildTarget.WebPlayerStreamed:
				return BuildPlatform.Web;
#endif
				
#if !UNITY_5
		//	case BuildTarget.NaCl:
			//	return BuildPlatform.Web;


		//	case BuildTarget.FlashPlayer:
			//	return BuildPlatform.Flash;
#endif

#if !UNITY_5
			case BuildTarget.iOS:
#else
			case BuildTarget.iOS:
#endif
				return BuildPlatform.iOS;

			case BuildTarget.Android:
				return BuildPlatform.Android;


			case BuildTarget.XBOX360:
				return BuildPlatform.XBOX360;

			case BuildTarget.PS3:
				return BuildPlatform.PS3;



			case BuildTarget.StandaloneWindows:
				return BuildPlatform.Windows32;

			case BuildTarget.StandaloneWindows64:
				return BuildPlatform.Windows64;


#if UNITY_4_AND_GREATER
			case BuildTarget.StandaloneLinux:
				return BuildPlatform.Linux32;

			case BuildTarget.StandaloneLinux64:
				return BuildPlatform.Linux64;

			case BuildTarget.StandaloneLinuxUniversal:
				return BuildPlatform.LinuxUniversal;
#endif


			case BuildTarget.StandaloneOSXIntel:
				return BuildPlatform.MacOSX32;

#if UNITY_4_2_AND_GREATER
			case BuildTarget.StandaloneOSXIntel64:
				return BuildPlatform.MacOSX64;

			case BuildTarget.StandaloneOSXUniversal:
				return BuildPlatform.MacOSXUniversal;
#endif
		}

		return BuildPlatform.None;
	}

	public static bool ShouldGetBuildReportNow
	{
		get
		{
			return EditorPrefs.GetBool("BRT_ShouldGetBuildReportNow", false);
		}
		set
		{
			EditorPrefs.SetBool("BRT_ShouldGetBuildReportNow", value);
		}
	}
	public static bool ShouldSaveGottenBuildReportNow
	{
		get
		{
			return EditorPrefs.GetBool("BRT_ShouldSaveGottenBuildReportNow", false);
		}
		set
		{
			EditorPrefs.SetBool("BRT_ShouldSaveGottenBuildReportNow", value);
		}
	}


	public static BuildTarget BuildTargetOfLastBuild
	{
		get
		{
			int gotBuildTargetIdx = EditorPrefs.GetInt("BRT_BuildTargetOfLastBuild", 0);
			return (BuildTarget)gotBuildTargetIdx;
		}
		set
		{
			int buildTargetIdx = Convert.ToInt32(value);
			EditorPrefs.SetInt("BRT_BuildTargetOfLastBuild", buildTargetIdx);
		}
	}


	public static bool IsAScriptDLL(string filename)
	{
		return filename.StartsWith("Assembly-");
	}

	public static string RemovePrefix(string prefix, string val)
	{
		if (val.StartsWith(prefix))
		{
			return val.Substring(prefix.Length, val.Length - prefix.Length);
		}
		return val;
	}

	public static string RemoveSuffix(string suffix, string val, int offset = 0)
	{
		if (val.EndsWith(suffix))
		{
			return val.Substring(0, val.Length - suffix.Length + offset);
		}
		return val;
	}



	static string GetLastFolder(string inFolder)
	{
		inFolder = inFolder.Replace('\\', '/');

		//LogMgr.Ins.LogInfo("folder: " + inFolder);
		//string folderName = Path.GetDirectoryName(folderEntries[n]);

		int lastSlashIdx = inFolder.LastIndexOf('/');
		if (lastSlashIdx == -1)
		{
			return "";
		}
		return inFolder.Substring(lastSlashIdx+1, inFolder.Length-lastSlashIdx-1);
	}

	public static string FindAssetFolder(string folderToStart, string desiredFolderName)
	{
		string[] folderEntries = Directory.GetDirectories(folderToStart);

		for (int n = 0, len = folderEntries.Length; n < len; ++n)
		{
			string folderName = GetLastFolder(folderEntries[n]);
			//LogMgr.Ins.LogInfo("folderName: " + folderName);

			if (folderName == desiredFolderName)
			{
				return folderEntries[n];
			}
			else
			{
				string recursed = FindAssetFolder(folderEntries[n], desiredFolderName);
				string recursedFolderName = GetLastFolder(recursed);
				if (recursedFolderName == desiredFolderName)
				{
					return recursed;
				}
			}
		}
		return "";
	}


	static string GetPathParentFolder(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return string.Empty;
		}

		return Path.GetDirectoryName(path);
	}

	public static string GetBuildSizePathDescription(BuildInfo buildReport)
	{
		if (string.IsNullOrEmpty(buildReport.BuildFilePath))
		{
			return string.Empty;
		}

		BuildReportTool.BuildPlatform buildPlatform = BuildReportTool.ReportGenerator.GetBuildPlatformFromString(buildReport.BuildType, buildReport.BuildTargetUsed);
		
		if (buildPlatform == BuildPlatform.Windows32 ||
			buildPlatform == BuildPlatform.Windows64 ||
			buildPlatform == BuildPlatform.Linux32 ||
			buildPlatform == BuildPlatform.Linux64)
		{
			// in windows builds, `buildFilePath` is the executable file
			// we additionaly need to get the size of the Data folder

			// in 32 bit builds, `buildFilePath` is the executable file (.x86 file). we still need the Data folder
			// in 64 bit builds, `buildFilePath` is the executable file (.x86_64 file). we still need the Data folder

			var exeFile = Path.GetFileName(buildReport.BuildFilePath);
			var dataFolder = BuildReportTool.Util.ReplaceFileType(exeFile, "_Data");
			var buildParentFolder = GetPathParentFolder(buildReport.BuildFilePath);

			return string.Format("File size of {0} and the {1} folder in <b>{2}</b>", exeFile, dataFolder, buildParentFolder);
		}

		if (buildPlatform == BuildPlatform.LinuxUniversal)
		{
			// in universal builds, `buildFilePath` is the 32-bit executable. we still need the 64-bit executable and the Data folder
			
			var exe32File = Path.GetFileName(buildReport.BuildFilePath);
			var exe64File = BuildReportTool.Util.ReplaceFileType(exe32File, ".x86_64");
			var dataFolder = BuildReportTool.Util.ReplaceFileType(exe32File, "_Data");
			var buildParentFolder = GetPathParentFolder(buildReport.BuildFilePath);

			return string.Format("File size of {0}, {1}, and the {2} folder in <b>{3}</b>", exe32File, exe64File, dataFolder, buildParentFolder);
		}

		return string.Format("File size of <b>{0}</b>", buildReport.BuildFilePath);
	}




	public static double GetObbSizeInEclipseProject(string eclipseProjectPath)
	{
		if (string.IsNullOrEmpty(eclipseProjectPath) || !Directory.Exists(eclipseProjectPath))
		{
			return 0;
		}

		double obbSize = 0;
		foreach (string file in DldUtil.TraverseDirectory.Do(eclipseProjectPath))
		{
			if (IsFileOfType(file, ".main.obb"))
			{
				obbSize += GetFileSizeInBytes(file);
				break;
			}
		}

		return obbSize;
	}

	public static string GetObbSizeInEclipseProjectReadable(string eclipseProjectPath)
	{
		if (string.IsNullOrEmpty(eclipseProjectPath) || !Directory.Exists(eclipseProjectPath))
		{
			return string.Empty;
		}

		return GetBytesReadable( GetObbSizeInEclipseProject(eclipseProjectPath) );
	}


	public static string GetPathSizeReadable(string fileOrFolder)
	{
		if (string.IsNullOrEmpty(fileOrFolder))
		{
			return string.Empty;
		}

		return GetBytesReadable( GetPathSizeInBytes(fileOrFolder) );
	}

	public static double GetPathSizeInBytes(string fileOrFolder)
	{
		if (Directory.Exists(fileOrFolder))
		{
			return GetFolderSizeInBytes(fileOrFolder);
		}
		if (File.Exists(fileOrFolder))
		{
			return GetFileSizeInBytes(fileOrFolder);
		}
		return 0;
	}

	public static double GetFolderSizeInBytes(string folderPath)
	{
		if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
		{
			return 0;
		}

		double totalBytesOfFilesInFolder = 0;
		foreach (string file in DldUtil.TraverseDirectory.Do(folderPath))
		{
			totalBytesOfFilesInFolder += GetFileSizeInBytes(file);
		}

		return totalBytesOfFilesInFolder;
	}

	public static string GetFolderSizeReadable(string folderPath)
	{
		if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
		{
			return string.Empty;
		}

		return GetBytesReadable(GetFolderSizeInBytes(folderPath));
	}

	public static string GetStreamingAssetsSizeReadable()
	{
		return GetFolderSizeReadable(Application.dataPath + "/StreamingAssets");
	}

	public static string GetProjectPath(string appDataPath)
	{
		const int AssetsFolderNameLength = 6; // "Assets"

		return appDataPath.Remove(appDataPath.Length - AssetsFolderNameLength);
	}

	// expects filename given to be full path
	public static long GetFileSizeInBytes(string filename)
	{
		if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
		{
			return 0;
		}

		FileInfo fi = new FileInfo(filename);
		return fi.Length;
	}

	public static string GetFileSizeReadable(string filename)
	{
		if (string.IsNullOrEmpty(filename))
		{
			return string.Empty;
		}

		return GetBytesReadable(GetFileSizeInBytes(filename));
	}

	const double ONE_TERABYTE = 1099511627776.0;
	const double ONE_GIGABYTE = 1073741824.0;
	const double ONE_MEGABYTE = 1048576.0;
	const double ONE_KILOBYTE = 1024.0;

	public static string GetBytesReadable(double bytes)
	{
		//return EditorUtility.FormatBytes(bytes);
		return MyFileSizeReadable(bytes);
	}

	static string MyFileSizeReadable(double bytes)
	{
		if (bytes < 0)
		{
			return "N/A";
		}

		double converted = bytes;
		string units = "B";

		if (bytes >= ONE_TERABYTE)
		{
			converted = bytes / ONE_TERABYTE;
			units = "TB";
		}
		else if (bytes >= ONE_GIGABYTE)
		{
			converted = bytes / ONE_GIGABYTE;
			units = "GB";
		}
		else if (bytes >= ONE_MEGABYTE)
		{
			converted = bytes / ONE_MEGABYTE;
			units = "MB";
		}
		else if (bytes >= ONE_KILOBYTE)
		{
			converted = bytes / ONE_KILOBYTE;
			units = "KB";
		}

		return String.Format("{0:0.##} {1}", converted, units);
	}

	public static double GetApproxSizeFromString(string size)
	{
		if (size == "N/A")
		{
			return 0;
		}

		// units is expected to be in the last two letters of the string
		string units = size.Substring(size.Length - 2, 2);

		// therefore, everything except the last two letters is expected to be a number
		string numOnly = size.Substring(0, size.Length - 2);

		double num = double.Parse(numOnly);

		//LogMgr.Ins.LogInfo(size + " " + num);

		switch (units)
		{
			case "KB":
				return num * ONE_KILOBYTE;
			case "MB":
				return num * ONE_MEGABYTE;
			case "GB":
				return num * ONE_GIGABYTE;
			case "TB":
				return num * ONE_TERABYTE;
		}
		return 0;
	}






	static bool TextFileHasContents(string filepath, string contents)
	{
		FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		StreamReader sr = new StreamReader(fs);

		bool ret = false;

		string line;
		while ((line = sr.ReadLine()) != null)
		{
			if (line.IndexOf(contents) != -1)
			{
				ret = true;
				break;
			}
		}

		fs.Close();
		return ret;
	}

	public static bool FileHasContents(string filepath, string contents)
	{
		return TextFileHasContents(filepath, contents);
	}



	public static string GetTextFileContents(string file)
	{
		// thanks to http://answers.unity3d.com/questions/167518/reading-editorlog-in-the-editor.html
		FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		StreamReader sr = new StreamReader(fs);

		string contents = "";
		if (fs != null && sr != null)
		{
			contents = sr.ReadToEnd();
		}

		fs.Close();
		return contents;
	}

	public static void DeleteSizePartFile(SizePart file)
	{
		//LogMgr.Ins.LogInfo("going to delete " + file.Name);
		DeleteFile(file.Name);
	}

	static void DeleteFile(string file)
	{
		string projectFolder = RemoveSuffix("Assets", Application.dataPath);

		DeleteFile(projectFolder, file);
	}

	public static bool HaveToUseSystemForDelete(string file)
	{
		return IsFileAUnixHiddenFile(file) || IsFileInVersionControlMetadataFolder(file);
	}

	static void DeleteFile(string projectFolder, string file)
	{
		//LogMgr.Ins.LogInfo("will delete " + file);

		if (HaveToUseSystemForDelete(file))
		{
			string fileAbsPath = Path.Combine(projectFolder, file);
			//LogMgr.Ins.LogInfo("will system delete " + fileAbsPath);
			SystemDeleteFile(fileAbsPath);
		}
		else
		{
			// AssetDatabase.MoveAssetToTrash also deletes .meta file if it exists
			AssetDatabase.MoveAssetToTrash(file);
		}
	}

	static void SystemDeleteFile(string file)
	{
		File.Delete(file);
	}


	public static string ReplaceFileType(string filename, string newFileType)
	{
		int idxOfDot = filename.LastIndexOf(".");

		if (idxOfDot < 0)
		{
			// dot is not found
			return string.Empty;
		}

		string newFile = filename.Remove(idxOfDot);


		newFile += newFileType;

		return newFile;
	}


	// low-level filename checks

	public static bool IsFileInAPath(string filepath, string pathToCheck)
	{
		if (string.IsNullOrEmpty(filepath))
		{
			return false;
		}

		return filepath.ToLower().IndexOf(pathToCheck.ToLower()) != -1;
	}

	public static bool IsFileOfType(string filepath, string typeExtenstion)
	{
		if (string.IsNullOrEmpty(filepath))
		{
			return false;
		}

		return filepath.ToLower().EndsWith(typeExtenstion.ToLower());
	}

	public static bool IsFileName(string filepath, string filenameToCheck)
	{
		return string.Equals(Path.GetFileName(filepath), filenameToCheck, StringComparison.CurrentCultureIgnoreCase);
	}

	public static bool IsFileAUnixHiddenFile(string filepath)
	{
		if (string.IsNullOrEmpty(filepath))
		{
			return false;
		}

		return Path.GetFileName(filepath).StartsWith(".");
	}

	public static bool DoesFileBeginWith(string filepath, string stringToCheck)
	{
		if (string.IsNullOrEmpty(filepath))
		{
			return false;
		}

		return Path.GetFileName(filepath).ToLower().StartsWith(stringToCheck.ToLower());
	}



	public static bool IsFileAUnityTexture(string file)
	{
		return IsFileOfType(file, ".psd") ||
		       IsFileOfType(file, ".jpg") ||
		       IsFileOfType(file, ".jpeg") ||
		       IsFileOfType(file, ".gif") ||
		       IsFileOfType(file, ".png") ||
		       IsFileOfType(file, ".tiff") ||
		       IsFileOfType(file, ".tif") ||
		       IsFileOfType(file, ".tga") ||
		       IsFileOfType(file, ".bmp") ||
		       IsFileOfType(file, ".dds") ||
		       IsFileOfType(file, ".exr") ||
		       IsFileOfType(file, ".iff") ||
		       IsFileOfType(file, ".pict");
	}
	
	public static bool IsFileAUnityMesh(string file)
	{
		return IsFileOfType(file, ".fbx") ||
		       IsFileOfType(file, ".dae") ||
		       IsFileOfType(file, ".mb") ||
		       IsFileOfType(file, ".ma") ||
		       IsFileOfType(file, ".max") ||
		       IsFileOfType(file, ".blend") ||
		       IsFileOfType(file, ".obj") ||
		       IsFileOfType(file, ".3ds") ||
		       IsFileOfType(file, ".dxf");
	}
	
	public static bool IsFileAUnitySound(string file)
	{
		return IsFileOfType(file, ".wav") ||
		       IsFileOfType(file, ".mp3") ||
		       IsFileOfType(file, ".ogg") ||
		       IsFileOfType(file, ".aif") ||
		       IsFileOfType(file, ".xm") ||
		       IsFileOfType(file, ".mod") ||
		       IsFileOfType(file, ".it") ||
		       IsFileOfType(file, ".s3m");
	}
	
	public static bool IsFileAUnityAnimation(string file)
	{
		return IsFileOfType(file, ".anim") ||
		       IsFileOfType(file, ".controller") ||
		       IsFileOfType(file, ".mask");
	}

	// high-level filename checks

	public static bool IsFileInBuildReportFolder(string filepath)
	{
		return DoesFileBeginWith(filepath, "BRT_") || DoesFileBeginWith(filepath, "DldUtil_") || IsFileInAPath(filepath, "/BuildReport/");
	}

	public static bool IsUselessFile(string filepath)
	{
		return IsFileName(filepath, "Thumbs.db") || IsFileName(filepath, ".DS_Store") || IsFileName(filepath, "._.DS_Store");
	}

	public static bool IsFileInEditorFolder(string filepath)
	{
		return IsFileInAPath(filepath, "/Editor/");
	}

	public static bool IsFileInVersionControlMetadataFolder(string filepath)
	{
		return IsFileInAPath(filepath, "/.svn/") ||
			IsFileInAPath(filepath, "/.git/") ||
			IsFileInAPath(filepath, "/.cvs/");
	}

	public static bool IsFileStreamingAsset(string filepath)
	{
		return IsFileInAPath(filepath, "/StreamingAssets/");
	}


	public static bool IsFileOkForDeleteAllOperation(string filepath)
	{
		return IsUselessFile(filepath) ||
			(!IsFileInBuildReportFolder(filepath) &&
			!IsFileInEditorFolder(filepath) &&
			!IsFileInVersionControlMetadataFolder(filepath) &&
			!IsFileAUnixHiddenFile(filepath));
	}

	
	// ----------------------------------------------------------------------------------------------------------------------------------------
	
	
	public static string GetAssetPath(string assetPath)
	{
		if (IsBuiltInAsset(assetPath))
		{
			return GetBuiltInAssetHeader(assetPath);
		}
		return Path.GetDirectoryName(assetPath);
	}
	
	public static string GetAssetFilename(string assetPath)
	{
		if (IsBuiltInAsset(assetPath))
		{
			return GetBuiltInAssetFilename(assetPath);
		}
		return Path.GetFileName(assetPath);
	}
	
	
	
	static bool IsBuiltInAsset(string assetPath)
	{
		return assetPath.StartsWith("Built-in ");
	}
	
	static string GetBuiltInAssetHeader(string assetPath)
	{
		bool hasSlash = assetPath.IndexOf("/") > 0;
		
		if (hasSlash)
		{
			return Path.GetDirectoryName(assetPath);
		}
		
		return assetPath.Substring(0, assetPath.IndexOf(":"));
	}
	
	static string GetBuiltInAssetFilename(string assetPath)
	{
		bool hasSlash = assetPath.IndexOf("/") > 0;
		
		if (hasSlash)
		{
			return Path.GetFileName(assetPath);
		}
		
		int idxOfColon = assetPath.IndexOf(":");
		return assetPath.Substring(idxOfColon+2, assetPath.Length - idxOfColon - 2); // -2 to get rid of ": "
	}
	
	
	public static string GetAssetPathToNameSeparator(string assetPath)
	{
		bool hasSlash = assetPath.IndexOf("/") > 0;
		
		if (hasSlash)
		{
			return "/";
		}
		
		return ": ";
	}
	
	// ----------------------------------------------------------------------------------------------------------------------------------------
	


	public static string GetPackageFileContents(string filename)
	{
		// try default path first
		string defaultBuildReportToolFullPath = Application.dataPath + "/" + BuildReportTool.Options.BUILD_REPORT_TOOL_DEFAULT_FOLDER_NAME;

		string filePath = defaultBuildReportToolFullPath + "/" + filename;

		if (File.Exists(filePath))
		{
			return GetTextFileContents(filePath);
		}

		// not in default path
		// search for it

#if BRT_SHOW_MINOR_WARNINGS
		LogMgr.Ins.LogInfoWarning(BuildReportTool.Options.BUILD_REPORT_PACKAGE_MOVED_MSG);
#endif

		string folderPath = BuildReportTool.Util.FindAssetFolder(Application.dataPath, BuildReportTool.Options.BUILD_REPORT_TOOL_DEFAULT_FOLDER_NAME);

		if (!string.IsNullOrEmpty(folderPath))
		{
			filePath = folderPath + "/" + filename;

			if (File.Exists(filePath))
			{
				return GetTextFileContents(filePath);
			}
		}

            // could not find it
            // giving up
	    Debug.LogError(BuildReportTool.Options.BUILD_REPORT_PACKAGE_MISSING_MSG);

		return "";
	}



	public static bool ShowFileDeleteProgress(int deletedSoFar, int totalToDelete, string filepath, bool showRecoverableMsg)
	{
		float progress = (float)(deletedSoFar+1)/((float)totalToDelete);

		if (EditorUtility.DisplayCancelableProgressBar(
			"Deleting file " + (deletedSoFar+1) + " of " + totalToDelete + " (" + (totalToDelete-deletedSoFar-1) + " left)",
			filepath,
			progress))
		{
			EditorUtility.ClearProgressBar();

			string filesReallyDeletedPlural = deletedSoFar > 1 ? "s" : "";

			string cancelTitle = "Delete operation canceled";
			string cancelMsg = null;

			if (deletedSoFar > 0)
			{
				cancelMsg = "Only " + deletedSoFar + " file" + filesReallyDeletedPlural + " (of " + totalToDelete + ") deleted.";
				if (showRecoverableMsg)
				{
					cancelMsg += " Those files can be recovered from your " + BuildReportTool.Util.NameOfOSTrashFolder + ".";
				}
			}
			else
			{
				cancelMsg = "No files deleted.";
			}

			EditorApplication.Beep();
			EditorUtility.DisplayDialog(cancelTitle, cancelMsg, "OK");

		    Debug.LogWarning(cancelTitle + ". " + cancelMsg);

			return true;
		}
		return false;
	}



	// thanks to http://answers.unity3d.com/questions/16804/retrieving-project-name.html
	public static string GetProjectName(string projectAssetsFolderPath)
	{
		var dp = projectAssetsFolderPath;
		string[] s = dp.Split("/"[0]);
		return s[s.Length - 2];
	}





	// we have two ways of getting user home folder here:

	// from http://stackoverflow.com/questions/1143706/getting-the-path-of-the-home-directory-in-c
	public static string UserHomePath
	{
		get
		{
			string homePath = (System.Environment.OSVersion.Platform == PlatformID.Unix ||
				System.Environment.OSVersion.Platform == PlatformID.MacOSX)
				? System.Environment.GetEnvironmentVariable("HOME")
				: System.Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

			return homePath;
		}
	}

	//[MenuItem("Window/Test 3")]
	public static string GetUserHomeFolder()
	{
		string ret;
		ret = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal).ToString();
		//LogMgr.Ins.LogInfo("GetUserHomeFolder: " + ret);
		ret = ret.Replace("\\", "/");
		return ret;
	}




	//[MenuItem("Window/Test 4")]
	public static void OpenInFileBrowserTest()
	{
		//string path = "/Users/Ferds/Unity Projects/BuildReportTool/BuildReportUnityProject/Assets/BuildReportDebug";
		//string path = "/Users/Ferds/Unity Projects/BuildReportTool/BuildReportUnityProject/Assets/BuildReportDebug/EditorMorel.log.txt";
		//string path = "/Users/Ferds/UnityBuildReports/";
		string path = "/Users/Ferds/UnityBuildReports/test4.xml";

		OpenInFileBrowser(path);
	}


	public static void OpenInMacFileBrowser(string path)
	{
		bool openInsidesOfFolder = false;

		// try mac
		string macPath = path.Replace("\\", "/"); // mac finder doesn't like backward slashes

		if (Directory.Exists(macPath)) // if path requested is a folder, automatically open insides of that folder
		{
			openInsidesOfFolder = true;
		}

		//LogMgr.Ins.LogInfo("macPath: " + macPath);
		//LogMgr.Ins.LogInfo("openInsidesOfFolder: " + openInsidesOfFolder);

		if (!macPath.StartsWith("\""))
		{
			macPath = "\"" + macPath;
		}
		if (!macPath.EndsWith("\""))
		{
			macPath = macPath + "\"";
		}
		string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;
		//LogMgr.Ins.LogInfo("arguments: " + arguments);
		try
		{
			System.Diagnostics.Process.Start("open", arguments);
		}
		catch(System.ComponentModel.Win32Exception e)
		{
			// tried to open mac finder in windows
			// just silently skip error
			// we currently have no platform define for the current OS we are in, so we resort to this
			e.HelpLink = ""; // do anything with this variable to silence warning about not using it
		}
	}

	public static void OpenInWinFileBrowser(string path)
	{
		bool openInsidesOfFolder = false;

		// try windows
		string winPath = path.Replace("/", "\\"); // windows explorer doesn't like forward slashes

		if (Directory.Exists(winPath)) // if path requested is a folder, automatically open insides of that folder
		{
			openInsidesOfFolder = true;
		}
		try
		{
			System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
		}
		catch(System.ComponentModel.Win32Exception e)
		{
			// tried to open win explorer in mac
			// just silently skip error
			// we currently have no platform define for the current OS we are in, so we resort to this
			e.HelpLink = ""; // do anything with this variable to silence warning about not using it
		}
	}

	public static void OpenInFileBrowser(string path)
	{
		if (IsInWinOS)
		{
			OpenInWinFileBrowser(path);
		}
		else if (IsInMacOS)
		{
			OpenInMacFileBrowser(path);
		}
		else // couldn't determine OS
		{
			OpenInWinFileBrowser(path);
			OpenInMacFileBrowser(path);
		}
	}

	public static bool IsInMacOS
	{
		get
		{
			return SystemInfo.operatingSystem.IndexOf("Mac OS") != -1;
		}
	}

	public static bool IsInWinOS
	{
		get
		{
			return SystemInfo.operatingSystem.IndexOf("Windows") != -1;
		}
	}

	public static string NameOfOSFileBrowser
	{
		get
		{
			return (IsInMacOS) ? "Finder" : "Explorer";
		}
	}

	public static string NameOfOSTrashFolder
	{
		get
		{
			return (IsInMacOS) ? "Trash folder" : "Recycle Bin";
		}
	}

	static string GetEditorLogFileInWindows(string editorFilename = "Editor.log")
	{
		string editorLogSubPath = "/Unity/Editor/" + editorFilename;

		// try getting from LOCALAPPDATA
		// this is the one used from after Windows XP

		string localAppDataVar = System.Environment.GetEnvironmentVariable("LOCALAPPDATA");

		if (!string.IsNullOrEmpty(localAppDataVar))
		{
			string nonXpStyleAppDataPath = localAppDataVar.Replace("\\", "/");
			if (Directory.Exists(nonXpStyleAppDataPath))
			{
				return nonXpStyleAppDataPath + editorLogSubPath;
			}
		}

		// didn't find it in LOCALAPPDATA
		// try USERPROFILE (WinXP style)

		string userProfileVar = System.Environment.GetEnvironmentVariable("USERPROFILE");

		if (!string.IsNullOrEmpty(userProfileVar))
		{
			string xpStyleAppDataPath = userProfileVar.Replace("\\", "/") + "/Local Settings/Application Data";
			if (Directory.Exists(xpStyleAppDataPath))
			{
				return xpStyleAppDataPath + editorLogSubPath;
			}
		}

	    Debug.LogError("Could not find path to Unity Editor log!");

		return "";
	}

	public static string EditorLogDefaultPath
	{
		get
		{
			if (System.Environment.OSVersion.Platform == PlatformID.Unix ||
				System.Environment.OSVersion.Platform == PlatformID.MacOSX)
			{
				return UserHomePath + "/Library/Logs/Unity/Editor.log";
			}

			return GetEditorLogFileInWindows();
		}
	}

	public static string EditorPrevLogPath
	{
		get
		{
			if (System.Environment.OSVersion.Platform == PlatformID.Unix ||
				System.Environment.OSVersion.Platform == PlatformID.MacOSX)
			{
				return UserHomePath + "/Library/Logs/Unity/Editor-prev.log";
			}

			return GetEditorLogFileInWindows("Editor-prev.log");
		}
	}

	public static string UsedEditorLogPath
	{
		get
		{
			if (IsDefaultEditorLogPathOverriden)
			{
				return BuildReportTool.Options.EditorLogOverridePath;
			}
			return EditorLogDefaultPath;
		}
	}

	public static string EditorLogPathOverrideMessage
	{
		get
		{
			if (IsDefaultEditorLogPathOverriden)
			{
				return "(Overriden)";
			}
			return "(Default)";
		}
	}

	public static bool IsDefaultEditorLogPathOverriden
	{
		get
		{
			return !string.IsNullOrEmpty(BuildReportTool.Options.EditorLogOverridePath);
		}
	}

	public static bool UsedEditorLogExists
	{
		get
		{
			return File.Exists(UsedEditorLogPath);
		}
	}




	public static string GetBuildManagedFolder(string buildFilePath)
	{
		string buildFolder = buildFilePath;

		const string WINDOWS_APP_FILE_TYPE = ".exe";
		const string MAC_APP_FILE_TYPE = ".app";

		if (buildFolder.EndsWith(WINDOWS_APP_FILE_TYPE)) // Windows
		{
			//
			// example:
			// "/Users/Ferds/Unity Projects/BuildReportTool/testwin64.exe"
			//
			// need to remove ".exe" at end
			// then append "_Data" at end
			//
			buildFolder = buildFolder.Substring(0, buildFolder.Length - WINDOWS_APP_FILE_TYPE.Length);
			buildFolder += "_Data/Managed";
		}
		else if (buildFolder.EndsWith(MAC_APP_FILE_TYPE)) // Mac OS X
		{
			//
			// example:
			// "/Users/Ferds/Unity Projects/BuildReportTool/testmac.app"
			//
			// .app is really just a folder.
			//
			buildFolder += "/Contents/Data/Managed";
		}
		else if (Directory.Exists(buildFolder + "/Data/Managed/")) // iOS
		{
			buildFolder += "/Data/Managed";
		}
		else if (!Directory.Exists(buildFolder))
		{
			// happens with users who use custom builders
			//LogMgr.Ins.LogInfoWarning("Folder \"" + buildFolder + "\" does not exist.");
			return "";
		}

		buildFolder += "/";

		return buildFolder;
	}

	static string GetProjectTempStagingArea(string projectDataPath)
	{
		string tempFolder = projectDataPath;
		const string assets = "Assets";
		tempFolder = tempFolder.Substring(0, tempFolder.Length - assets.Length);
		tempFolder += "Temp/StagingArea";
		return tempFolder;
	}

	public static bool AttemptGetWebTempStagingArea(string projectDataPath, out string path)
	{
		string tempFolder = GetProjectTempStagingArea(projectDataPath) + "/Data/Managed/";

		if (Directory.Exists(tempFolder))
		{
			path = tempFolder;
			return true;
		}
		path = "";
		return false;
	}

	public static bool AttemptGetAndroidTempStagingArea(string projectDataPath, out string path)
	{
		string tempFolder = GetProjectTempStagingArea(projectDataPath) + "/assets/bin/Data/Managed/";

		//LogMgr.Ins.LogInfo(tempFolder);

		if (Directory.Exists(tempFolder))
		{
			path = tempFolder;
			return true;
		}
		path = "";
		return false;
	}

	public static bool AttemptGetUnityFolderMonoDLLs(bool wasWebBuild, bool wasAndroidApkBuild, string editorAppContentsPath, ApiCompatibilityLevel monoLevel, StrippingLevel codeStrippingLevel, out string path, out string higherPriorityPath)
	{
		bool success = false;

		// more hackery
		// attempt to get DLL size info
		// from Unity install folder
		//
		// this only happens in:
		//  1. Web build
		//  2. Android build
		//  3. Custom builders
		//
		string[] pathTries = new string[]
		{
			editorAppContentsPath + "/Frameworks/Mono/lib/mono",
			editorAppContentsPath + "/Mono/lib/mono",
			"/Applications/Unity/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"C:/Program Files (x86)/Unity/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity/Editor/Data/Mono/lib/mono",
#if UNITY_3_5
			"/Applications/Unity3/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"/Applications/Unity 3/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"/Applications/Unity3.5/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"/Applications/Unity 3.5/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"C:/Program Files (x86)/Unity3/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity 3/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity3.5/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity 3.5/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity3/Editor/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity 3/Editor/Data/Mono/lib/mono",
#endif
#if UNITY_4_AND_GREATER
			"/Applications/Unity4/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"/Applications/Unity 4/Unity.app/Contents/Frameworks/Mono/lib/mono",
			"C:/Program Files (x86)/Unity4/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity 4/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity4/Editor/Data/Mono/lib/mono",
			"C:/Program Files (x86)/Unity 4/Editor/Data/Mono/lib/mono",
#endif
		};

		string tryPath = "";

		for (int n = 0, len = pathTries.Length; n < len; ++n)
		{
			tryPath = pathTries[n];
			if (Directory.Exists(tryPath))
			{
				break;
			}
			tryPath = "";
		}

		if (!string.IsNullOrEmpty(tryPath))
		{
			success = true;

			// "unity_web" is obviously for the web build. Presumably, this one has DLLs removed that can compromise web security.
			// "2.0" is likely the full featured Mono libraries
			// "unity" is most likely the one used when selecting 2.0 subset in the player settings. this is the setting by default.
			// "micro" is probably the one used in StrippingLevel.UseMicroMSCorlib. only makes sense to be here when building on Android.
			//   since in iOS, we already have the DLL files. No need for this hackery in iOS. But since in Android we do not have a project folder,
			//   we resort to this.

			if (wasWebBuild)
			{
				path = tryPath + "/unity_web/";
			}
			else if (monoLevel == ApiCompatibilityLevel.NET_2_0_Subset)
			{
				path = tryPath + "/unity/";
			}
			else
			{
				path = tryPath + "/2.0/";
			}

			if (wasAndroidApkBuild && codeStrippingLevel == StrippingLevel.UseMicroMSCorlib)
			{
				higherPriorityPath = tryPath + "/micro/";
			}
			else
			{
				higherPriorityPath = "";
			}
		}
		else
		{
			path = "";
			higherPriorityPath = "";
		}

		return success;
	}




	public static string[] GetAllScenesUsedInProject()
	{
		return EditorBuildSettings.scenes.Where(s => s.enabled).Select(n => n.path).ToArray();
	}


	public static BuildReportTool.SizePart CreateSizePartFromFile(string filename, string fileFullPath)
	{
		BuildReportTool.SizePart outPart = new BuildReportTool.SizePart();

		outPart.Name = System.Security.SecurityElement.Escape(filename);

		if (File.Exists(fileFullPath))
		{
			long fileSizeBytes = GetFileSizeInBytes(fileFullPath);
			outPart.RawSizeBytes = fileSizeBytes;
			outPart.RawSize = GetBytesReadable(fileSizeBytes);


			long importedSizeBytes = -1;

			outPart.ImportedSizeBytes = importedSizeBytes;
			outPart.ImportedSize = BuildReportTool.Util.GetBytesReadable(importedSizeBytes);
		}
		else
		{
			outPart.RawSizeBytes = -1;
			outPart.RawSize = "???";
		}

		/// \todo perhaps compute percentage: file size of this DLL out of total build size (would need to convert string of total build size into an int of bytes)
		outPart.Percentage = -1;

		return outPart;
	}



	static void SaveTextFile(string saveFilePath, string data)
	{
		string folder = System.IO.Path.GetDirectoryName(saveFilePath);
		System.IO.Directory.CreateDirectory(folder);

#if UNITY_WEBPLAYER && !UNITY_EDITOR
		LogMgr.Ins.LogError("Current build target is set to Web Player. Cannot perform file input/output when in Web Player.");
#else
		System.IO.StreamWriter write = new System.IO.StreamWriter(saveFilePath, false, System.Text.Encoding.UTF8); // Unity's TextAsset.text borks when encoding used is UTF8 :(
		write.Write(data);
		write.Flush();
		write.Close();
		write.Dispose();
#endif
	}

	static string FixXmlBuildReportFile(string serializedBuildInfoFilePath)
	{
		string xmlData = GetTextFileContents(serializedBuildInfoFilePath);

		if (string.IsNullOrEmpty(xmlData))
		{
			return "";
		}

		xmlData = xmlData.Replace("BuildSizePart", "SizePart");

		// quick and dirty fix for invalid XML characters in filenames
		xmlData = xmlData.Replace("&#x1;", "");
		xmlData = xmlData.Replace("&#x2;", "");
		xmlData = xmlData.Replace("&#x3;", "");
		xmlData = xmlData.Replace("&#x4;", "");
		xmlData = xmlData.Replace("&#x5;", "");
		xmlData = xmlData.Replace("&#x6;", "");
		xmlData = xmlData.Replace("&#x7;", "");
		xmlData = xmlData.Replace("&#x8;", "");
		xmlData = xmlData.Replace("&#xb;", "");
		xmlData = xmlData.Replace("&#xc;", "");
		xmlData = xmlData.Replace("&#xe;", "");
		xmlData = xmlData.Replace("&#xf;", "");
		xmlData = xmlData.Replace("&#x10;", "");
		xmlData = xmlData.Replace("&#x11;", "");
		xmlData = xmlData.Replace("&#x12;", "");
		xmlData = xmlData.Replace("&#x13;", "");
		xmlData = xmlData.Replace("&#x14;", "");
		xmlData = xmlData.Replace("&#x15;", "");
		xmlData = xmlData.Replace("&#x16;", "");
		xmlData = xmlData.Replace("&#x17;", "");
		xmlData = xmlData.Replace("&#x18;", "");
		xmlData = xmlData.Replace("&#x19;", "");
		xmlData = xmlData.Replace("&#x1a;", "");
		xmlData = xmlData.Replace("&#x1b;", "");
		xmlData = xmlData.Replace("&#x1c;", "");
		xmlData = xmlData.Replace("&#x1d;", "");
		xmlData = xmlData.Replace("&#x1e;", "");
		xmlData = xmlData.Replace("&#x1f;", "");
		xmlData = xmlData.Replace("&#x7f;", "");
		xmlData = xmlData.Replace("&#x81;", "");

		SaveTextFile(serializedBuildInfoFilePath, xmlData);

		return xmlData;
	}

	public static string MyHtmlDecode(string input)
	{
		input = input.Replace("&lt;", "<");
		input = input.Replace("&gt;", ">");
		input = input.Replace("&amp;", "&");
		input = input.Replace("&apos;", "'");
		input = input.Replace("&quot;", "\"");
		input = input.Replace("&ntilde;", "ñ");
		input = input.Replace("&Ntilde;", "Ñ");
		input = input.Replace("&copy;", "©");
		input = input.Replace("&reg;", "®");
		input = input.Replace("&#8482;", "™");

		return input;
	}



	public static BuildInfo OpenSerializedBuildInfo(string serializedBuildInfoFilePath, bool fromMainThread = true)
	{
		BuildInfo ret = null;

		XmlSerializer x = new XmlSerializer( typeof(BuildInfo) );

		string correctedXmlData = FixXmlBuildReportFile(serializedBuildInfoFilePath);

		try
		{
			// when the string has contents, it means there were corrections to the xml data
			// and we should load that updated content instead of reading the file
			if (!string.IsNullOrEmpty(correctedXmlData))
			{
				TextReader reader = new StringReader(correctedXmlData);
				ret = (BuildInfo)x.Deserialize(reader);
			}
			else
			{
				// no corrections in the xml file
				// proceed to open the file normally
				using(FileStream fs = new FileStream(serializedBuildInfoFilePath, FileMode.Open))
				{
					XmlReader reader = new XmlTextReader(fs);
					ret = (BuildInfo)x.Deserialize(reader);
					fs.Close();
				}
			}
		}
		catch (Exception e)
		{
		    Debug.LogError(e);
		}

		if (fromMainThread)
		{
			if (BuildInfoHasContents(ret))
			{
				ret.OnDeserialize();
				ret.SetSavedPath(serializedBuildInfoFilePath);
			}
			else
			{
			    Debug.LogError("Build Report Tool: Invalid data in build info file: " + serializedBuildInfoFilePath);
			}
		}

		return ret;
	}

	public static void SerializeBuildInfoAtFolder(BuildInfo buildInfo, string pathToSave)
	{
		string filePath = pathToSave;
		if (!string.IsNullOrEmpty(pathToSave))
		{
			if (!Directory.Exists(pathToSave))
			{
				Directory.CreateDirectory(pathToSave);
			}
			filePath += "/";
		}
		//LogMgr.Ins.LogInfo("built folder");
		filePath += buildInfo.ProjectName + "-" + buildInfo.BuildType + buildInfo.TimeGot.ToString("-yyyyMMMdd-HHmmss") + ".xml";
		//LogMgr.Ins.LogInfo("built filepath " + filePath);

		SerializeBuildInfo(buildInfo, filePath);
	}

	public static void SerializeBuildInfo(BuildInfo buildInfo, string serializedBuildInfoFilePath)
	{
		XmlSerializer x = new XmlSerializer( typeof(BuildInfo) );
		TextWriter writer = new StreamWriter(serializedBuildInfoFilePath);
		x.Serialize(writer, buildInfo);
		writer.Close();

		buildInfo.SetSavedPath(serializedBuildInfoFilePath);

	    Debug.Log("Build Report Tool: Saved build info at \"" + buildInfo.SavedPath + "\"");
	}

	public static bool BuildInfoHasContents(BuildInfo n)
	{
		return n != null && n.HasContents;
	}
}

} // namespace BuildReportTool
