using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class MyJob
{
    [MenuItem("Custom/MyJobs/Show Leak Detection Mode")]
    static void ShowLeakDetection()
    {
        EditorUtility.DisplayDialog("�ڴ�й©�������", string.Format("NativeLeakDetection.Mode��{0}", NativeLeakDetection.Mode.ToString()), "OK");
    }

    [MenuItem("Custom/MyJobs/Leak Detection Enabled")]
    static void LeakDetectionEnabled()
    {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.Enabled;
    }

    [MenuItem("Custom/MyJobs/Leak Detection Enabled", true)]  //�ڶ���������ʾ�������ǲ˵��Ƿ���õ���֤����
    static bool ValidateLeakDetectionEnabled()
    {
        return NativeLeakDetection.Mode != NativeLeakDetectionMode.Enabled;
    }

    [MenuItem("Custom/MyJobs/Leak Detection Enabled With StackTrace")]
    static void LeakDetectionEnabledWithStackTrace()
    {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
    }

    [MenuItem("Custom/MyJobs/Leak Detection Enabled With StackTrace", true)]  //�ڶ���������ʾ�������ǲ˵��Ƿ���õ���֤����
    static bool ValidateLeakDetectionEnabledWithStackTrace()
    {
        return NativeLeakDetection.Mode != NativeLeakDetectionMode.EnabledWithStackTrace;
    }

    [MenuItem("Custom/MyJobs/Leak Detection Disable")]
    static void LeakDetectionDisable()
    {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
    }

    [MenuItem("Custom/MyJobs/Leak Detection Disable", true)]  //�ڶ���������ʾ�������ǲ˵��Ƿ���õ���֤����
    static bool ValidateLeakDetectionDisable()
    {
        return NativeLeakDetection.Mode != NativeLeakDetectionMode.Disabled;
    }
}

