using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridSlot))]
public class GridSlotEditor : Editor
{
  private SerializedProperty filledSlotSprite;
  private SerializedProperty emptySlotSprite;
  private SerializedProperty comboSlotSprite;
  private SerializedProperty redSprite;
  private SerializedProperty greenSprite;

  private SerializedProperty yellowSprite;
  private SerializedProperty blueSprite;
  private SerializedProperty whiteSprite;
  private SerializedProperty blackSprite;
  private SerializedProperty greySprite;
  private SerializedProperty pinkSprite;
  private SerializedProperty purpleSprite;
  private SerializedProperty orangeSprite;
  private SerializedProperty emptySprite;

  private void OnEnable()
  {
    filledSlotSprite = serializedObject.FindProperty("FilledSlotSprite");
    emptySlotSprite = serializedObject.FindProperty("EmptySlotSprite");
    comboSlotSprite = serializedObject.FindProperty("ComboSlotSprite");
    redSprite = serializedObject.FindProperty("RedSprite");
    greenSprite = serializedObject.FindProperty("GreenSprite");

    yellowSprite = serializedObject.FindProperty("YellowSprite");
    blueSprite = serializedObject.FindProperty("BlueSprite");
    whiteSprite = serializedObject.FindProperty("WhiteSprite");
    blackSprite = serializedObject.FindProperty("BlackSprite");
    greySprite = serializedObject.FindProperty("GreySprite");
    pinkSprite = serializedObject.FindProperty("PinkSprite");
    purpleSprite = serializedObject.FindProperty("PurpleSprite");
    orangeSprite = serializedObject.FindProperty("OrangeSprite");
    emptySprite = serializedObject.FindProperty("EmptySprite");

  }

  public override void OnInspectorGUI()
  {
    serializedObject.Update();
    DrawDefaultInspector();

    if (GUILayout.Button("FillBlack"))
    {
      ((GridSlot)target).Fill(Parameters.FrogTypes.Black);
    }
    if (GUILayout.Button("FillGreen"))
    {
      ((GridSlot)target).Fill(Parameters.FrogTypes.Green);
    }
    if (GUILayout.Button("FillRed"))
    {
      ((GridSlot)target).Fill(Parameters.FrogTypes.Red);
    }






    serializedObject.ApplyModifiedProperties();
  }
}
