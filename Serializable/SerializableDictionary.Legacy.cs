using System;
using System.Collections.Generic;
using UnityEngine;

namespace AriaPlugin.Serializable
{

    [Serializable]
    public class Dictionary<X, Y, Type> where Type : KeyAndValue<X, Y>, new()
    {

        #region Field

        [SerializeField] private List<Type> tree = new List<Type>();

        #endregion // Field End.

        #region Method

        /// <summary> 
        ///  .NetのSystemDictionaryに変換します. 
        /// </summary> 
        public Dictionary<X, Y> ToDictionary()
        {
            var newDictionary = new Dictionary<X,Y>();

            foreach(var node in tree)
            {
                newDictionary[node.key] = node.value;
            }
            return newDictionary;
        }

        /// <summary>
        ///  別のDictionaryを元に値をコピーします.
        /// </summary>
        public void Copy(Dictionary<X,Y> dictionary)
        {
            Clear();
            Override(dictionary);
        }

        /// <summary>
        ///  別のDictionaryを元に内容を上書きします.
        /// </summary>
        public void Override(Dictionary<X,Y> dictionary)
        {
            foreach(var node in dictionary)
            {
                Add(node.Key, node.Value);
            }
        }

        /// <summary>
        ///  新規Key,Valueを追加します.
        /// </summary>
        private void Add(X key,Y value)
        {
            var newNode = new Type();
            newNode.Insert(key, value);
            tree.Add(newNode);
        }

        /// <summary>
        ///  キーに対応した値を探します.
        /// </summary>
        private Y Search(X key)
        {
            Y searchValue = default(Y);
            foreach(var node in tree)
            {
                if(!node.key.Equals(key)){
                    continue;
                }
                searchValue = node.value;
                break;
            }
            return searchValue;
        }

        /// <summary>
        ///  重複しているキーの削除
        /// </summary>
        public void DeleteDuplicate()
        {
            var dictionary = ToDictionary();
            Override(dictionary);
        }

        /// <summary>
        ///  内部データの削除.
        /// </summary>
        public void Clear()
        {
            tree.Clear();
        }

        /// <summary>
        ///  通常のDictionaryの用に[]でアクセスさせる為のオペレーター
        /// </summary>
        public Y this[X key]
        {
            set { Add(key, value); }
            get { return Search(key); }
        }

        #endregion // Method End.
    }

    [Serializable]
    public class KeyAndValue<Key, Value> 
    {
        public void Insert(Key newKey, Value newValue)
        {
            key = newKey;
            value = newValue;
        }
        public void Insert(KeyValuePair<Key, Value> pair)
        {
            key = pair.Key;
            value = pair.Value;
        }

        public Key key;
        public Value value;
    }

}
