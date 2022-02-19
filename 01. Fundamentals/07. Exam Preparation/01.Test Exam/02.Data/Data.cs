namespace _02.Data
{
    using _02.Data.Interfaces;
    using _02.Data.Models;
    using System;
    using System.Collections.Generic;

    public class Data : IRepository
    {
        private PriorityQueue<IEntity> entities;

        public Data()
        {
            this.entities = new PriorityQueue<IEntity>();
        }

        public Data(PriorityQueue<IEntity> repository)
        {
            this.entities = repository;
        }

        public int Size => this.entities.Size;

        public void Add(IEntity entity)
        {
            this.entities.Add(entity);
        }

        public IRepository Copy()
        {
            return new Data(entities);
        }

        public IEntity DequeueMostRecent()
        {
            return entities.Dequeue();
        }

        public List<IEntity> GetAll()
        {
            return entities.ToList;
        }

        public List<IEntity> GetAllByType(string type)
        {
            List<IEntity> temp = new List<IEntity>(entities.ToList);
            
            return temp.FindAll(x => x.GetType().Name.ToString().Equals(type));
        }

        private void isValid(string type)
        {
            
            if (!typeof(User).Name.Equals(type) &&
                !typeof(Invoice).Name.Equals(type) &&
                !typeof(StoreClient).Name.Equals(type))
            {
                throw new InvalidOperationException("Invalid type: " + type);
            }
        }

        public IEntity GetById(int id)
        {
            return entities.ToList.Find(x => x.Id.Equals(id));
        }

        public List<IEntity> GetByParentId(int parentId)
        {
            return entities.ToList.FindAll(x => x.ParentId.Equals(parentId));
        }

        public IEntity PeekMostRecent()
        {
            return entities.Peek();
        }
    }
}
