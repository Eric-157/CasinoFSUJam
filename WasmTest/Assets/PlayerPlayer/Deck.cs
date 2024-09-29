using System;
using System.Collections.Generic;
using System.Linq;

public class Deck
{
    public List<Card> cards;

    public void Reset()
    {
        cards = Enum.GetValues(typeof(Card)).Cast<Card>().ToList();
    }

    public void Shuffle()
    {
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            (cards[k], cards[n]) = (cards[n], cards[k]);
        }
    }

    public Card DrawFromIndex(int index)
    {
        var card = cards[index];
        cards.RemoveAt(index);
        return card;
    }

    public Card DrawAny()
    {
        return DrawFromIndex(cards.Count - 1);
    }

    public Card DrawBasic()
    {
        return DrawFromIndex(cards.FindLastIndex(c => c.Kind() is CardKind.Basic));
    }

    public Card DrawAnomoly()
    {
        return DrawFromIndex(cards.FindLastIndex(c => c.Kind() is CardKind.Anomoly));
    }

    
}
