using Project_Authentication.Model;

namespace Project_Authentication.Data
{
    public interface IProfileRepo
    {
        public Profile AddNewProfile(Profile profile);
        public IEnumerable <Profile> SearchByInterest (string interest);  
        public IEnumerable<Profile> SearchBySemeseter (string semeseter);
        public IEnumerable<Profile> SearchByMainCodingLanguage (string language);
    }
}
