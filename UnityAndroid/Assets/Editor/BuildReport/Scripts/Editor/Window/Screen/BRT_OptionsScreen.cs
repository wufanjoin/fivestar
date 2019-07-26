using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;



namespace BuildReportTool.Window.Screen
{

public class Options : BaseScreen
{
	public override string Name { get{ return Labels.OPTIONS_CATEGORY_LABEL; } }

	string[] _saveTypeLabels = null;

	string[] _autoShowWindowLabels = new string[] {
		"Never",
		"Always",
		"Yes, but not during batchmode"};

	int _selectedAutoShowWindowIdx = 2;





	string[] _fileFilterToUseType = new string[] {Labels.FILTER_GROUP_TO_USE_CONFIGURED_LABEL, Labels.FILTER_GROUP_TO_USE_EMBEDDED_LABEL};



	string OPEN_IN_FILE_BROWSER_OS_SPECIFIC_LABEL
	{
		get
		{
			if (BuildReportTool.Util.IsInWinOS)
				return Labels.OPEN_IN_FILE_BROWSER_WIN_LABEL;
			if (BuildReportTool.Util.IsInMacOS)
				return Labels.OPEN_IN_FILE_BROWSER_MAC_LABEL;

			return Labels.OPEN_IN_FILE_BROWSER_DEFAULT_LABEL;
		}
	}

	string SAVE_PATH_TYPE_PERSONAL_OS_SPECIFIC_LABEL
	{
		get
		{
			if (BuildReportTool.Util.IsInWinOS)
				return Labels.SAVE_PATH_TYPE_PERSONAL_WIN_LABEL;
			if (BuildReportTool.Util.IsInMacOS)
				return Labels.SAVE_PATH_TYPE_PERSONAL_MAC_LABEL;

			return Labels.SAVE_PATH_TYPE_PERSONAL_DEFAULT_LABEL;
		}
	}





	string[] _calculationTypeLabels = new string[] {
		Labels.CALCULATION_LEVEL_FULL_NAME,
		Labels.CALCULATION_LEVEL_NO_PREFAB_NAME,
		Labels.CALCULATION_LEVEL_NO_UNUSED_NAME,
		Labels.CALCULATION_LEVEL_MINIMAL_NAME};

	int _selectedCalculationLevelIdx = 0;

	string CalculationLevelDescription
	{
		get
		{
			switch (_selectedCalculationLevelIdx)
			{
				case 0:
					return Labels.CALCULATION_LEVEL_FULL_DESC;
				case 1:
					return Labels.CALCULATION_LEVEL_NO_PREFAB_DESC;
				case 2:
					return Labels.CALCULATION_LEVEL_NO_UNUSED_DESC;
				case 3:
					return Labels.CALCULATION_LEVEL_MINIMAL_DESC;
			}
			return "";
		}
	}

	int GetCalculationLevelGuiIdxFromOptions()
	{
		if (BuildReportTool.Options.IsCurrentCalculationLevelAtFull)
		{
			return 0;
		}
		if (BuildReportTool.Options.IsCurrentCalculationLevelAtNoUnusedPrefabs)
		{
			return 1;
		}
		if (BuildReportTool.Options.IsCurrentCalculationLevelAtNoUnusedAssets)
		{
			return 2;
		}
		if (BuildReportTool.Options.IsCurrentCalculationLevelAtOverviewOnly)
		{
			return 3;
		}
		return 0;
	}

	void SetCalculationLevelFromGuiIdx(int selectedIdx)
	{
		switch (selectedIdx)
		{
			case 0:
				BuildReportTool.Options.SetCalculationLevelToFull();
				break;
			case 1:
				BuildReportTool.Options.SetCalculationLevelToNoUnusedPrefabs();
				break;
			case 2:
				BuildReportTool.Options.SetCalculationLevelToNoUnusedAssets();
				break;
			case 3:
				BuildReportTool.Options.SetCalculationLevelToOverviewOnly();
				break;
		}
	}




	Vector2 _assetListScrollPos;




	public override void RefreshData(BuildInfo buildReport)
	{
		_saveTypeLabels = new string[] {SAVE_PATH_TYPE_PERSONAL_OS_SPECIFIC_LABEL, Labels.SAVE_PATH_TYPE_PROJECT_LABEL};

		_selectedAutoShowWindowIdx = GetAutoShowWindowTypeGuiIdxFromOptions();
		_selectedCalculationLevelIdx = GetCalculationLevelGuiIdxFromOptions();
	}

