using System;
using FluentAssertions;
using JobSeeker.Domain;
using Xunit;

namespace JobSeeker.Test
{
    public sealed class CandidateTest
    {
        private const string JohnDoe = "John Doe";
        private const string BlankName = " ";
        private const string JaneDoe = "Jane Doe";
        private const string Position = "Java";
        private const string Company = "Accenture";
        private const string Description = "Description";
        private const string OtherPosition = ".Net";

        [Fact]
        private void CannotBeCreatedWithBlankName()
        {
            Action act = () => Candidate.Named(BlankName);
            act.Should().Throw<CandidateException>()
                .WithMessage(Candidate.CandidateNameCannotBeBlank);
        }

        [Fact]
        private void CanBeCreatedWithNonBlankName()
        {
            Candidate candidate = Candidate.Named(JohnDoe);
            candidate.IsNamed(JohnDoe).Should().BeTrue();
        }

        [Fact]
        void IsNamedReturnsFalseWhenAskedWithOtherName()
        {
            Candidate candidate = Candidate.Named(JohnDoe);
            candidate.IsNamed(JaneDoe).Should().BeFalse();
        }

        [Fact]
        private void WhenCreatedHaveNotAppliedJobs()
        {
            Candidate candidate = Candidate.Named(JohnDoe);
            candidate.HasAppliedToJobs().Should().BeFalse();
        }

        [Fact]
        private void CanApplyToJobs()
        {
            Candidate candidate = Candidate.Named(JohnDoe);
            candidate.ApplyToJob(Position, Company, Description);

            candidate.HasAppliedToJobs().Should().BeTrue();
            candidate.HasAppliedToJob(Position, Company).Should().BeTrue();
            candidate.NumberOfApplications().Should().Be(1);
        }

        [Fact]
        private void KnowsJobsApplied()
        {
            Candidate candidate = Candidate.Named(JohnDoe);
            candidate.HasAppliedToJobs().Should().BeFalse();
            candidate.HasAppliedToJob(Position, Company).Should().BeFalse();
        }
        
        
        [Fact]
        private void CannotApplyToJobMoreThanOnce()
        {
            Candidate candidate = Candidate.Named(JohnDoe);
            candidate.ApplyToJob(Position, Company, Description);

            candidate.Invoking(c => c.ApplyToJob(Position, Company, Description))
                .Should().Throw<CandidateException>()
                .WithMessage(Candidate.CannotApplyToAnAlreadyAppliedJob);
            candidate.NumberOfApplications().Should().Be(1);
        }

        [Fact]
        private void CanApplyToMultipleJobInSameCompany()
        {
            Candidate candidate = Candidate.Named(JohnDoe);
            candidate.ApplyToJob(Position, Company, Description);
            candidate.ApplyToJob(OtherPosition, Company, Description);
            candidate.NumberOfApplications().Should().Be(2);
            candidate.HasAppliedToJob(Position, Company);
            candidate.HasAppliedToJob(OtherPosition, Company);
        }

        [Fact]
        private void CanAddCommentsToAppliedJobs()
        {
            Candidate candidate = Candidate.Named(JohnDoe);
            candidate.ApplyToJob(Position, Company, Description);
            candidate.CommentApplication(Position, Company, "A comment");
            candidate.ApplicationHasComment(Position, Company, "A comment").Should().BeTrue();
        }

        [Fact]
        private void CannnotCommentOnNotAppliedJobCompany()
        {
            Candidate candidate = Candidate.Named(JohnDoe);
            candidate.Invoking(c => c.CommentApplication(Position, Company, "A comment"))
                .Should()
                .Throw<CandidateException>()
                .WithMessage(Candidate.CannotAddACommentToNonExistingJob);
            candidate.ApplicationHasComment(Position, Company, "A comment").Should().BeFalse();
        }

        [Fact]
        private void CannnotCommentOnNotAppliedJobPosition()
        {
            Candidate candidate = Candidate.Named(JohnDoe);
            candidate.ApplyToJob(Position, Company, Description);

            candidate.Invoking(c => c.CommentApplication(OtherPosition, Company, "A comment"))
                .Should()
                .Throw<CandidateException>()
                .WithMessage(Candidate.CannotAddACommentToNonExistingJob);
            candidate.ApplicationHasComment(Position, Company, "A comment").Should().BeFalse();
        }
    }
}