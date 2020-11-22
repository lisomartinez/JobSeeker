using System;
using JobSeeker.Domain;

namespace JobSeeker.Test
{
    public class TestObjectFactory
    {
        public const string JohnDoe = "John Doe";
        public const string JaneDoe = "Jane Doe";
        public const string JaneDoeUserName = "1adc093d-e924-4efd-8d50-c6a4e3088a43";
        public const string JohnDoeUserName = "e584f085-52bc-496e-8afb-d23c95b8cb16";
        public const string JohnDoeEmail = "jdoe@gmail.com";
        public const string Position = "Java";
        public const string Company = "Accenture";
        public const string OtherCompany = "Globant";
        public const string Description = "Description";
        public const string OtherPosition = ".Net";
        public const string ADescription = "a description";
        public const string BlankString = " ";
        public const string JohnDoePassword = "Secure.4.Password";
        public const string BlankUsername = " ";
        public static readonly DateTime Date = new DateTime(2010, 10, 1);


        public static Candidate JohnDoeCandidate()
        {
            var johnDoe = CreateJohnDoeUser();
            Candidate candidate = Candidate.With(johnDoe);
            return candidate;
        }

        public static User CreateJohnDoeUser()
        {
            User johnDoe = User.Named(JohnDoeUserName);
            return johnDoe;
        }

        public static Application CreateJavaAtAccentureApplication()
        {
            return Application.Builder()
                .WithPosition(TestObjectFactory.Position)
                .WithCompany(TestObjectFactory.Company)
                .WithDate(TestObjectFactory.Date)
                .WithDescription(TestObjectFactory.Description)
                .Build();
        }

        public static Application CreateJavaAtGlobantApplication()
        {
            return Application.Builder()
                .WithPosition(TestObjectFactory.Position)
                .WithCompany(TestObjectFactory.OtherCompany)
                .WithDate(TestObjectFactory.Date)
                .WithDescription(TestObjectFactory.Description)
                .Build();
        }
    }
}