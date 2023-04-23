using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;

public class SheetMakerWindow : EditorWindow
{
    private string LiveSheetLink = "1SSoewXJOTTsT1yzDM6HD0CV3vL4P026OXZgJI8hbAJ4";
    private string DevSheetLink = "1vwyljr1NCfNi0cHIJjtnlGFGSBPmKGTFD9OgKFUqXS0";

    private string mClientID = "1041877617748-60k5dfh20pcgoaekvkqbpqp3ib0csn5o.apps.googleusercontent.com";
    private string mSecretCode = "yBMwbgLXBJt8xTUE6lX_xObs";

    private string mSavePath = "Assets/projectFile/AddressableAssets";
    private string mJsonFolder = "json_data";
    private string mCsvFolder = "csv_data";

    private bool mIsDevLink = true;
    private bool mIsLiveLink = false;

    private static float mMinX = 350f;
    private static float mMaxX = 400f;
    private static float mMinY = 250f;
    private static float mMaxY = 300f;

    private double mValue = 0;

    private int mSelectedIndex = -1;

    private string[] mMetaIDArr = new string[]
        {
            "UNIT_META",
            "SUB_EFFECT_META",
            "ACTION_META",
            "ACTION_IMAGE_META",
            "OBJECT_META"
    #if UNITY_EDITOR
            ,"MAP_META"
    #endif
            ,"COUNT"
        };

    public static void Initialize()
    {
        SheetMakerWindow window = (SheetMakerWindow)GetWindowWithRect(typeof(SheetMakerWindow), new Rect(0, 0, mMinX, mMinY), false, "Sheet Maker ver_1.0");
        window.minSize = new Vector2(mMinX, mMinY);
        window.maxSize = new Vector2(mMaxX, mMaxY);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("DEV URL", GUILayout.Width(80f));
        mIsDevLink = EditorGUILayout.Toggle("", mIsDevLink);
        mIsLiveLink = !mIsDevLink;
        GUILayout.EndHorizontal();
        EditorGUI.BeginDisabledGroup(true);
        GUILayout.TextField(DevSheetLink);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(3f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("LIVE URL", GUILayout.Width(80f));
        mIsLiveLink = EditorGUILayout.Toggle("", mIsLiveLink);
        mIsDevLink = !mIsLiveLink;
        GUILayout.EndHorizontal();
        EditorGUI.BeginDisabledGroup(true);
        GUILayout.TextField(LiveSheetLink);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("SAVE PATH", GUILayout.Width(80f));
        GUILayout.EndHorizontal();
        EditorGUI.BeginDisabledGroup(true);
        GUILayout.TextField(mSavePath);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(5f);
        if (mSelectedIndex == -1)
        {
            mSelectedIndex = mMetaIDArr.Length - 1;
        }
        mSelectedIndex = EditorGUILayout.Popup("META_TYPE", mSelectedIndex, mMetaIDArr);
        GUILayout.Space(5f);
        Rect rect = EditorGUILayout.BeginHorizontal();
        Handles.color = Color.gray;
        Handles.DrawLine(new Vector2(rect.x + 5, rect.y), new Vector3(rect.width - 5, rect.y));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Make Json File") == true)
        {
            EditorUtility.DisplayProgressBar("Save Json Files..", mSavePath + mJsonFolder, (float)(mValue / 0.01));
            string sLink = mIsDevLink ? DevSheetLink : LiveSheetLink;
            GoogleSheetManager.MakeSheetFileToJson(mClientID, mSecretCode, sLink, mSavePath, mJsonFolder, (eMetaType) mSelectedIndex);
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        if (GUILayout.Button("Make CSV File") == true)
        {
            EditorUtility.DisplayProgressBar("Save CSV Files..", mSavePath + "/" + mCsvFolder, (float)(mValue / 0.01));
            string sLink = mIsDevLink ? DevSheetLink : LiveSheetLink;
            GoogleSheetManager.MakeSheetFileToCSV(mClientID, mSecretCode, sLink, mSavePath, mCsvFolder, (eMetaType) mSelectedIndex);
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        Rect rect2 = EditorGUILayout.BeginHorizontal();
        Handles.color = Color.gray;
        Handles.DrawLine(new Vector2(rect2.x + 5, rect2.y), new Vector3(rect2.width - 5, rect2.y));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Make All File") == true)
        {
            EditorUtility.DisplayProgressBar("Save ALL Files..", "", (float)(mValue / 0.01));
            string sLink = mIsDevLink ? DevSheetLink : LiveSheetLink;
            GoogleSheetManager.MakeSheetFileToALL(mClientID, mSecretCode, sLink, mSavePath, mJsonFolder, mCsvFolder);
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }
        GUILayout.EndHorizontal();

    }
}
