using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace IinaChristmas.Models
{
    [DebuggerDisplay("ID: {Id}, Count = {_children.Count}")]
    [JsonObject]
    public class PinkieEvent : IEnumerable<PinkieLine>
    {        
        public Guid Id { get; private set; }

        [JsonProperty]
        private List<PinkieLine> _children;

        public PinkieEvent()
        {
            Id = Guid.NewGuid();
            _children = new List<PinkieLine>();
        }

        public PinkieEvent(Guid id)
        {
            Id = id;
            _children = new List<PinkieLine>();
        }

        public void Add(string imagePath, string text, int millisecondsTime)
        {
            _children.Add(new PinkieLine
            {
                ImagePath = imagePath,
                Text = text,
                MillisecondsTime = millisecondsTime,
            });
        }
        
        public IEnumerator<PinkieLine> GetEnumerator() => _children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [DebuggerDisplay("{Text}, {MillisecondsTime}ms")]
    public class PinkieLine
    {
        public string ImagePath { get; set; }
        public string Text { get; set; }
        public int MillisecondsTime { get; set; }
    }
}
