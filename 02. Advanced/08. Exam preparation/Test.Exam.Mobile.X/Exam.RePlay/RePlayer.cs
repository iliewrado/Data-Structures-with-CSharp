using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.RePlay
{
    public class RePlayer : IRePlayer
    {
        private Dictionary<string, Track> rePlayer;
        private Dictionary<string, Dictionary<string, Track>> albums;
        private Queue<Track> listeningQueue;

        public RePlayer()
        {
            this.rePlayer = new Dictionary<string, Track>();
            this.albums = new Dictionary<string, Dictionary<string, Track>>();
            this.listeningQueue = new Queue<Track>();
        }

        public int Count => this.rePlayer.Count;

        public void AddToQueue(string trackName, string albumName)
        {
            if (!this.albums.ContainsKey(albumName)
                || !this.albums[albumName].ContainsKey(trackName))
                throw new ArgumentException();

            this.listeningQueue.Enqueue(this.albums[albumName][trackName]);
        }

        public void AddTrack(Track track, string album)
        {
            if (!this.albums.ContainsKey(album))
                this.albums[album] = new Dictionary<string, Track>();

            this.albums[album].Add(track.Title, track);
            track.Album = album;
            this.rePlayer.Add(track.Id, track);
        }

        public bool Contains(Track track)
        {
            return this.rePlayer.ContainsKey(track.Id);
        }

        public IEnumerable<Track> GetAlbum(string albumName)
        {
            if (!this.albums.ContainsKey(albumName))
                throw new ArgumentException();

            return this.albums[albumName].Values
                .OrderByDescending(x => x.Plays);
        }

        public Dictionary<string, List<Track>> GetDiscography(string artistName)
        {
            List<Track> tracks = this.rePlayer.Values
                .Where(x => x.Artist == artistName)
                .ToList();

            if (tracks.Count == 0)
                throw new ArgumentException();

            Dictionary<string, List<Track>> result =
                new Dictionary<string, List<Track>>();
            
            foreach (var item in tracks)
            {
                if (!result.ContainsKey(item.Album))
                    result[item.Album] = new List<Track>();

                result[item.Album].Add(item);
            }

            return result;
        }
                
        public Track GetTrack(string title, string albumName)
        {
            if (!this.albums.ContainsKey(albumName)
                || !this.albums[albumName].ContainsKey(title))
                throw new ArgumentException();

            return this.albums[albumName][title];
        }

        public IEnumerable<Track> 
            GetTracksInDurationRangeOrderedByDurationThenByPlaysDescending(int lowerBound, int upperBound)
        {
            return this.rePlayer.Values
                .Where(x => x.DurationInSeconds >= lowerBound
                && x.DurationInSeconds <= upperBound)
                .OrderBy(x => x.DurationInSeconds)
                .ThenByDescending(x => x.Plays);
        }

        public IEnumerable<Track> 
            GetTracksOrderedByAlbumNameThenByPlaysDescendingThenByDurationDescending()
        {
            return this.rePlayer.Values
                .OrderBy(x => x.Album)
                .ThenByDescending(x => x.Plays)
                .ThenByDescending(x => x.DurationInSeconds);
        }

        public Track Play()
        {
            if (this.listeningQueue.Count == 0)
                throw new ArgumentException();

            Track track = this.listeningQueue.Dequeue();
            track.Plays++;
            return track;
        }

        public void RemoveTrack(string trackTitle, string albumName)
        {
            if (!this.albums.ContainsKey(albumName)
                || !this.albums[albumName].ContainsKey(trackTitle))
                throw new ArgumentException();

            Track track = this.albums[albumName][trackTitle];

            this.rePlayer.Remove(track.Id);
            this.albums[albumName].Remove(trackTitle);

            Queue<Track> temp = new Queue<Track>();

            this.listeningQueue
                .Where(x => x.Id != track.Id)
                .ToList()
                .ForEach(x => temp.Enqueue(x));

            this.listeningQueue = temp;
        }
    }
}
