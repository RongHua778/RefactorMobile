using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShapeSelectUI : IUserInterface//控制形状生成
{
    private Animator m_Anim;
    [SerializeField] TileSelect[] tileSelects = default;
    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
    }

    public List<ShapeInfo> GetCurrent3ShapeInfos()
    {
        List<ShapeInfo> shapeInfos = new List<ShapeInfo>();
        if (IsVisible())
        {
            foreach (var item in tileSelects)
            {
                if (item.Shape != null)
                    shapeInfos.Add(item.Shape.m_ShapeInfo);
            }
        }
        return shapeInfos;
    }
    public void ShowThreeShapes()
    {
        ClearAllSelections();
        for (int i = 0; i < tileSelects.Length; i++)
        {
            TileShape shape = GameRes.PreSetShape[i] != null ?
                ConstructHelper.GetTutorialShape(GameRes.PreSetShape[i]) : ConstructHelper.GetRandomShape(); // ConstructHelper.GetRandomShapeByLevel();
            tileSelects[i].InitializeDisplay(i, shape);
        }
        Show();
    }
    public void LoadSaveGame()
    {
        if (LevelManager.Instance.LastGameSave.SaveShapes.Count <= 0)
            return;
        for (int i = 0; i < tileSelects.Length; i++)
        {
            TileShape shape = ConstructHelper.GetTutorialShape(LevelManager.Instance.LastGameSave.SaveShapes[i]);
            tileSelects[i].InitializeDisplay(i, shape);
        }
        Show();
    }

    public void ClearAllSelections()
    {
        foreach (TileSelect select in tileSelects)
        {
            select.ClearShape();
        }
        Hide();
    }

    public override void Show()
    {
        base.Show();
        m_Anim.SetTrigger("Show");
        //tileSelects[0].Shape.draggingShape.ShapeFindPath();
    }


}
