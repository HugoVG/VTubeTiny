﻿using ImGuiNET;
using VTTiny.Editor;

namespace VTTiny.Scenery
{
    public partial class Stage
    {
        /// <summary>
        /// Renders the editor GUI for this scene and all the actors within this scene.
        /// </summary>
        internal void RenderEditorGUI()
        {
            Dimensions = EditorGUI.DragVector2("Scene dimensions", Dimensions);
            if (ImGui.IsItemDeactivatedAfterEdit())
                ResizeStage(Dimensions);

            ClearColor = EditorGUI.ColorEdit("Clear color", ClearColor);
            RenderBoundingBoxes = EditorGUI.Checkbox("Render bounding boxes", RenderBoundingBoxes);

            TargetFPS = EditorGUI.DragInt("Target FPS", TargetFPS);
            if (ImGui.IsItemDeactivatedAfterEdit())
                SetTargetFPS(TargetFPS);

            ImGui.Text("Actors");

            foreach (var actor in _actors)
            {
                if (actor.RenderEditorGUI())
                    break;

                ImGui.Separator();
            }

            if (ImGui.Button("Add Actor"))
                CreateActor();
        }
    }
}
