using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Sevens.Editor
{
    [CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
    public class SubclassSelectorDrawer : PropertyDrawer
    {
        private bool initialized = false;
        private Type[] inheritedTypes;
        private string[] typePopupNameArray;
        private string[] typeFullNameArray;
        private int currentTypeIndex;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference) return;
            if (!initialized)
            {
                Initialize(property);
                initialized = true;
            }

            GetCurrentTypeIndex(property.managedReferenceFullTypename);
            var selectedTypeIndex = EditorGUI.Popup(GetPopupPosition(position), currentTypeIndex, typePopupNameArray);
            UpdatePropertyToSelectedTypeIndex(property, selectedTypeIndex);
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }

        private void Initialize(SerializedProperty property)
        {
            SubclassSelectorAttribute utility = (SubclassSelectorAttribute)attribute;
            GetAllInheritedTypes(GetFieldType(property), utility.IsIncludeMono());
            GetInheritedTypeNameArrays();
        }

        private void GetCurrentTypeIndex(string typeFullName)
        {
            currentTypeIndex = Array.IndexOf(typeFullNameArray, typeFullName);
        }

        private void GetAllInheritedTypes(Type baseType, bool includeMono)
        {
            var monoType = typeof(MonoBehaviour);
            inheritedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) && p.IsClass && (!monoType.IsAssignableFrom(p) || includeMono))
                .Prepend(null)
                .ToArray();
        }

        private void GetInheritedTypeNameArrays()
        {
            typePopupNameArray = inheritedTypes.Select(type => type == null ? "<null>" : type.Name).ToArray();
            typeFullNameArray = inheritedTypes.Select(type => type == null ? "" : string.Format("{0} {1}", type.Assembly.ToString().Split(',')[0], type.FullName)).ToArray();
        }

        public static Type GetFieldType(SerializedProperty property)
        {
            var fieldTypename = property.managedReferenceFieldTypename.Split(' ');
            var assembly = Assembly.Load(fieldTypename[0]);
            return assembly.GetType(fieldTypename[1]);
        }

        private void UpdatePropertyToSelectedTypeIndex(SerializedProperty property, int selectedTypeIndex)
        {
            if (currentTypeIndex == selectedTypeIndex) return;
            currentTypeIndex = selectedTypeIndex;
            var selectedType = inheritedTypes[selectedTypeIndex];
            property.managedReferenceValue =
                selectedType == null ? null : Activator.CreateInstance(selectedType);
        }

        private Rect GetPopupPosition(Rect currentPosition)
        {
            var popupPosition = new Rect(currentPosition);
            popupPosition.width -= EditorGUIUtility.labelWidth;
            popupPosition.x += EditorGUIUtility.labelWidth;
            popupPosition.height = EditorGUIUtility.singleLineHeight;
            return popupPosition;
        }
    }
}