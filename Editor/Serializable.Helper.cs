using UnityEngine;

namespace AriaPlugin.Editor.Serializable
{
    public static class Utillity
    {
        #if UNITY_EDITOR

        public static Rect AddSingleLine(this Rect self)
        {
            return new Rect(self.x, self.y + UnityEditor.EditorGUIUtility.singleLineHeight, self.width, self.height);
        }

        public static Rect VerticalLayout(this Rect self, float offsetPercent, float widthPercent)
        {
            return new Rect(self.x + (self.width * offsetPercent * 0.01f), self.y, self.width * widthPercent * 0.01f, UnityEditor.EditorGUIUtility.singleLineHeight);
        } 

        #endif // UNITY_EDITOR END.
    }
}
