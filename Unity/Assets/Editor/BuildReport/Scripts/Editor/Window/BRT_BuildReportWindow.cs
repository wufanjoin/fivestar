//#define BRT_SHOW_MINOR_WARNINGS

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Threading;

using BuildReportTool.Window;

public class BRT_BuildReportWindow : EditorWindow
{

	void OnDisable()
	{
		ForceStopFileLoadThread();
		IsOpen = false;
	}

	void OnFocus()
	{
		usedAssetsScreen.RefreshData(_buildInfo);
		unusedAssetsScreen.RefreshData(_buildInfo);

		// check if configured file filters changed and only then do we need to recategorize

		if (BuildReportTool.Options.ShouldUseConfiguredFileFilters())
		{
			RecategorizeDisplayedBuildInfo();
		}
	}

	void OnEnable()
	{
		//LogMgr.Ins.LogInfo("BuildReportWindow.OnEnable() " + System.DateTime.Now);


		IsOpen = true;

		InitGUISkin();


		if (BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			//LogMgr.Ins.LogInfo("recompiled " + _buildInfo.SavedPath);
			if (!string.IsNullOrEmpty(_buildInfo.SavedPath))
			{
				BuildReportTool.BuildInfo loadedBuild = BuildReportTool.Util.OpenSerializedBuildInfo(_buildInfo.SavedPath);
				if (BuildReportTool.Util.BuildInfoHasContents(loadedBuild))
				{
					_buildInfo = loadedBuild;
				}
			}
			else
			{
				if (_buildInfo.HasUsedAssets)
				{
					_buildInfo.UsedAssets.AssignPerCategoryList( BuildReportTool.ReportGenerator.SegregateAssetSizesPerCategory(_buildInfo.UsedAssets.All, _buildInfo.FileFilters) );
				}
				if (_buildInfo.HasUnusedAssets)
				{
					_buildInfo.UnusedAssets.AssignPerCategoryList( BuildReportTool.ReportGenerator.SegregateAssetSizesPerCategory(_buildInfo.UnusedAssets.All, _buildInfo.FileFilters) );
				}
			}
		}

		// lol wtf have I done
		usedAssetsScreen.SetListToDisplay(BuildReportTool.Window.Screen.AssetList.ListToDisplay.UsedAssets);
		unusedAssetsScreen.SetListToDisplay(BuildReportTool.Window.Screen.AssetList.ListToDisplay.UnusedAssets);

		overviewScreen.RefreshData(_buildInfo);
		buildSettingsScreen.RefreshData(_buildInfo);
		sizeStatsScreen.RefreshData(_buildInfo);
		usedAssetsScreen.RefreshData(_buildInfo);
		unusedAssetsScreen.RefreshData(_buildInfo);

		optionsScreen.RefreshData(_buildInfo);
		helpScreen.RefreshData(_buildInfo);
	}

	void Update()
	{
		if (_buildInfo != null && BuildReportTool.ReportGenerator.IsFinishedGettingValuesFromThread)
		{
			OnFinishGeneratingBuildReport();
		}

		if (BuildReportTool.Util.ShouldGetBuildReportNow && !BuildReportTool.ReportGenerator.IsGettingValuesFromThread && !EditorApplication.isCompiling)
		{
			//LogMgr.Ins.LogInfo("BuildReportWindow getting build info right after the build... " + System.DateTime.Now);
			Refresh();
			GoToOverviewScreen();
		}

		if (_finishedOpeningFromThread)
		{
			OnFinishOpeningBuildReportFile();
		}

		if (_buildInfo != null)
		{
			if (_buildInfo.RequestedToRefresh)
			{
				Repaint();
				_buildInfo.FlagFinishedRefreshing();
			}
		}
	}

	// ==========================================================================================
	// sub-screens

