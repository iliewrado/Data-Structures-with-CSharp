using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Board : IBoard
{
    private Dictionary<string, Card> deck;

    public Board()
    {
        deck = new Dictionary<string, Card>();
    }
    public bool Contains(string name)
    {
        return deck.ContainsKey(name);
    }

    public int Count() => deck.Count;

    public void Draw(Card card)
    {
        if (Contains(card.Name))
            throw new ArgumentException();

        deck.Add(card.Name, card);
    }

    public IEnumerable<Card> GetBestInRange(int start, int end)
    {
        List<Card> cards = new List<Card>();

        foreach (var item in deck.Values)
        {
            if (item.Score >= start && item.Score <= end)
            {
                cards.Add(item);
            }
        }

        return cards.OrderByDescending(x => x.Level);
    }

    public void Heal(int health)
    {
        Card card = deck.Values.OrderBy(x => x.Health).FirstOrDefault();
        card.Health += health;
    }

    public IEnumerable<Card> ListCardsByPrefix(string prefix)
    {
        Dictionary<string, Card> cards = new Dictionary<string, Card>();

        foreach (var item in deck)
        {
            if (item.Key.StartsWith(prefix))
            {
                char[] reverse = item.Key.ToCharArray();
                Array.Reverse(reverse);
                string key = new string(reverse);
                cards.Add(key, item.Value);
            }
        }
        Dictionary<string, Card> result = new Dictionary<string, Card>
            (cards.OrderBy(x => Convert.ToInt32(x.Key.ElementAt(0))).ThenBy(x => x.Value.Level));
        
        return result.Values;
    }

    public void Play(string attackerCardName, string attackedCardName)
    {
        if (!Contains(attackerCardName))
            throw new ArgumentException();
        if (!Contains(attackedCardName))
            throw new ArgumentException();

        Card attackerCard = deck[attackerCardName];
        Card attackedCard = deck[attackedCardName];

        if (attackedCard.Health <= 0) 
            return;

        if (attackerCard.Level != attackedCard.Level)
            throw new ArgumentException();
        
        attackedCard.Health -= attackerCard.Damage;

        if (attackedCard.Health <= 0)
        {
            attackerCard.Score += attackedCard.Level;
        }
    }

    public void Remove(string name)
    {
        if (!Contains(name))
            throw new ArgumentException();

        deck.Remove(name);
    }

    public void RemoveDeath()
    {
        List<Card> cards = deck.Values.ToList();

        foreach (var item in cards)
        {
            if (item.Health <= 0)
            {
                deck.Remove(item.Name);
            }
        }
    }

    public IEnumerable<Card> SearchByLevel(int level)
    {
        List<Card> result = new List<Card>();

        foreach (var item in deck.Values)
        {
            if (item.Level == level)
            {
                result.Add(item);
            }
        }

        return result.OrderByDescending(x => x.Score);
    }
}