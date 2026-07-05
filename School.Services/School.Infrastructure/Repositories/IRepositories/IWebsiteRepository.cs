using School.Domain.Website;

namespace School.Infrastructure.Repositories.IRepositories
{
    public interface IWebsiteRepository
    {
        Task<SliderImage> AddSliderImageAsync(SliderImage entity);
        Task<IEnumerable<SliderImage>> GetAllSliderImagesAsync();
        Task<SliderImage?> GetSliderImageByIdAsync(int id);
        Task UpdateSliderImageAsync(SliderImage entity);
        Task DeleteSliderImageAsync(SliderImage entity);
        Task<List<SliderImage>> GetSliderImagesByIdsAsync(List<int> ids);

        Task<HeroSection?> GetHeroSectionAsync();
        Task<HeroSection?> GetHeroSectionByIdAsync(int id);
        Task<HeroSection> AddHeroSectionAsync(HeroSection entity);
        Task UpdateHeroSectionAsync(HeroSection entity);
        Task<List<HeroSection>> GetActiveHeroSectionsAsync();

        Task<NoticeBar?> GetNoticeBarAsync();
        Task<NoticeBar?> GetNoticeBarByIdAsync(int id);
        Task<NoticeBar> AddNoticeBarAsync(NoticeBar entity);
        Task UpdateNoticeBarAsync(NoticeBar entity);
        Task<List<NoticeBar>> GetActiveNoticeBarsAsync();

        Task<WelcomeSection?> GetWelcomeSectionAsync();
        Task<WelcomeSection?> GetWelcomeSectionByIdAsync(int id);
        Task<WelcomeSection> AddWelcomeSectionAsync(WelcomeSection entity);
        Task UpdateWelcomeSectionAsync(WelcomeSection entity);
        Task<List<WelcomeSection>> GetActiveWelcomeSectionsAsync();

        Task<AboutSection?> GetAboutSectionAsync();
        Task<AboutSection?> GetAboutSectionByIdAsync(int id);
        Task<AboutSection> AddAboutSectionAsync(AboutSection entity);
        Task UpdateAboutSectionAsync(AboutSection entity);
        Task<List<AboutSection>> GetActiveAboutSectionsAsync();

        Task<AboutPage?> GetAboutPageAsync();
        Task<AboutPage?> GetAboutPageByIdAsync(int id);
        Task<AboutPage> AddAboutPageAsync(AboutPage entity);
        Task UpdateAboutPageAsync(AboutPage entity);
        Task<List<AboutPage>> GetActiveAboutPagesAsync();

        Task<ContactInfo?> GetContactInfoAsync();
        Task<ContactInfo?> GetContactInfoByIdAsync(int id);
        Task<ContactInfo> AddContactInfoAsync(ContactInfo entity);
        Task UpdateContactInfoAsync(ContactInfo entity);
        Task<List<ContactInfo>> GetActiveContactInfosAsync();

        Task<GalleryImage> AddGalleryImageAsync(GalleryImage entity);
        Task<IEnumerable<GalleryImage>> GetAllGalleryImagesAsync();
        Task<GalleryImage?> GetGalleryImageByIdAsync(int id);
        Task DeleteGalleryImageAsync(GalleryImage entity);

        Task<Achievement> AddAchievementAsync(Achievement entity);
        Task<IEnumerable<Achievement>> GetAllAchievementsAsync();
        Task<Achievement?> GetAchievementByIdAsync(int id);
        Task UpdateAchievementAsync(Achievement entity);
        Task DeleteAchievementAsync(Achievement entity);

        Task<TeamMember> AddTeamMemberAsync(TeamMember entity);
        Task<IEnumerable<TeamMember>> GetAllTeamMembersAsync();
        Task<TeamMember?> GetTeamMemberByIdAsync(int id);
        Task UpdateTeamMemberAsync(TeamMember entity);
        Task DeleteTeamMemberAsync(TeamMember entity);
    }
}
