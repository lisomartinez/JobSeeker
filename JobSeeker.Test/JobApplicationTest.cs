using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace JobSeeker.Test
{
    public sealed class JobApplicationTest
    {
        private static readonly DateTime Date = new DateTime(2010, 10, 1);
        private const string Position = "Java";
        private const string Company = "Accenture";
        private const string ADescription = "a description";
        private const string BlankString = " ";

        [Fact]
        private void WhenCreatedDoesNotHaveComments()
        {
            Application application = Application.Of(Position, Company, Date, ADescription);
            application.HasComments().Should().BeFalse();
        }

        [Fact]
        private void CanAddComments()
        {
            Application application = Application.Of(Position, Company, Date, ADescription);
            application.AddComment("a comment", new DateTime(2010, 10, 2));
            application.HasComments().Should().BeTrue();
            application.NumberOfComments().Should().Be(1);
        }

        [Fact]
        private void CannotAddCommentBeforeApplicationDate()
        {
            Application application = Application.Of(Position, Company, Date, ADescription);
            application
                .Invoking(app => app.AddComment("a comment", new DateTime(2010, 9, 30)))
                .Should().Throw<JobApplicationException>()
                .WithMessage(Application.CanNotAddACommentBeforeApplication);
            application.HasComments().Should().BeFalse();
            application.NumberOfComments().Should().Be(0);
        }

        [Fact]
        private void CanAddMultipleComments()
        {
            Application application = Application.Of(Position, Company, Date, ADescription);
            application.AddComment("a comment", new DateTime(2010, 10, 2));
            application.AddComment("other comment", new DateTime(2010, 10, 3));
            application.HasComments().Should().BeTrue();
            application.NumberOfComments().Should().Be(2);
        }

        [Fact]
        private void CanAddMultipleCommentsSameDay()
        {
            Application application = Application.Of(Position, Company, Date, ADescription);
            application.AddComment("a comment", new DateTime(2010, 10, 2));
            application.AddComment("other comment", new DateTime(2010, 10, 2));
            application.HasComments().Should().BeTrue();
            application.NumberOfComments().Should().Be(2);
        }

        [Fact]
        private void CannotAddCommentBeforeTheExistinsLastOne()
        {
            Application application = Application.Of(Position, Company, Date, ADescription);
            application.AddComment("a comment", new DateTime(2010, 10, 2));

            application.Invoking(app => app.AddComment("other comment", Date))
                .Should()
                .Throw<JobApplicationException>()
                .WithMessage(Application.CannotAddACommentBeforeTheExistingLastOne);

            application.HasComments().Should().BeTrue();
            application.NumberOfComments().Should().Be(1);
        }

        [Fact]
        private void PositionCannotBeBlank()
        {
            Action act = () => Application.Of(BlankString, Company, Date, ADescription);
            act
                .Should()
                .Throw<JobApplicationException>()
                .WithMessage(Application.PositionCannotBeBlank);
        }

        [Fact]
        private void CompanyCannotBeBlank()
        {
            Action act = () => Application.Of(Position, BlankString, Date, ADescription);
            act
                .Should()
                .Throw<JobApplicationException>()
                .WithMessage(Application.CompanyCannotBeBlank);
        }

        [Fact]
        private void EqualsComparesPositionCompanyCaseInsensitive()
        {
            Application firstApplication = Application.Of(Position, Company, Date, ADescription);
            Application secondApplication =
                Application.Of(Position.ToLower(), Company.ToLower(), Date.AddDays(1), ADescription);
            firstApplication.Equals(secondApplication).Should().BeTrue();
        }
    }

    internal class JobApplicationException : Exception
    {
        public JobApplicationException(string message) : base(message)
        {
        }
    }

    internal class Application : IEquatable<Application>
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