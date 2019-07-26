using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETEditor
{
    public static class GetSelectionGoType
    {
        public static string GetFileType(string path)
        {
            if (path.Contains(ToyGameType.Common))
            {
                return ToyGameType.Common;
            }
            else if (path.Contains(ToyGameType.JoyLandlords))
            {
                return ToyGameType.JoyLandlords;
            }
            else if (path.Contains(ToyGameType.CardFiveStar))
            {
                return ToyGameType.CardFiveStar;
            }
            else
            {
                return ToyGameType.Common;
            }
        }
    }
}
