/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;

namespace InfinityCode.ProjectContextActions.Actions
{
    [InitializeOnLoad]
    public static class CreateMaterial
    {
        static CreateMaterial()
        {
            ItemDrawer.Register(ItemDrawers.CreateMaterial, DrawButton, ToolOrder.CreateMaterial);
        }
        
        private static void Create(ProjectItem item)
        {
            Selection.activeObject = item.asset;
            Material material = new Material(RenderPipelineHelper.GetDefaultShader());
            ProjectWindowUtil.CreateAsset(material, "New Material.mat");
        }

        private static void DrawButton(ProjectItem item)
        {
            if (!item.isFolder) return;
            if (!item.hovered) return;
            if (!item.path.StartsWith("Assets")) return;
            if (!item.path.Contains("Material")) return;

            Rect r = item.rect;
            r.xMin = r.xMax - 18;
            r.height = 16;

            item.rect.xMax -= 18;

            GUIContent content = TempContent.Get(EditorIconContents.Material.image, "Create Material");
            if (GUI.Button(r, content, GUIStyle.none)) Create(item);
        }
    }
}