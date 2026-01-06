using School.Models.Website;
using School_DTOs;
using School_DTOs.Website;

namespace School.Services.Interfaces
{
    public interface IWebsiteService
    {
        // Slider Images
        Task<APIResponse<SliderImageDto>> AddSliderImageAsync(SliderImageModel model);
        Task<APIResponse<IEnumerable<SliderImageDto>>> GetAllSliderImagesAsync();
        Task<APIResponse> UpdateSliderImageAsync(SliderImageModel model);
        Task<APIResponse> DeleteSliderImageAsync(int id, string updatedBy);
        Task<APIResponse> UpdateSliderOrderAsync(List<int> sliderIds, string updatedBy);

        // Hero Section
        Task<APIResponse<HeroSectionDto>> GetHeroSectionAsync();
        Task<APIResponse<HeroSectionDto>> SaveHeroSectionAsync(HeroSectionModel model);

        // Notice Bar
        Task<APIResponse<NoticeBarDto>> GetNoticeBarAsync();
        Task<APIResponse<NoticeBarDto>> SaveNoticeBarAsync(NoticeBarModel model);

        // Welcome Section
        Task<APIResponse<WelcomeSectionDto>> GetWelcomeSectionAsync();
        Task<APIResponse<WelcomeSectionDto>> SaveWelcomeSectionAsync(WelcomeSectionModel model);

        // About Section (Home Page)
        Task<APIResponse<AboutSectionDto>> GetAboutSectionAsync();
        Task<APIResponse<AboutSectionDto>> SaveAboutSectionAsync(AboutSectionModel model);

        // About Page
        Task<APIResponse<AboutPageDto>> GetAboutPageAsync();
        Task<APIResponse<AboutPageDto>> SaveAboutPageAsync(AboutPageModel model);

        // Contact Info
        Task<APIResponse<ContactInfoDto>> GetContactInfoAsync();
        Task<APIResponse<ContactInfoDto>> SaveContactInfoAsync(ContactInfoModel model);

        // Gallery Images
        Task<APIResponse<GalleryImageDto>> AddGalleryImageAsync(GalleryImageModel model);
        Task<APIResponse<IEnumerable<GalleryImageDto>>> GetAllGalleryImagesAsync();
        Task<APIResponse> DeleteGalleryImageAsync(int id);

        // Achievements
        Task<APIResponse<AchievementDto>> AddAchievementAsync(AchievementModel model);
        Task<APIResponse<IEnumerable<AchievementDto>>> GetAllAchievementsAsync();
        Task<APIResponse> UpdateAchievementAsync(AchievementModel model);
        Task<APIResponse> DeleteAchievementAsync(int id, string updatedBy);

        // Team Members
        Task<APIResponse<TeamMemberDto>> AddTeamMemberAsync(TeamMemberModel model);
        Task<APIResponse<IEnumerable<TeamMemberDto>>> GetAllTeamMembersAsync();
        Task<APIResponse> UpdateTeamMemberAsync(TeamMemberModel model);
        Task<APIResponse> DeleteTeamMemberAsync(int id, string updatedBy);
    }
}
