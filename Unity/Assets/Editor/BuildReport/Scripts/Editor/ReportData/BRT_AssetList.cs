using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildReportTool
{

// a collection of file entries in a build report
// used to display the "Used Assets" and the "Unused Assets"
[System.Serializable]
public class AssetList
{
	// ==================================================================================
	[SerializeField]
	BuildReportTool.SizePart[] _all;

	int[] _viewOffsets;

	[SerializeField]
	BuildReportTool.SizePart[][] _perCategory;


	[SerializeField]
	string[] _labels;


	public BuildReportTool.SizePart[] All { get{ return _all; } private set{ _all = value; PostSetListAll(); } }
	public BuildReportTool.SizePart[][] PerCategory { get{ return _perCategory; } }

	public string[] Labels { get{ return _labels; } set{ _labels = value; } }
	
	// ==================================================================================

	BuildReportTool.SizePart[] _topTenLargest;
	
	public BuildReportTool.SizePart[] TopTenLargest { get{ return _topTenLargest; } }
	
	void PostSetListAll()
	{
		List<BuildReportTool.SizePart> _topTenLargestList = new List<BuildReportTool.SizePart>();
		
		SortRawSize(_all, SortOrder.Descending);
		
		// in case entries in "all" list is lesser than 10
		int len = Mathf.Min(10, _all.Length);
		
		for (int n = 0; n < len; ++n)
		{
			_topTenLargestList.Add(_all[n]);
		}
		_topTenLargest = _topTenLargestList.ToArray();
	}

	public void ResortDefault()
	{
		PostSetListAll();
	}

	// ==================================================================================
	// Sort Type

	public enum SortType
	{
		None,
		AssetFullPath,
		AssetFilename,
		RawSize,
		ImportedSize,
		PercentSize
	}

	public enum SortOrder
	{
		None,
		Ascending,
		Descending
	}

	SortType _currentSortType = SortType.RawSize;
	SortOrder _currentSortOrder = SortOrder.Descending;

	public SortType CurrentSortType { get{ return _currentSortType; } }
	public SortOrder CurrentSortOrder { get{ return _currentSortOrder; } }

	public void ToggleSort(SortType newSortType)
	{
		if (_currentSortType != newSortType)
		{
			_currentSortType = newSortType;
			_currentSortOrder = SortOrder.Descending; // descending by default
		}
		else
		{
			// already in this sort type
			// now toggle the sort order
			if (_currentSortOrder == SortOrder.Descending)
			{
				_currentSortOrder = SortOrder.Ascending;
			}
			else
			{
				_currentSortOrder = SortOrder.Descending;
			}
		}

		SetSort(_currentSortType, _currentSortOrder);
	}

	public void SetSort(SortType sortType, SortOrder sortOrder)
	{
		if (sortType == SortType.RawSize)
		{
			SortRawSize(_all, sortOrder);
			for (int n = 0, len = _perCategory.Length; n < len; ++n)
			{
				SortRawSize(_perCategory[n], sortOrder);
			}
		}
		else if (sortType == SortType.ImportedSize)
		{
			SortImportedSize(_all, sortOrder);
			for (int n = 0, len = _perCategory.Length; n < len; ++n)
			{
				SortImportedSize(_perCategory[n], sortOrder);
			}
		}
		else if (sortType == SortType.PercentSize)
		{
			SortPercentSize(_all, sortOrder);
			for (int n = 0, len = _perCategory.Length; n < len; ++n)
			{
				SortPercentSize(_perCategory[n], sortOrder);
			}
		}
		else if (sortType == SortType.AssetFullPath)
		{
			SortAssetFullPath(_all, sortOrder);
			for (int n = 0, len = _perCategory.Length; n < len; ++n)
			{
				SortAssetFullPath(_perCategory[n], sortOrder);
			}
		}
		else if (sortType == SortType.AssetFilename)
		{
			SortAssetName(_all, sortOrder);
			for (int n = 0, len = _perCategory.Length; n < len; ++n)
			{
				SortAssetName(_perCategory[n], sortOrder);
			}
		}
	}


	static void SortRawSize(BuildReportTool.SizePart[] assetList, SortOrder sortOrder)
	{
		if (sortOrder == SortOrder.Descending)
		{
			Array.Sort(assetList, delegate(BuildReportTool.SizePart entry1, BuildReportTool.SizePart entry2) {
				if (entry1.UsableSize > entry2.UsableSize) return -1;
				if (entry1.UsableSize < entry2.UsableSize) return 1;
				return 0;
			});
		}
		else
		{
			Array.Sort(assetList, delegate(BuildReportTool.SizePart entry1, BuildReportTool.SizePart entry2) {
				if (entry1.UsableSize > entry2.UsableSize) return 1;
				if (entry1.UsableSize < entry2.UsableSize) return -1;
				return 0;
			});
		}
	}


	static void SortImportedSize(BuildReportTool.SizePart[] assetList, SortOrder sortOrder)
	{
		if (sortOrder == SortOrder.Descending)
		{
			Array.Sort(assetList, delegate(BuildReportTool.SizePart entry1, BuildReportTool.SizePart entry2) {
				if (entry1.ImportedSizeBytes > entry2.ImportedSizeBytes) return -1;
				if (entry1.ImportedSizeBytes < entry2.ImportedSizeBytes) return 1;
				return 0;
			});
		}
		else
		{
			Array.Sort(assetList, delegate(BuildReportTool.SizePart entry1, BuildReportTool.SizePart entry2) {
				if (entry1.ImportedSizeBytes > entry2.ImportedSizeBytes) return 1;
				if (entry1.ImportedSizeBytes < entry2.ImportedSizeBytes) return -1;
				return 0;
			});
		}
	}


	static void SortPercentSize(BuildReportTool.SizePart[] assetList, SortOrder sortOrder)
	{
		if (sortOrder == SortOrder.Descending)
		{
			Array.Sort(assetList, delegate(BuildReportTool.SizePart entry1, BuildReportTool.SizePart entry2) {
				if (entry1.Percentage > entry2.Percentage) return -1;
				if (entry1.Percentage < entry2.Percentage) return 1;
				return 0;
			});
		}
		else
		{
			Array.Sort(assetList, delegate(BuildReportTool.SizePart entry1, BuildReportTool.SizePart entry2) {
				if (entry1.Percentage > entry2.Percentage) return 1;
				if (entry1.Percentage < entry2.Percentage) return -1;
				return 0;
			});
		}
	}


	static void SortAssetFullPath(BuildReportTool.SizePart[] assetList, SortOrder sortOrder)
	{
		if (sortOrder == SortOrder.Descending)
		{
			Array.Sort(assetList, delegate(BuildReportTool.SizePart entry1, BuildReportTool.SizePart entry2) {
				int result = string.Compare(entry1.Name, entry2.Name, true);

				return result;
			});
		}
		else
		{
			Array.Sort(assetList, delegate(BuildReportTool.SizePart entry1, BuildReportTool.SizePart entry2) {
				int result = string.Compare(entry1.Name, entry2.Name, true);

				// invert the result
				if (result == 1) return -1;
				if (result == -1) return 1;
				return 0;
			});
		}
	}


	static void SortAssetName(BuildReportTool.SizePart[] assetList, SortOrder sortOrder)
	{
		if (sortOrder == SortOrder.Descending)
		{
			Array.Sort(assetList, delegate(BuildReportTool.SizePart entry1, BuildReportTool.SizePart entry2) {
				string entry1Name = BuildReportTool.Util.GetAssetFilename(entry1.Name);
				string entry2Name = BuildReportTool.Util.GetAssetFilename(entry2.Name);
				
				int result = string.Compare(entry1Name, entry2Name, true);

				return result;
			});
		}
		else
		{
			Array.Sort(assetList, delegate(BuildReportTool.SizePart entry1, BuildReportTool.SizePart entry2) {
				string entry1Name = BuildReportTool.Util.GetAssetFilename(entry1.Name);
				string entry2Name = BuildReportTool.Util.GetAssetFilename(entry2.Name);
				
				int result = string.Compare(entry1Name, entry2Name, true);

				// invert the result
				if (result == 1) return -1;
				if (result == -1) return 1;
				return 0;
			});
		}
	}

	// Queries
	// ==================================================================================

	public List<BuildReportTool.SizePart> GetAllAsList()
	{
		return _all.ToList();
	}

	public int AllCount
	{
		get
		{
			return _all.Length;
		}
	}

	public double GetTotalSizeInBytes()
	{
		double total = 0;

		for (int n = 0, len = _all.Length; n < len; ++n)
		{
			if (_all[n].UsableSize > 0)
			{
				total += _all[n].UsableSize;
			}
		}
		return total;
	}

	public int GetViewOffsetForDisplayedList(FileFilterGroup fileFilters)
	{
		if (fileFilters.SelectedFilterIdx == -1)
		{
			return _viewOffsets[0];
		}
		else if (PerCategory != null && PerCategory.Length >= fileFilters.SelectedFilterIdx+1)
		{
			return _viewOffsets[fileFilters.SelectedFilterIdx+1];
		}
		return 0;
	}

	public BuildReportTool.SizePart[] GetListToDisplay(FileFilterGroup fileFilters)
	{
		BuildReportTool.SizePart[] ret = null;
		if (fileFilters.SelectedFilterIdx == -1)
		{
			ret = All;
		}
		else if (PerCategory != null && PerCategory.Length >= fileFilters.SelectedFilterIdx+1)
		{
			ret = PerCategory[fileFilters.SelectedFilterIdx];
		}
		return ret;
	}




	// Commands
	// ==================================================================================

	public void UnescapeAssetNames()
	{
		for (int n = 0, len = _all.Length; n < len; ++n)
		{
			_all[n].Name = BuildReportTool.Util.MyHtmlDecode(_all[n].Name);
		}
	}

	public void SetViewOffsetForDisplayedList(FileFilterGroup fileFilters, int newVal)
	{
		if (fileFilters.SelectedFilterIdx == -1)
		{
			_viewOffsets[0] = newVal;
		}
		else if (PerCategory != null && PerCategory.Length >= fileFilters.SelectedFilterIdx+1)
		{
			_viewOffsets[fileFilters.SelectedFilterIdx+1] = newVal;
		}
	}



	public void PopulateRawSizes()
	{
		/*long importedSize = -1;
		for (int n = 0, len = _all.Length; n < len; ++n)
		{
			importedSize = BRT_LibCacheUtil.GetImportedFileSize(_all[n].Name);

			_all[n].SizeBytes = importedSize;
			_all[n].Size = BuildReportTool.Util.GetBytesReadable(importedSize);
		}*/
	}


	public void PopulateImportedSizes()
	{
		long importedSize = -1;
		for (int n = 0, len = _all.Length; n < len; ++n)
		{
			importedSize = BRT_LibCacheUtil.GetImportedFileSize(_all[n].Name);

			_all[n].ImportedSizeBytes = importedSize;
			_all[n].ImportedSize = BuildReportTool.Util.GetBytesReadable(importedSize);
		}
	}



	// Commands: Initialization
	// ==================================================================================

	public void Init(BuildReportTool.SizePart[] all, BuildReportTool.SizePart[][] perCategory, FileFilterGroup fileFilters)
	{
		All = all;
		_perCategory = perCategory;

		_viewOffsets = new int[1 + PerCategory.Length];

		if (_currentSortType == SortType.None)
		{
			ToggleSort(SortType.RawSize);
		}
		else
		{
			SetSort(_currentSortType, _currentSortOrder);
		}

		RefreshFilterLabels(fileFilters);
	}

	public void Init(BuildReportTool.SizePart[] all, BuildReportTool.SizePart[][] perCategory, FileFilterGroup fileFilters, SortType newSortType, SortOrder newSortOrder)
	{
		_currentSortType = newSortType;
		_currentSortOrder = newSortOrder;

		Init(all, perCategory, fileFilters);
	}

	public void Reinit(BuildReportTool.SizePart[] all, BuildReportTool.SizePart[][] perCategory)
	{
		All = all;
		_perCategory = perCategory;
	}

	public void AssignPerCategoryList(BuildReportTool.SizePart[][] perCategory)
	{
		_perCategory = perCategory;
		_viewOffsets = new int[1 + _perCategory.Length];
	}

	public void RefreshFilterLabels(FileFilterGroup fileFiltersToUse)
	{
		_labels = new string[1 + PerCategory.Length];
		_labels[0] = "All (" + All.Length + ")";
		for (int n = 0, len = fileFiltersToUse.Count; n < len; ++n)
		{
			_labels[n+1] = fileFiltersToUse[n].Label + " (" + PerCategory[n].Length + ")";
		}
		_labels[_labels.Length-1] = "Unknown (" + PerCategory[PerCategory.Length-1].Length + ")";
	}








	// Sum Selection
	// ==================================================================================

	[SerializeField]
	Dictionary <string, BuildReportTool.SizePart> _selectedForSum = new Dictionary<string, BuildReportTool.SizePart>();




	// Sum Selection: Queries
	// --------------------------------------------------------------------

	public bool InSumSelection(BuildReportTool.SizePart b)
	{
		return _selectedForSum.ContainsKey(b.Name);
	}

	double GetSizeOfSumSelection()
	{
		double total = 0;
		foreach (var pair in _selectedForSum)
		{
			if (pair.Value.UsableSize > 0)
			{
				total += pair.Value.UsableSize;
			}
		}
		return total;
	}

	public double GetPercentageOfSumSelection()
	{
		double total = 0;
		foreach (var pair in _selectedForSum)
		{
			if (pair.Value.Percentage > 0)
			{
				if (pair.Value.Percentage > 0)
				{
					total += pair.Value.Percentage;
				}
			}
		}
		return total;
	}

	public string GetReadableSizeOfSumSelection()
	{
		return BuildReportTool.Util.GetBytesReadable( GetSizeOfSumSelection() );
	}

	public bool AtLeastOneSelectedForSum
	{
		get
		{
			return _selectedForSum.Count > 0;
		}
	}

	public bool IsNothingSelected
	{
		get
		{
			return _selectedForSum.Count <= 0;
		}
	}

	public int GetSelectedCount()
	{
		return _selectedForSum.Count;
	}




	// Sum Selection: Commands
	// --------------------------------------------------------------------

	public void ToggleSumSelection(BuildReportTool.SizePart b)
	{
		if (InSumSelection(b))
		{
			RemoveFromSumSelection(b);
		}
		else
		{
			AddToSumSelection(b);
		}
	}

	public void RemoveFromSumSelection(BuildReportTool.SizePart b)
	{
		_selectedForSum.Remove(b.Name);
	}

	public void AddToSumSelection(BuildReportTool.SizePart b)
	{
		_selectedForSum.Add(b.Name, b);
	}

	public void AddDisplayedRangeToSumSelection(FileFilterGroup fileFilters, int offset, int range)
	{
		BuildReportTool.SizePart[] listForSelection = GetListToDisplay(fileFilters);

		for (int n = offset; n < offset+range; ++n)
		{
			if (!InSumSelection(listForSelection[n]))
			{
				AddToSumSelection(listForSelection[n]);
			}
		}
	}

	public void AddAllDisplayedToSumSelection(FileFilterGroup fileFilters)
	{
		BuildReportTool.SizePart[] listForSelection = GetListToDisplay(fileFilters);

		for (int n = 0; n < listForSelection.Length; ++n)
		{
			if (!InSumSelection(listForSelection[n]))
			{
				AddToSumSelection(listForSelection[n]);
			}
		}
	}

	public void ClearSelection()
	{
		_selectedForSum.Clear();
	}
}

} // namespace BuildReportTool
