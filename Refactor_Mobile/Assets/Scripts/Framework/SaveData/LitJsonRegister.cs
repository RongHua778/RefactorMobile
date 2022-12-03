using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public static class LitJsonRegister
{
    public static void Register()
    {
        RegiterType();
        RegisterFloat();
        RegisterVector3();
        RegisterVector2();
        RegisterVector2Int();
        RegisterQuaternion();
        RegisterGameObject();

        RegisterEnemySequence();
    }

    public static void RegiterType()
    {
        void Exporter(Type obj, JsonWriter writer)
        {
            writer.Write(obj.FullName);
        }

        JsonMapper.RegisterExporter((ExporterFunc<Type>)Exporter);

        Type Importer(string obj)
        {
            return Type.GetType(obj);
        }

        JsonMapper.RegisterImporter((ImporterFunc<string, Type>)Importer);
    }

    private static void RegisterFloat()
    {
        void Exporter(float obj, JsonWriter writer)
        {
            writer.Write(obj);
        }

        JsonMapper.RegisterExporter((ExporterFunc<float>)Exporter);

        float Importer(double obj)
        {
            return (float)obj;
        }

        JsonMapper.RegisterImporter((ImporterFunc<double, float>)Importer);
    }




    private static void RegisterVector3()
    {
        void Exporter(Vector3 obj, JsonWriter writer)
        {
            writer.WriteObjectStart();

            writer.WritePropertyName("x");//д��������
            writer.Write(obj.x);//д��ֵ
            writer.WritePropertyName("y");
            writer.Write(obj.y);
            writer.WritePropertyName("z");
            writer.Write(obj.z);

            writer.WriteObjectEnd();
        }

        JsonMapper.RegisterExporter((ExporterFunc<Vector3>)Exporter);//���л�
    }

    private static void RegisterVector2()
    {
        void Exporter(Vector2 obj, JsonWriter writer)
        {
            writer.WriteObjectStart();

            writer.WritePropertyName("x");//д��������
            writer.Write(obj.x);//д��ֵ
            writer.WritePropertyName("y");
            writer.Write(obj.y);

            writer.WriteObjectEnd();
        }

        JsonMapper.RegisterExporter((ExporterFunc<Vector2>)Exporter);//���л�
    }
    private static void RegisterVector2Int()
    {
        void Exporter(Vector2Int obj, JsonWriter writer)
        {
            writer.WriteObjectStart();

            writer.WritePropertyName("x");//д��������
            writer.Write(obj.x);//д��ֵ
            writer.WritePropertyName("y");
            writer.Write(obj.y);

            writer.WriteObjectEnd();
        }

        JsonMapper.RegisterExporter((ExporterFunc<Vector2Int>)Exporter);//���л�
    }
    private static void RegisterQuaternion()
    {
        void Exporter(Quaternion obj, JsonWriter writer)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName("x");//д��������
            writer.Write(obj.x);//д��ֵ
            writer.WritePropertyName("y");
            writer.Write(obj.y);
            writer.WritePropertyName("z");
            writer.Write(obj.z);
            writer.WritePropertyName("w");
            writer.Write(obj.w);
            writer.WriteObjectEnd();
        }
        JsonMapper.RegisterExporter((ExporterFunc<Quaternion>)Exporter);//���л�
    }

    private static void RegisterGameObject()
    {
        void Exporter(GameObject obj, JsonWriter writer)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName("name");//д��������
            if (obj != null)
                writer.Write(obj.name);//д��ֵ
            else
                writer.Write("PrefabΪNull");//д��ֵ
            writer.WriteObjectEnd();
        }
        JsonMapper.RegisterExporter((ExporterFunc<GameObject>)Exporter);//���л�
    }

    private static void RegisterEnemySequence()
    {
        void Exporter(EnemySequence obj, JsonWriter writer)
        {
            writer.WriteObjectStart();
            writer.WritePropertyName("EnemyType");//д��������
            writer.Write((int)obj.EnemyType);//д��ֵ
            writer.WritePropertyName("Amount");
            writer.Write(obj.Amount);
            writer.WritePropertyName("CoolDown");
            writer.Write(obj.CoolDown);
            writer.WritePropertyName("Intensify");
            writer.Write(obj.Intensify);
            writer.WritePropertyName("IsBoss");
            writer.Write(obj.IsBoss);
            writer.WritePropertyName("Wave");
            writer.Write(obj.Wave);
            writer.WritePropertyName("DmgResist");
            writer.Write(obj.DmgResist);
            writer.WriteObjectEnd();
        }
        JsonMapper.RegisterExporter((ExporterFunc<EnemySequence>)Exporter);//���л�
    }

}
