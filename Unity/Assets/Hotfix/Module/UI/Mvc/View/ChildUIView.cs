namespace ETHotfix
{
    public class ChildUIView:UIView
    {
        public string pParentUIType
        {
            private set;
            get;
        }
        public override string pCavasName
        {
            get
            {
                return CanvasType.ChildCanvas;
            }
        }


        public void SetParentUIType(string uiType)
        {
            pParentUIType = uiType;
        }
    }
}

