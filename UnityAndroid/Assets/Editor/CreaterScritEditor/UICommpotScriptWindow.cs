using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ETModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ETEditor
{
    enum ScrpitType
    {
        Component,
        Factory,
        UIType,
        Mediator,
        Item,
        View,
        AssetBundleName
    }
    enum CoompotType
    {
        
        Normal,
        Child,
        PopUp,
        Hint,
        Error,
    }
    public class UICommpotScriptWindow : EditorWindow
    {
        [MenuItem("Tools/生成UIComponent脚本")]
        public static void ShowWindow()
        {
            GetWindow(typeof(UICommpotScriptWindow));
        }
        //UILoginComponent
        private string className = "NewComponent";
        private string scriptText = "";
        private CoompotType coompotType = CoompotType.Normal;
        private void OnGUI()
        {
            EditorGUILayout.LabelField("类名根据预制体自动生成");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("生成UIComponent脚本"))
            {
                CreateScript(ScrpitType.Component);
            }
            this.coompotType = (CoompotType)EditorGUILayout.EnumPopup("CoompotType: ", this.coompotType);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("生成UIItem脚本"))
            {
                CreateScript(ScrpitType.Item);
            }
            if (GUILayout.Button("生成View脚本"))
            {
                CreateScript(ScrpitType.View);
            }
            if (GUILayout.Button("生成UIFactory脚本"))
            {
                CreateScript(ScrpitType.Factory);
            }
            if (GUILayout.Button("生成Mediator脚本"))
            {
                CreateScript(ScrpitType.Mediator);
            }
            if (GUILayout.Button("添加UIType"))
            {
                CreateScript(ScrpitType.UIType);
            }
            if (GUILayout.Button("标记AssetBundleName"))
            {
                SignAssetBundleName();
            }
            scriptText=EditorGUILayout.TextField("Script Text", scriptText);
        }
        //创建UI脚本
        private void CreateScript(ScrpitType type)
        {
            //获得选择的所有对象
            GameObject[] arr = Selection.gameObjects;
            //判断对象是否为空
            if (arr != null && arr.Length>0&&arr[0]!=null)
            {
                //只对选择的第一个对象进行处理
                GameObject go = arr[0];
                //按照命名规则是否为Panel
                if (go.name.Contains("Panel")|| go.name.Contains("Item") || go.name.Contains("View"))
                {
                    switch (type)
                    {
                        case ScrpitType.Component:
                            CreateUIComponentScript(go);
                            break;
                        case ScrpitType.Factory:
                            CreateUIFactoryScript(go);
                            break;  
                        case ScrpitType.UIType:
                            CreateUIType(go);
                            break;
                        case ScrpitType.Mediator:
                            CreateMediatorScript(go);
                            break;
                        case ScrpitType.Item:
                            CreateItemScript(go);
                            break;
                        case ScrpitType.View:
                            CreateViewScript(go);
                            break;
                    }
                   
                }
                else
                {
                    Log.Error("请选择一个Panle预制体");
                }
            }
            else
            {
                Log.Error("请选择一个Panle预制体");
            }
        }

        //创建View脚本
        private void CreateViewScript(GameObject go)
        {
            //获取变量存储工具
            ReferenceCollector referenceCollector = go.GetComponent<ReferenceCollector>();
            if (referenceCollector != null)
            {
                StringBuilder strVar = new StringBuilder();
                StringBuilder strProcess = new StringBuilder();
                Ergodic(referenceCollector.data, ref strVar, ref strProcess);
                StringBuilder strFile = new StringBuilder();

                strFile.AppendLine("using System;");
                strFile.AppendLine("using ETModel;");
                strFile.AppendLine("using UnityEngine;");
                strFile.AppendLine("using UnityEngine.UI;");

                strFile.AppendLine();
                strFile.AppendLine("namespace ETHotfix");
                strFile.AppendLine("{");


                strFile.AppendLine($"public class {go.name}:BaseView");
                strFile.AppendLine("{");
                strFile.AppendLine("\t#region 脚本工具生成的代码");
                strFile.Append(strVar);
                strFile.AppendLine("public override void Init(GameObject go)");
                strFile.AppendLine("{");
                strFile.AppendLine("base.Init(go);");
                strFile.Append(strProcess);
                strFile.AppendLine("\tInitPanel();");
                strFile.AppendLine("}");
                strFile.AppendLine("\t#endregion");
                strFile.AppendLine("\tpublic void InitPanel()");
                strFile.AppendLine("\t{");
                strFile.AppendLine("\t}");
                strFile.AppendLine("}");
                strFile.AppendLine("}");
                scriptText = strFile.ToString();
                Log.Debug($"成功生成{go.name}");
            }
            else
            {
                Log.Error("无法获取Panel上的ReferenceCollector组件");
            }
        }

        //创建Item脚本
        private void CreateItemScript(GameObject go)
        {
            //获取变量存储工具
            ReferenceCollector referenceCollector = go.GetComponent<ReferenceCollector>();
            if (referenceCollector != null)
            {
                StringBuilder strVar = new StringBuilder();
                StringBuilder strProcess = new StringBuilder();
                Ergodic(referenceCollector.data, ref strVar, ref strProcess);
                StringBuilder strFile = new StringBuilder();

                strFile.AppendLine("using System;");
                strFile.AppendLine("using ETModel;");
                strFile.AppendLine("using UnityEngine;");
                strFile.AppendLine("using UnityEngine.UI;");

                strFile.AppendLine();
                strFile.AppendLine("namespace ETHotfix");
                strFile.AppendLine("{");

                strFile.AppendLine($"[ObjectSystem]");
                strFile.AppendLine($"public class {go.name}AwakeSystem : AwakeSystem<{go.name}, GameObject, T>");
                strFile.AppendLine("{");
                strFile.AppendLine($" public override void Awake({go.name} self, GameObject go, T data)");
                strFile.AppendLine("{");
                strFile.AppendLine($"self.Awake(go, data, UIType.);");
                strFile.AppendLine("}");
                strFile.AppendLine("}");

                strFile.AppendLine($"public class {go.name}:BaseItem<T>");
                strFile.AppendLine("{");
                strFile.AppendLine("\t#region 脚本工具生成的代码");
                strFile.Append(strVar);
                strFile.AppendLine("public override void Awake(GameObject go, T data, string uiType)");
                strFile.AppendLine("{");
                strFile.AppendLine("base.Awake(go, data, uiType);");
                strFile.Append(strProcess);
                strFile.AppendLine("\tInitPanel();");
                strFile.AppendLine("}");
                strFile.AppendLine("\t#endregion");
                strFile.AppendLine("\tpublic void InitPanel()");
                strFile.AppendLine("\t{");
                strFile.AppendLine("\t}");
                strFile.AppendLine("}");
                strFile.AppendLine("}");
                scriptText = strFile.ToString();
                Log.Debug($"成功生成{go.name}Item");
            }
            else
            {
                Log.Error("无法获取Panel上的ReferenceCollector组件");
            }
        }

        private void SignAssetBundleName()
        {
            foreach (var go in Selection.gameObjects)
            {
                string path = AssetDatabase.GetAssetPath(go);
                string gameType=GetSelectionGoType.GetFileType(path);
                string prefabPath = AssetDatabase.GetAssetPath(go);
                AssetImporter importer = AssetImporter.GetAtPath(prefabPath);
                importer.assetBundleName = gameType+"/" + go.name + ".unity3d";
            }
        }
        //创建UIType
        private void CreateUIType(GameObject go)
        {
            FileStream fs = null;  
            string filePath = "Assets/Hotfix/Module/UI/UIType.cs";  
            //将待写的入数据从字符串转换为字节数组  
            Encoding encoder = Encoding.UTF8;  
            byte[] bytes = encoder.GetBytes($"\t\tpublic const string {go.name} = \"{go.name}\";\n"+"\t}\n}");  
            if (!File.Exists(filePath))
            {
                 Log.Error($"文件不存在{filePath}");
                 return;
            }
            try  
            {
                if (File.ReadAllText(filePath).Contains($"public const string {go.name} = \"{go.name}\";"))
                {
                    Log.Error($"UIType已经存在{go.name}");
                    return;
                }
                fs = File.OpenWrite(filePath);  
                //设定书写的开始位置为文件的末尾  
                fs.Position = fs.Length - 4;  
                //将待写入内容追加到文件末尾  
                fs.Write(bytes, 0, bytes.Length);  
                Log.Debug("添加完成UIType:"+go.name);
            }  
            catch (Exception ex)  
            {  
                Log.Error($"文件打开失败{ex.ToString()}");  
            }  
            fs.Close();  
        }
        

        //创建Panel的Factory脚本
        private void CreateUIFactoryScript(GameObject go)
        {
            StringBuilder strFile=new StringBuilder();
            strFile.AppendLine("using System;");
            strFile.AppendLine("using ETModel;");
            strFile.AppendLine("using UnityEngine;");
            
            strFile.AppendLine();
            strFile.AppendLine("namespace ETHotfix");
            strFile.AppendLine("{");
            strFile.AppendLine($"[UIFactory(UIType.{go.name})]");
            strFile.AppendLine($"public class {go.name}Factory : IUIFactory");
            strFile.AppendLine("{");
            strFile.AppendLine("public UI Create(Scene scene, string type, GameObject gameObject)");
            strFile.AppendLine("{");
            strFile.AppendLine("try");
            strFile.AppendLine("{");
            strFile.AppendLine("ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();");
            strFile.AppendLine("resourcesComponent.LoadBundle($\"{type}.unity3d\");");
            strFile.AppendLine("GameObject bundleGameObject = (GameObject)resourcesComponent.BundleNameGetAsset($\"{type}.unity3d\", $\"{type}\");");
            strFile.AppendLine("GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);");
            strFile.AppendLine("go.layer = LayerMask.NameToLayer(LayerNames.UI);");
            strFile.AppendLine("UI ui = ComponentFactory.Create<UI, GameObject>(go);");
            strFile.AppendLine($"ui.AddComponent<{go.name}Component>();");
            strFile.AppendLine("return ui;");
            strFile.AppendLine("}");
            
            strFile.AppendLine("catch (Exception e)");
            strFile.AppendLine("{");
            strFile.AppendLine("Log.Error(e);");
            strFile.AppendLine("return null;");
            strFile.AppendLine("}");
            
            strFile.AppendLine("}");
            
            strFile.AppendLine("public void Remove(string type)");
            strFile.AppendLine("{");
            strFile.AppendLine("ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($\"{type}.unity3d\");");
            strFile.AppendLine("}");
            
            strFile.AppendLine("}");
            strFile.AppendLine("}");
            scriptText = strFile.ToString();
            Log.Debug($"成功生成{go.name}Factory");
        }
        //创建Mediator脚本
        private void CreateMediatorScript(GameObject go)
        {
                StringBuilder strFile=new StringBuilder();
                
                strFile.AppendLine("using System;");
                strFile.AppendLine("using ETModel;");
                strFile.AppendLine("using UnityEngine;");
                strFile.AppendLine("using UnityEngine.UI;");
                
                strFile.AppendLine();
                strFile.AppendLine("namespace ETHotfix");
                strFile.AppendLine("{");
                

                strFile.AppendLine($"[UIMediator(UIType.{go.name})]");
                strFile.AppendLine($"public class {go.name}Mediator:UIMediator<{go.name}Component,{go.name}Mediator>");
                strFile.AppendLine("{");
                
                strFile.AppendLine("public override void Awake()");
                strFile.AppendLine("{");
                strFile.AppendLine("base.Awake();");
                strFile.AppendLine("}");
                
                strFile.AppendLine("public override void OnDestroy()");
                strFile.AppendLine("{");
                strFile.AppendLine("base.OnDestroy();");
                strFile.AppendLine("}");
 
                strFile.AppendLine("}");
                strFile.AppendLine("}");
                scriptText = strFile.ToString();
                Log.Debug($"成功生成{go.name}Mediator");
        }

        //创建Panel的Component脚本
        private void CreateUIComponentScript(GameObject go)
        {
            //获取变量存储工具
            ReferenceCollector referenceCollector = go.GetComponent<ReferenceCollector>();
            if (referenceCollector != null)
            {
                
                StringBuilder strVar = new StringBuilder();
                StringBuilder strProcess = new StringBuilder();
                Ergodic(referenceCollector.data,ref strVar,ref strProcess);
                StringBuilder strFile=new StringBuilder();
                
                strFile.AppendLine("using System;");
                strFile.AppendLine("using ETModel;");
                strFile.AppendLine("using UnityEngine;");
                strFile.AppendLine("using UnityEngine.UI;");
                
                strFile.AppendLine();
                strFile.AppendLine("namespace ETHotfix");
                strFile.AppendLine("{");
                

                strFile.AppendLine($"[UIComponent(UIType.{go.name})]");
                strFile.AppendLine($"public class {go.name}Component:{coompotType}UIView");
                strFile.AppendLine("{");
                strFile.AppendLine("\t#region 脚本工具生成的代码");
                strFile.Append(strVar);
                strFile.AppendLine("public override void Awake()");
                strFile.AppendLine("{");
                strFile.AppendLine("base.Awake();");
                strFile.AppendLine("\tReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();");
                strFile.Append(strProcess);
                strFile.AppendLine("\tInitPanel();");
                strFile.AppendLine("}");
                strFile.AppendLine("\t#endregion");
                strFile.AppendLine("\tpublic void InitPanel()");
                strFile.AppendLine("\t{");
                strFile.AppendLine("\t}");
                strFile.AppendLine("}");
                strFile.AppendLine("}");
                scriptText = strFile.ToString();
                Log.Debug($"成功生成{go.name}Component");
            }
            else
            {
                Log.Error("无法获取Panel上的ReferenceCollector组件");
            }
        }

        private void Ergodic(List<ReferenceCollectorData> datas, ref StringBuilder strVar, ref StringBuilder strProcess)
        {
            foreach (var data in datas)
            {
                string name = data.key;
                string type = "";
                if (name.EndsWith("Btn"))
                {
                    type = "Button";
                    if ((data.gameObject as GameObject).GetComponent<Button>() == null)
                    {
                        (data.gameObject as GameObject).AddComponent<Button>();
                    }
                }
                else if (name.EndsWith("Text"))
                {
                    type = "Text";
                }
                else if (name.EndsWith("Image"))
                {
                    type = "Image";
                }
                else if (name.EndsWith("Go"))
                {
                    type = "GameObject";
                }
                else if (name.EndsWith("Toggle"))
                {
                    type = "Toggle";
                }
                else if (name.EndsWith("InputField"))
                {
                    type = "InputField";
                }
                else if (name.EndsWith("Slider"))
                {
                    type = "Slider";
                }
                else
                {
                    Log.Error("生成脚本有不存在的类型"+ name);
                }
                strVar.AppendLine($"\tprivate {type} m{name};");
                if (type == "GameObject")
                {
                    strProcess.AppendLine($"\tm{name}=rc.Get<GameObject>(\"{name}\");");
                }
                else
                {
                    strProcess.AppendLine($"\tm{name}=rc.Get<GameObject>(\"{name}\").GetComponent<{type}>();");
                }
            }
        }
        
    }
}