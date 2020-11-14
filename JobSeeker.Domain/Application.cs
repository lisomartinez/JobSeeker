using System;
using System.Collections.Generic;
using System.Linq;

namespace JobSeeker.Domain
{
    public  class Application : IEquatable<Application>
    {
        public const string CanNotAddACommentBeforeApplication = "Can not add a comment before application";

        public const string CannotAddACommentBeforeTheExistingLastOne =
            "Cannot add a comment before the existing last one";

        public const string PositionCannotBeBlank = "Position cannot be blank";
        public const string CompanyCannotBeBlank = "Company cannot be blank";

        private readonly string _position;
        public string Company { get; }
        private readonly DateTime _date;
        private readonly string _description;
        private readonly SortedDictionary<DateTime, List<string>> _commentsByDay;


        private Application(string position, string company, in DateTime date, string description)
        {
            _position = position;
            Company = company;
            _date = date;
            _description = description;
            _commentsByDay = new SortedDictionary<DateTime, List<string>>();
        }

        public static Application Of(string position, string company, DateTime date, string description = "")
        {
            AssertPositionIsNotBlank(position);
            AssertCompanyIsNotBlank(company);

            return new Application(position, company, date, description);
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

        public bool HasComments()
        {
            return _commentsByDay.Count != 0;
        }

        public void AddComment(string content, DateTime date)
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
            if (date.Subtract(_date).Days < 0)
            {
                throw new JobApplicationException(CanNotAddACommentBeforeApplication);
            }
        }

        public int NumberOfComments()
        {
            return _commentsByDay.Values.Sum(comments => comments.Count);
        }

        public bool Equals(Application? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_position, other._position, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(Company, other.Company, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Application) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_position, Company);
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
            return _position == applicationPosition;
        }

        public bool HasComment(string aComment)
        {
            return _commentsByDay.Values.Count(list => list.Contains(aComment)) != 0;
        }
    }
}