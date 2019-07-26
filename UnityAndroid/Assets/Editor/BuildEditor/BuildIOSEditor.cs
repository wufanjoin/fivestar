#if UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.IO;
using ETModel;

namespace MyEditor
{
	public class BuildIOSEditor : EditorWindow
	{
		[PostProcessBuild]
		private static void OnPostprocessBuild(BuildTarget target, string pathToBuildProject)
		{
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			string _projPath = PBXProject.GetPBXProjectPath(pathToBuildProject);
			PBXProject project = new PBXProject();
			project.ReadFromString(File.ReadAllText(_projPath));
			string targetGuid = project.TargetGuidByName("Unity-iPhone");

            project.AddFrameworkToProject(targetGuid, "CFNetwork.framework", false);
		    project.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
		    project.AddFrameworkToProject(targetGuid, "Security.framework", false);
		    project.AddFrameworkToProject(targetGuid, "SystemConfiguration.framework", false);
		    project.AddFrameworkToProject(targetGuid, "Security.framework", false);
		    project.AddFrameworkToProject(targetGuid, "AdSupport.framework", false);
		    project.AddFrameworkToProject(targetGuid, "CoreLocation.framework", false);

		    project.AddFileToBuild(targetGuid, project.AddFile("usr/lib/libc++.tbd", "libc++.tbd", PBXSourceTree.Sdk));
		    project.AddFileToBuild(targetGuid, project.AddFile("usr/lib/libsqlite3.0.tbd", "libsqlite3.0.tbd", PBXSourceTree.Sdk));
		    project.AddFileToBuild(targetGuid, project.AddFile("usr/lib/libz.tbd", "libz.tbd", PBXSourceTree.Sdk));

		    project.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");

		    project.SetBuildProperty(targetGuid, "LOCATION_UPDATES", "YES");

            project.WriteToFile(_projPath);


			// plist
			string plistPath = pathToBuildProject + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));
			PlistElementDict rootDict = plist.root;
			
			PlistElementDict dictTmp = rootDict.CreateDict("NSAppTransportSecurity");
			dictTmp.SetBoolean("NSAllowsArbitraryLoads", true);

			// 保存plist  
			plist.WriteToFile(plistPath);
            Log.Debug("完成IOS工程配置");
		}
	}
}
#endif