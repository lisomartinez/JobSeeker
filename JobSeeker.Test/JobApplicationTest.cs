using System;
using FluentAssertions;
using JobSeeker.Domain;
using Xunit;
using static JobSeeker.Test.TestObjectFactory;

namespace JobSeeker.Test
{
    public sealed class JobApplicationTest
    {
        [Fact]
        private void WhenCreatedDoesNotHaveComments()
        {
            Application application = CreateJavaAtAccentureApplication();
            application.HasComments().Should().BeFalse();
        }

        [Fact]
        private void CanAddComments()
        {
            Application application = CreateJavaAtAccentureApplication();
            application.AddComment("a comment", new DateTime(2010, 10, 2));
            application.HasComments().Should().BeTrue();
            application.NumberOfComments().Should().Be(1);
        }

        [Fact]
        private void CannotAddCommentBeforeApplicationDate()
        {
            Application application = CreateJavaAtAccentureApplication();
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
            Application application = CreateJavaAtAccentureApplication();
            application.AddComment("a comment", new DateTime(2010, 10, 2));
            application.AddComment("other comment", new DateTime(2010, 10, 3));
            application.HasComments().Should().BeTrue();
            application.NumberOfComments().Should().Be(2);
        }

        [Fact]
        private void CanAddMultipleCommentsSameDay()
        {
            Application application = CreateJavaAtAccentureApplication();
            application.AddComment("a comment", new DateTime(2010, 10, 2));
            application.AddComment("other comment", new DateTime(2010, 10, 2));
            application.HasComments().Should().BeTrue();
            application.NumberOfComments().Should().Be(2);
        }

        [Fact]
        private void CannotAddCommentBeforeTheExistinsLastOne()
        {
            Application application = CreateJavaAtAccentureApplication();
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
            Action act = () => Application.Builder()
                .WithPosition(BlankString)
                .WithCompany(Company)
                .WithDate(Date)
                .WithDescription(ADescription)
                .Build();
            act
                .Should()
                .Throw<JobApplicationException>()
                .WithMessage(Application.PositionCannotBeBlank);
        }

        [Fact]
        private void CompanyCannotBeBlank()
        {
            Action act = () =>
                Application.Builder()
                    .WithPosition(Position)
                    .WithCompany(BlankString)
                    .WithDate(Date)
                    .WithDescription(ADescription)
                    .Build();
            act
                .Should()
                .Throw<JobApplicationException>()
                .WithMessage(Application.CompanyCannotBeBlank);
        }

        [Fact]
        private void EqualsComparesPositionCompanyCaseInsensitive()
        {
            Application firstApplication = CreateJavaAtAccentureApplication();
            Application secondApplication =
                Application.Builder()
                    .WithPosition(Position.ToLower())
                    .WithCompany(Company.ToLower())
                    .WithDate(Date.AddDays(1))
                    .WithDescription(ADescription)
                    .Build();

            firstApplication.Equals(secondApplication).Should().BeTrue();
        }
    }
}