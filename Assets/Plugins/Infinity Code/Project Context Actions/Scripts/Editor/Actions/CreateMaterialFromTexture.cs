/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using UnityEditor;
using UnityEngine;

namespace InfinityCode.ProjectContextActions.Actions
{
    [InitializeOnLoad]
    public static class CreateMaterialFromTexture
    {
        static CreateMaterialFromTexture()
        {
            ItemDrawer.Register(ItemDrawers.CreateMaterialFromTexture, DrawButton, ToolOrder.CreateMaterialFromTexture);
        }

        private static void CreateMaterial(Object asset)
        {
            Selection.activeObject = asset;
            Material material = new Material(RenderPipelineHelper.GetDefaultShader());
            material.mainTexture = asset as Texture2D;
            ProjectWindowUtil.CreateAsset(material, asset.name + ".mat");
        }

        private static void DrawButton(ProjectItem item)
        {
            if (!item.hovered) return;
            Object asset = item.asset;
            if (asset == null) return;
            if (!(asset is Texture2D)) return;
        
            Rect r = item.rect;
            r.xMin = r.xMax - 18;
            r.height = 16;

            item.rect.xMax -= 18;

            Texture icon = EditorIconContents.Material.image;
            string tooltip = "Create Material From Texture";

            ButtonEvent be = GUILayoutUtils.Button(r, TempContent.Get(icon, tooltip), GUIStyle.none);
            if (be == ButtonEvent.Click)
            {
                Event e = Event.current;
                if (e.button == 0)
                {
                    CreateMaterial(asset);
                }
            }
        }
    }
}