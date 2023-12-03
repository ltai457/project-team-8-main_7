using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Project_Authentication.Model;

namespace Project_Authentication.Data
{
    public class ProfileRepo : IProfileRepo
    {
        private readonly ProjectDBContext _dbcontext;
        public ProfileRepo(ProjectDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public Profile AddNewProfile(Profile profile)
        {
            // Retrieve the user from the database based on the UserID in the Comment
            User user = _dbcontext.Users.FirstOrDefault(u => u.UserID == profile.UserID);

            // Set the UserName property of the Comment to the Username of the User
            profile.UserName = user?.UserName;

            // Add the Comment to the DbContext and save changes
            EntityEntry<Profile> e = _dbcontext.Profiles.Add(profile);
            Profile c = e.Entity;
            _dbcontext.SaveChanges();

            return c;
        }

        public IEnumerable<Profile> SearchByInterest(string interest)
        {
            return _dbcontext.Profiles.Where(p => EF.Functions.Like(p.Interest, "%" + interest + "%"));
        }
        public IEnumerable<Profile> SearchBySemeseter(string semeseter)
        {
            return _dbcontext.Profiles.Where(p => EF.Functions.Like(p.Semester , "%" + semeseter+ "%"));
        }
        public IEnumerable<Profile> SearchByMainCodingLanguage(string language)
        {
            return _dbcontext.Profiles.Where(p => EF.Functions.Like(p.MainCoding_Language, "%" + language + "%"));       
        }
    }
}

