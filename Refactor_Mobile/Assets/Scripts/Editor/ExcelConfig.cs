using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lanuguage;
using System.Data;
using System.IO;
using Excel;
using UnityEditor;

//namespace EditorTool
//{
public class ExcelConfig
{
    public static readonly string excelsFolderPath = Application.dataPath + "/Excels/";
    public static readonly string assetPath = "Assets/Datas/DataAssets/";
}

public class ExcelTool
{
    public static List<LanguageData> CreateLanguageArrayWithExcel(string filePath)
    {

        List<LanguageData> dataList = new List<LanguageData>();
        int columnNum = 0, rowNum = 0;
        DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);

        //����excel�Ķ��壬�ڶ��п�ʼ��������
        //LanguageData[] array = new LanguageData[rowNum - 1];
        for (int i = 1; i < rowNum; i++)
        {
            LanguageData item = new LanguageData();
            //����ÿ�е�����
            item.Key = collect[i][0].ToString();
            item.Chinese = collect[i][1].ToString();
            item.English = collect[i][2].ToString();
            dataList.Add(item);
        }
        return dataList;
    }

    static DataRowCollection ReadExcel(string filePath, ref int columnNum, ref int rowNum)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        DataSet result = excelReader.AsDataSet();
        //Tables[0] �±�0��ʾexcel�ļ��е�һ�ű������
        columnNum = result.Tables[0].Columns.Count;
        rowNum = result.Tables[0].Rows.Count;
        return result.Tables[0].Rows;
    }

}

public class ExcelBuild : Editor
{

    [MenuItem("CustomEditor/CreateItemAsset")]
    public static void CreateItemAsset()
    {
        //bool hasAsset = false;
        //LanguageManager manager;
        //manager = Resources.Load<LanguageManager>("DataAssets/Language");
        //if (manager == null)
        //    manager = ScriptableObject.CreateInstance<LanguageManager>();
        //else
        //    hasAsset = true;
        LanguageManager manager = CreateInstance<LanguageManager>();
        if (!Directory.Exists(ExcelConfig.assetPath))
        {
            Directory.CreateDirectory(ExcelConfig.assetPath);
        }
        //��ֵҪ��CreateAsset֮ǰ�����������ᵼ�����ݶ�ʧ
        manager.dataArray = ExcelTool.CreateLanguageArrayWithExcel(ExcelConfig.excelsFolderPath + "LanguageExcel.xlsx");

        //if (!hasAsset)
        //{
        //asset�ļ���·�� Ҫ��"Assets/..."��ʼ������CreateAsset�ᱨ��
        string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Language");
        //����һ��Asset�ļ�
        AssetDatabase.CreateAsset(manager, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        // }

        //��ֵ
        //manager.dataArray = ExcelTool.CreateLanguageArrayWithExcel(ExcelConfig.excelsFolderPath + "LanguageExcel.xlsx");


    }
}
//}


