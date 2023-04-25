using UnityEngine;
using UnityEditor;
using System.IO;


public class ScaleAnimationKeyframes : ScriptableWizard
{
    public string NewFileName = "";
    public float KeyframeScale = 100;

    [MenuItem("PlanetMaenad/Face Performance Framework/Scale Animation Keyframes")]
    private static void ReverseAnimationClipWizard()
    {
        DisplayWizard<ScaleAnimationKeyframes>("Scale Animation Keyframes...", "Scale");
    }


    private void OnWizardCreate()
    {
        string directoryPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
        string fileName = Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject));
        string fileExtension = Path.GetExtension(AssetDatabase.GetAssetPath(Selection.activeObject));

        fileName = fileName.Split('.')[0];

        string copiedFilePath = "";

        if (NewFileName != null && NewFileName != "")
        {
            copiedFilePath = directoryPath + Path.DirectorySeparatorChar + NewFileName + fileExtension;
        }
        else
        {
            copiedFilePath = directoryPath + Path.DirectorySeparatorChar + fileName + "_Scaled" + fileExtension;
        }

        AnimationClip originalClip = GetSelectedClip();
        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(Selection.activeObject), copiedFilePath);

        AnimationClip scaledClip = (AnimationClip)AssetDatabase.LoadAssetAtPath(copiedFilePath, typeof(AnimationClip));

        if (originalClip == null)
        {
            return;
        }


        //Check each CurveBinding
        EditorCurveBinding[] _Values = AnimationUtility.GetCurveBindings(originalClip);
        //scaledClip.ClearCurves();

        for (int i = 0; i < _Values.Length; ++i)
        {
            AnimationCurve _Curve = AnimationUtility.GetEditorCurve(originalClip, _Values[i]);
            AnimationCurve _NewCurve = new AnimationCurve();

            for (int k = 0; k < _Curve.keys.Length; ++k)
            {
                Debug.Log("Key value starts as " + _Curve.keys[k].value);

                //Round Up
                if (Mathf.Abs(_Curve.keys[k].value) >= 0.0001f)// && Mathf.Abs(_Curve.keys[k].value) <= 0.02f)
                {
                    //Debug.Log("Key value is <= 0.02f! Multiplying...");
                    Keyframe _NewKey = _Curve.keys[k];
                    _NewKey.value *= KeyframeScale;

                    _NewCurve.AddKey(_NewKey);
                    Debug.Log("Key value is now " + _NewKey.value);
                }
                //Round to 0
                else if (Mathf.Abs(_Curve.keys[k].value) < 0.0001f)
                {
                    //Debug.Log("Key with value too small detected! Setting to 0...");
                    Keyframe _NewKey = new Keyframe();
                    _NewKey.value = 0;

                    _NewCurve.AddKey(_NewKey);
                    Debug.Log("Kew Value is now " + _NewKey.value);
                }
                else
                {

                    _NewCurve.AddKey(_Curve.keys[k]);
                }

                AnimationUtility.SetEditorCurve(scaledClip, _Values[i], _NewCurve);
            }
        }

        Debug.Log("[[ScaleAnimationKeyframes.cs]] Successfully scaled keyframes in " + "animation clip " + fileName + ".");
    }

    private AnimationClip GetSelectedClip()
    {
        Object[] clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets);
        if (clips.Length > 0)
        {
            return clips[0] as AnimationClip;
        }
        return null;
    }



}