	public override void DrawGUI(Rect position, BuildInfo buildReportToDisplay)
	{
		GUILayout.Space(10); // extra top padding


		_assetListScrollPos = GUILayout.BeginScrollView(_assetListScrollPos);

		GUILayout.BeginHorizontal();
			GUILayout.Space(20); // extra left padding
			GUILayout.BeginVertical();

				// === Main Options ===

				GUILayout.Label("Main Options", BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);

				BuildReportTool.Options.CollectBuildInfo = GUILayout.Toggle(BuildReportTool.Options.CollectBuildInfo, Labels.COLLECT_BUILD_INFO_LABEL);

				BuildReportTool.Options.AllowDeletingOfUsedAssets = GUILayout.Toggle(BuildReportTool.Options.AllowDeletingOfUsedAssets, "Allow deleting of Used Assets (practice caution!)");

				GUILayout.Space(10);


				BuildReportTool.Options.IncludeBuildSizeInReportCreation = GUILayout.Toggle(BuildReportTool.Options.IncludeBuildSizeInReportCreation, "Get build's file size upon creation of a build report");

				BuildReportTool.Options.GetImportedSizesForUsedAssets = GUILayout.Toggle(BuildReportTool.Options.GetImportedSizesForUsedAssets, "Get imported sizes of Used Assets upon creation of a build report");

				BuildReportTool.Options.GetImportedSizesForUnusedAssets = GUILayout.Toggle(BuildReportTool.Options.GetImportedSizesForUnusedAssets, "Get imported sizes of Unused Assets upon creation of a build report");

				BuildReportTool.Options.GetProjectSettings = GUILayout.Toggle(BuildReportTool.Options.GetProjectSettings, "Get Unity project settings upon creation of a build report");

				GUILayout.Space(10);

				GUILayout.BeginHorizontal();
					GUILayout.Label("Automatically show Build Report Window after building: ");

					GUILayout.BeginVertical();
						int newSelectedAutoShowWindowIdx = EditorGUILayout.Popup(_selectedAutoShowWindowIdx, _autoShowWindowLabels, "Popup", GUILayout.Width(300));
					GUILayout.EndVertical();

					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();


				if (newSelectedAutoShowWindowIdx != _selectedAutoShowWindowIdx)
				{
					_selectedAutoShowWindowIdx = newSelectedAutoShowWindowIdx;
					SetAutoShowWindowTypeFromGuiIdx(newSelectedAutoShowWindowIdx);
				}



				GUILayout.Space(10);

				GUILayout.BeginHorizontal();
					GUILayout.Label("Calculation Level: ");

					GUILayout.BeginVertical();
						int newSelectedCalculationLevelIdx = EditorGUILayout.Popup(_selectedCalculationLevelIdx, _calculationTypeLabels, "Popup", GUILayout.Width(300));
						GUILayout.BeginHorizontal();
							GUILayout.Space(20);
							GUILayout.Label(CalculationLevelDescription, GUILayout.MaxWidth(500), GUILayout.MinHeight(75));
						GUILayout.EndHorizontal();
					GUILayout.EndVertical();

					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(BuildReportTool.Window.Settings.CATEGORY_VERTICAL_SPACING);

				if (newSelectedCalculationLevelIdx != _selectedCalculationLevelIdx)
				{
					_selectedCalculationLevelIdx = newSelectedCalculationLevelIdx;
					SetCalculationLevelFromGuiIdx(newSelectedCalculationLevelIdx);
				}


				// === Editor Log File ===

				GUILayout.Label("Editor Log File", BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);

				// which Editor.log is used
				GUILayout.BeginHorizontal();
					GUILayout.Label(Labels.EDITOR_LOG_LABEL + BuildReportTool.Util.EditorLogPathOverrideMessage + ": " + BuildReportTool.Util.UsedEditorLogPath);
					if (GUILayout.Button(OPEN_IN_FILE_BROWSER_OS_SPECIFIC_LABEL) && BuildReportTool.Util.UsedEditorLogExists)
					{
						BuildReportTool.Util.OpenInFileBrowser(BuildReportTool.Util.UsedEditorLogPath);
					}
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				if (!BuildReportTool.Util.UsedEditorLogExists)
				{
					GUILayout.Label(Labels.EDITOR_LOG_INVALID_MSG);
				}

				// override which log is opened
				GUILayout.BeginHorizontal();
					if (GUILayout.Button(Labels.SET_OVERRIDE_LOG_LABEL))
					{
						string filepath = EditorUtility.OpenFilePanel(
							"", // title
							"", // default path
							""); // file type (only one type allowed?)

						if (!string.IsNullOrEmpty(filepath))
						{
							BuildReportTool.Options.EditorLogOverridePath = filepath;
						}
					}
					if (GUILayout.Button(Labels.CLEAR_OVERRIDE_LOG_LABEL))
					{
						BuildReportTool.Options.EditorLogOverridePath = "";
					}
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(BuildReportTool.Window.Settings.CATEGORY_VERTICAL_SPACING);




				// === Asset Lists ===

				GUILayout.Label("Asset Lists", BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);

				BuildReportTool.Options.IncludeSvnInUnused = GUILayout.Toggle(BuildReportTool.Options.IncludeSvnInUnused, Labels.INCLUDE_SVN_LABEL);
				BuildReportTool.Options.IncludeGitInUnused = GUILayout.Toggle(BuildReportTool.Options.IncludeGitInUnused, Labels.INCLUDE_GIT_LABEL);

				GUILayout.Space(10);

				// pagination length
				GUILayout.BeginHorizontal();
					GUILayout.Label("View assets per groups of:");
					string pageInput = GUILayout.TextField(BuildReportTool.Options.AssetListPaginationLength.ToString(), GUILayout.MinWidth(100));
					pageInput = Regex.Replace(pageInput, @"[^0-9]", ""); // positive numbers only, no fractions
					if (string.IsNullOrEmpty(pageInput))
					{
						pageInput = "0";
					}
					BuildReportTool.Options.AssetListPaginationLength = int.Parse(pageInput);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(10);

				// unused assets entries per batch
				GUILayout.BeginHorizontal();
					GUILayout.Label("Process unused assets per batches of:");
					string entriesPerBatchInput = GUILayout.TextField(BuildReportTool.Options.UnusedAssetsEntriesPerBatch.ToString(), GUILayout.MinWidth(100));
					entriesPerBatchInput = Regex.Replace(entriesPerBatchInput, @"[^0-9]", ""); // positive numbers only, no fractions
					if (string.IsNullOrEmpty(entriesPerBatchInput))
					{
						entriesPerBatchInput = "0";
					}
					BuildReportTool.Options.UnusedAssetsEntriesPerBatch = int.Parse(entriesPerBatchInput);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();


				GUILayout.Space(10);

				// choose which file filter group to use
				GUILayout.BeginHorizontal();
					GUILayout.Label(Labels.FILTER_GROUP_TO_USE_LABEL);
					BuildReportTool.Options.FilterToUseInt = GUILayout.SelectionGrid(BuildReportTool.Options.FilterToUseInt, _fileFilterToUseType, _fileFilterToUseType.Length);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				// display which file filter group is used
				GUILayout.BeginHorizontal();
					GUILayout.Label(Labels.FILTER_GROUP_FILE_PATH_LABEL + BuildReportTool.FiltersUsed.GetProperFileFilterGroupToUseFilePath()); // display path to used file filter
					if (GUILayout.Button(OPEN_IN_FILE_BROWSER_OS_SPECIFIC_LABEL))
					{
						BuildReportTool.Util.OpenInFileBrowser( BuildReportTool.FiltersUsed.GetProperFileFilterGroupToUseFilePath() );
					}
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(BuildReportTool.Window.Settings.CATEGORY_VERTICAL_SPACING);




				// === Build Report Files ===

				GUILayout.Label("Build Report Files", BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);

				// build report files save path
				GUILayout.BeginHorizontal();
					GUILayout.Label(Labels.SAVE_PATH_LABEL + BuildReportTool.Options.BuildReportSavePath);
					if (GUILayout.Button(OPEN_IN_FILE_BROWSER_OS_SPECIFIC_LABEL))
					{
						BuildReportTool.Util.OpenInFileBrowser( BuildReportTool.Options.BuildReportSavePath );
					}
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				// change name of build reports folder
				GUILayout.BeginHorizontal();
					GUILayout.Label(Labels.SAVE_FOLDER_NAME_LABEL);
					BuildReportTool.Options.BuildReportFolderName = GUILayout.TextField(BuildReportTool.Options.BuildReportFolderName, GUILayout.MinWidth(250));
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				// where to save build reports (my docs/home, or beside project)
				GUILayout.BeginHorizontal();
					GUILayout.Label(Labels.SAVE_PATH_TYPE_LABEL);
					BuildReportTool.Options.SaveType = GUILayout.SelectionGrid(BuildReportTool.Options.SaveType, _saveTypeLabels, _saveTypeLabels.Length);
					GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(BuildReportTool.Window.Settings.CATEGORY_VERTICAL_SPACING);


			GUILayout.EndVertical();
			GUILayout.Space(20); // extra right padding
		GUILayout.EndHorizontal();

		GUILayout.EndScrollView();

		if (BuildReportTool.Options.SaveType == BuildReportTool.Options.SAVE_TYPE_PERSONAL)
		{
			// changed to user's personal folder
			BuildReportTool.ReportGenerator.ChangeSavePathToUserPersonalFolder();
		}
		else if (BuildReportTool.Options.SaveType == BuildReportTool.Options.SAVE_TYPE_PROJECT)
		{
			// changed to project folder
			BuildReportTool.ReportGenerator.ChangeSavePathToProjectFolder();
		}
	}



	int GetAutoShowWindowTypeGuiIdxFromOptions()
	{
		if (BuildReportTool.Options.IsAutoShowWindowTypeSetToNever)
		{
			return 0;
		}
		if (BuildReportTool.Options.IsAutoShowWindowTypeSetToAlways)
		{
			return 1;
		}
		if (BuildReportTool.Options.IsAutoShowWindowTypeSetToNotInBatchMode)
		{
			return 2;
		}
		return 2;
	}

	void SetAutoShowWindowTypeFromGuiIdx(int selectedIdx)
	{
		switch (selectedIdx)
		{
			case 0: // never
				BuildReportTool.Options.SetAutoShowWindowTypeToNever();
				break;
			case 1: // always
				BuildReportTool.Options.SetAutoShowWindowTypeToAlways();
				break;
			case 2: // yes, but not during batchmode
				BuildReportTool.Options.SetAutoShowWindowTypeToNotInBatchMode();
				break;
		}
	}
}

}
