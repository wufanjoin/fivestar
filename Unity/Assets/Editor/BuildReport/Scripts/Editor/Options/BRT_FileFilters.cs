using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BuildReportTool
{

[System.Serializable]
public class FileFilters
{
	public FileFilters(string label, string[] filters)
	{
		_label = label;

		bool shouldBeAllLowerCase = false;
		for (int n = 0, len = filters.Length; n < len; ++n)
		{
			_filtersDict.Add(filters[n], false);
			shouldBeAllLowerCase = true;

			if ((filters[n].StartsWith("/") || filters[n].ToLower().StartsWith("assets/")) && filters[n].EndsWith("/"))
			{
				_usesFolderFilter = true;
			}
			else if (filters[n].StartsWith(BUILT_IN_ASSET_KEYWORD))
			{
				_usesFolderFilter = true;

				// note: filters for built-in types are case-sensitive
				shouldBeAllLowerCase = false;

				//LogMgr.Ins.LogInfo("uses built-in: " + label + ", " + filters[n]);
			}
			else if (filters[n].StartsWith("\"") && filters[n].EndsWith("\""))
			{
				_usesExactFileMatching = true;
			}

			if (shouldBeAllLowerCase)
			{
				filters[n] = filters[n].ToLower();
			}
		}

		_filtersList = filters;
	}

	public FileFilters()
	{
		_label = "";
	}
	
	const string BUILT_IN_ASSET_KEYWORD = "Built-in";
	

	[SerializeField]
	string _label;

	Dictionary<string, bool> _filtersDict = new Dictionary<string, bool>();

	[SerializeField]
	string[] _filtersList;

	[SerializeField]
	bool _usesFolderFilter = false;

	[SerializeField]
	bool _usesExactFileMatching = false;

	public string Label { get{ return _label; } set{ _label = value; } }
	public string[] FiltersList
	{
		get{ return _filtersList; }
		set
		{
			_filtersList = value;

			bool shouldBeAllLowerCase = false;
			for (int n = 0, len = _filtersList.Length; n < len; ++n)
			{
				_filtersDict.Add(_filtersList[n], false);
				shouldBeAllLowerCase = true;

				if ((_filtersList[n].StartsWith("/") || _filtersList[n].StartsWith("assets/")) && _filtersList[n].EndsWith("/"))
				{
					_usesFolderFilter = true;
				}
				else if (_filtersList[n].StartsWith(BUILT_IN_ASSET_KEYWORD))
				{
					_usesFolderFilter = true;
					shouldBeAllLowerCase = false;
					//LogMgr.Ins.LogInfo("uses built-in: " + _label + ", " + _filtersList[n]);
				}
				else if (_filtersList[n].StartsWith("\"") && _filtersList[n].EndsWith("\""))
				{
					_usesExactFileMatching = true;
				}

				if (shouldBeAllLowerCase)
				{
					_filtersList[n] = _filtersList[n].ToLower();
				}
			}
		}
	}




	public string GetFileExt(string file)
	{
		int lastDotIdx = file.LastIndexOf(".");
		if (lastDotIdx == -1) return "";
		return file.Substring(lastDotIdx, file.Length-lastDotIdx);
	}

	public bool IsFileInFilter(string file)
	{
		string fileAllLower = file.ToLower();


		// -------------------------------------------------
		// try using folder filter method:

		if (_usesFolderFilter)
		{
			//LogMgr.Ins.LogInfo(_label + " uses folder filter");
			for (int n = 0, len = _filtersList.Length; n < len; ++n)
			{
				if (_filtersList[n].StartsWith(BUILT_IN_ASSET_KEYWORD) && file.StartsWith(BUILT_IN_ASSET_KEYWORD) && file.IndexOf(_filtersList[n]) != -1) // built-in asset compare is case-sensitive
				{
					return true;
				}

				//LogMgr.Ins.LogInfo(file + " ---- " + _filtersList[n]);
				if (fileAllLower.IndexOf(_filtersList[n]) != -1)
				{
					return true;
				}
			}
		}

		// -------------------------------------------------
		// if not found using folder filter method, try exact file matching next:

		if (_usesExactFileMatching)
		{
			//LogMgr.Ins.LogInfo("_usesExactFileMatching");

			for (int n = 0, len = _filtersList.Length; n < len; ++n)
			{
				//LogMgr.Ins.LogInfo("in quotes: " + _filtersList[n] + " " + (_filtersList[n].StartsWith("\"") && _filtersList[n].EndsWith("\"")));


				if (_filtersList[n].StartsWith("\"") && _filtersList[n].EndsWith("\""))
				{
					string fileWithQuotes = "\"" + System.IO.Path.GetFileName(fileAllLower) + "\"";

					//LogMgr.Ins.LogInfo("match? " + _filtersList[n] + " == " + fileWithQuotes);

					if (_filtersList[n] == fileWithQuotes)
					{
						return true;
					}
				}
			}
		}

		// -------------------------------------------------
		// if not found using exact file matching, try checking in dictionary next:

		return _filtersDict.ContainsKey(GetFileExt(fileAllLower));
	}
}



[System.Serializable, XmlRoot("FileFilterGroup")]
public class FileFilterGroup
{
	[SerializeField]
	FileFilters[] _fileFilters;

	public FileFilters[] FileFilters
	{
		get{ return _fileFilters; }
		set
		{
			_fileFilters = value;
			InitNames();
		}
	}

	[SerializeField]
	string[] _names;

	public FileFilterGroup()
	{
		_fileFilters = null;
		_names = null;
	}

	public FileFilterGroup(FileFilters[] filters)
	{
		_fileFilters = filters;
		InitNames();
	}

	void InitNames()
	{
		_names = new string[_fileFilters.Length+2];

		_names[0] = "All";

		for (int n = 0, len = _fileFilters.Length; n < len; ++n)
		{
			_names[n+1] = _fileFilters[n].Label;
		}
		_names[_names.Length-1] = "Unknown";
	}

	int _selectedFilterIdx = 0;
	public int SelectedFilterIdx { get{ return _selectedFilterIdx-1; } }
	public int GetSelectedFilterIdx()
	{
		return _selectedFilterIdx;
	}
	public void ForceSetSelectedFilterIdx(int idx)
	{
		if ((idx < _fileFilters.Length+2) && idx >= 0)
		{
			_selectedFilterIdx = idx;
		}
	}

	const string UNPRESSED_STYLE_NAME = "ButtonNoContents";
	const string ALREADY_PRESSED_STYLE_NAME = "ButtonAlreadyPressed";

	const string HAS_CONTENTS_UNPRESSED_STYLE_NAME = "ButtonHasContents";
	const string HAS_CONTENTS_ALREADY_PRESSED_STYLE_NAME = "ButtonAlreadyPressed";

	string GetStyleToUse(int assetNum, int selectedIdx, int idxOfThisGroup)
	{
		string styleToUse;

		if (assetNum > 0)
		{
			styleToUse = HAS_CONTENTS_UNPRESSED_STYLE_NAME;
			if (selectedIdx == idxOfThisGroup)
			{
				styleToUse = HAS_CONTENTS_ALREADY_PRESSED_STYLE_NAME;
			}
		}
		else
		{
			styleToUse = UNPRESSED_STYLE_NAME;
			if (selectedIdx == idxOfThisGroup)
			{
				styleToUse = ALREADY_PRESSED_STYLE_NAME;
			}
		}
		return styleToUse;
	}

	public void Draw(AssetList assetList, float width)
	{
		BuildReportTool.Options.FileFilterDisplay displayType = BuildReportTool.Options.GetOptionFileFilterDisplay();
		switch (displayType)
		{
			case BuildReportTool.Options.FileFilterDisplay.DropDown:
				DrawFiltersAsDropDown(assetList, width);
				break;
			case BuildReportTool.Options.FileFilterDisplay.Buttons:
				DrawFiltersAsButtons(assetList, width);
				break;
		}
	}

	void DrawFiltersAsDropDown(AssetList assetList, float width)
	{
		GUILayout.BeginHorizontal();
			GUILayout.Space(3);
			GUILayout.Label("Filter: ", BuildReportTool.Window.Settings.TOP_BAR_LABEL_STYLE_NAME);

			_selectedFilterIdx = EditorGUILayout.Popup(_selectedFilterIdx, assetList.Labels,
				BuildReportTool.Window.Settings.FILE_FILTER_POPUP_STYLE_NAME);
		GUILayout.EndHorizontal();
	}

	void DrawFiltersAsButtons(AssetList assetList, float width)
	{
		string styleToUse;

		GUILayout.BeginHorizontal();

		float overallWidth = 0;
		float widthToAdd = 0;
		string label = "";



		styleToUse = GetStyleToUse(assetList.All.Length, _selectedFilterIdx, 0);
		label = "All (" + assetList.All.Length + ")";

		widthToAdd = GUI.skin.GetStyle(styleToUse).CalcSize(new GUIContent(label)).x;

		overallWidth += widthToAdd;

		if (GUILayout.Button(label, styleToUse))
		{
			_selectedFilterIdx = 0;
		}

		if (overallWidth >= width)
		{
			overallWidth = 0;
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
		}

		if (assetList.PerCategory != null && assetList.PerCategory.Length >= _fileFilters.Length)
		{
			for (int n = 0, len = _fileFilters.Length; n < len; ++n)
			{
				styleToUse = GetStyleToUse(assetList.PerCategory[n].Length, _selectedFilterIdx, n+1);
				label = _fileFilters[n].Label + " (" + assetList.PerCategory[n].Length + ")";

				widthToAdd = GUI.skin.GetStyle(styleToUse).CalcSize(new GUIContent(label)).x;

				if (overallWidth + widthToAdd >= width)
				{
					overallWidth = 0;
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
				}

				overallWidth += widthToAdd;

				if (GUILayout.Button(label, styleToUse))
				{
					_selectedFilterIdx = n+1;
				}


			}

			styleToUse = GetStyleToUse(assetList.PerCategory[assetList.PerCategory.Length-1].Length, _selectedFilterIdx, assetList.PerCategory.Length);

			label = "Unknown (" + assetList.PerCategory[assetList.PerCategory.Length-1].Length + ")";
			widthToAdd = GUI.skin.GetStyle(styleToUse).CalcSize(new GUIContent(label)).x;
			if (overallWidth + widthToAdd >= width)
			{
				overallWidth = 0;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}

			if (GUILayout.Button(label, styleToUse))
			{
				_selectedFilterIdx = assetList.PerCategory.Length;
			}
		}

		GUILayout.EndHorizontal();
	}

	public FileFilters this[int idx]
	{
		get{ return _fileFilters[idx]; }
	}

	public int Count { get{ return _fileFilters.Length; } }

	public override string ToString()
	{
		string ret = "(" + _names.Length + ") ";
		for (int n = 0, len = _names.Length; n < len; ++n)
		{
			ret += _names[n] + ", ";
		}
		return ret;
	}
}

} // namespace BuildReportTool
