//added some of anomolies here
public enum Card
{
    ClubA, Club2, Club3, Club4, Club5, Club6, Club7, Club8, Club9, Club10, ClubJ, ClubK, ClubQ,
    SpadeA, Spade2, Spade3, Spade4, Spade5, Spade6, Spade7, Spade8, Spade9, Spade10, SpadeJ, SpadeK, SpadeQ,
    DiamondA, Diamond2, Diamond3, Diamond4, Diamond5, Diamond6, Diamond7, Diamond8, Diamond9, Diamond10, DiamondJ, DiamondK, DiamondQ,
    HeartA, Heart2, Heart3, Heart4, Heart5, Heart6, Heart7, Heart8, Heart9, Heart10, HeartJ, HeartK, HeartQ,
    Uno2, Uno4, Pot, Empty,
}

public enum CardKind
{
    Basic,
    Anomoly,
}

public enum CardSoundEffect
{
    Basic, Anomaly, Explodoing, Magic, Pokemon, Uno, Yugioh
}

public static class CardExtentions
{
    public static int Value(this Card card)
    {
        return card switch
        {
            Card.ClubA or Card.SpadeA or Card.DiamondA or Card.HeartA => 1,
            Card.Club2 or Card.Spade2 or Card.Diamond2 or Card.Heart2 => 2,
            Card.Club3 or Card.Spade3 or Card.Diamond3 or Card.Heart3 => 3,
            Card.Club4 or Card.Spade4 or Card.Diamond4 or Card.Heart4 => 4,
            Card.Club5 or Card.Spade5 or Card.Diamond5 or Card.Heart5 => 5,
            Card.Club6 or Card.Spade6 or Card.Diamond6 or Card.Heart6 => 6,
            Card.Club7 or Card.Spade7 or Card.Diamond7 or Card.Heart7 => 7,
            Card.Club8 or Card.Spade8 or Card.Diamond8 or Card.Heart8 => 8,
            Card.Club9 or Card.Spade9 or Card.Diamond9 or Card.Heart9 => 9,

            Card.Club10 or Card.Spade10 or Card.Diamond10 or Card.Heart10 or
            Card.ClubJ or Card.SpadeJ or Card.DiamondJ or Card.HeartJ or
            Card.ClubK or Card.SpadeK or Card.DiamondK or Card.HeartK or
            Card.ClubQ or Card.SpadeQ or Card.DiamondQ or Card.HeartQ => 10,

            _ => 0,
        };
    }

    //giving anomolies the anomoly type
    public static CardKind Kind(this Card card)
    {
        return card switch
        {
            Card.Uno2 or Card.Uno4 or Card.Pot or Card.Empty => CardKind.Anomoly,
            _ => CardKind.Basic
        };
    }

    public static CardSoundEffect SoundEffect(this Card card)
    {
        return card switch
        {
            Card.Uno2 or Card.Uno4 => CardSoundEffect.Uno,
            Card.Pot => CardSoundEffect.Magic,
            Card.Empty => CardSoundEffect.Anomaly,
            _ => CardSoundEffect.Basic,
        };
    }
}
