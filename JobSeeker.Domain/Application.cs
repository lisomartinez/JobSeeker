using System;
using System.Collections.Generic;
using System.Linq;

namespace JobSeeker.Domain
{
    public sealed class Application : IEquatable<Application>
    {
        public const string CanNotAddACommentBeforeApplication = "Can not add a comment before application";

        public const string CannotAddACommentBeforeTheExistingLastOne =
            "Cannot add a comment before the existing last one";

        public const string PositionCannotBeBlank = "Position cannot be blank";
        public const string CompanyCannotBeBlank = "Company cannot be blank";
        private readonly SortedDictionary<DateTime, List<string>> _commentsByDay;

        private Application(ApplicationBuilder builder)
        {
            Position = builder.Position;
            Company = builder.Company;
            Date = builder.Date;
            Description = builder.Description;
            _commentsByDay = new SortedDictionary<DateTime, List<string>>();
        }

        public DateTime Date { get; }
        public string Description { get; }

        public string Position { get; }

        public string Company { get; }

        public bool Equals(Application? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Position, other.Position, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Company, other.Company, StringComparison.InvariantCultureIgnoreCase);
        }

        public static ApplicationBuilder Builder()
        {
            return new ApplicationBuilder();
        }

        public bool HasComments()
        {
            return _commentsByDay.Count != 0;
        }

        internal void AddComment(string content, DateTime date)
        {
            AssertCommentIsNotBeforeApplicationDate(date);
            AssertCommentIsNotBeforeTheLastOne(date);

            AddCommentToDate(content, date);
        }

        private void AssertCommentIsNotBeforeTheLastOne(DateTime date)
        {
            if (_commentsByDay.Count > 0 && _commentsByDay.Last().Key > date)
            {
                throw new JobApplicationException(CannotAddACommentBeforeTheExistingLastOne);
            }
        }

        private void AddCommentToDate(string content, DateTime date)
        {
            var addedCommentToNewDate = _commentsByDay.TryAdd(date, new List<string> {content});
            if (!addedCommentToNewDate)
            {
                _commentsByDay[date].Add(content);
            }
        }

        private void AssertCommentIsNotBeforeApplicationDate(in DateTime date)
        {
            if (date.Subtract(Date).Days < 0)
            {
                throw new JobApplicationException(CanNotAddACommentBeforeApplication);
            }
        }

        public int NumberOfComments()
        {
            return _commentsByDay.Values.Sum(comments => comments.Count);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Application) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Company);
        }

        public static bool operator ==(Application? left, Application? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Application? left, Application? right)
        {
            return !Equals(left, right);
        }

        public bool HasPosition(string applicationPosition)
        {
            return Position == applicationPosition;
        }

        public bool HasComment(string aComment)
        {
            return _commentsByDay.Values.Any(list => list.Contains(aComment));
        }

        public class ApplicationBuilder
        {
            public ApplicationBuilder()
            {
            }

            public string Position { get; private set; } = "";
            public string Company { get; private set; } = "";
            public DateTime Date { get; private set; } = DateTime.Today;
            public string Description { get; private set; } = "";

            public ApplicationBuilder WithPosition(string position)
            {
                Position = position;
                return this;
            }

            public ApplicationBuilder WithCompany(string company)
            {
                Company = company;
                return this;
            }

            public ApplicationBuilder WithDate(DateTime date)
            {
                Date = date;
                return this;
            }

            public ApplicationBuilder WithDescription(string description)
            {
                Description = description;
                return this;
            }

            public Application Build()
            {
                AssertPositionIsNotBlank(Position);
                AssertCompanyIsNotBlank(Company);
                return new Application(this);
            }

            private static void AssertCompanyIsNotBlank(string company)
            {
                if (string.IsNullOrWhiteSpace(company))
                {
                    throw new JobApplicationException(CompanyCannotBeBlank);
                }
            }

            private static void AssertPositionIsNotBlank(string position)
            {
                if (string.IsNullOrWhiteSpace(position))
                {
                    throw new JobApplicationException(PositionCannotBeBlank);
                }
            }
        }
    }
}