	BuildReportTool.Window.Screen.Overview overviewScreen = new BuildReportTool.Window.Screen.Overview();
	BuildReportTool.Window.Screen.BuildSettings buildSettingsScreen = new BuildReportTool.Window.Screen.BuildSettings();
	BuildReportTool.Window.Screen.SizeStats sizeStatsScreen = new BuildReportTool.Window.Screen.SizeStats();
	BuildReportTool.Window.Screen.AssetList usedAssetsScreen = new BuildReportTool.Window.Screen.AssetList();
	BuildReportTool.Window.Screen.AssetList unusedAssetsScreen = new BuildReportTool.Window.Screen.AssetList();

	BuildReportTool.Window.Screen.Options optionsScreen = new BuildReportTool.Window.Screen.Options();
	BuildReportTool.Window.Screen.Help helpScreen = new BuildReportTool.Window.Screen.Help();


	// ==========================================================================================



	public static string GetValueMessage { set; get; }

	public static bool LoadingValuesFromThread { get{ return !string.IsNullOrEmpty(GetValueMessage); } }


	[SerializeField]
	static BuildReportTool.BuildInfo _buildInfo;


	GUISkin _usedSkin = null;


	public static bool IsOpen { get; set; }




	Texture2D _toolbarIconLog;
	Texture2D _toolbarIconOpen;
	Texture2D _toolbarIconSave;
	Texture2D _toolbarIconOptions;
	Texture2D _toolbarIconHelp;


	GUIContent _toolbarLabelLog;
	GUIContent _toolbarLabelOpen;
	GUIContent _toolbarLabelSave;
	GUIContent _toolbarLabelOptions;
	GUIContent _toolbarLabelHelp;



	void RecategorizeDisplayedBuildInfo()
	{
		if (BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			BuildReportTool.ReportGenerator.RecategorizeAssetList(_buildInfo);
		}
	}


