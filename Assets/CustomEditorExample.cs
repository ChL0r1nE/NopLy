#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomEditorExample)), CanEditMultipleObjects]
public class PlayerEditor : Editor
{
    public bool show = false;

    CustomEditorExample _player;
    SerializedObject _serializedPlayer;
    SerializedProperty _serializedHealth;
    SerializedProperty _serialized;

    private void OnEnable()
    {
        _serialized = serializedObject.FindProperty("Player");

        _player = target as CustomEditorExample;
        _serializedPlayer = new SerializedObject(_player);
        _serializedHealth = _serializedPlayer.FindProperty("_health");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("CuST0m_1337 InsP3cT0r.", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.HelpBox("He-He-He. HelL0", MessageType.Error);
        EditorGUILayout.Space();

        _player.Show = EditorGUILayout.Toggle(new GUIContent("Показать что-то", "Нажмите, чтобы узреть какие-то подкапотные переменные."), _player.Show);
        if (!_player.Show) return;

        _player.A = EditorGUILayout.IntField(new GUIContent("Переменная А", "Переменная типа int"), _player.A);
        _player.B = EditorGUILayout.TextField(new GUIContent("Переменная B:", "Переменная типа string"), _player.B);
        _player.C = EditorGUILayout.FloatField(new GUIContent("Переменная C", "Переменная типа float"), _player.C);
        EditorGUILayout.PropertyField(_serialized);

        _player.MoveSpeed = EditorGUILayout.Slider(new GUIContent("Скорость игрока", "Настраиваемая скорость игрока"), _player.MoveSpeed, 1, 10);
        _serializedHealth.floatValue = EditorGUILayout.FloatField(new GUIContent("Здоровье", "Уровень здоровья игрока"), _serializedHealth.floatValue);

        //Bild
        Texture2D cover = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Sprites/HP.png", typeof(Texture2D));
        float imageWidth = EditorGUIUtility.currentViewWidth;
        float imageHeight = imageWidth * cover.height / cover.width;
        Rect rect = GUILayoutUtility.GetRect(imageWidth, imageHeight);
        GUI.DrawTexture(rect, cover, ScaleMode.ScaleToFit);

        //Mark
        GUILayout.Label("1mM0rT4L_", new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
        _serializedPlayer.Update();

        //TastenBegin
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Восстановить здоровье", new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 30 }))
        {
            _serializedHealth.floatValue = 100;
            ShowHealthMessage();
        }
        if (GUILayout.Button("Ударить игрока", new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 30 }))
        {
            _player.ApplyDamage(20);
            ShowHealthMessage();
        }
        GUILayout.EndHorizontal();
        //TastenEnd

        void ShowHealthMessage()
        {
            if (_serializedHealth.floatValue > 0)
                Debug.Log(string.Format("Player's health is {0} now!", _serializedHealth.floatValue));
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_player);
            _serializedPlayer.ApplyModifiedProperties();
        }
    }
}

public class CustomEditorExample : MonoBehaviour
{
    public bool Show;

    public Player.Move Player;
    public int A;
    public float C;
    public float MoveSpeed;
    public string B;

    [SerializeField] private float _health = 100;

    public void ApplyDamage(float damage) => _health -= damage;
}
#endif