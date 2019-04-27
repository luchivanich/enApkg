using System;

namespace AnkiCards
{
    public class Card
    {
        int id { get; set; }
        int nid { get; set; }
        int did { get; set; }
        int ord { get; set; }
        int mod { get; set; }
        int usn { get; set; }
        int type { get; set; }
        int queue { get; set; }
        int due { get; set; }
        int ivl { get; set; }
        int factor { get; set; }
        int reps { get; set; }
        int lapses { get; set; }
        int left { get; set; }
        int odue { get; set; }
        int odid { get; set; }
        int flags { get; set; }
        string data { get; set; }
    }
}