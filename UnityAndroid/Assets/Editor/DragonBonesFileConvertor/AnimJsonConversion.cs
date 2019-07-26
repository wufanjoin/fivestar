


using System.Collections.Generic;
using JetBrains.Annotations;

public  class AnimJsonConversion
{
    //记录当前正在转换的DragonBonesJsonRoot
    public DragonBonesJsonRoot DragonCurrBonesRoot;
    //记录当前正在被转换的JoyLandlordsJsonRoot
    public JoyLandlordsJsonRoot DragonCurrLandlordsRoot;
    //转换根节点
    public  DragonBonesJsonRoot ConversionRoot(JoyLandlordsJsonRoot joyRoot)
    {
        DragonBonesJsonRoot braBonesJsonRoot=new DragonBonesJsonRoot();

        DragonCurrBonesRoot = braBonesJsonRoot;
        DragonCurrLandlordsRoot = joyRoot;

        braBonesJsonRoot.name = joyRoot.name;
        braBonesJsonRoot.version = joyRoot.version;
        braBonesJsonRoot.frameRate =int.Parse(joyRoot.frameRate);
        braBonesJsonRoot.armature=ConversionArmatureArray(joyRoot.armature);
        return braBonesJsonRoot;
    }

    //转换Armature数组节点
    private DragonBonesJsonArmature[] ConversionArmatureArray(JoyLandlordsJsonArmature[] joyLandlordsJsonArmatures)
    {
        DragonBonesJsonArmature[] bonesJsonArmatures =new DragonBonesJsonArmature[joyLandlordsJsonArmatures.Length];
        int i = 0;
        foreach (var joyLandlordsJsonArmature in joyLandlordsJsonArmatures)
        {
            bonesJsonArmatures[i++]=ConversionArmature(joyLandlordsJsonArmature);
        }
        return bonesJsonArmatures;
    }

    //转换Armature
    private DragonBonesJsonArmature ConversionArmature(JoyLandlordsJsonArmature joyLandlordsJsonArmature)
    {
        DragonBonesJsonArmature bonesJsonArmatures = new DragonBonesJsonArmature();
        bonesJsonArmatures.aabb=new DragonBonesJsonAabb();
        bonesJsonArmatures.aabb.x = -177.98;
        bonesJsonArmatures.aabb.y = -209.1;
        bonesJsonArmatures.aabb.width = 370.36;
        bonesJsonArmatures.aabb.height = 230.81;

        bonesJsonArmatures.defaultActions=new DragonBonesDefaultActions[1]{ new DragonBonesDefaultActions()};
        bonesJsonArmatures.defaultActions[0].gotoAndPlay = "idle";

        bonesJsonArmatures.name = joyLandlordsJsonArmature.name;
        bonesJsonArmatures.type = "Armature";
        bonesJsonArmatures.frameRate = DragonCurrBonesRoot.frameRate;
        //-------------------------转换Bone-----------------------------
        if (joyLandlordsJsonArmature.bone != null)
        {
            bonesJsonArmatures.bone=new DragonBonesJsonBone[1];
            bonesJsonArmatures.bone[0] = ConversionBone(joyLandlordsJsonArmature.bone);
        }
        else
        {
            bonesJsonArmatures.bone = new DragonBonesJsonBone[joyLandlordsJsonArmature.bones.Length];
            int i = 0;
            foreach (var bone in joyLandlordsJsonArmature.bones)
            {
                bonesJsonArmatures.bone[i++]=ConversionBone(bone);
            }
        }
        //-------------------------转换Skin-----------------------------
        bonesJsonArmatures.skin=new DragonBonesJsonSkin[1]{ ConversionSkin(joyLandlordsJsonArmature.skin)};

        //-------------------------转换Animation-----------------------------
        if (joyLandlordsJsonArmature.animation != null)
        {
            bonesJsonArmatures.animation = new DragonBonesJsonAnimation[1] { ConversionAnimation(joyLandlordsJsonArmature.animation) };
        }
        else
        {
            bonesJsonArmatures.animation = new DragonBonesJsonAnimation[joyLandlordsJsonArmature.animations.Length];
            int i = 0;
            foreach (var animation in joyLandlordsJsonArmature.animations)
            {
                bonesJsonArmatures.animation[i++] = ConversionAnimation(animation);
            }
        }
        //-------------------------获取Slot-----------------------------
        bonesJsonArmatures.slot = GetDragonBonesSlot();
        return bonesJsonArmatures;
    }


