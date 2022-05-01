namespace RoyaleArena
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Arena : IArena
    {
        private Dictionary<int, BattleCard> battleCards;

        public Arena()
        {
            battleCards = new Dictionary<int, BattleCard>();
        }

        public int Count => this.battleCards.Count;

        public void Add(BattleCard card)
        {
            if (!this.battleCards.ContainsKey(card.Id))
            {
                battleCards.Add(card.Id, card);
            }
        }

        public void ChangeCardType(int id, CardType type)
        {
            this.CheckIfPresent(id);

            this.battleCards[id].Type = type;
        }

        private void CheckIfPresent(int id)
        {
            if (!this.battleCards.ContainsKey(id))
                throw new InvalidOperationException();
        }

        public bool Contains(BattleCard card)
        {
            return this.battleCards.ContainsKey(card.Id);
        }

        public IEnumerable<BattleCard> FindFirstLeastSwag(int n)
        {
            if (this.Count < n)
                throw new InvalidOperationException();

            return this.battleCards
                .OrderBy(x => x.Value.Swag)
                .ThenBy(x => x.Value.Id)
                .Take(n)
                .Select(x => x.Value);
        }

        public IEnumerable<BattleCard> GetAllInSwagRange(double lo, double hi)
        {
            return this.battleCards
                .Where(x => x.Value.Swag >= lo
                && x.Value.Swag <= hi)
                .Select(x => x.Value)
                .OrderBy(x => x.Swag);
        }

        public IEnumerable<BattleCard> GetByCardType(CardType type)
        {
            List<BattleCard> result = this.battleCards
                .Where(x => x.Value.Type == type)
                .Select(x => x.Value)
                .ToList();

            return result.Count > 0 ? result
                .OrderByDescending(x => x.Damage)
                .ThenBy(x => x.Id)
                : throw new InvalidOperationException();
        }

        public IEnumerable<BattleCard> GetByCardTypeAndMaximumDamage(CardType type, double damage)
        {
            List<BattleCard> result = this.battleCards
                .Where(x => x.Value.Type == type
                && x.Value.Damage <= damage)
                .Select(x => x.Value)
                .ToList();

            return result.Count > 0 ? result
                .OrderByDescending(x => x.Damage)
                .ThenBy(x => x.Id)
                : throw new InvalidOperationException();
        }

        public BattleCard GetById(int id)
        {
            this.CheckIfPresent(id);

            return this.battleCards[id];
        }

        public IEnumerable<BattleCard> GetByNameAndSwagRange(string name, double lo, double hi)
        {
            List<BattleCard> result = this.battleCards
                .Where(x => x.Value.Name == name 
                && x.Value.Swag >= lo
                && x.Value.Swag < hi)
                .Select(x => x.Value)
                .ToList();

            return result.Count > 0 ? result
                .OrderByDescending(x => x.Swag)
                .ThenBy(x => x.Id)
                : throw new InvalidOperationException();
        }

        public IEnumerable<BattleCard> GetByNameOrderedBySwagDescending(string name)
        {
            List<BattleCard> result = this.battleCards
                .Where(x => x.Value.Name == name)
                .Select(x => x.Value)
                .ToList();

            return result.Count > 0 ? result
                .OrderByDescending(x => x.Swag)
                .ThenBy(x => x.Id)
                : throw new InvalidOperationException();
        }

        public IEnumerable<BattleCard> GetByTypeAndDamageRangeOrderedByDamageThenById(CardType type, int lo, int hi)
        {
            List<BattleCard> result = this.battleCards
                .Where(x => x.Value.Type == type 
                && x.Value.Damage >= lo 
                && x.Value.Damage <= hi)
                .Select(x => x.Value)
                .ToList();

            return result.Count > 0 ? result
                .OrderByDescending(x => x.Damage)
                .ThenBy(x => x.Id)
                : throw new InvalidOperationException();
        }

        public IEnumerator<BattleCard> GetEnumerator()
        {
            foreach (var card in this.battleCards)
            {
                yield return card.Value;
            }
        }

        public void RemoveById(int id)
        {
            this.CheckIfPresent(id);

            this.battleCards.Remove(id);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}