	void InitGUISkin()
	{
		string guiSkinToUse = BuildReportTool.Window.Settings.DEFAULT_GUI_SKIN_FILENAME;
		if (EditorGUIUtility.isProSkin)
		{
			guiSkinToUse = BuildReportTool.Window.Settings.DARK_GUI_SKIN_FILENAME;
		}

		// try default path
		_usedSkin = AssetDatabase.LoadAssetAtPath(BuildReportTool.Options.BUILD_REPORT_TOOL_DEFAULT_PATH + "/GUI/" + guiSkinToUse, typeof(GUISkin)) as GUISkin;

		if (_usedSkin == null)
		{
#if BRT_SHOW_MINOR_WARNINGS
			LogMgr.Ins.LogInfoWarning(BuildReportTool.Options.BUILD_REPORT_PACKAGE_MOVED_MSG);
#endif

			string folderPath = BuildReportTool.Util.FindAssetFolder(Application.dataPath, BuildReportTool.Options.BUILD_REPORT_TOOL_DEFAULT_FOLDER_NAME);
			if (!string.IsNullOrEmpty(folderPath))
			{
				folderPath = folderPath.Replace('\\', '/');
				int assetsIdx = folderPath.IndexOf("/Assets/");
				if (assetsIdx != -1)
				{
					folderPath = folderPath.Substring(assetsIdx+8, folderPath.Length-assetsIdx-8);
				}
				//LogMgr.Ins.LogInfo(folderPath);

				_usedSkin = AssetDatabase.LoadAssetAtPath("Assets/" + folderPath + "/GUI/" + guiSkinToUse, typeof(GUISkin)) as GUISkin;
			}
			else
			{
			    Debug.LogError(BuildReportTool.Options.BUILD_REPORT_PACKAGE_MISSING_MSG);
			}
			//LogMgr.Ins.LogInfo("_usedSkin " + (_usedSkin != null));
		}

		if (_usedSkin != null)
		{
			if (!EditorGUIUtility.isProSkin)
			{
				GUISkin nativeSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);

				_usedSkin.verticalScrollbar = nativeSkin.verticalScrollbar;
				_usedSkin.verticalScrollbarThumb = nativeSkin.verticalScrollbarThumb;
				_usedSkin.verticalScrollbarUpButton = nativeSkin.verticalScrollbarUpButton;
				_usedSkin.verticalScrollbarDownButton = nativeSkin.verticalScrollbarDownButton;

				_usedSkin.horizontalScrollbar = nativeSkin.horizontalScrollbar;
				_usedSkin.horizontalScrollbarThumb = nativeSkin.horizontalScrollbarThumb;
				_usedSkin.horizontalScrollbarLeftButton = nativeSkin.horizontalScrollbarLeftButton;
				_usedSkin.horizontalScrollbarRightButton = nativeSkin.horizontalScrollbarRightButton;

				// change the toggle skin to use the Unity builtin look, but keep our settings
				GUIStyle toggleSaved = new GUIStyle(_usedSkin.toggle);

				// make our own copy of the native skin toggle so that editing it won't affect the rest of the editor GUI
				GUIStyle nativeToggleCopy = new GUIStyle(nativeSkin.toggle);

				_usedSkin.toggle = nativeToggleCopy;
				_usedSkin.toggle.border = toggleSaved.border;
				_usedSkin.toggle.margin = toggleSaved.margin;
				_usedSkin.toggle.padding = toggleSaved.padding;
				_usedSkin.toggle.overflow = toggleSaved.overflow;
				_usedSkin.toggle.contentOffset = toggleSaved.contentOffset;

				_usedSkin.box = nativeSkin.box;
				_usedSkin.label = nativeSkin.label;
				_usedSkin.textField = nativeSkin.textField;
				_usedSkin.button = nativeSkin.button;


				_usedSkin.label.wordWrap = true;
			}

			// ----------------------------------------------------

			_toolbarIconLog = _usedSkin.GetStyle("Icon-Toolbar-Log").normal.background;
			_toolbarIconOpen = _usedSkin.GetStyle("Icon-Toolbar-Open").normal.background;
			_toolbarIconSave = _usedSkin.GetStyle("Icon-Toolbar-Save").normal.background;
			_toolbarIconOptions = _usedSkin.GetStyle("Icon-Toolbar-Options").normal.background;
			_toolbarIconHelp = _usedSkin.GetStyle("Icon-Toolbar-Help").normal.background;

			_toolbarLabelLog = new GUIContent(Labels.REFRESH_LABEL, _toolbarIconLog);
			_toolbarLabelOpen = new GUIContent(Labels.OPEN_LABEL, _toolbarIconOpen);
			_toolbarLabelSave = new GUIContent(Labels.SAVE_LABEL, _toolbarIconSave);
			_toolbarLabelOptions = new GUIContent(Labels.OPTIONS_CATEGORY_LABEL, _toolbarIconOptions);
			_toolbarLabelHelp = new GUIContent(Labels.HELP_CATEGORY_LABEL, _toolbarIconHelp);
		}
	}






	public void Init(BuildReportTool.BuildInfo buildInfo)
	{
		_buildInfo = buildInfo;
		
		minSize = new Vector2(903, 378);
	}

	void Refresh()
	{
		BuildReportTool.ReportGenerator.RefreshData(ref _buildInfo);
	}

	bool IsWaitingForBuildCompletionToGenerateBuildReport
	{
		get
		{
			return BuildReportTool.Util.ShouldGetBuildReportNow && EditorApplication.isCompiling;
		}
	}

	void OnFinishOpeningBuildReportFile()
	{
		_finishedOpeningFromThread = false;

		if (BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			buildSettingsScreen.RefreshData(_buildInfo);
			usedAssetsScreen.RefreshData(_buildInfo);
			unusedAssetsScreen.RefreshData(_buildInfo);
			sizeStatsScreen.RefreshData(_buildInfo);


			_buildInfo.OnDeserialize();
			_buildInfo.SetSavedPath(_lastOpenedBuildInfoFilePath);
		}
		Repaint();
		GoToOverviewScreen();
	}

	void OnFinishGeneratingBuildReport()
	{
		BuildReportTool.ReportGenerator.OnFinishedGetValues(_buildInfo);
		_buildInfo.UnescapeAssetNames();
		GoToOverviewScreen();

		buildSettingsScreen.RefreshData(_buildInfo);
	}



	void GoToOverviewScreen()
	{
		_selectedCategoryIdx = OVERVIEW_IDX;
	}






	// ==========================================================================

	void DrawOverviewScreen()
	{
		overviewScreen.DrawGUI(position, _buildInfo);
	}

	void DrawBuildSettingsScreen()
	{
		buildSettingsScreen.DrawGUI(position, _buildInfo);
	}

	void DrawSizeStatsScreen()
	{
		sizeStatsScreen.DrawGUI(position, _buildInfo);
	}

	void DrawOptionsScreen()
	{
		optionsScreen.DrawGUI(position, _buildInfo);
	}

	void DrawHelpScreen()
	{
		helpScreen.DrawGUI(position, _buildInfo);
	}

	// ==========================================================================











	int _fileFilterGroupToUseOnOpeningOptionsWindow = 0;
	int _fileFilterGroupToUseOnClosingOptionsWindow = 0;






	int _selectedCategoryIdx = 0;

	bool IsInOverviewCategory
	{
		get
		{
			return _selectedCategoryIdx == OVERVIEW_IDX;
		}
	}

	bool IsInBuildSettingsCategory
	{
		get
		{
			return _selectedCategoryIdx == BUILD_SETTINGS_IDX;
		}
	}

	bool IsInSizeStatsCategory
	{
		get
		{
			return _selectedCategoryIdx == SIZE_STATS_IDX;
		}
	}

	bool IsInUsedAssetsCategory
	{
		get
		{
			return _selectedCategoryIdx == USED_ASSETS_IDX;
		}
	}

	bool IsInUnusedAssetsCategory
	{
		get
		{
			return _selectedCategoryIdx == UNUSED_ASSETS_IDX;
		}
	}

	bool IsInOptionsCategory
	{
		get
		{
			return _selectedCategoryIdx == OPTIONS_IDX;
		}
	}

	bool IsInHelpCategory
	{
		get
		{
			return _selectedCategoryIdx == HELP_IDX;
		}
	}


	const int OVERVIEW_IDX = 0;
	const int BUILD_SETTINGS_IDX = 1;
	const int SIZE_STATS_IDX = 2;
	const int USED_ASSETS_IDX = 3;
	const int UNUSED_ASSETS_IDX = 4;

	const int OPTIONS_IDX = 5;
	const int HELP_IDX = 6;













	bool _finishedOpeningFromThread = false;
	string _lastOpenedBuildInfoFilePath = "";

	void _OpenBuildInfo(string filepath)
	{
		if (string.IsNullOrEmpty(filepath))
		{
			return;
		}

		_finishedOpeningFromThread = false;
		GetValueMessage = "Opening...";
		BuildReportTool.BuildInfo loadedBuild = BuildReportTool.Util.OpenSerializedBuildInfo(filepath, false);


		if (BuildReportTool.Util.BuildInfoHasContents(loadedBuild))
		{
			_buildInfo = loadedBuild;
			_lastOpenedBuildInfoFilePath = filepath;
		}
		else
		{
		    Debug.LogError("Build Report Tool: Invalid data in build info file: " + filepath);
		}

		_finishedOpeningFromThread = true;

		GetValueMessage = "";
	}


	Thread _currentBuildReportFileLoadThread = null;

	bool IsCurrentlyOpeningAFile
	{
		get { return _currentBuildReportFileLoadThread != null && _currentBuildReportFileLoadThread.ThreadState != ThreadState.Running; }
	}

	void ForceStopFileLoadThread()
	{
		if (IsCurrentlyOpeningAFile)
		{
			try { _currentBuildReportFileLoadThread.Abort(); }
			catch (ThreadStateException) { }
		}
	}

	void OpenBuildInfoAsync(string filepath)
	{
		if (string.IsNullOrEmpty(filepath))
		{
			return;
		}

		_currentBuildReportFileLoadThread = new Thread(() => _OpenBuildInfo(filepath));
		_currentBuildReportFileLoadThread.Start();
	}































	void DrawCentralMessage(string msg)
	{
		float w = 300;
		float h = 100;
		float x = (position.width - w) * 0.5f;
		float y = (position.height - h) * 0.25f;

		GUI.Label(new Rect(x, y, w, h), msg);
	}






	void DrawTopRowButtons()
	{
		int toolbarX = 10;

		if (GUI.Button(new Rect(toolbarX, 5, 50, 40), _toolbarLabelLog, BuildReportTool.Window.Settings.TOOLBAR_LEFT_STYLE_NAME) && !LoadingValuesFromThread)
		{
			Refresh();
		}
		toolbarX += 50;
		if (GUI.Button(new Rect(toolbarX, 5, 40, 40), _toolbarLabelOpen, BuildReportTool.Window.Settings.TOOLBAR_MIDDLE_STYLE_NAME) && !LoadingValuesFromThread)
		{
			string filepath = EditorUtility.OpenFilePanel(
				Labels.OPEN_SERIALIZED_BUILD_INFO_TITLE,
				BuildReportTool.Options.BuildReportSavePath,
				"xml");

			OpenBuildInfoAsync(filepath);
		}
		toolbarX += 40;

		if (GUI.Button(new Rect(toolbarX, 5, 40, 40), _toolbarLabelSave, BuildReportTool.Window.Settings.TOOLBAR_RIGHT_STYLE_NAME) && BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			string filepath = EditorUtility.SaveFilePanel(
				Labels.SAVE_MSG,
				BuildReportTool.Options.BuildReportSavePath,
				_buildInfo.GetDefaultFilename(),
				"xml");

			if (!string.IsNullOrEmpty(filepath))
			{
				BuildReportTool.Util.SerializeBuildInfo(_buildInfo, filepath);
			}
		}
		toolbarX += 40;



		toolbarX += 20;

		//if (!BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			if (GUI.Button(new Rect(toolbarX, 5, 55, 40), _toolbarLabelOptions, BuildReportTool.Window.Settings.TOOLBAR_LEFT_STYLE_NAME))
			{
				_selectedCategoryIdx = OPTIONS_IDX;
			}
			toolbarX += 55;
			if (GUI.Button(new Rect(toolbarX, 5, 70, 40), _toolbarLabelHelp, BuildReportTool.Window.Settings.TOOLBAR_RIGHT_STYLE_NAME))
			{
				_selectedCategoryIdx = HELP_IDX;
			}
		}
	}


	void OnGUI()
	{
		//GUI.Label(new Rect(5, 100, 800, 20), "BuildReportTool.Util.ShouldReload: " + BuildReportTool.Util.ShouldReload + " EditorApplication.isCompiling: " + EditorApplication.isCompiling);
		if (_usedSkin == null)
		{
			GUI.Label(new Rect(20, 20, 500, 100), BuildReportTool.Options.BUILD_REPORT_PACKAGE_MISSING_MSG);
			return;
		}

		GUI.skin = _usedSkin;

		DrawTopRowButtons();


		GUI.Label(new Rect(0, 0, position.width, 20), BuildReportTool.Info.ReadableVersion, BuildReportTool.Window.Settings.VERSION_STYLE_NAME);


		// loading message
		if (LoadingValuesFromThread)
		{
			DrawCentralMessage(GetValueMessage);
			return;
		}
		// content to show when there is no build report on display
		else if (!BuildReportTool.Util.BuildInfoHasContents(_buildInfo))
		{
			if (IsInOptionsCategory)
			{
				GUILayout.Space(40);
				DrawOptionsScreen();
			}
			else if (IsInHelpCategory)
			{
				GUILayout.Space(40);
				DrawHelpScreen();
			}
			else if (IsWaitingForBuildCompletionToGenerateBuildReport)
			{
				DrawCentralMessage(Labels.WAITING_FOR_BUILD_TO_COMPLETE_MSG);
			}
			else
			{
				DrawCentralMessage(Labels.NO_BUILD_INFO_FOUND_MSG);
			}

			return;
		}



		GUILayout.Space(50); // top padding (top row buttons are 40 pixels)






		// category buttons

		int oldSelectedCategoryIdx = _selectedCategoryIdx;

		GUILayout.BeginHorizontal();
		if (GUILayout.Toggle(IsInOverviewCategory, "Overview", BuildReportTool.Window.Settings.TAB_LEFT_STYLE_NAME, GUILayout.ExpandWidth(true)))
		{
			_selectedCategoryIdx = OVERVIEW_IDX;
		}
		if (GUILayout.Toggle(IsInBuildSettingsCategory, "Project Settings", BuildReportTool.Window.Settings.TAB_MIDDLE_STYLE_NAME, GUILayout.ExpandWidth(true)))
		{
			_selectedCategoryIdx = BUILD_SETTINGS_IDX;
		}
		if (GUILayout.Toggle(IsInSizeStatsCategory, "Size Stats", BuildReportTool.Window.Settings.TAB_MIDDLE_STYLE_NAME, GUILayout.ExpandWidth(true)))
		{
			_selectedCategoryIdx = SIZE_STATS_IDX;
		}
		if (GUILayout.Toggle(IsInUsedAssetsCategory, "Used Assets", BuildReportTool.Window.Settings.TAB_MIDDLE_STYLE_NAME, GUILayout.ExpandWidth(true)))
		{
			_selectedCategoryIdx = USED_ASSETS_IDX;
		}
		if (GUILayout.Toggle(IsInUnusedAssetsCategory, "Unused Assets", BuildReportTool.Window.Settings.TAB_RIGHT_STYLE_NAME, GUILayout.ExpandWidth(true)))
		{
			_selectedCategoryIdx = UNUSED_ASSETS_IDX;
		}

		/*GUILayout.Space(20);

		if (GUILayout.Toggle(IsInOptionsCategory, _toolbarLabelOptions, BuildReportTool.Window.Settings.TAB_LEFT_STYLE_NAME, GUILayout.ExpandWidth(true)))
		{
			_selectedCategoryIdx = OPTIONS_IDX;
		}
		if (GUILayout.Toggle(IsInHelpCategory, _toolbarLabelHelp, BuildReportTool.Window.Settings.TAB_RIGHT_STYLE_NAME, GUILayout.ExpandWidth(true)))
		{
			_selectedCategoryIdx = HELP_IDX;
		}*/
		GUILayout.EndHorizontal();


		if (oldSelectedCategoryIdx != OPTIONS_IDX && _selectedCategoryIdx == OPTIONS_IDX)
		{
			// moving into the options screen
			_fileFilterGroupToUseOnOpeningOptionsWindow = BuildReportTool.Options.FilterToUseInt;
		}
		else if (oldSelectedCategoryIdx == OPTIONS_IDX && _selectedCategoryIdx != OPTIONS_IDX)
		{
			// moving away from the options screen
			_fileFilterGroupToUseOnClosingOptionsWindow = BuildReportTool.Options.FilterToUseInt;

			if (_fileFilterGroupToUseOnOpeningOptionsWindow != _fileFilterGroupToUseOnClosingOptionsWindow)
			{
				RecategorizeDisplayedBuildInfo();
			}
		}


		// main content
		GUILayout.BeginHorizontal();
			//GUILayout.Space(3); // left padding
			GUILayout.BeginVertical();

					if (IsInOverviewCategory)
					{
						DrawOverviewScreen();
					}
					else if (IsInBuildSettingsCategory)
					{
						DrawBuildSettingsScreen();
					}
					else if (IsInSizeStatsCategory)
					{
						DrawSizeStatsScreen();
					}
					else if (IsInUsedAssetsCategory)
					{
						usedAssetsScreen.DrawGUI(position, _buildInfo);
					}
					else if (IsInUnusedAssetsCategory)
					{
						unusedAssetsScreen.DrawGUI(position, _buildInfo);
					}
					else if (IsInOptionsCategory)
					{
						DrawOptionsScreen();
					}
					else if (IsInHelpCategory)
					{
						DrawHelpScreen();
					}

					GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			//GUILayout.Space(5); // right padding
		GUILayout.EndHorizontal();


		//GUILayout.Space(10); // bottom padding
	}
}

#endif
