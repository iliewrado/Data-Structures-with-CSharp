namespace _02.FitGym
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FitGym : IGym
    {
        private Dictionary<int, Member> members;
        private Dictionary<int, Trainer> trainers;

        public FitGym()
        {
            this.members = new Dictionary<int, Member>();
            this.trainers = new Dictionary<int, Trainer>();
        }

        public void AddMember(Member member)
        {
            if (this.members.ContainsKey(member.Id))
                throw new ArgumentException();

            this.members.Add(member.Id, member);
        }

        public void HireTrainer(Trainer trainer)
        {
            if (this.trainers.ContainsKey(trainer.Id))
                throw new ArgumentException();

            this.trainers.Add(trainer.Id, trainer);
        }

        public void Add(Trainer trainer, Member member)
        {
            if (!this.members.ContainsKey(member.Id))
                this.AddMember(member);

            if (member.Trainer != null
                || !this.trainers.ContainsKey(trainer.Id))
                throw new ArgumentException();

            trainer.Members.Add(member);
            member.Trainer = trainer;
        }

        public bool Contains(Member member)
        {
            return this.members.ContainsKey(member.Id);
        }

        public bool Contains(Trainer trainer)
        {
            return this.trainers.ContainsKey(trainer.Id);
        }

        public Trainer FireTrainer(int id)
        {
            if (!this.trainers.ContainsKey(id))
                throw new ArgumentException();

            Trainer trainer = this.trainers[id];
            this.trainers.Remove(id);
            return trainer;
        }

        public Member RemoveMember(int id)
        {
            if (!this.members.ContainsKey(id))
                throw new ArgumentException();

            Member member = this.members[id];
            this.members.Remove(id);
            return member;
        }

        public int MemberCount => this.members.Count;
        public int TrainerCount => this.trainers.Count;

        public IEnumerable<Member>
            GetMembersInOrderOfRegistrationAscendingThenByNamesDescending()
        {
            return this.members.Values
                .OrderBy(x => x.RegistrationDate)
                .ThenByDescending(x => x.Name);
        }

        public IEnumerable<Trainer> GetTrainersInOrdersOfPopularity()
        {
            return this.trainers.Values
                .OrderBy(x => x.Popularity);
        }

        public IEnumerable<Member>
            GetTrainerMembersSortedByRegistrationDateThenByNames(Trainer trainer)
        {
            return this.trainers[trainer.Id].Members
                .OrderBy(x => x.RegistrationDate)
                .ThenBy(x => x.Name);
        }

        public IEnumerable<Member>
            GetMembersByTrainerPopularityInRangeSortedByVisitsThenByNames(int lo, int hi)
        {
            return this.members.Values
                .Where(x => x.Trainer.Popularity >= lo
                && x.Trainer.Popularity <= hi)
                .OrderBy(x => x.Visits)
                .ThenBy(x => x.Name);
        }

        public Dictionary<Trainer, HashSet<Member>>
            GetTrainersAndMemberOrderedByMembersCountThenByPopularity()
        {
            return this.trainers
                .OrderBy(x => x.Value.Members.Count)
                .ThenBy(x => x.Value.Popularity)
                .ToDictionary(x => x.Value, x => x.Value.Members);
        }
    }
}