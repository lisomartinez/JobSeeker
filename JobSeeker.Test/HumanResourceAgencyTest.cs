using System;
using FluentAssertions;
using JobSeeker.Domain;
using Xunit;
using static JobSeeker.Test.TestObjectFactory;

namespace JobSeeker.Test
{
    public class HumanResourceAgencyTest
    {
        [Fact]
        private void WhenCreatedHaveNotRegisteredUser()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();
            agency.HasUsers().Should().BeFalse();
        }

        [Fact]
        private void CanRegisterUsers()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();
            agency.Register(JohnDoeUserName, JohnDoePassword,
                JohnDoeEmail, JohnDoe);
            agency.HasUsers().Should().BeTrue();
            agency.HasRegisteredUser(JohnDoeUserName).Should().BeTrue();
            agency.NumberOfUsers().Should().Be(1);
        }

        [Fact]
        private void KnowItsRegisteredUsers()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();
            agency.Register(JohnDoeUserName, JohnDoePassword,
                JohnDoeEmail, JohnDoe);
            agency.HasUsers().Should().BeTrue();
            agency.HasRegisteredUser(JaneDoeUserName).Should().BeFalse();
            agency.NumberOfUsers().Should().Be(1);
        }

        [Fact]
        private void CanRegisterMultipleUsers()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();
            agency.Register(JohnDoeUserName, JohnDoePassword,
                JohnDoeEmail, JohnDoe);
            agency.Register("otherusername", JohnDoePassword, JohnDoeEmail,
                JohnDoe);
            agency.HasUsers().Should().BeTrue();
            agency.HasRegisteredUser(JohnDoeUserName).Should().BeTrue();
            agency.HasRegisteredUser("otherusername").Should().BeTrue();
            agency.NumberOfUsers().Should().Be(2);
        }

        [Fact]
        private void CannotRegisterUserTwice()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();
            agency.Register(JohnDoeUserName, JohnDoePassword,
                JohnDoeEmail, JohnDoe);
            agency.Invoking(a => agency.Register(JohnDoeUserName, JohnDoePassword,
                    JohnDoeEmail, JohnDoe))
                .Should()
                .Throw<HumanResourcesAgencyException>()
                .WithMessage(HumanResourcesAgency.CannotRegisterUserMoreThanOnce);
            agency.HasRegisteredUser(JohnDoeUserName).Should().BeTrue();
            agency.NumberOfUsers().Should().Be(1);
        }

        [Fact]
        private void CanLoginRegisteredUser()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();
            User johnDoe = CreateJohnDoeUser();
            agency.Register(JohnDoeUserName, JohnDoePassword,
                JohnDoeEmail, JohnDoe);
            bool logged = false;
            agency.DoOnLogin(
                JohnDoeUserName, JohnDoePassword,
                (user) => logged = true,
                () => logged = false);

            logged.Should().BeTrue();
        }

        [Fact]
        private void CannotLoginUnregisteredUser()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();

            agency.Invoking(a => a.DoOnLogin<User>(
                    JohnDoeUserName, JohnDoePassword,
                    user => CreateJohnDoeUser(),
                    () => throw new HumanResourcesAgencyException(HumanResourcesAgency.CannotLoginUserNotRegistered)))
                .Should()
                .Throw<HumanResourcesAgencyException>()
                .WithMessage(HumanResourcesAgency.CannotLoginUserNotRegistered);
        }

        [Fact]
        private void KnowsApplicationOfEachCandidate()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();
            agency.Register(JohnDoeUserName, JohnDoePassword,
                JohnDoeEmail, JohnDoe);
            // agency.ApplyToJob(JohnDoeUserName, Position, Company, Description);
        }
    }
}