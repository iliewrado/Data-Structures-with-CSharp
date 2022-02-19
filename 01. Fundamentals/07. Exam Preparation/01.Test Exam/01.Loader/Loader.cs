namespace _01.Loader
{
    using _01.Loader.Interfaces;
    using _01.Loader.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Loader : IBuffer
    {
        private List<IEntity> entities;

        public Loader()
        {
            entities = new List<IEntity>();
        }

        public int EntitiesCount => 
            this.entities.Count;

        public void Add(IEntity entity)
        {
            entities.Add(entity);
        }

        public void Clear()
        {
            entities.Clear();
        }

        public bool Contains(IEntity entity)
        {
            return entities.Contains(entity);
        }

        public IEntity Extract(int id)
        {
            IEntity entity = this.FindById(id);
            
            if (entity != null)
            {
                entities.Remove(entity);
            }

            return entity;
        }

        public IEntity Find(IEntity entity)
        {
            return this.GetEntity(entity);
        }

        public List<IEntity> GetAll()
        {
            return new List<IEntity>(entities);
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        public void RemoveSold()
        {
            List<IEntity> removedSold = new List<IEntity>();

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].Status != BaseEntityStatus.Sold)
                {
                    removedSold.Add(entities[i]);
                }
            }

            entities = removedSold;
        }

        public void Replace(IEntity oldEntity, IEntity newEntity)
        {
            int indexOfOld = entities.IndexOf(oldEntity);
            this.IsValid(indexOfOld, "Entity not found");
            this.entities[indexOfOld] = newEntity;
        }

        public List<IEntity> RetainAllFromTo(BaseEntityStatus lowerBound, BaseEntityStatus upperBound)
        {
            List<IEntity> inBound = new List<IEntity>();

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].Status >= lowerBound && entities[i].Status <= upperBound)
                {
                    inBound.Add(entities[i]);
                }
            }

            return inBound;
        }

        public void Swap(IEntity first, IEntity second)
        {
            int indexOfFirst = entities.IndexOf(first);
            int indexOfSecond = entities.IndexOf(second);
            this.IsValid(indexOfFirst, "Entity not found");
            this.IsValid(indexOfSecond, "Entity not found");

            IEntity temp = entities[indexOfFirst];
            entities[indexOfFirst] = entities[indexOfSecond];
            entities[indexOfSecond] = temp;
        }

        public IEntity[] ToArray()
        {
            return this.entities.ToArray();
        }

        public void UpdateAll(BaseEntityStatus oldStatus, BaseEntityStatus newStatus)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].Status == oldStatus)
                {
                    entities[i].Status = newStatus;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        private IEntity FindById(int id)
        {
            IEntity entity = null;

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].Id == id)
                {
                    entity = entities[i];
                }
            }

            return entity;
        }

        private IEntity GetEntity(IEntity entity)
        {
            int index = entities.IndexOf(entity);

            if (index >= 0)
            {
                return entities[index];
            }

            return null;
        }

        private void IsValid(int index, string message)
        {
            if (index < 0)
                throw new InvalidOperationException(message);
        }
    }
}
