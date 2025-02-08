/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;

namespace InfinityCode.ProjectContextActions.Actions
{
    [InitializeOnLoad]
    public static class CreateShader
    {
        static CreateShader()
        {
            ItemDrawer.Register("CREATE_SHADER", DrawButton, ToolOrder.CreateShader);
        }

        private static void DrawButton(ProjectItem item)
        {
            if (!item.isFolder) return;
            if (!item.hovered) return;
            if (!item.path.StartsWith("Assets")) return;
            if (!item.path.Contains("Shaders")) return;
            
            Rect r = item.rect;
            r.xMin = r.xMax - 18;
            r.height = 16;

            item.rect.xMax -= 18;

            ButtonEvent be = GUILayoutUtils.Button(r, TempContent.Get(EditorIconContents.Shader.image, "Create Shader"), GUIStyle.none);
            if (be == ButtonEvent.Click) ProcessClick(item);
        }

        private static void ProcessClick(ProjectItem item)
        {
            Event e = Event.current;
            if (e.button != 0) return;

            GenericMenu menu = new GenericMenu();
            
            foreach (string submenu in Unsupported.GetSubmenus("Assets"))
            {
                if (!submenu.StartsWith("Assets/Create/Shader") && !submenu.StartsWith("Assets/Create/Shader Graph")) continue;
                Object asset = item.asset;

                string name = submenu.Substring(14);
                menu.AddItem(new GUIContent(name), false, () =>
                {
                    Selection.activeObject = asset;
                    EditorApplication.ExecuteMenuItem(submenu);
                });
            }
            
            menu.ShowAsContext();
        }
    }
}