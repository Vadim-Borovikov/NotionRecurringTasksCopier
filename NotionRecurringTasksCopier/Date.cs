using System;
using System.Globalization;

namespace NotionRecurringTasksCopier
{
    internal sealed class Date
    {
        private DateTime _day;
        private DateTime? _time;

        public Date(string source)
        {
            if (!DateTime.TryParseExact(source, DayFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out _day))
            {
                _time = DateTime.Parse(source, styles: DateTimeStyles.RoundtripKind);
                _day = _time.Value.Date;
            }
        }

        public DateTime Day
        {
            get => _day;
            set
            {
                _day = value;
                if (_time.HasValue)
                {
                    _time += _day - _time.Value.Date;
                }
            }
        }

        public override string ToString() => _time?.ToString("O") ?? _day.ToString(DayFormat);

        private const string DayFormat = "yyyy-MM-dd";
    }
}
