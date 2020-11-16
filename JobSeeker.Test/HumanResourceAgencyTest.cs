using System;
using System.Collections.Generic;
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
            agency.ApplyToJob(JohnDoeUserName, Position, Company, Description);
            agency.CandidateHasAppliedTo(JohnDoeUserName, Position, Company).Should().BeTrue();
            agency.NumberOfUserApplications(JohnDoeUserName).Should().Be(1);
        }

        [Fact]
        private void NoRegisteredUserCannotApplyToJobs()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();

            agency.Invoking(a => a.ApplyToJob(JohnDoeUserName, Position, Company, Description))
                .Should()
                .Throw<HumanResourcesAgencyException>()
                .WithMessage(HumanResourcesAgency.NotRegisteredUserCannotOperate);

            agency.CandidateHasAppliedTo(JohnDoeUserName, Position, Company).Should().BeFalse();
            agency.NumberOfUserApplications(JohnDoeUserName).Should().Be(0);
        }

        [Fact]
        private void CandidateCanApplyToMoreThanOneJobInSameCompany()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();
            agency.Register(JohnDoeUserName, JohnDoePassword,
                JohnDoeEmail, JohnDoe);
            agency.ApplyToJob(JohnDoeUserName, Position, Company, Description);
            agency.ApplyToJob(JohnDoeUserName, OtherPosition, Company, Description);
            agency.CandidateHasAppliedTo(JohnDoeUserName, Position, Company).Should().BeTrue();
            agency.CandidateHasAppliedTo(JohnDoeUserName, OtherPosition, Company).Should().BeTrue();
            agency.NumberOfUserApplications(JohnDoeUserName).Should().Be(2);
        }

        [Fact]
        private void CandidateCanApplyToJobsInDifferentCompanies()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();
            agency.Register(JohnDoeUserName, JohnDoePassword,
                JohnDoeEmail, JohnDoe);
            agency.ApplyToJob(JohnDoeUserName, Position, Company, Description);
            agency.ApplyToJob(JohnDoeUserName, Position, OtherCompany, Description);
            agency.CandidateHasAppliedTo(JohnDoeUserName, Position, Company).Should().BeTrue();
            agency.CandidateHasAppliedTo(JohnDoeUserName, Position, OtherCompany).Should().BeTrue();
            agency.NumberOfUserApplications(JohnDoeUserName).Should().Be(2);
        }

        [Fact]
        private void CanGetCollectionOfRegisteredCandidatesApplications()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();
            Application first = CreateJavaAtAccentureApplication();
            Application second = CreateJavaAtGlobantApplication();
            
            agency.Register(JohnDoeUserName, JohnDoePassword,
                JohnDoeEmail, JohnDoe);
            agency.ApplyToJob(JohnDoeUserName, Position, Company, Description);
            agency.ApplyToJob(JohnDoeUserName, Position, OtherCompany, Description);
            
            List<Application> applications = agency.ApplicationsOf(JohnDoeUserName);
            applications.Count.Should().Be(2);
            applications.Should().ContainInOrder(first, second);
        }

        [Fact]
        private void CannotGetApplicationsOfNotRegisteredCandidates()
        {
            HumanResourcesAgency agency = new HumanResourcesAgency();
            agency.Invoking(a => a.ApplicationsOf(JohnDoeUserName))
                .Should()
                .Throw<HumanResourcesAgencyException>()
                .WithMessage(HumanResourcesAgency.NotRegisteredUserCannotOperate);
        }
    }
}