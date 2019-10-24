#if UNITY_EDITOR
using UnityEngine;

namespace SerializableCollections.SerializableDictionaryPropertyDrawerHelper
{

    public abstract class SingleLineStyle<Key, Value> : SingleLineStyle<Key, Value, Key, Value>
    {
    }

    public abstract class SingleLineStyle<Key, Value, EditorKey, EditorValue> : SerializableDictionaryPropertyDrawer<Key, Value, EditorKey, EditorValue> 
    {

        protected abstract Key OnGUI_Key(Rect position, EditorKey param);
        protected abstract Value OnGUI_Value(Rect position, EditorValue param);

        protected override Key OnGUI_Key(ref Rect position, EditorKey param)
        {
            var _position = position.VerticalLayout(5,45);
            return OnGUI_Key(_position, param);
        }

        protected override Value OnGUI_Value(ref Rect position, EditorValue param)
        {
            var _position = position.VerticalLayout(50,50);
            var type = OnGUI_Value(_position, param);
            return type;
        }

        protected override bool IsRemoveKey(ref Rect position, Key param)
        {
            bool isToggle = true;
            isToggle = UnityEditor.EditorGUI.Toggle ( position.VerticalLayout(0,10), isToggle);
            return !isToggle;
        }
    }        

}


#endif // UNITY_EDITOR End.