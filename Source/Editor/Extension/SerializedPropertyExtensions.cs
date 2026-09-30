using System;
using UnityEditor;
using UnityEngine;

namespace BadPractice.Cursoriam.Extension.Editor
{
    public static partial class SerializedPropertyExtensions
    {
        public static bool TrySetValue(this SerializedProperty property, object value, bool apply = true)
        {
            if (value == null)
                return false;

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                {
                    property.intValue = Convert.ToInt32(value);
                    break;
                }
                case SerializedPropertyType.Boolean:
                {
                    property.boolValue = Convert.ToBoolean(value);
                    break;
                }
                case SerializedPropertyType.Float:
                {
                    property.floatValue = Convert.ToSingle(value);
                    break;
                }
                case SerializedPropertyType.String:
                {
                    property.stringValue = value.ToString();
                    break;
                }
                case SerializedPropertyType.Enum:
                {
                    property.enumValueIndex = Convert.ToInt32(value);
                    break;
                }
                case SerializedPropertyType.Vector2 when value is Vector3 v3:
                {
                    property.vector2Value = v3;
                    break;
                }
                case SerializedPropertyType.Vector2:
                {
                    property.vector2Value = (Vector2)value;
                    break;
                }
                case SerializedPropertyType.Vector2Int when value is Vector3 v3:
                {
                    property.vector2IntValue = Vector2Int.RoundToInt(v3);
                    break;
                }
                case SerializedPropertyType.Vector2Int:
                {
                    property.vector2IntValue = (Vector2Int)value;
                    break;
                }
                case SerializedPropertyType.Vector3:
                {
                    property.vector3Value = (Vector3)value;
                    break;
                }
                case SerializedPropertyType.Vector3Int:
                {
                    property.vector3IntValue = value is Vector3 v3Int ? Vector3Int.RoundToInt(v3Int) : (Vector3Int)value;
                    break;
                }
                case SerializedPropertyType.Vector4:
                {
                    property.vector4Value = value is Vector3 v3Four
                        ? new(v3Four.x, v3Four.y, v3Four.z, 0f)
                        : (Vector4)value;
                    break;
                }
                default:
                {
                    return false;
                }
            }

            if (apply)
            {
                property.serializedObject.ApplyModifiedProperties();
            }

            return true;
        }
    }
}