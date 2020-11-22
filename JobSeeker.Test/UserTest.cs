using System;
using FluentAssertions;
using JobSeeker.Domain;
using Xunit;

namespace JobSeeker.Test
{
    public class UserTest
    {
        [Fact]
        private void CannotBeCreatedWithBlankUsername()
        {
            Action act = () => User.Named(TestObjectFactory.BlankUsername);
            act.Should().Throw<UserException>()
                .WithMessage(User.CandidateUsernameCannotBeBlank);
        }

        [Fact]
        private void KnowsItsUserName()
        {
            User candidate = JohnDoeCandidate();
            candidate.HasUserName(TestObjectFactory.JohnDoeUserName).Should().BeTrue();
        }

        [Fact]
        private void KnowIfItsNotHerUsername()
        {
            User candidate = JohnDoeCandidate();
            candidate.HasUserName(TestObjectFactory.JaneDoeUserName).Should().BeFalse();
        }

        [Fact]
        private void UsernameHasGuidFormat()
        {
            Action act = () => User.Named("no guid username");
            act
                .Should()
                .Throw<UserException>()
                .WithMessage(User.CannotHaveNotGuidUsernameFormat);
        }

        private static User JohnDoeCandidate()
        {
            return User.Named(TestObjectFactory.JohnDoeUserName);
        }
    }
}