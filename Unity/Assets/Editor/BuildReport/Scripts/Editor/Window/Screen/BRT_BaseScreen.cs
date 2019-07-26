using UnityEngine;



namespace BuildReportTool.Window.Screen
{

public abstract class BaseScreen
{
	public abstract string Name { get; }

	public abstract void RefreshData(BuildInfo buildReport);

	public abstract void DrawGUI(Rect position, BuildInfo buildReportToDisplay);
}

}
