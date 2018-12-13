using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
namespace Dungeon
{
    [System.Serializable]
    public class MapchipInfo
    {
        public enum Rotate
        {
            Rot_0,
            Rot_90,
            Rot_180,
            Rot_270,
        }
        [SerializeField]
        public GameObject prefab;
        [SerializeField]
        public Rotate rotate;
        public Quaternion Quaternion
        {
            get
            {
                switch (rotate)
                {
                    default:
                    case Rotate.Rot_0: return Quaternion.Euler(0, 0, 0);
                    case Rotate.Rot_90: return Quaternion.Euler(0, 90, 0);
                    case Rotate.Rot_180: return Quaternion.Euler(0, 180, 0);
                    case Rotate.Rot_270: return Quaternion.Euler(0, 270, 0);
                }
            }
        }
    }

    [CreateAssetMenu(menuName = @"MapchipSet")]
    public class MapchipSet : ScriptableObject
    {
        [SerializeField] Vector2Int gridsize;
        [SerializeField] PostProcessingProfile postProcessingProfile;

        [SerializeField] MapchipInfo[] up_left;
        [SerializeField] MapchipInfo[] up_left_right;
        [SerializeField] MapchipInfo[] up_right;

        [SerializeField] MapchipInfo[] left_up_down;
        [SerializeField] MapchipInfo[] all;
        [SerializeField] MapchipInfo[] right_up_down;

        [SerializeField] MapchipInfo[] down_left;
        [SerializeField] MapchipInfo[] down_left_right;
        [SerializeField] MapchipInfo[] down_right;

        [SerializeField] MapchipInfo[] left_up_corner;
        [SerializeField] MapchipInfo[] right_up_corner;
        [SerializeField] MapchipInfo[] left_down_corner;
        [SerializeField] MapchipInfo[] right_down_corner;

        [SerializeField] MapchipInfo[] up_stairs;
        [SerializeField] MapchipInfo[] down_stairs;

        [SerializeField] MapchipInfo[] start;
        [SerializeField] MapchipInfo[] goal;
        [SerializeField] MapchipInfo[] obstacle;
        

        public Vector2Int GridSize { get { return gridsize; } }
        public PostProcessingProfile PostProcessingProfile { get { return postProcessingProfile; } }

        MapchipInfo GetChipInternal(MapchipInfo[] infos, System.Random random)
        {
            if (random == null) return infos[0];
            return infos[random.Next(infos.Length)];
        }

        public MapchipInfo GetChip(Tile tile, System.Random random = null)
        {
            // 0 1 2
            // 3 4 5
            // 6 7 8
            switch ((int)tile)
            {
                // 0 1 2 
                case (int)(Tile.UP | Tile.Left):                return GetChipInternal(up_left, random);
                case (int)(Tile.UP | Tile.Left | Tile.Right):   return GetChipInternal(up_left_right, random);
                case (int)(Tile.UP | Tile.Right):               return GetChipInternal(up_right, random);
                // 3 4 5 
                case (int)(Tile.Left | Tile.UP | Tile.Down):    return GetChipInternal(left_up_down, random);
                case (int)(Tile.All):                           return GetChipInternal(all, random);
                case (int)(Tile.Right | Tile.UP | Tile.Down):   return GetChipInternal(right_up_down, random);
                // 6 7 8 
                case (int)(Tile.Down | Tile.Left):              return GetChipInternal(down_left, random);
                case (int)(Tile.Down | Tile.Left | Tile.Right): return GetChipInternal(down_left_right, random);
                case (int)(Tile.Down | Tile.Right):             return GetChipInternal(down_right, random);
                // 角
                case (int)(Tile.LeftUpCorner):                  return GetChipInternal(left_up_corner, random);
                case (int)(Tile.RightUpCorner):                 return GetChipInternal(right_up_corner, random);
                case (int)(Tile.LeftDownCorner):                return GetChipInternal(left_down_corner, random);
                case (int)(Tile.RightDownCorner):               return GetChipInternal(right_down_corner, random);
                // 階段
                case (int)(Tile.UpStairs):                      return GetChipInternal(up_stairs, random);
                case (int)(Tile.DownStairs):                    return GetChipInternal(down_stairs, random);
                // 開始/終了
                case (int)(Tile.Start):                         return GetChipInternal(start, random);
                case (int)(Tile.Goal):                          return GetChipInternal(goal, random);
                // 障害物
                case (int)(Tile.Obstacle):                      return GetChipInternal(obstacle, random);
                default:
                {
                    if(tile != Tile.None) Debug.Log(tile);
                    return null;
                }
            }
        }
    }
#if UNITY_EDITOR

    [CustomEditor(typeof(MapchipSet))]
    public class MapchipSetInspector : Editor
    {
        SerializedProperty gridsize;
        SerializedProperty postProcessingProfile;
        List<ReorderableList> reorderableList = new List<ReorderableList>();
        Dictionary<string, string> nameset = new Dictionary<string, string>
        {
            { "up_left", "左上" }, { "up_left_right", "上" }, { "up_right", "右上" },
            { "left_up_down", "左" }, { "all", "真ん中" }, { "right_up_down", "右" },
            { "down_left", "左下" }, { "down_left_right", "下" }, { "down_right", "右下" },
            { "left_up_corner", "左上角" }, { "right_up_corner", "右上角" }, { "left_down_corner", "左下角" }, { "right_down_corner", "右下角" },
            { "up_stairs", "上り階段" }, { "down_stairs", "下り階段" },
            { "start", "スタート" }, { "goal", "ゴール" },
            { "obstacle", "障害物" },
        };

        private void OnEnable()
        {
            reorderableList.Clear();
            foreach (var kv in nameset)
            {
                reorderableList.Add(CreateReorderableList(kv));
            }
            gridsize = serializedObject.FindProperty("gridsize");
            postProcessingProfile = serializedObject.FindProperty("postProcessingProfile");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            gridsize.vector2IntValue = EditorGUILayout.Vector2IntField("グリッドサイズ", gridsize.vector2IntValue);
            postProcessingProfile.objectReferenceValue = EditorGUILayout.ObjectField("PostProcessingProfile", postProcessingProfile.objectReferenceValue,typeof(PostProcessingProfile), true);

            foreach (var item in reorderableList)
            {
                item.DoLayoutList();
            }
            serializedObject.ApplyModifiedProperties();
        }

        ReorderableList CreateReorderableList(KeyValuePair<string, string> kv)
        {
            var key = kv.Key;
            var value = kv.Value;
            var property = serializedObject.FindProperty(key);
            // 生成
            var res = new ReorderableList(serializedObject, property);
            // ヘッダー描画イベント
            res.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, value);
            // エレメント描画イベント
            res.drawElementCallback = (rect, index, active, focused) =>
            {
                rect.x += 10;
                rect.width -= 10;
                var element = property.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, true);
            };

            res.elementHeightCallback = (index) =>
            {
                if (res.serializedProperty.GetArrayElementAtIndex(index).isExpanded)
                {
                    return 18 * 3;
                }
                else
                {
                    return 16;
                }
            };
            return res;
        }
    }
#endif
}
