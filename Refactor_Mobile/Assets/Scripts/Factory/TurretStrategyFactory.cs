using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(menuName = "Factory/BlueprintFactory", fileName = "blueprintFactory")]
public class TurretStrategyFactory : GameObjectFactory
{
    //new
    public RefactorStrategy GetRandomRefactorStrategy(TurretAttribute attribute)
    {
        int[] compositionLevel = StaticData.GetSomeRandoms(attribute.minElementLevel,attribute.maxElementLevel, attribute.totalLevel, attribute.elementNumber);
        List<Composition> compositions = new List<Composition>();
        for (int i = 0; i < attribute.elementNumber; i++)
        {
            int element = (int)StaticData.Instance.ContentFactory.GetRandomElementAttribute().element;
            Composition c = new Composition(compositionLevel[i], element);
            compositions.Add(c);
        }
        RefactorStrategy strategy = FormStrategy(attribute, 1, compositions);
        return strategy;
    }

    public RefactorStrategy GetSpecificRefactorStrategy(TurretAttribute attribute, List<int> elements, List<int> qualities, int quality = 1)
    {
        List<Composition> compositions = new List<Composition>();
        for (int i = 0; i < elements.Count; i++)
        {
            Composition c = new Composition(qualities[i], elements[i]);
            compositions.Add(c);
        }
        RefactorStrategy strategy = FormStrategy(attribute, quality, compositions);
        return strategy;
    }

    private RefactorStrategy FormStrategy(TurretAttribute att,int quality, List<Composition> compositions)
    {
        RefactorStrategy strategy;
        switch (att.RefactorName)
        {
            case RefactorTurretName.Boomerrang:
                strategy = new BoomerangStrategy(att, quality, compositions);
                break;
            case RefactorTurretName.Firer:
                strategy = new FirerStrategy(att, quality, compositions);
                break;
            default:
                strategy = new RefactorStrategy(att, quality, compositions);
                break;
        }
        return strategy;
    }



    //public RefactorStrategy GetBuidlingStrategy(TurretAttribute attribute)
    //{
    //    RefactorStrategy strategy = new RefactorStrategy(attribute, 1, null);
    //    return strategy;
    //}



}
