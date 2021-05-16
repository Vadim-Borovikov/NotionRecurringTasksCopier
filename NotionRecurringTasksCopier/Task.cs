using System;
using System.Collections.Generic;
using System.Linq;
using NotionRecurringTasksCopier.Dto;
using NotionRecurringTasksCopier.Dto.Properties;
using NotionRecurringTasksCopier.Dto.RichTexts;

namespace NotionRecurringTasksCopier
{
    internal sealed class Task
    {
        public string Name
        {
            get => _title.Content;
            set => _title.Content = value;
        }

        public DateProperty.Dates Date => GetProperty<DateProperty>(DateName).Date;

        public readonly List<string> Recurring;

        public bool Meeting => GetProperty<CheckboxProperty>(MeetingName).Checkbox;

        public List<RichText> Comment => GetProperty<RichTextProperty>(CommentName).Text;

        public Task(Page source)
        {
            _source = source;

            _title = GetTitle();

            Recurring = GetRecurring();
        }

        public Dictionary<string, Property> GetPageProperties() => _source.Properties;

        private RichTextText.TextType GetTitle()
        {
            var property = GetProperty<TitleProperty>(NameName);
            var text = property?.Title.SingleOrDefault() as RichTextText;
            RichTextText.TextType title = text?.Text;
            if (title == null)
            {
                throw new Exception();
            }
            return title;
        }

        private List<string> GetRecurring()
        {
            return GetProperty<MultiSelectProperty>(RecurringName).MultiSelect?.Select(o => o.Name).ToList();
        }

        private T GetProperty<T>(string name) where T : Property
        {
            // ReSharper disable once UseNullPropagationWhenPossible
            if (_source?.Properties == null)
            {
                throw new Exception();
            }
            if (!_source.Properties.ContainsKey(name))
            {
                throw new Exception();
            }
            if (_source.Properties[name] is T casted)
            {
                return casted;
            }
            throw new Exception();
        }

        private readonly Page _source;
        private readonly RichTextText.TextType _title;

        private const string NameName = "Задача";
        private const string DateName = "Дата";
        private const string RecurringName = "Повторяется";
        private const string MeetingName = "Встреча";
        private const string CommentName = "Комментарий";
    }
}
