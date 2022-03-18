using System;
using System.Collections.Generic;
public class Olympics : IOlympics
{
    private Dictionary<int, Competitor> competitors;
    private Dictionary<int, Competition> competitions;

    public Olympics()
    {
        competitors = new Dictionary<int, Competitor>();
        competitions = new Dictionary<int, Competition>();
    }

    public void AddCompetition(int id, string name, int participantsLimit)
    {
        CheckValidCompetition(id);

        competitions.Add(id, new Competition(name, id, participantsLimit));
    }

    private void CheckValidCompetition(int id)
    {
        if (competitions.ContainsKey(id))
            throw new ArgumentException();
    }

    public void AddCompetitor(int id, string name)
    {
        CheckValidCompetitor(id);

        competitors.Add(id, new Competitor(id, name));
    }

    private void CheckValidCompetitor(int id)
    {
        if (competitors.ContainsKey(id))
            throw new ArgumentException();
    }

    public void Compete(int competitorId, int competitionId)
    {
        CheckInvalidCompetitor(competitorId);
        CheckInvalidCompetition(competitionId);
        Competitor competitor = competitors[competitorId];
        competitor.TotalScore += competitions[competitionId].Score;
        competitions[competitionId].Competitors.Add(competitor);
    }
    private void CheckInvalidCompetitor(int id)
    {
        if (!competitors.ContainsKey(id))
            throw new ArgumentException();
    }

    public int CompetitionsCount()
        => competitions.Count;

    public int CompetitorsCount()
        => competitors.Count;

    public bool Contains(int competitionId, Competitor comp)
    {
        CheckInvalidCompetition(competitionId);
        return competitions[competitionId].Competitors.Contains(comp);
    }

    public void Disqualify(int competitionId, int competitorId)
    {
        CheckInvalidCompetitor(competitorId);
        CheckInvalidCompetition(competitionId);
        Competition competition = competitions[competitionId];
        Competitor competitor = competitors[competitorId];
        competition.Competitors.Remove(competitor);
        competitor.TotalScore -= competition.Score;
    }

    public IEnumerable<Competitor> FindCompetitorsInRange(long min, long max)
    {
        SortedList<int ,Competitor> result = new SortedList<int, Competitor>();

        foreach (var item in competitors)
        {
            if (item.Value.TotalScore > min && 
                item.Value.TotalScore <= max)
            {
                result.Add(item.Key, item.Value);
            }
        }

        return result.Values;
    }

    public IEnumerable<Competitor> GetByName(string name)
    {
        SortedList<int, Competitor> result = new SortedList<int, Competitor>();

        foreach (var item in competitors)
        {
            if (item.Value.Name == name)
            {
                result.Add(item.Key, item.Value);
            }
        }

        return result.Count > 0 ? result.Values : throw new ArgumentException();
    }

    public Competition GetCompetition(int id)
    {
        CheckInvalidCompetition(id);

        return competitions[id];
    }

    private void CheckInvalidCompetition(int id)
    {
        if (!competitions.ContainsKey(id))
            throw new ArgumentException();
    }

    public IEnumerable<Competitor> SearchWithNameLength(int min, int max)
    {
        SortedList<int, Competitor> result = new SortedList<int, Competitor>();

        foreach (var item in competitors)
        {
            if (item.Value.Name.Length >= min && item.Value.Name.Length <= max)
            {
                result.Add(item.Key, item.Value);
            }
        }


        return result.Values;
    }
}