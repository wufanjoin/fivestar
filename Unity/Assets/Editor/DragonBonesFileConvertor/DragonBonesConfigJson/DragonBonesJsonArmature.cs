using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DragonBonesJsonArmature
{
    public string type;//
    public int frameRate;//
    public string name;//
    public DragonBonesJsonAabb aabb;//
    public DragonBonesJsonBone[] bone;
    public DragonBonesJsonSlot[] slot;
    public DragonBonesJsonSkin[] skin;
    public DragonBonesJsonAnimation[] animation;
    public DragonBonesDefaultActions[] defaultActions;//
}

