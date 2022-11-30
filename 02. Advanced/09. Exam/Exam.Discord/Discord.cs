using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Discord
{
    public class Discord : IDiscord
    {
        private Dictionary<string, Message> messages;
        private Dictionary<string, List<Message>> channels;

        public Discord()
        {
            this.channels = new Dictionary<string, List<Message>>();
            this.messages = new Dictionary<string, Message>();
        }

        public int Count => this.messages.Count;

        public bool Contains(Message message)
        {
            return this.messages.ContainsKey(message.Id);
        }

        public void DeleteMessage(string messageId)
        {
            if (!this.messages.ContainsKey(messageId))
                throw new ArgumentException();

            this.channels.Remove(this.messages[messageId].Channel);
            this.messages.Remove(messageId);
        }

        public IEnumerable<Message> 
            GetAllMessagesOrderedByCountOfReactionsThenByTimestampThenByLengthOfContent()
        {
            if (this.Count == 0)
                return new List<Message>();

            return this.messages.Values
                .OrderByDescending(x => x.Reactions.Count)
                .ThenBy(x => x.Timestamp)
                .ThenBy(x => x.Content.Length);
        }

        public IEnumerable<Message> GetChannelMessages(string channel)
        {
            if (!this.channels.ContainsKey(channel)
                || this.channels[channel].Count == 0)
                throw new ArgumentException();

            return this.channels[channel];
        }

        public Message GetMessage(string messageId)
        {
            if (!this.messages.ContainsKey(messageId))
                throw new ArgumentException();

            return this.messages[messageId];
        }

        public IEnumerable<Message> GetMessageInTimeRange(int lowerBound, int upperBound)
        {
            return this.messages.Values
                .Where(x => x.Timestamp >= lowerBound
                && x.Timestamp <= upperBound)
                .OrderByDescending(x => this.channels[x.Channel].Count);
        }

        public IEnumerable<Message> GetMessagesByReactions(List<string> reactions)
        {
            return this.messages.Values
                .Where(x => x.Reactions.All(reactions.Contains))
                .OrderByDescending(x => x.Reactions.Count)
                .ThenBy(x => x.Timestamp);
        }

        public IEnumerable<Message> GetTop3MostReactedMessages()
        {
            if (this.Count == 0)
                return new List<Message>();

            return this.messages.Values
                .OrderByDescending(x => x.Reactions.Count)
                .Take(3);
        }

        public void ReactToMessage(string messageId, string reaction)
        {
            if (!this.messages.ContainsKey(messageId))
                throw new ArgumentException();

            this.messages[messageId].Reactions.Add(reaction);
        }

        public void SendMessage(Message message)
        {
            this.messages.Add(message.Id, message);

            if (!this.channels.ContainsKey(message.Channel))
                this.channels[message.Channel] = new List<Message>();

            this.channels[message.Channel].Add(message);
        }
    }
}