    //------------------------------转换整个Animation--------------------------------
    private DragonBonesJsonAnimation ConversionAnimation(JoyLandlordsJsonAnimation joyLandlordsJsonAnimation)
    {
        DragonBonesJsonAnimation dragonBonesJsonAnimation=new DragonBonesJsonAnimation();
        dragonBonesJsonAnimation.duration =int.Parse(joyLandlordsJsonAnimation.duration);
        dragonBonesJsonAnimation.fadeInTime = double.Parse(joyLandlordsJsonAnimation.fadeInTime);
        dragonBonesJsonAnimation.name = joyLandlordsJsonAnimation.name;
        if (joyLandlordsJsonAnimation.timeline != null)
        {
            dragonBonesJsonAnimation.bone =new DragonBonesJsonAnimationBone[]{ ConversionAnimationBone(joyLandlordsJsonAnimation.timeline) };
        }
        else if (joyLandlordsJsonAnimation.timelines != null)
        {
            dragonBonesJsonAnimation.bone=new DragonBonesJsonAnimationBone[joyLandlordsJsonAnimation.timelines.Length];
            int i = 0;
            foreach (var timeline in joyLandlordsJsonAnimation.timelines)
            {
                dragonBonesJsonAnimation.bone[i++] = ConversionAnimationBone(timeline);
            }
        }
        
        return dragonBonesJsonAnimation;
    }

    private DragonBonesJsonAnimationBone ConversionAnimationBone(JoyLandlordsJsonTimeline joyLandlordsJsonTimeline)
    {
        DragonBonesJsonAnimationBone brAnimationBone=new DragonBonesJsonAnimationBone();
        brAnimationBone.name = joyLandlordsJsonTimeline.name;
        if (joyLandlordsJsonTimeline.frame == null)
        {
            joyLandlordsJsonTimeline.frame = joyLandlordsJsonTimeline.frames[0];
            if (joyLandlordsJsonTimeline.frame.transform == null)
            {
                joyLandlordsJsonTimeline.frame.transform=new JoyLandlordsJsonTransform();
            }
        }
        DragonBonesJsonAnimationFrame dragbonesFrame = new DragonBonesJsonAnimationFrame();
        dragbonesFrame.x =double.Parse(joyLandlordsJsonTimeline.frame.transform.x);
        dragbonesFrame.duration =int.Parse(joyLandlordsJsonTimeline.frame.duration);
        dragbonesFrame.y = double.Parse(joyLandlordsJsonTimeline.frame.transform.y);
        brAnimationBone.rotateFrame=new DragonBonesJsonAnimationFrame[]{ dragbonesFrame };
        brAnimationBone.scaleFrame = new DragonBonesJsonAnimationFrame[] { dragbonesFrame };
        brAnimationBone.translateFrame = new DragonBonesJsonAnimationFrame[] { dragbonesFrame };
        return brAnimationBone;
    }


    //------------------------------转换整个Skin--------------------------------
    private DragonBonesJsonSkin ConversionSkin(JoyLandlordsJsonSkin joyLandlordsJsonSkin)
    {
        DragonBonesJsonSkin dragonBonesJsonSkin=new DragonBonesJsonSkin();
        if (joyLandlordsJsonSkin.slot != null)
        {
            dragonBonesJsonSkin.slot=new DragonBonesJsonSkinSlot[1]{ ConversionSkinSlot(joyLandlordsJsonSkin.slot) }; 
        }
        else
        {
            dragonBonesJsonSkin.slot=new DragonBonesJsonSkinSlot[joyLandlordsJsonSkin.slots.Length];
            int i = 0;
            foreach (var slot in joyLandlordsJsonSkin.slots)
            {
                dragonBonesJsonSkin.slot[i++] = ConversionSkinSlot(slot);
            }
        }
        return dragonBonesJsonSkin;
    }

