using System;
using JobSeeker.Domain;

namespace JobSeeker.Test
{
    public class TestObjectFactory
    {
       public const string JohnDoe = "John Doe";
       public const string JaneDoe = "Jane Doe";
       public const string JaneDoeUserName = "jaDoe";
       public const string JohnDoeUserName = "jdoe";
       public const string JohnDoeEmail = "jdoe@gmail.com";
       public const string Position = "Java";
       public const string Company = "Accenture";
       public const string Description = "Description";
       public const string OtherPosition = ".Net";
       public static readonly DateTime Date = new DateTime(2010, 10, 1);
       public const string ADescription = "a description";
       public const string BlankString = " ";
       public const string JohnDoePassword = "Secure.4.Password";
       

       public static Candidate JohnDoeCandidate()
        {
            var johnDoe = CreateJohnDoeUser();
            Candidate candidate = Candidate.With(johnDoe);
            return candidate;
        }

        public static User CreateJohnDoeUser()
        {
            User johnDoe = User.Named(JohnDoeUserName, JohnDoe, JohnDoeEmail);
            return johnDoe;
        }
        
        public static Application CreateJavaAtAccentureApplication()
        {
            return Application.Of(TestObjectFactory.Position, TestObjectFactory.Company, TestObjectFactory.Date,
                TestObjectFactory.Description);
        }
    }
}