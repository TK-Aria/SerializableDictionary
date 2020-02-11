#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace AriaPlugin.Editor.Serializable
{

    public abstract class SerializableDictionaryPropertyDrawer<Key, Value> : SerializableDictionaryPropertyDrawer<Key, Value, Key, Value> 
    { 
    }

    public abstract class SerializableDictionaryPropertyDrawer<Key, Value, EditorKey, EditorValue> : PropertyDrawer
    {

        #region Field

        private List<Key> removeKeyList = new List<Key>();
        private bool isFoldout = true;

        #endregion // Field End.

        #region Method
    

        protected abstract Key OnGUI_Key(ref Rect rect, EditorKey param);

        protected abstract Value OnGUI_Value(ref Rect rect, EditorValue param);

        protected virtual EditorKey CastEditorKey(object _key, object _value)
        {
            return (EditorKey)_key;
        }

        protected virtual EditorValue CastEditorValue(object _key, object _value)
        {
            return (EditorValue)_value; 
        }

        protected virtual bool IsRemoveKey(ref Rect rect, Key param) 
        {
            return false; 
        }

        protected virtual void AddParam(ref Dictionary<Key,Value> dictionary, Key _key, Value _value)
        {
            dictionary[_key] = _value; 
        }

        #region UnityCallback

        /// <summary>
        ///  プロパティ拡張部分の描画コールバック. 
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            var target = property.serializedObject.targetObject;
            var dictionary = fieldInfo.GetValue(target) as IDictionary;
            if (dictionary == null)
            {
                return;
            }

            // ヘッダー部分.
            var headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            isFoldout = EditorGUI.Foldout(headerRect, isFoldout, label, true);
            EditorGUI.LabelField(headerRect, label, new GUIContent() { text = "Count : " + dictionary.Count });
            if(!isFoldout)
            {
                return;
            }

            // Dictionaryの内容描画部分.
            var _dictionary = OnGUI_Dictionary(ref position, dictionary);

            OnGUI_Footer(ref position, _dictionary, dictionary);
            ApplyDictionary(property, dictionary, _dictionary);
        }

        /// <summary>
        ///  プロパティの描画領域サイズの取得処理.
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = base.GetPropertyHeight(property, label);

            var target = property.serializedObject.targetObject;
            var dictionary = fieldInfo.GetValue(target) as IDictionary;
            if (dictionary == null) return height;

            return isFoldout ? ((dictionary.Count + 1) * (EditorGUIUtility.singleLineHeight) + EditorGUIUtility.singleLineHeight): EditorGUIUtility.singleLineHeight;
        }

        #endregion // UnityCallback End.

        /// <summary>
        ///
        /// </summary>
        private Dictionary<Key,Value> OnGUI_Dictionary(ref Rect position, IDictionary dictionary)
        {

            var workDictionary = new Dictionary<Key,Value>();

            foreach (DictionaryEntry item in dictionary )
            {
                position = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height);

                // Keyの描画
                Key _key = OnGUI_Key(ref position, CastEditorKey(item.Key, item.Value));

                // Valueの描画
                Value _value = OnGUI_Value(ref position, CastEditorValue(item.Key, item.Value));

                // 削除するKeyの設定.
                if(IsRemoveKey(ref position, _key))
                {
                    removeKeyList.Add(_key);
                }

                // Key,Valueの適用.
                AddParam(ref workDictionary, _key, _value);
            }

            return workDictionary;
        }

        /// <summary>
        ///
        /// </summary>
        public void ApplyDictionary(SerializedProperty property, IDictionary serializableDictionary, Dictionary<Key, Value> dictionary)
        {

            foreach(var item in dictionary)
            {
                serializableDictionary[item.Key] = item.Value;
            }

            if(removeKeyList.Count > 0)
            {
                foreach(var key in removeKeyList)
                {
                    serializableDictionary.Remove(key);
                }
                removeKeyList.Clear();
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///  
        /// </summary>
        public virtual void OnGUI_Footer(ref Rect position, Dictionary<Key,Value> _dictionary, IDictionary dictionary)
        {
            position = position.AddSingleLine();
            if(GUI.Button(position.VerticalLayout(25,50), "Add"))
            {
                dictionary[default(Key)] = default(Value);
            }
        }

        #endregion // Method End.

    }

}

#endif