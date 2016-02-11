using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieGuess
{
    public class Clue
    {
        public string DisplayName { get; set; }
        public int SecondsToShow { get; set; }
        public string FieldId { get; set; }
        public string Value { get; set; }

        public Clue (string displayName, int secondsToShow, string fieldId, string value)
        {
            DisplayName = displayName;
            SecondsToShow = secondsToShow;
            FieldId = fieldId;
            Value = value;
        }
    }
}