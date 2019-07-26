
#if UNITY_4 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#define UNITY_5_2_AND_LESSER
#endif

using UnityEditor;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace BuildReportTool
{

// class for holding a build report
// this is the class that is serialized when saving a build report to a file
[System.Serializable, XmlRoot("BuildInfo")]
public class BuildInfo
{
	// General Info
	// ==================================================================================
	public string ProjectName;
	public string BuildType; // type of build as reported by the Unity editor log

	public string SuitableTitle
	{
		get
		{
			if (UnityBuildSettings != null && UnityBuildSettings.HasValues && !string.IsNullOrEmpty(UnityBuildSettings.ProductName))
			{
				return UnityBuildSettings.ProductName;
			}
			return ProjectName;
		}
	}

	
	System.TimeSpan _reportGenerationTime;
	
	[XmlIgnore]
	public TimeSpan ReportGenerationTime
    {
		get { return _reportGenerationTime; }
		set { _reportGenerationTime = value; }
    }
	
	[XmlElement("ReportGenerationTime")]
    public long ReportGenerationTimeInTicks
    {
		get { return _reportGenerationTime.Ticks; }
		set { _reportGenerationTime = new TimeSpan(value); }
    }
	

	public DateTime TimeGot;
	public string TimeGotReadable;

	public string GetTimeReadable()
	{
		if (!string.IsNullOrEmpty(TimeGotReadable))
		{
			return TimeGotReadable;
		}
		return TimeGot.ToString(BuildReportTool.ReportGenerator.TIME_OF_BUILD_FORMAT);
	}



	public string UnityVersion = "";


	public string UnityVersionDisplayed
	{
		get
		{
			if (UnityBuildSettings != null && UnityBuildSettings.HasValues)
			{
				return UnityVersion + (UnityBuildSettings.UsingAdvancedLicense ? " Pro" : "");
			}
			return UnityVersion;
		}
	}

	public string EditorAppContentsPath = "";
	public string ProjectAssetsPath = "";
	public string BuildFilePath = "";

	public ApiCompatibilityLevel MonoLevel;
	public StrippingLevel CodeStrippingLevel;


	// Build Settings
	// ==================================================================================
	public UnityBuildSettings UnityBuildSettings;
	public bool HasUnityBuildSettings;


	// unity/os environment values at time of build report creation
	// ==================================================================================

	public string[] ScenesIncludedInProject;
	public string[] PrefabsUsedInScenes;

	public bool AndroidUseAPKExpansionFiles;
	public bool AndroidCreateProject;




	// sizes
	// ==================================================================================

	public BuildReportTool.SizePart[] BuildSizes;

	public void SortSizes()
	{
		Array.Sort(BuildSizes, delegate(BuildReportTool.SizePart b1, BuildReportTool.SizePart b2) {
			if (b1.Percentage > b2.Percentage) return -1;
			else if (b1.Percentage < b2.Percentage) return 1;
			// if percentages are equal, check actual file size (approximate values)
			else if (b1.DerivedSize > b2.DerivedSize) return -1;
			else if (b1.DerivedSize < b2.DerivedSize) return 1;
			return 0;
		});
	}


	/// <summary>
	/// This is called right after generating a build report.
	/// </summary>
	public void FixSizes()
	{
#if UNITY_5_2_AND_LESSER
		// this bug has already been fixed since Unity 5.2.1
		// so we only execute this for Unity 5.2.0 and below

		if (DldUtil.UnityVersion.IsUnityVersionAtLeast(5, 2, 1))
		{
			return;
		}
		
		// --------------------------------------------------------------------------------
		// fix imported sizes of Resources files

		for (int n = 0; n < UsedAssets.All.Length; ++n)
		{
			if (BuildReportTool.Util.IsFileInAPath(UsedAssets.All[n].Name, "/Resources/"))
			{
				UsedAssets.All[n].ImportedSizeBytes = BRT_LibCacheUtil.GetImportedFileSize(UsedAssets.All[n].Name);
				UsedAssets.All[n].ImportedSize = BuildReportTool.Util.GetBytesReadable(UsedAssets.All[n].ImportedSizeBytes);

				UsedAssets.All[n].RawSizeBytes = UsedAssets.All[n].ImportedSizeBytes;
				UsedAssets.All[n].RawSize = UsedAssets.All[n].ImportedSize;
				
				UsedAssets.All[n].DerivedSize = 0;
				UsedAssets.All[n].Percentage = -1;
			}
		}

		UsedAssets.ResortDefault();

		// --------------------------------------------------------------------------------
		// recalculate percentages
		
		// add textures, meshes, sounds, and animations that are in resources folder to the build size
		// since they are not included anymore in Unity 5
		
		var resourcesTextureSizeSum = GetSizeSumForUsedAssets("/Resources/", BuildReportTool.Util.IsFileAUnityTexture);
		AddToSize("Textures", resourcesTextureSizeSum);

		var resourcesMeshSizeSum = GetSizeSumForUsedAssets("/Resources/", BuildReportTool.Util.IsFileAUnityMesh);
		AddToSize("Meshes", resourcesMeshSizeSum);
		
		var resourcesSoundsSizeSum = GetSizeSumForUsedAssets("/Resources/", BuildReportTool.Util.IsFileAUnitySound);
		AddToSize("Sounds", resourcesSoundsSizeSum);

		var resourcesAnimationsSizeSum = GetSizeSumForUsedAssets("/Resources/", BuildReportTool.Util.IsFileAUnityAnimation);
		AddToSize("Animations", resourcesAnimationsSizeSum);
		
		AddToTotalSize(resourcesTextureSizeSum);
		AddToTotalSize(resourcesMeshSizeSum);
		AddToTotalSize(resourcesSoundsSizeSum);
		AddToTotalSize(resourcesAnimationsSizeSum);

		FixPercentages();

		// sort sizes again since we modified them
		SortSizes();
#endif
	}

	void FixPercentages()
	{
		double totalSize = 0;
		
		var totalSizePart = BuildSizes.FirstOrDefault(part => part.IsTotal);

		if (totalSizePart != null)
		{
			totalSize = totalSizePart.DerivedSize;
		}

		if (totalSize > 0)
		{
			for (int n = 0, len = BuildSizes.Length; n < len; ++n)
			{
				BuildSizes[n].Percentage = Math.Round((BuildSizes[n].DerivedSize/totalSize) * 100, 2, MidpointRounding.AwayFromZero);
			}
		}
	}

	long GetSizeSumForUsedAssets(string assetFolderName, Func<string, bool> fileTypePredicate)
	{
		if (UsedAssets == null || UsedAssets.All == null)
		{
			return 0;
		}

		return UsedAssets.All.Where(part => BuildReportTool.Util.IsFileInAPath(part.Name, assetFolderName) && fileTypePredicate(part.Name))
					.Sum(part => BRT_LibCacheUtil.GetImportedFileSize(part.Name));
	}

	static void AddToSize(BuildReportTool.SizePart buildSize, long sizeToAdd)
	{
		if (buildSize != null)
		{
			buildSize.DerivedSize += sizeToAdd;
			buildSize.Size = BuildReportTool.Util.GetBytesReadable(buildSize.DerivedSize);
		}
	}
	
	void AddToSize(string buildSizeName, long sizeToAdd)
	{
		if (sizeToAdd == 0)
		{
			return;
		}

		BuildReportTool.SizePart buildSize = BuildSizes.FirstOrDefault(part => part.Name == buildSizeName);

		if (buildSize != null)
		{
			//LogMgr.Ins.LogInfoFormat("{0} size before: {1}", buildSizeName, buildSize.DerivedSize);

			AddToSize(buildSize, sizeToAdd);

			//LogMgr.Ins.LogInfoFormat("{0} size after: {1}", buildSizeName, buildSize.DerivedSize);
		}
	}
	
	void AddToTotalSize(long sizeToAdd)
	{
		if (sizeToAdd == 0)
		{
			return;
		}

		BuildReportTool.SizePart buildSize = BuildSizes.FirstOrDefault(part => part.IsTotal);

		if (buildSize != null)
		{
			//LogMgr.Ins.LogInfoFormat("total size before: {0}", buildSize.DerivedSize);

			AddToSize(buildSize, sizeToAdd);

			UsedTotalSize = buildSize.Size;

			//LogMgr.Ins.LogInfoFormat("total size after: {0}", buildSize.DerivedSize);
		}
	}

	// total sizes
	// ==================================================================================

	// old size values were only TotalBuildSize and CompressedBuildSize

	public bool HasOldSizeValues
	{
		get
		{
			return
				string.IsNullOrEmpty(UnusedTotalSize) &&
				string.IsNullOrEmpty(UsedTotalSize) &&
				string.IsNullOrEmpty(StreamingAssetsSize) &&
				string.IsNullOrEmpty(WebFileBuildSize) &&
				string.IsNullOrEmpty(AndroidApkFileBuildSize) &&
				string.IsNullOrEmpty(AndroidObbFileBuildSize);
		}
	}

	public string CompressedBuildSize = ""; // not used anymore

	public string UnusedTotalSize = "";
	public string UsedTotalSize = "";
	public string StreamingAssetsSize = "";

	public bool HasStreamingAssets
	{
		get
		{
			return StreamingAssetsSize != "0 B";
		}
	}

	public string TotalBuildSize = "";


	// per-platform sizes
	public string WebFileBuildSize = "";
	public string AndroidApkFileBuildSize = "";
	public string AndroidObbFileBuildSize = "";




	// file entries
	// ==================================================================================

	public BuildReportTool.SizePart[] MonoDLLs;
	public BuildReportTool.SizePart[] ScriptDLLs;

	public FileFilterGroup FileFilters = null;

	public AssetList UsedAssets = null;
	public AssetList UnusedAssets = null;

	public bool HasUsedAssets
	{
		get
		{
			return UsedAssets != null;
		}
	}
	public bool HasUnusedAssets
	{
		get
		{
			return UnusedAssets != null;
		}
	}





	// build report tool options values at time of building
	// ==================================================================================

	public bool IncludedSvnInUnused;
	public bool IncludedGitInUnused;

	public bool UsedAssetsIncludedInCreation;
	public bool UnusedAssetsIncludedInCreation;
	public bool UnusedPrefabsIncludedInCreation;

	public int UnusedAssetsEntriesPerBatch;




	// temp variables that are not serialized into the XML file
	// ==================================================================================

	// only used while generating the build report or opening one

	int _unusedAssetsBatchNum = 0;

	public int UnusedAssetsBatchNum { get{ return _unusedAssetsBatchNum; } }
	public void MoveUnusedAssetsBatchNumToNext()
	{
		++_unusedAssetsBatchNum;
	}
	public void MoveUnusedAssetsBatchNumToPrev()
	{
		if (_unusedAssetsBatchNum == 0)
		{
			return;
		}
		--_unusedAssetsBatchNum;
	}
	
	// ---------------------------------------

	string _savedPath;

	public string SavedPath { get{ return _savedPath; } }
	public void SetSavedPath(string val)
	{
		_savedPath = val;
	}
	
	// ---------------------------------------

	BuildTarget _buildTarget;

	public BuildTarget BuildTargetUsed { get{ return _buildTarget; } }
	public void SetBuildTargetUsed(BuildTarget val)
	{
		_buildTarget = val;
	}

	// ---------------------------------------

	bool _refreshRequest;

	public void FlagOkToRefresh()
	{
		_refreshRequest = true;
	}

	public void FlagFinishedRefreshing()
	{
		_refreshRequest = false;
	}

	public bool RequestedToRefresh { get{ return _refreshRequest; } }




	// Queries
	// ==================================================================================

	public bool HasContents
	{
		get
		{
			// build sizes can't be empty (they are always there when you build)
			// script dlls can't be empty (the project is sure to have some scripts in it)
			return !string.IsNullOrEmpty(ProjectName) && (BuildSizes != null && BuildSizes.Length > 0) && (ScriptDLLs != null && ScriptDLLs.Length > 0);
		}
	}




	// Commands
	// ==================================================================================

	public string GetDefaultFilename()
	{
		return ProjectName + "-" + BuildType + TimeGot.ToString("-yyyyMMMdd-HHmmss") + ".xml";
	}

	public void UnescapeAssetNames()
	{
		if (UsedAssets != null)
		{
			UsedAssets.UnescapeAssetNames();
		}

		if (UnusedAssets != null)
		{
			UnusedAssets.UnescapeAssetNames();
		}
	}

	public void RecategorizeAssetLists()
	{
		FileFilterGroup fileFiltersToUse = FileFilters;

		if (BuildReportTool.Options.ShouldUseConfiguredFileFilters())
		{
			fileFiltersToUse = BuildReportTool.FiltersUsed.GetProperFileFilterGroupToUse();
			//LogMgr.Ins.LogInfo("going to use configured file filters instead... loaded: " + (fileFiltersToUse != null));
		}

		if (UsedAssets != null)
		{
			UsedAssets.AssignPerCategoryList( BuildReportTool.ReportGenerator.SegregateAssetSizesPerCategory(UsedAssets.All, fileFiltersToUse) );

			UsedAssets.RefreshFilterLabels(fileFiltersToUse);
		}

		if (UnusedAssets != null)
		{
			UnusedAssets.AssignPerCategoryList( BuildReportTool.ReportGenerator.SegregateAssetSizesPerCategory(UnusedAssets.All, fileFiltersToUse) );

			UnusedAssets.RefreshFilterLabels(fileFiltersToUse);
		}
	}

	void CalculateUsedAssetsDerivedSizes()
	{
		if (UsedAssets != null)
		{
			for (int n = 0, len = UsedAssets.All.Length; n < len; ++n)
			{
				UsedAssets.All[n].DerivedSize = BuildReportTool.Util.GetApproxSizeFromString(UsedAssets.All[n].Size);
			}
		}
	}




	// Events
	// ==================================================================================

	public void OnDeserialize()
	{
		if (HasContents)
		{
			CalculateUsedAssetsDerivedSizes();
			UnescapeAssetNames();
			RecategorizeAssetLists();
		}
	}
}

} // namespace BuildReportTool