    private DragonBonesJsonSkinSlot ConversionSkinSlot(JoyLandlordsJsonSlot joyLandlordsJsonSlot)
    {
        DragonBonesJsonSkinSlot dragonBonesJsonSkinSlot = new DragonBonesJsonSkinSlot();
        dragonBonesJsonSkinSlot.name = joyLandlordsJsonSlot.name;
        if (!string.IsNullOrEmpty(joyLandlordsJsonSlot.parent))
        {
            if (!parentRelations.ContainsKey(joyLandlordsJsonSlot.name))
            {
                parentRelations.Add(joyLandlordsJsonSlot.name, joyLandlordsJsonSlot.parent);
            }
        }

        if (joyLandlordsJsonSlot.display != null)
        {
            dragonBonesJsonSkinSlot.display = new DragonBonesJsonDisplay[1] { ConversionDisplay(joyLandlordsJsonSlot.display) };
        }
        else
        {
            dragonBonesJsonSkinSlot.display = new DragonBonesJsonDisplay[joyLandlordsJsonSlot.displays.Length];
            int i = 0;
            foreach (var slot in joyLandlordsJsonSlot.displays)
            {
                dragonBonesJsonSkinSlot.display[i++] = ConversionDisplay(slot);
            }
        }

        return dragonBonesJsonSkinSlot;
    }

    private DragonBonesJsonDisplay ConversionDisplay(JoyLandlordsJsonDisplay joyLandlordsJsonDisplay)
    {
        DragonBonesJsonDisplay dragonBonesJsonDisplay=new DragonBonesJsonDisplay();
        dragonBonesJsonDisplay.name = joyLandlordsJsonDisplay.name;
        dragonBonesJsonDisplay.transform = ConversionTransform(joyLandlordsJsonDisplay.transform);
        return dragonBonesJsonDisplay;
    }
    //记录图片父子关系的容器
    private Dictionary<string,string> parentRelations=new Dictionary<string, string>();

    //------------------------------转换整个Bone--------------------------------
    //转换Bone
    private DragonBonesJsonBone ConversionBone(JoyLandlordsJsonBone joyLandlordsJsonBone)
    {
        DragonBonesJsonBone brBonesJsonBone=new DragonBonesJsonBone();
        brBonesJsonBone.name = joyLandlordsJsonBone.name;
        if (!string.IsNullOrEmpty(joyLandlordsJsonBone.parent))
        {
            if (!parentRelations.ContainsKey(joyLandlordsJsonBone.name))
            {
                parentRelations.Add(joyLandlordsJsonBone.name, joyLandlordsJsonBone.parent);
            }
        }
        brBonesJsonBone.transform = ConversionTransform(joyLandlordsJsonBone.transform);
        return brBonesJsonBone;
    }
    //转换Transform
    private DragonBonesJsonTransform ConversionTransform(JoyLandlordsJsonTransform joyLandlordsJsonTransform)
    {
        DragonBonesJsonTransform dragonBonesJsonTransform=new DragonBonesJsonTransform();
        dragonBonesJsonTransform.x = double.Parse(joyLandlordsJsonTransform.x);
        dragonBonesJsonTransform.y = double.Parse(joyLandlordsJsonTransform.y);
        dragonBonesJsonTransform.skX = double.Parse(joyLandlordsJsonTransform.skX);
        dragonBonesJsonTransform.skY = double.Parse(joyLandlordsJsonTransform.skY);
        dragonBonesJsonTransform.scX = double.Parse(joyLandlordsJsonTransform.scX);
        dragonBonesJsonTransform.scY = double.Parse(joyLandlordsJsonTransform.scY);
        return dragonBonesJsonTransform;
    }
    //---------------------------------------------------------------------------------------

    //-------------------------获取Slot------------------------------------------------------
    private DragonBonesJsonSlot[] GetDragonBonesSlot()
    {
        DragonBonesJsonSlot[] dragonBonesJsonSlots = new DragonBonesJsonSlot[parentRelations.Count];
        int i = 0;
        foreach (var parentRelation in parentRelations)
        {
            DragonBonesJsonSlot slot = new DragonBonesJsonSlot();
            slot.name = parentRelation.Key;
            slot.parent = parentRelation.Value;
            dragonBonesJsonSlots[i++] = slot;
        }
        return dragonBonesJsonSlots;
    }

}

