namespace _02.LegionSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using _02.LegionSystem.Interfaces;

    public class Legion : IArmy
    {
        private SortedDictionary<int, IEnemy> legion;

        public Legion()
        {
            this.legion = new SortedDictionary<int, IEnemy>();
        }
        public int Size => this.legion.Count;

        public bool Contains(IEnemy enemy)
        {
            return this.legion.ContainsKey(enemy.AttackSpeed);
        }

        public void Create(IEnemy enemy)
        {
            if(!legion.ContainsKey(enemy.AttackSpeed))
                this.legion.Add(enemy.AttackSpeed, enemy);
        }

        public IEnemy GetByAttackSpeed(int speed)
        {
            if(legion.ContainsKey(speed))
                return legion[speed];
            return null;
        }

        public List<IEnemy> GetFaster(int speed)
        {
            List<IEnemy> result = new List<IEnemy>();

            foreach (var item in legion)
            {
                if (item.Key > speed)
                {
                    result.Add(item.Value);
                }
            }

            return result;
        }

        private void EnsureNotEmpty()
        {
            if (legion.Count == 0)
                throw new InvalidOperationException("Legion has no enemies!");
        }

        public IEnemy GetFastest()
        {
            this.EnsureNotEmpty();

            return legion.Last().Value;
        }

        public IEnemy[] GetOrderedByHealth()
        {
            return this.legion.Values.OrderByDescending(x => x.Health).ToArray();
        }

        public List<IEnemy> GetSlower(int speed)
        {
            List<IEnemy> result = new List<IEnemy>();

            foreach (var item in legion)
            {
                if (item.Key < speed)
                {
                    result.Add(item.Value);
                }
            }

            return result;
        }

        public IEnemy GetSlowest()
        {
            this.EnsureNotEmpty();

            return legion.First().Value;
        }

        public void ShootFastest()
        {
            this.EnsureNotEmpty();

            IEnemy enemy = legion.Last().Value;
            legion.Remove(enemy.AttackSpeed);
        }

        public void ShootSlowest()
        {
            this.EnsureNotEmpty();

            IEnemy enemy = legion.First().Value;
            legion.Remove(enemy.AttackSpeed);
        }
    }
}
