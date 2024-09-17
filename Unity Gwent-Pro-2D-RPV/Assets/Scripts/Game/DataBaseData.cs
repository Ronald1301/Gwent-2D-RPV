using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using Gwent;

[CreateAssetMenu(fileName = "New DataBase", menuName = "DataBase")]

public class DataBaseData : ScriptableObject
{
  public List<Sprite> frontImages;
  public List<Sprite> backImages;
  //public Dictionary<string, CardData> cards = new Dictionary<string, CardData>();
  public List<GameObject> Decks;
  public Dictionary<(string,string), (EffectComplete, SelectorExpression)> effectsCompiled;
  public Dictionary<string, DataCardComplete> cardsCompiled;

}

