using System;
using FluentAssertions;
using Xunit;

namespace JobSeeker.Test
{
    public class CandidateTest
    {
        private const string JohnDoe = "John Doe";
        private const string BlankName = " ";
        private const string JaneDoe = "Jane Doe";

        [Fact]
        void CannotBeCreatedWithBlankName()
        {
            Action act = () => Candidate.Named(BlankName);
            act.Should().Throw<CandidateException>()
                .WithMessage(Candidate.CandidateNameCannotBeBlank);
        }

        [Fact]
        void CanBeCreatedWithNonBlankName()
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
    }
}