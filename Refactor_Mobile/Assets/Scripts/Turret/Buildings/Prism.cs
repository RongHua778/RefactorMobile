using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : BuildingContent
{
    float rotSpeed = 0;
    float RotSpeed { get => 180f; set => rotSpeed = value; }
    //[SerializeField] PrismDetector m_Detector = default;


    public override bool GameUpdate()
    {
        base.GameUpdate();
        SelfRotateControl();
        return true;
    }
    private void SelfRotateControl()
    {
        rotTrans.Rotate(Vector3.forward * -RotSpeed * Time.deltaTime, Space.Self);
    }

    //public override void GenerateRange()
    //{
    //    base.GenerateRange();
    //    m_Detector.SetSize(Strategy.FinalRange);

    //}



}
