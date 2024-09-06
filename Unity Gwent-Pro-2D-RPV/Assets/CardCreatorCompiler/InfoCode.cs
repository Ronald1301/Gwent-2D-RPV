using System;

[Serializable]
public class InfoCode
{
    public string Code;

    public InfoCode(string code) { this.Code = code; }

    string code = " effect{\"Name\":\"Damage\",\n\nParams: {\n\namount: Number\n\n}\n\nAction: (targets, context) => {\n\nfor target in targets {\n\ni = 0;\n\nwhile (i++ < amount) target.Power -= 1;\n\n};\n\n}\n}\n\n";
    string code1 = "effect {\n\"nName: \"Draw\",\n\nAction: (targets, context) => { topCard = context.Deck.Pop(); context.Hand.Add(topCard); context.Hand.Shuffle();\n\n}\n\n}\n\n";
    string code2 = "effect {\n\nName: \"Return ToDeck\",\n\nAction: (targets, context) => {\n\nfor target in targets {\nowner = target.Owner;\ndeck = context.DeckOfPlayer (owner);\ndeck. Push(target); deck.Shuffle(); context.Board. Remove (target);\n};\n\n}\n\n}\n\n";
    string code3 = "  card {\n\nType: \"Oro\",\n\nName: \"Beluga\",\n\nFaction: \"Northern Realms\",\n\nPower: 10,\n\nRange: [\"Melee\", \"Ranged\"],\n\nOnActivation: [\n\n{\n\nEffect: {\n\nName: \"Damage\", .\n\nAmount: 5, .\n\n}\n\nSelector: {\n\nSource: \"board\", // o \"hand\", \"otherHand\", \"deck\", \"otherDeck\", \"field\", \"otherField\", \"parent\".\n\nSingle: false, .\n\nPredicate: (unit) => unit. Faction == \"Northern\" @@ \"Realms\"\n\n}\n\nPostAction: {\n\nType: \"Return ToDeck\",\n\nSelector: {.\n\nSource: \"parent\",\n\nSingle: false,\n\nPredicate: (unit) => unit.Power < 1\n}\n\n}\n\n},\n\n{\n\n}\n\nEffect: \"Draw\" ";

}
