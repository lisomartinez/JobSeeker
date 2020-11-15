using System;
using System.Collections.Generic;
using System.Linq;

namespace JobSeeker.Domain
{
    public class Candidate
    {
        private readonly string _name;
        private readonly Dictionary<string, HashSet<Application>> positionByCompany;
        public const string CandidateNameCannotBeBlank = "Candidate name cannot be blank";
        public const string CannotApplyToAnAlreadyAppliedJob = "Cannot apply to an already applied job";
        public const string CannotAddACommentToNonExistingJob = "Cannot add a comment to non existing job";

        private Candidate(string name)
        {
            _name = name;
            positionByCompany = new Dictionary<string, HashSet<Application>>();
        }

        public static Candidate Named(string name)
        {
            AssertNameIsNotBlank(name);
            return new Candidate(name);
        }

        private static void AssertNameIsNotBlank(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new CandidateException(CandidateNameCannotBeBlank);
            }
        }

        public bool IsNamed(string aName)
        {
            return _name == aName;
        }

        public bool HasAppliedToJobs()
        {
            return positionByCompany.Count != 0;
        }

        public void ApplyToJob(string position, string company, string description)
        {
            AssertHasNotAlreadyAppliedTo(position, company);
            AddPositionToCompany(position, company, description);
        }

        private void AddPositionToCompany(string position, string company, string description)
        {
            var application = Application.Of(position, company, DateTime.Today, description);

            var hasAppliedToCompany = positionByCompany.ContainsKey(application.Company);
            if (hasAppliedToCompany)
            {
                positionByCompany[application.Company].Add(application);
            }
            else
            {
                positionByCompany[application.Company] = new HashSet<Application> {application};
            }
        }

        public bool HasAppliedToJob(string position, string company)
        {
            var doesCompanyExists = positionByCompany.TryGetValue(company, out var positions);
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
            return positionByCompany.Values.Sum(set => set.Count);
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
            return positionByCompany[company].First(pos => pos.HasPosition(position));
        }

        public bool ApplicationHasComment(string position, string company, string aComment)
        {
            return HasAppliedToJob(position, company) && GetApplication(position, company).HasComment(aComment);
        }
    }
}