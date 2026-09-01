using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class HierarchyCustom
{
    static HierarchyCustom()
    {
        EditorApplication.hierarchyWindowItemByEntityIdOnGUI -= OnHierarchyItemGUI;
        EditorApplication.hierarchyWindowItemByEntityIdOnGUI += OnHierarchyItemGUI;
    }

    private static void OnHierarchyItemGUI(EntityId entityId, Rect selectionRect)
    {
        var gameObject = EditorUtility.EntityIdToObject(entityId) as GameObject;

        if (gameObject == null)
        {
            return;
        }

        if (!IsSectionHeader(gameObject.name))
        {
            DrawTagBadge(gameObject.tag, selectionRect);
            DrawActiveToggle(gameObject, selectionRect);
            return;
        }

        var title = GetSectionTitle(gameObject.name);
        DrawSectionHeader(title, selectionRect);
    }

    private static bool IsSectionHeader(string objectName)
    {
        return !string.IsNullOrWhiteSpace(objectName)
            && objectName.StartsWith("=");
    }

    private static string GetSectionTitle(string objectName)
    {
        return objectName.TrimStart('=', ' ');
    }

    private static readonly Color HeaderColor =
    new Color(0.15f, 0.15f, 0.15f, 1.0f);

    private static GUIStyle headerStyle;

    private static GUIStyle HeaderStyle
    {
        get
        {
            if (headerStyle != null)
            {
                return headerStyle;
            }

            headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal =
            {
                textColor = Color.white
            }
            };

            return headerStyle;
        }
    }

    private static void DrawSectionHeader(string title, Rect selectionRect)
    {
        // HierarchyÇÃçsëSëÃÇï¢Ç§ãÈå`ÇçÏÇÈ
        var rowRect = selectionRect;
        rowRect.x = 0f;
        rowRect.width = EditorGUIUtility.currentViewWidth;

        // îwåiÇï`Ç≠
        EditorGUI.DrawRect(rowRect, HeaderColor);

        // å©èoÇµï∂éöÇï`Ç≠
        GUI.Label(rowRect, title, HeaderStyle);
    }


    private static GUIStyle tagStyle;

    private static GUIStyle TagStyle
    {
        get
        {
            if (tagStyle != null)
            {
                return tagStyle;
            }

            tagStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal =
            {
                textColor = Color.yellow
            }
            };

            return tagStyle;
        }
    }

    private static void DrawTagBadge(string tag, Rect selectionRect)
    {
        if (tag == "Untagged")
        {
            return;
        }

        var content = new GUIContent("Tag: " + tag);
        var badgeWidth = TagStyle.CalcSize(content).x + 12f;

        var badgeRect = new Rect(
            EditorGUIUtility.currentViewWidth - badgeWidth - 6f,
            selectionRect.y + 1f,
            badgeWidth,
            selectionRect.height - 2f);

        GUI.Label(badgeRect, content, TagStyle);
    }

    private static void DrawActiveToggle(GameObject gameObject, Rect selectionRect)
    {
        const float toggleSize = 16f;
        const float Padding = 25f;

        var toggleRect = new Rect(
            selectionRect.x - Padding,
            selectionRect.y + (selectionRect.height - toggleSize) * 0.5f,
            toggleSize,
            toggleSize);

        EditorGUI.BeginChangeCheck();

        var newActiveSelf = GUI.Toggle(
            toggleRect,
            gameObject.activeSelf,
            GUIContent.none);

        if (!EditorGUI.EndChangeCheck())
        {
            return;
        }

        Undo.RecordObject(
            gameObject,
            newActiveSelf ? "Enable GameObject" : "Disable GameObject");

        gameObject.SetActive(newActiveSelf);
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
    }
}