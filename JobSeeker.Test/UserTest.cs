using System;
using FluentAssertions;
using JobSeeker.Domain;
using Xunit;

namespace JobSeeker.Test
{
    public class UserTest
    {
        private const string JohnDoe = "John Doe";
        private const string JohnDoeUserName = "jdoe";
        private const string JohnDoeEmail = "jdoe@gmail.com";
        private const string BlankName = " ";
        private const string JaneDoe = "Jane Doe";
        private const string JaneDoeUserName = "jadoe";
        private const string BlankUsername = " ";
        
        [Fact]
        private void CannotBeCreatedWithBlankName()
        {
            Action act = () => User.Named(JohnDoeUserName, BlankName, JohnDoeEmail);
            act.Should().Throw<CandidateException>()
                .WithMessage(User.CandidateNameCannotBeBlank);
        }

        [Fact]
        private void CannotBeCreatedWithBlankUsername()
        {
            Action act = () => User.Named(BlankUsername, JohnDoeEmail, " ");
            act.Should().Throw<CandidateException>()
                .WithMessage(User.CandidateUsernameCannotBeBlank);
        }
        
        [Fact]
        private void CannotBeCreatedWithBlankEmail()
        {
            Action act = () => User.Named(JohnDoeUserName, JohnDoe, " ");
            act.Should().Throw<CandidateException>()
                .WithMessage(User.CandidateEmailCannotBeBlank);
        }

        [Fact]
        private void CanBeCreatedWithNonBlankUsernameAndName()
        {
            User candidate = JohnDoeCandidate();
            candidate.IsNamed(JohnDoe).Should().BeTrue();
        }

        [Fact]
        private void KnowsItsUserName()
        {
            User candidate = JohnDoeCandidate();
            candidate.HasUserName(JohnDoeUserName).Should().BeTrue();
        }

        [Fact]
        private void KnowIfItsNotHerUsername()
        {
            User candidate = JohnDoeCandidate();
            candidate.HasUserName(JaneDoeUserName).Should().BeFalse();
        }

        [Fact]
        void IsNamedReturnsFalseWhenAskedWithOtherName()
        {
            User candidate = JohnDoeCandidate();
            candidate.IsNamed(JaneDoe).Should().BeFalse();
        }
        
        private static User JohnDoeCandidate()
        {
            return User.Named(JohnDoeUserName, JohnDoe, JohnDoeEmail);
        }
    }
}