using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class PreBuildStep : IPreprocessBuildWithReport 
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int callbackOrder { get; }
    public void OnPreprocessBuild(BuildReport report)
    {
        Version version;
        string newVersion;
        
        if (Version.TryParse(PlayerSettings.bundleVersion, out version))
        {
            newVersion = new Version(version.Major, version.Minor, version.Build + 1).ToString();
        }
        else
        {
            newVersion = new Version().ToString();
        }
        
        PlayerSettings.bundleVersion = newVersion;
        
        var buildData = ScriptableObject.CreateInstance<BuildNumberData>();
        buildData.BuildNumber = newVersion;

        AssetDatabase.DeleteAsset("Assets/Resources/Build.asset");
        AssetDatabase.CreateAsset(buildData, "Assets/Resources/Build.asset");
        AssetDatabase.SaveAssets();
    }
}
