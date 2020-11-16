using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace JobSeeker.Domain
{
    public class Candidate
    {
        public const string CannotApplyToAnAlreadyAppliedJob = "Cannot apply to an already applied job";
        public const string CannotAddACommentToNonExistingJob = "Cannot add a comment to non existing job";
        private readonly Dictionary<string, HashSet<Application>> _positionByCompany;
        private readonly User _user;

        public List<Application> Applications => _positionByCompany
            .Values
            .SelectMany(set => set.AsEnumerable())
            .ToList();


        public static Candidate With(User user)
        {
            return new Candidate(user);
        }

        private Candidate(User user)
        {
            _user = user;
            _positionByCompany = new Dictionary<string, HashSet<Application>>();
        }

        public bool HasAppliedToJobs()
        {
            return _positionByCompany.Count != 0;
        }

        public void ApplyToJob(string position, string company, string description)
        {
            AssertHasNotAlreadyAppliedTo(position, company);
            AddPositionToCompany(position, company, description);
        }

        private void AddPositionToCompany(string position, string company, string description)
        {
            var positions = _positionByCompany.GetValueOrDefault(company, new HashSet<Application>());
            var application = Application.Builder()
                .WithPosition(position)
                .WithCompany(company)
                .WithDate(DateTime.Today)
                .WithDescription(description)
                .Build();
            positions.Add(application);
            _positionByCompany[application.Company] = positions;
        }

        public bool HasAppliedToJob(string position, string company)
        {
            var doesCompanyExists = _positionByCompany.TryGetValue(company, out var positions);
            return doesCompanyExists && ApplicationsForCompanyContainsPosition(position, positions!);
        }

        private static bool ApplicationsForCompanyContainsPosition(string position, HashSet<Application> positions)
        {
            return positions!.Count(application => application.HasPosition(position)) != 0;
        }

        private void AssertHasNotAlreadyAppliedTo(string position, string company)
        {
            if (HasAppliedToJob(position, company))
            {
                throw new CandidateException(CannotApplyToAnAlreadyAppliedJob);
            }
        }

        public int NumberOfApplications()
        {
            return _positionByCompany.Values.Sum(set => set.Count);
        }

        public void CommentApplication(string position, string company, string comment)
        {
            AssertHasAppliedToJob(position, company);
            GetApplication(position, company).AddComment(comment, DateTime.Today);
        }

        private void AssertHasAppliedToJob(string position, string company)
        {
            if (!HasAppliedToJob(position, company))
            {
                throw new CandidateException(CannotAddACommentToNonExistingJob);
            }
        }

        private Application GetApplication(string position, string company)
        {
            return _positionByCompany[company].First(pos => pos.HasPosition(position));
        }

        public bool ApplicationHasComment(string position, string company, string aComment)
        {
            return HasAppliedToJob(position, company) && GetApplication(position, company).HasComment(aComment);
        }
    }
}