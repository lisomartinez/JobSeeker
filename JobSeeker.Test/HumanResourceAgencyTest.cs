using System.Collections.Generic;
using FluentAssertions;
using JobSeeker.Domain;
using Xunit;
using static JobSeeker.Test.TestObjectFactory;

namespace JobSeeker.Test
{
    public class HumanResourceAgencyTest
    {
        private readonly TransientCandidateFolder _candidateFolder;

        public HumanResourceAgencyTest()
        {
            _candidateFolder = new TransientCandidateFolder();
        }

        [Fact]
        private void WhenCreatedHaveNotRegisteredUser()
        {
            var agency = CreateHumanResourcesAgency();
            _candidateFolder.HasUsers().Should().BeFalse();
        }

        private HumanResourcesAgency CreateHumanResourcesAgency()
        {
            return new(_candidateFolder);
        }

        [Fact]
        private void CanRegisterUsers()
        {
            var agency = CreateHumanResourcesAgency();
            agency.Register(JohnDoeUserName);
            _candidateFolder.HasUsers().Should().BeTrue();
            _candidateFolder.HasRegisteredUser(JohnDoeUserName).Should().BeTrue();
            _candidateFolder.NumberOfUsers().Should().Be(1);
        }

        [Fact]
        private void KnowItsRegisteredUsers()
        {
            var agency = CreateHumanResourcesAgency();
            agency.Register(JohnDoeUserName);
            _candidateFolder.HasUsers().Should().BeTrue();
            _candidateFolder.HasRegisteredUser(JaneDoeUserName).Should().BeFalse();
            _candidateFolder.NumberOfUsers().Should().Be(1);
        }

        [Fact]
        private void CanRegisterMultipleUsers()
        {
            var agency = CreateHumanResourcesAgency();
            agency.Register(JohnDoeUserName);
            agency.Register(JaneDoeUserName);
            _candidateFolder.HasUsers().Should().BeTrue();
            _candidateFolder.HasRegisteredUser(JohnDoeUserName).Should().BeTrue();
            _candidateFolder.HasRegisteredUser(JaneDoeUserName).Should().BeTrue();
            _candidateFolder.NumberOfUsers().Should().Be(2);
        }

        [Fact]
        private void CannotRegisterUserTwice()
        {
            var agency = CreateHumanResourcesAgency();

            agency.Register(JohnDoeUserName);
            agency.Invoking(a => agency.Register(JohnDoeUserName))
                .Should()
                .Throw<HumanResourcesAgencyException>()
                .WithMessage(HumanResourcesAgency.CannotRegisterUserMoreThanOnce);
            _candidateFolder.HasRegisteredUser(JohnDoeUserName).Should().BeTrue();
            _candidateFolder.NumberOfUsers().Should().Be(1);
        }

        [Fact]
        private void KnowsApplicationOfEachCandidate()
        {
            var agency = CreateHumanResourcesAgency();
            agency.Register(JohnDoeUserName);
            agency.ApplyToJob(JohnDoeUserName, Position, Company, Description);
            agency.CandidateHasAppliedTo(JohnDoeUserName, Position, Company).Should().BeTrue();
            agency.NumberOfUserApplications(JohnDoeUserName).Should().Be(1);
        }

        [Fact]
        private void NoRegisteredUserCannotApplyToJobs()
        {
            var agency = CreateHumanResourcesAgency();

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
            var agency = CreateHumanResourcesAgency();
            agency.Register(JohnDoeUserName);
            agency.ApplyToJob(JohnDoeUserName, Position, Company, Description);
            agency.ApplyToJob(JohnDoeUserName, OtherPosition, Company, Description);
            agency.CandidateHasAppliedTo(JohnDoeUserName, Position, Company).Should().BeTrue();
            agency.CandidateHasAppliedTo(JohnDoeUserName, OtherPosition, Company).Should().BeTrue();
            agency.NumberOfUserApplications(JohnDoeUserName).Should().Be(2);
        }

        [Fact]
        private void CandidateCanApplyToJobsInDifferentCompanies()
        {
            var agency = CreateHumanResourcesAgency();
            agency.Register(JohnDoeUserName);
            agency.ApplyToJob(JohnDoeUserName, Position, Company, Description);
            agency.ApplyToJob(JohnDoeUserName, Position, OtherCompany, Description);
            agency.CandidateHasAppliedTo(JohnDoeUserName, Position, Company).Should().BeTrue();
            agency.CandidateHasAppliedTo(JohnDoeUserName, Position, OtherCompany).Should().BeTrue();
            agency.NumberOfUserApplications(JohnDoeUserName).Should().Be(2);
        }

        [Fact]
        private void CanGetCollectionOfRegisteredCandidatesApplications()
        {
            var agency = CreateHumanResourcesAgency();
            Application first = CreateJavaAtAccentureApplication();
            Application second = CreateJavaAtGlobantApplication();

            agency.Register(JohnDoeUserName);
            agency.ApplyToJob(JohnDoeUserName, Position, Company, Description);
            agency.ApplyToJob(JohnDoeUserName, Position, OtherCompany, Description);

            List<Application> applications = agency.ApplicationsOf(JohnDoeUserName);
            applications.Count.Should().Be(2);
            applications.Should().ContainInOrder(first, second);
        }

        [Fact]
        private void CannotGetApplicationsOfNotRegisteredCandidates()
        {
            var agency = CreateHumanResourcesAgency();
            agency.Invoking(a => a.ApplicationsOf(JohnDoeUserName))
                .Should()
                .Throw<HumanResourcesAgencyException>()
                .WithMessage(HumanResourcesAgency.NotRegisteredUserCannotOperate);
        }
    }
}