using System;
using System.Configuration;
using FluentAssertions;
using JobSeeker.Domain;
using Xunit;
using static JobSeeker.Test.TestObjectFactory;

namespace JobSeeker.Test
{
    public sealed class CandidateTest
    {
        [Fact]
        private void WhenCreatedHaveNotAppliedJobs()
        {
            Candidate candidate = JohnDoeCandidate();
            candidate.HasAppliedToJobs().Should().BeFalse();
        }

        [Fact]
        private void CanApplyToJobs()
        {
            Candidate candidate = JohnDoeCandidate();
            candidate.ApplyToJob(Position, Company, Description);

            candidate.HasAppliedToJobs().Should().BeTrue();
            candidate.HasAppliedToJob(Position, Company).Should().BeTrue();
            candidate.NumberOfApplications().Should().Be(1);
        }

        [Fact]
        private void KnowsJobsApplied()
        {
            Candidate candidate = JohnDoeCandidate();
            candidate.HasAppliedToJobs().Should().BeFalse();
            candidate.HasAppliedToJob(Position, Company).Should().BeFalse();
        }


        [Fact]
        private void CannotApplyToJobMoreThanOnce()
        {
            Candidate candidate = JohnDoeCandidate();
            candidate.ApplyToJob(Position, Company, Description);

            candidate.Invoking(c =>
                    c.ApplyToJob(Position, Company, Description))
                .Should().Throw<CandidateException>()
                .WithMessage(Candidate.CannotApplyToAnAlreadyAppliedJob);
            candidate.NumberOfApplications().Should().Be(1);
        }

        [Fact]
        private void CanApplyToMultipleJobInSameCompany()
        {
            Candidate candidate = JohnDoeCandidate();
            candidate.ApplyToJob(Position, Company, Description);
            candidate.ApplyToJob(OtherPosition, Company,
                Description);

            candidate.NumberOfApplications().Should().Be(2);
            candidate.HasAppliedToJob(Position, Company).Should().BeTrue();
            candidate.HasAppliedToJob(OtherPosition, Company).Should().BeTrue();
        }

        [Fact]
        private void CanAddCommentsToAppliedJobs()
        {
            Candidate candidate = JohnDoeCandidate();
            candidate.ApplyToJob(Position, Company, Description);
            candidate.CommentApplication(Position, Company, "A comment");
            candidate.ApplicationHasComment(Position, Company, "A comment").Should()
                .BeTrue();
        }

        [Fact]
        private void CannnotCommentOnNotAppliedJobCompany()
        {
            Candidate candidate = JohnDoeCandidate();
            candidate.Invoking(c =>
                    c.CommentApplication(Position, Company, "A comment"))
                .Should()
                .Throw<CandidateException>()
                .WithMessage(Candidate.CannotAddACommentToNonExistingJob);
            candidate.ApplicationHasComment(Position, Company, "A comment").Should()
                .BeFalse();
        }

        [Fact]
        private void CannnotCommentOnNotAppliedJobPosition()
        {
            Candidate candidate = JohnDoeCandidate();
            candidate.ApplyToJob(Position, Company, Description);

            candidate.Invoking(c =>
                    c.CommentApplication(OtherPosition, Company, "A comment"))
                .Should()
                .Throw<CandidateException>()
                .WithMessage(Candidate.CannotAddACommentToNonExistingJob);
            candidate.ApplicationHasComment(Position, Company, "A comment").Should()
                .BeFalse();
        }
    }
}