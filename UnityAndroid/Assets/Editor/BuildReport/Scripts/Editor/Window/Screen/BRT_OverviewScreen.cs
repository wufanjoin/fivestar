using UnityEngine;
using UnityEditor;
using System.IO;



namespace BuildReportTool.Window.Screen
{

public class Overview : BaseScreen
{
	Vector2 _scrollPos = Vector2.zero;

	public override string Name { get{ return Labels.OVERVIEW_CATEGORY_LABEL; } }

	public override void RefreshData(BuildInfo buildReport)
	{
	}

	public override void DrawGUI(Rect position, BuildInfo buildReportToDisplay)
	{
		GUILayout.Space(2); // top padding for scrollbar

		_scrollPos = GUILayout.BeginScrollView(_scrollPos);

		GUILayout.BeginHorizontal();
			GUILayout.Space(10); // extra left padding


			GUILayout.BeginVertical();

			GUILayout.Space(10); // top padding


			// report title
			GUILayout.Label(buildReportToDisplay.SuitableTitle, BuildReportTool.Window.Settings.MAIN_TITLE_STYLE_NAME);





			GUILayout.Space(10);


			// two-column layout
			GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();

					GUILayout.BeginVertical(GUILayout.MaxWidth(350));
						GUILayout.Label(Labels.TIME_OF_BUILD_LABEL, BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);
						GUILayout.Label(buildReportToDisplay.GetTimeReadable(), BuildReportTool.Window.Settings.INFO_SUBTITLE_STYLE_NAME);

						GUILayout.Label("Report generation took:", BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);
						GUILayout.Label(buildReportToDisplay.ReportGenerationTime.ToString(), BuildReportTool.Window.Settings.INFO_SUBTITLE_STYLE_NAME);
						
						if (!string.IsNullOrEmpty(buildReportToDisplay.TotalBuildSize) && !string.IsNullOrEmpty(buildReportToDisplay.BuildFilePath))
						{
							GUILayout.BeginVertical();
							GUILayout.Label(Labels.BUILD_TOTAL_SIZE_LABEL, BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);

							GUILayout.Label(BuildReportTool.Util.GetBuildSizePathDescription(buildReportToDisplay),
								BuildReportTool.Window.Settings.TINY_HELP_STYLE_NAME);

							GUILayout.Label(buildReportToDisplay.TotalBuildSize, BuildReportTool.Window.Settings.BIG_NUMBER_STYLE_NAME);
							GUILayout.EndVertical();
						}

						GUILayout.Space(20);

						string emphasisColor = "black";
						if (EditorGUIUtility.isProSkin)
						{
							emphasisColor = "white";
						}

						GUILayout.Label("<color=" + emphasisColor + "><size=20><b>" + buildReportToDisplay.BuildSizes[1].Name + "</b></size></color> are the largest,\ntaking up <color=" + emphasisColor + "><size=20><b>" + buildReportToDisplay.BuildSizes[1].Percentage + "%</b></size></color> of the build" + (buildReportToDisplay.HasStreamingAssets ? "\n<size=12>(not counting streaming assets)</size>" : ""), BuildReportTool.Window.Settings.INFO_TEXT_STYLE_NAME);
						GUILayout.Space(20);
					GUILayout.EndVertical();

					GUILayout.BeginVertical(GUILayout.MaxWidth(350));
						GUILayout.Label("Made for:", BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);
						GUILayout.Label(buildReportToDisplay.BuildType, BuildReportTool.Window.Settings.INFO_SUBTITLE_STYLE_NAME);

						GUILayout.Label("Built in:", BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);
						GUILayout.Label(buildReportToDisplay.UnityVersionDisplayed, BuildReportTool.Window.Settings.INFO_SUBTITLE_STYLE_NAME);
					GUILayout.EndVertical();

				GUILayout.EndHorizontal();


				GUILayout.BeginHorizontal();

					if (buildReportToDisplay.HasUsedAssets)
					{
						GUILayout.BeginVertical();
							GUILayout.Label("Top ten largest in build:", BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);
							DrawAssetList(buildReportToDisplay.UsedAssets);
						GUILayout.EndVertical();
					}
					if (buildReportToDisplay.HasUsedAssets && buildReportToDisplay.HasUnusedAssets)
					{
						GUILayout.Space(50);
					}
					if (buildReportToDisplay.HasUnusedAssets)
					{
						GUILayout.BeginVertical();
							GUILayout.Label("Top ten largest not in build:", BuildReportTool.Window.Settings.INFO_TITLE_STYLE_NAME);
							DrawAssetList(buildReportToDisplay.UnusedAssets);
						GUILayout.EndVertical();
					}
				GUILayout.EndHorizontal();

			GUILayout.EndVertical();


			GUILayout.Space(20);




			GUILayout.EndVertical();

			GUILayout.Space(20); // extra right padding
		GUILayout.EndHorizontal();

		GUILayout.EndScrollView();
	}

	void DrawAssetList(BuildReportTool.AssetList assetList)
	{
		BuildReportTool.SizePart[] assetsToShow = assetList.TopTenLargest;
		
		if (assetsToShow == null)
		{
			//LogMgr.Ins.LogError("no top ten largest");
			return;
		}

		bool useAlt = true;

		GUILayout.BeginHorizontal();

			// 1st column: name
			GUILayout.BeginVertical();
			for (int n = 0; n < assetsToShow.Length; ++n)
			{
				BuildReportTool.SizePart b = assetsToShow[n];

				string styleToUse = useAlt ? BuildReportTool.Window.Settings.LIST_NORMAL_ALT_STYLE_NAME : BuildReportTool.Window.Settings.LIST_NORMAL_STYLE_NAME;

				string prettyName = " " + (n+1) + ". " + Path.GetFileName(b.Name);
				Texture icon = AssetDatabase.GetCachedIcon(b.Name);
				if (GUILayout.Button(new GUIContent(prettyName, icon), styleToUse, GUILayout.MinWidth(100), GUILayout.MaxWidth(400), GUILayout.Height(30)))
				{
					Utility.PingAssetInProject(b.Name);
				}

				useAlt = !useAlt;
			}
			GUILayout.EndVertical();

			// 2nd column: size
			useAlt = true;
			GUILayout.BeginVertical();
			for (int n = 0; n < assetsToShow.Length; ++n)
			{
				BuildReportTool.SizePart b = assetsToShow[n];

				string styleToUse = useAlt ? BuildReportTool.Window.Settings.LIST_NORMAL_ALT_STYLE_NAME : BuildReportTool.Window.Settings.LIST_NORMAL_STYLE_NAME;

				GUILayout.Label(b.RawSize, styleToUse, GUILayout.MaxWidth(100), GUILayout.Height(30));

				useAlt = !useAlt;
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

	}
}

}
