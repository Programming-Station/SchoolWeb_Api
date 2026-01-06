using School.Domain.Website;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace School.Infrastructure.Repositories
{
    public class WebsiteRepository : IWebsiteRepository
    {
        private readonly SchoolDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public WebsiteRepository(
            SchoolDbContext context,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        #region Slider Images

        public async Task<SliderImage> AddSliderImageAsync(SliderImage entity)
        {
            _context.SliderImages.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<SliderImage>> GetAllSliderImagesAsync()
        {
            return await _context.SliderImages
                .Where(s => !s.IsDeleted && s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ThenBy(s => s.Id)
                .ToListAsync();
        }

        public async Task<SliderImage?> GetSliderImageByIdAsync(int id)
        {
            return await _context.SliderImages
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task UpdateSliderImageAsync(SliderImage entity)
        {
            _context.SliderImages.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteSliderImageAsync(SliderImage entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.UtcNow;
            _context.SliderImages.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<SliderImage>> GetSliderImagesByIdsAsync(List<int> ids)
        {
            return await _context.SliderImages
                .Where(s => ids.Contains(s.Id) && !s.IsDeleted)
                .ToListAsync();
        }

        #endregion

        #region Hero Section

        public async Task<HeroSection?> GetHeroSectionAsync()
        {
            return await _context.HeroSections
                .FirstOrDefaultAsync(h => !h.IsDeleted && h.IsActive);
        }

        public async Task<HeroSection?> GetHeroSectionByIdAsync(int id)
        {
            return await _context.HeroSections
                .FirstOrDefaultAsync(h => h.Id == id && !h.IsDeleted);
        }

        public async Task<HeroSection> AddHeroSectionAsync(HeroSection entity)
        {
            _context.HeroSections.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task UpdateHeroSectionAsync(HeroSection entity)
        {
            _context.HeroSections.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<HeroSection>> GetActiveHeroSectionsAsync()
        {
            return await _context.HeroSections
                .Where(h => !h.IsDeleted && h.IsActive)
                .ToListAsync();
        }

        #endregion

        #region Notice Bar

        public async Task<NoticeBar?> GetNoticeBarAsync()
        {
            return await _context.NoticeBars
                .FirstOrDefaultAsync(n => !n.IsDeleted && n.IsActive);
        }

        public async Task<NoticeBar?> GetNoticeBarByIdAsync(int id)
        {
            return await _context.NoticeBars
                .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted);
        }

        public async Task<NoticeBar> AddNoticeBarAsync(NoticeBar entity)
        {
            _context.NoticeBars.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task UpdateNoticeBarAsync(NoticeBar entity)
        {
            _context.NoticeBars.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<NoticeBar>> GetActiveNoticeBarsAsync()
        {
            return await _context.NoticeBars
                .Where(n => !n.IsDeleted && n.IsActive)
                .ToListAsync();
        }

        #endregion

        #region Welcome Section

        public async Task<WelcomeSection?> GetWelcomeSectionAsync()
        {
            return await _context.WelcomeSections
                .FirstOrDefaultAsync(w => !w.IsDeleted && w.IsActive);
        }

        public async Task<WelcomeSection?> GetWelcomeSectionByIdAsync(int id)
        {
            return await _context.WelcomeSections
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
        }

        public async Task<WelcomeSection> AddWelcomeSectionAsync(WelcomeSection entity)
        {
            _context.WelcomeSections.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task UpdateWelcomeSectionAsync(WelcomeSection entity)
        {
            _context.WelcomeSections.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<WelcomeSection>> GetActiveWelcomeSectionsAsync()
        {
            return await _context.WelcomeSections
                .Where(w => !w.IsDeleted && w.IsActive)
                .ToListAsync();
        }

        #endregion

        #region About Section

        public async Task<AboutSection?> GetAboutSectionAsync()
        {
            return await _context.AboutSections
                .FirstOrDefaultAsync(a => !a.IsDeleted && a.IsActive);
        }

        public async Task<AboutSection?> GetAboutSectionByIdAsync(int id)
        {
            return await _context.AboutSections
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }

        public async Task<AboutSection> AddAboutSectionAsync(AboutSection entity)
        {
            _context.AboutSections.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task UpdateAboutSectionAsync(AboutSection entity)
        {
            _context.AboutSections.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<AboutSection>> GetActiveAboutSectionsAsync()
        {
            return await _context.AboutSections
                .Where(a => !a.IsDeleted && a.IsActive)
                .ToListAsync();
        }

        #endregion

        #region About Page

        public async Task<AboutPage?> GetAboutPageAsync()
        {
            return await _context.AboutPages
                .FirstOrDefaultAsync(a => !a.IsDeleted && a.IsActive);
        }

        public async Task<AboutPage?> GetAboutPageByIdAsync(int id)
        {
            return await _context.AboutPages
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }

        public async Task<AboutPage> AddAboutPageAsync(AboutPage entity)
        {
            _context.AboutPages.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task UpdateAboutPageAsync(AboutPage entity)
        {
            _context.AboutPages.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<AboutPage>> GetActiveAboutPagesAsync()
        {
            return await _context.AboutPages
                .Where(a => !a.IsDeleted && a.IsActive)
                .ToListAsync();
        }

        #endregion

        #region Contact Info

        public async Task<ContactInfo?> GetContactInfoAsync()
        {
            return await _context.ContactInfos
                .FirstOrDefaultAsync(c => !c.IsDeleted && c.IsActive);
        }

        public async Task<ContactInfo?> GetContactInfoByIdAsync(int id)
        {
            return await _context.ContactInfos
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<ContactInfo> AddContactInfoAsync(ContactInfo entity)
        {
            _context.ContactInfos.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task UpdateContactInfoAsync(ContactInfo entity)
        {
            _context.ContactInfos.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<ContactInfo>> GetActiveContactInfosAsync()
        {
            return await _context.ContactInfos
                .Where(c => !c.IsDeleted && c.IsActive)
                .ToListAsync();
        }

        #endregion

        #region Gallery Images

        public async Task<GalleryImage> AddGalleryImageAsync(GalleryImage entity)
        {
            _context.GalleryImages.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<GalleryImage>> GetAllGalleryImagesAsync()
        {
            return await _context.GalleryImages
                .Where(g => !g.IsDeleted && g.IsActive)
                .OrderBy(g => g.DisplayOrder)
                .ThenBy(g => g.Id)
                .ToListAsync();
        }

        public async Task<GalleryImage?> GetGalleryImageByIdAsync(int id)
        {
            return await _context.GalleryImages
                .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);
        }

        public async Task DeleteGalleryImageAsync(GalleryImage entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.UtcNow;
            _context.GalleryImages.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        #endregion

        #region Achievements

        public async Task<Achievement> AddAchievementAsync(Achievement entity)
        {
            _context.Achievements.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<Achievement>> GetAllAchievementsAsync()
        {
            return await _context.Achievements
                .Where(a => !a.IsDeleted && a.IsActive)
                .OrderBy(a => a.DisplayOrder)
                .ThenBy(a => a.Id)
                .ToListAsync();
        }

        public async Task<Achievement?> GetAchievementByIdAsync(int id)
        {
            return await _context.Achievements
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }

        public async Task UpdateAchievementAsync(Achievement entity)
        {
            _context.Achievements.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAchievementAsync(Achievement entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.UtcNow;
            _context.Achievements.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        #endregion

        #region Team Members

        public async Task<TeamMember> AddTeamMemberAsync(TeamMember entity)
        {
            _context.TeamMembers.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<TeamMember>> GetAllTeamMembersAsync()
        {
            return await _context.TeamMembers
                .Where(t => !t.IsDeleted && t.IsActive)
                .OrderBy(t => t.DisplayOrder)
                .ThenBy(t => t.Id)
                .ToListAsync();
        }

        public async Task<TeamMember?> GetTeamMemberByIdAsync(int id)
        {
            return await _context.TeamMembers
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task UpdateTeamMemberAsync(TeamMember entity)
        {
            _context.TeamMembers.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteTeamMemberAsync(TeamMember entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.UtcNow;
            _context.TeamMembers.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        #endregion
    }
}
