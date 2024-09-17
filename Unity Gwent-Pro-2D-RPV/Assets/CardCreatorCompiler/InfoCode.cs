using System;

[Serializable]
public class InfoCode
{
    public string Code;

    public InfoCode(string code) { this.Code = code; }

    //Tests

    // Effects

    #region 

    /*

        effect{ 
                Name :"Damage",
                Params: { 
                         amount: Number
                        }, 
                Action: (targets, context) =>
                        {
                            for target in targets
                            {
                                i = 0;
                                while (i++ < amount) target.Power -= 1;
                            };
                        }
                }

        effect {
                 Name: "Draw",
                 Action: (targets, context) =>
                    {
                        topCard = context.Deck.Pop();
                        context.Hand.Add(topCard);
                        context.Hand.Shuffle();
                    }
                }

        effect {
                 Name: "Return" @@ "ToDeck",
                 Action: (targets, context) =>
                    {
                        for target in targets
                        {
                            owner = target.Owner;
                            deck = context.DeckOfPlayer(owner);
                            deck.Push(target);
                            deck.Shuffle();
                            context.Board.Remove(target);
                        };
                    }
                }
         */

    #endregion

    //Cards

    #region 

    /*

   card { 
        Type: "Oro",
        Name: "Beluga", 
        Faction: "Pira" @ "tes",
        Power: 10,
        Range: ["Melee", "Ranged"],
        OnActivation:
                    [
                        {
                            Effect:
                                     {
                                        Name: "Damage",
                                        amount: 5
                                     } , 
                             Selector:
                                    {
                                        Source: "board",
                                        Single: false, 
                                        Predicate: (unit) => unit.Faction == "Pirates"
                                      } ,
                            PostAction:
                                    {
                                        Type: "Return ToDeck", 
                                        Selector:
                                                {
                                                    Source: "parent", 
                                                     Single: false,
                                                     Predicate: (unit) => unit.Power < 1
                                                  }
                                        }   
                            }, 
                        {
                            Effect: "Draw"
                        } 
                    ] 
        }



        card { 
           Type: "Silver",
            Name: "Ciri",
           Faction: "Pirates",
            Power: 6,
            Range: ["Siege", "Melee"],
           OnActivation:
                        [
                            {
                                Effect:
                                    {
                                        Name: "Damage",
                                        amount: 3
                                    } , 
                                Selector:
                                        {
                                            Source: "field",
                                            Single: true, 
                                        }
                             }
                         ]
        }

        card{
            Type: "Silver",
            Name: "Dandelion",
            Faction: "Resistance",
            Power: 1,
            Range: ["Melee"],
            OnActivation:
                        [
                            
                                Effect:
                                        {
                                            Name: "Draw",
                                        }
                             
                         ]
             } 



   sin effects

        card { 
           Type: "Silver",
            Name: "Cyclops",
           Faction: "Resistance",
            Power: 4,
            Range: ["Ranged"],
           OnActivation: [
           
                            Effect:
                                     {
                                        Name: "Return ToDeck"
                                     } , 
                            Selector:
                                    {
                                        Source: "otherField",
                                        Single: false,
                                         Predicate: (unit) => unit.Power < 1
                                    }
                         ]
        }

   card { 
           Type: "Silver",
            Name: "Geralt",
           Faction: "Resistance",
            Power: 2,
            Range: ["Siege"],
           OnActivation: [ 
            Effect:
                                     {
                                        Name: "Return ToDeck"
                                     } , 
                            Selector:
                                    {
                                       Source: "field",
                                        Single: false,
                                         Predicate: (unit) => unit.Power < 1
                                    }
                                    ]
        }

        card { 
           Type: "Silver",
            Name: "Triss",
           Faction: "Resistance",
            Power: 2,
            Range: ["Siege"],
           OnActivation: [ 
            Effect:
                                        {
                                            Name: "Draw",
                                        }
                                        ]
        }
        card { 
           Type: "Silver",
            Name: "Potest",
           Faction: "Resistance",
            Power: 2,
            Range: ["Siege"],
           OnActivation: [ 
            Effect:
                                        {
                                            Name: "Draw",
                                        }
                                        ]
        }

             card { 
           Type: "Climate",
            Name: "Rain",
           Faction: "Resistance",
            Power: 0,
            Range: ["Ranged"],
           OnActivation: [ ]
        }

        card { 
           Type: "Increase",
            Name: "Blade",
           Faction: "Pirates",
            Power: 0,
            Range: ["Siege"],
           OnActivation: [ ]
        }

        card { 
           Type: "Lure",
            Name: "Joker",
           Faction: "Resistance",
            Power: 0,
            Range: [],
           OnActivation: [ ]
        }

        card { 
           Type: "Lure",
            Name: "Teacher",
           Faction: "Resistance",
            Power: 0,
            Range: [],
           OnActivation: [ ]
        }
        card { 
           Type: "Lure",
            Name: "Student",
           Faction: "Pirates",
            Power: 0,
            Range: [],
           OnActivation: [ ]
        }

        card { 
           Type: "Lure",
            Name: "Lamborghini",
           Faction: "Pirates",
            Power: 0,
            Range: [],
           OnActivation: [ ]
        }

        card { 
           Type: "Clearance",
            Name: "Broom",
           Faction: "Resistance",
            Power: 4,
            Range: ["Melee"],
           OnActivation: [ ]
        }

    */

    #endregion

}
