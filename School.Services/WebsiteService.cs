using AutoMapper;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Website;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Website;
using System.Net;

namespace School.Services
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IWebsiteRepository _websiteRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public WebsiteService(IWebsiteRepository websiteRepository, IMapper mapper, IImageService imageService)
        {
            _websiteRepository = websiteRepository;
            _mapper = mapper;
            _imageService = imageService;
        }

        #region Slider Images

        public async Task<APIResponse<SliderImageDto>> AddSliderImageAsync(SliderImageModel model)
        {
            try
            {
                // Map Model to Entity using AutoMapper
                var entity = _mapper.Map<Domain.Website.SliderImage>(model);
                entity.CreatedDate = DateTime.UtcNow;
                entity.CreatedBy = model.CreatedBy;

                // Business Logic: Set display order if not provided
                if (entity.DisplayOrder == 0)
                {
                    var existingImages = await _websiteRepository.GetAllSliderImagesAsync();
                    entity.DisplayOrder = existingImages.Any() ? existingImages.Max(i => i.DisplayOrder) + 1 : 1;
                }

                var savedEntity = await _websiteRepository.AddSliderImageAsync(entity);

                // Map Entity to DTO using AutoMapper
                var dto = _mapper.Map<SliderImageDto>(savedEntity);

                return new APIResponse<SliderImageDto>
                {
                    Success = true,
                    Message = "Slider image added successfully",
                    StatusCode = HttpStatusCode.Created,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<SliderImageDto>
                {
                    Success = false,
                    Message = $"Failed to add slider image: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<SliderImageDto>>> GetAllSliderImagesAsync()
        {
            try
            {
                var entities = await _websiteRepository.GetAllSliderImagesAsync();
                var dtos = _mapper.Map<IEnumerable<SliderImageDto>>(entities);

                return new APIResponse<IEnumerable<SliderImageDto>>
                {
                    Success = true,
                    Message = "Slider images fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<SliderImageDto>>
                {
                    Success = false,
                    Message = $"Failed to get slider images: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> UpdateSliderImageAsync(SliderImageModel model)
        {
            try
            {
                if (!model.Id.HasValue)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Slider image ID is required",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var existingEntity = await _websiteRepository.GetSliderImageByIdAsync(model.Id.Value);
                if (existingEntity == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Slider image not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                // Map Model to Entity, preserving existing values
                _mapper.Map(model, existingEntity);
                existingEntity.UpdatedDate = DateTime.UtcNow;
                existingEntity.UpdatedBy = model.UpdatedBy;

                await _websiteRepository.UpdateSliderImageAsync(existingEntity);

                return new APIResponse
                {
                    Success = true,
                    Message = "Slider image updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to update slider image: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> DeleteSliderImageAsync(int id, string updatedBy)
        {
            try
            {
                var entity = await _websiteRepository.GetSliderImageByIdAsync(id);
                if (entity == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Slider image not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                // Delete image file from folder
                if (!string.IsNullOrEmpty(entity.ImageUrl))
                {
                    await _imageService.DeleteImageAsync(entity.ImageUrl);
                }

                entity.UpdatedBy = updatedBy;
                await _websiteRepository.DeleteSliderImageAsync(entity);

                return new APIResponse
                {
                    Success = true,
                    Message = "Slider image deleted successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to delete slider image: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> UpdateSliderOrderAsync(List<int> sliderIds, string updatedBy)
        {
            try
            {
                var entities = await _websiteRepository.GetSliderImagesByIdsAsync(sliderIds);
                
                // Business Logic: Update display order
                foreach (var entity in entities)
                {
                    entity.DisplayOrder = sliderIds.IndexOf(entity.Id);
                    entity.UpdatedDate = DateTime.UtcNow;
                    entity.UpdatedBy = updatedBy;
                    await _websiteRepository.UpdateSliderImageAsync(entity);
                }

                return new APIResponse
                {
                    Success = true,
                    Message = "Slider order updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to update slider order: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion

        #region Hero Section

        public async Task<APIResponse<HeroSectionDto>> GetHeroSectionAsync()
        {
            try
            {
                var entity = await _websiteRepository.GetHeroSectionAsync();
                if (entity == null)
                {
                    return new APIResponse<HeroSectionDto>
                    {
                        Success = false,
                        Message = "Hero section not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var dto = _mapper.Map<HeroSectionDto>(entity);
                return new APIResponse<HeroSectionDto>
                {
                    Success = true,
                    Message = "Hero section fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<HeroSectionDto>
                {
                    Success = false,
                    Message = $"Failed to get hero section: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<HeroSectionDto>> SaveHeroSectionAsync(HeroSectionModel model)
        {
            try
            {
                Domain.Website.HeroSection entity;
                List<Domain.Website.HeroSection> oldSections;

                if (model.Id.HasValue)
                {
                    entity = await _websiteRepository.GetHeroSectionByIdAsync(model.Id.Value);
                    if (entity == null)
                    {
                        return new APIResponse<HeroSectionDto>
                        {
                            Success = false,
                            Message = "Hero section not found",
                            StatusCode = HttpStatusCode.NotFound
                        };
                    }

                    _mapper.Map(model, entity);
                    entity.UpdatedDate = DateTime.UtcNow;
                    entity.UpdatedBy = model.UpdatedBy;
                    await _websiteRepository.UpdateHeroSectionAsync(entity);
                }
                else
                {
                    // Business Logic: Deactivate old sections
                    oldSections = await _websiteRepository.GetActiveHeroSectionsAsync();
                    foreach (var old in oldSections)
                    {
                        old.IsActive = false;
                        old.UpdatedDate = DateTime.UtcNow;
                        old.UpdatedBy = model.CreatedBy; // Set by the user creating new section
                        await _websiteRepository.UpdateHeroSectionAsync(old);
                    }

                    entity = _mapper.Map<Domain.Website.HeroSection>(model);
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.CreatedBy = model.CreatedBy;
                    entity = await _websiteRepository.AddHeroSectionAsync(entity);
                }

                var dto = _mapper.Map<HeroSectionDto>(entity);
                return new APIResponse<HeroSectionDto>
                {
                    Success = true,
                    Message = "Hero section saved successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<HeroSectionDto>
                {
                    Success = false,
                    Message = $"Failed to save hero section: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion

        #region Notice Bar

        public async Task<APIResponse<NoticeBarDto>> GetNoticeBarAsync()
        {
            try
            {
                var entity = await _websiteRepository.GetNoticeBarAsync();
                if (entity == null)
                {
                    return new APIResponse<NoticeBarDto>
                    {
                        Success = false,
                        Message = "Notice bar not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var dto = _mapper.Map<NoticeBarDto>(entity);
                return new APIResponse<NoticeBarDto>
                {
                    Success = true,
                    Message = "Notice bar fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<NoticeBarDto>
                {
                    Success = false,
                    Message = $"Failed to get notice bar: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<NoticeBarDto>> SaveNoticeBarAsync(NoticeBarModel model)
        {
            try
            {
                Domain.Website.NoticeBar entity;

                if (model.Id.HasValue)
                {
                    entity = await _websiteRepository.GetNoticeBarByIdAsync(model.Id.Value);
                    if (entity == null)
                    {
                        return new APIResponse<NoticeBarDto>
                        {
                            Success = false,
                            Message = "Notice bar not found",
                            StatusCode = HttpStatusCode.NotFound
                        };
                    }

                    _mapper.Map(model, entity);
                    entity.UpdatedDate = DateTime.UtcNow;
                    await _websiteRepository.UpdateNoticeBarAsync(entity);
                }
                else
                {
                    var oldBars = await _websiteRepository.GetActiveNoticeBarsAsync();
                    foreach (var old in oldBars)
                    {
                        old.IsActive = false;
                        old.UpdatedDate = DateTime.UtcNow;
                        old.UpdatedBy = model.CreatedBy;
                        await _websiteRepository.UpdateNoticeBarAsync(old);
                    }

                    entity = _mapper.Map<Domain.Website.NoticeBar>(model);
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.CreatedBy = model.CreatedBy;
                    entity = await _websiteRepository.AddNoticeBarAsync(entity);
                }

                var dto = _mapper.Map<NoticeBarDto>(entity);
                return new APIResponse<NoticeBarDto>
                {
                    Success = true,
                    Message = "Notice bar saved successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<NoticeBarDto>
                {
                    Success = false,
                    Message = $"Failed to save notice bar: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion

        #region Welcome Section

        public async Task<APIResponse<WelcomeSectionDto>> GetWelcomeSectionAsync()
        {
            try
            {
                var entity = await _websiteRepository.GetWelcomeSectionAsync();
                if (entity == null)
                {
                    return new APIResponse<WelcomeSectionDto>
                    {
                        Success = false,
                        Message = "Welcome section not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var dto = _mapper.Map<WelcomeSectionDto>(entity);
                return new APIResponse<WelcomeSectionDto>
                {
                    Success = true,
                    Message = "Welcome section fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<WelcomeSectionDto>
                {
                    Success = false,
                    Message = $"Failed to get welcome section: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<WelcomeSectionDto>> SaveWelcomeSectionAsync(WelcomeSectionModel model)
        {
            try
            {
                Domain.Website.WelcomeSection entity;

                if (model.Id.HasValue)
                {
                    entity = await _websiteRepository.GetWelcomeSectionByIdAsync(model.Id.Value);
                    if (entity == null)
                    {
                        return new APIResponse<WelcomeSectionDto>
                        {
                            Success = false,
                            Message = "Welcome section not found",
                            StatusCode = HttpStatusCode.NotFound
                        };
                    }

                    _mapper.Map(model, entity);
                    entity.UpdatedDate = DateTime.UtcNow;
                    entity.UpdatedBy = model.UpdatedBy;
                    await _websiteRepository.UpdateWelcomeSectionAsync(entity);
                }
                else
                {
                    var oldSections = await _websiteRepository.GetActiveWelcomeSectionsAsync();
                    foreach (var old in oldSections)
                    {
                        old.IsActive = false;
                        old.UpdatedDate = DateTime.UtcNow;
                        old.UpdatedBy = model.CreatedBy;
                        await _websiteRepository.UpdateWelcomeSectionAsync(old);
                    }

                    entity = _mapper.Map<Domain.Website.WelcomeSection>(model);
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.CreatedBy = model.CreatedBy;
                    entity = await _websiteRepository.AddWelcomeSectionAsync(entity);
                }

                var dto = _mapper.Map<WelcomeSectionDto>(entity);
                return new APIResponse<WelcomeSectionDto>
                {
                    Success = true,
                    Message = "Welcome section saved successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<WelcomeSectionDto>
                {
                    Success = false,
                    Message = $"Failed to save welcome section: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion

        #region About Section

        public async Task<APIResponse<AboutSectionDto>> GetAboutSectionAsync()
        {
            try
            {
                var entity = await _websiteRepository.GetAboutSectionAsync();
                if (entity == null)
                {
                    return new APIResponse<AboutSectionDto>
                    {
                        Success = false,
                        Message = "About section not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var dto = _mapper.Map<AboutSectionDto>(entity);
                return new APIResponse<AboutSectionDto>
                {
                    Success = true,
                    Message = "About section fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<AboutSectionDto>
                {
                    Success = false,
                    Message = $"Failed to get about section: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<AboutSectionDto>> SaveAboutSectionAsync(AboutSectionModel model)
        {
            try
            {
                Domain.Website.AboutSection entity;

                if (model.Id.HasValue)
                {
                    entity = await _websiteRepository.GetAboutSectionByIdAsync(model.Id.Value);
                    if (entity == null)
                    {
                        return new APIResponse<AboutSectionDto>
                        {
                            Success = false,
                            Message = "About section not found",
                            StatusCode = HttpStatusCode.NotFound
                        };
                    }

                    _mapper.Map(model, entity);
                    entity.UpdatedDate = DateTime.UtcNow;
                    entity.UpdatedBy = model.UpdatedBy;
                    await _websiteRepository.UpdateAboutSectionAsync(entity);
                }
                else
                {
                    var oldSections = await _websiteRepository.GetActiveAboutSectionsAsync();
                    foreach (var old in oldSections)
                    {
                        old.IsActive = false;
                        old.UpdatedDate = DateTime.UtcNow;
                        old.UpdatedBy = model.CreatedBy;
                        await _websiteRepository.UpdateAboutSectionAsync(old);
                    }

                    entity = _mapper.Map<Domain.Website.AboutSection>(model);
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.CreatedBy = model.CreatedBy;
                    entity = await _websiteRepository.AddAboutSectionAsync(entity);
                }

                var dto = _mapper.Map<AboutSectionDto>(entity);
                return new APIResponse<AboutSectionDto>
                {
                    Success = true,
                    Message = "About section saved successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<AboutSectionDto>
                {
                    Success = false,
                    Message = $"Failed to save about section: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion

        #region About Page

        public async Task<APIResponse<AboutPageDto>> GetAboutPageAsync()
        {
            try
            {
                var entity = await _websiteRepository.GetAboutPageAsync();
                if (entity == null)
                {
                    return new APIResponse<AboutPageDto>
                    {
                        Success = false,
                        Message = "About page not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var dto = _mapper.Map<AboutPageDto>(entity);
                return new APIResponse<AboutPageDto>
                {
                    Success = true,
                    Message = "About page fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<AboutPageDto>
                {
                    Success = false,
                    Message = $"Failed to get about page: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<AboutPageDto>> SaveAboutPageAsync(AboutPageModel model)
        {
            try
            {
                Domain.Website.AboutPage entity;

                if (model.Id.HasValue)
                {
                    entity = await _websiteRepository.GetAboutPageByIdAsync(model.Id.Value);
                    if (entity == null)
                    {
                        return new APIResponse<AboutPageDto>
                        {
                            Success = false,
                            Message = "About page not found",
                            StatusCode = HttpStatusCode.NotFound
                        };
                    }

                    _mapper.Map(model, entity);
                    entity.UpdatedDate = DateTime.UtcNow;
                    entity.UpdatedBy = model.UpdatedBy;
                    await _websiteRepository.UpdateAboutPageAsync(entity);
                }
                else
                {
                    var oldPages = await _websiteRepository.GetActiveAboutPagesAsync();
                    foreach (var old in oldPages)
                    {
                        old.IsActive = false;
                        old.UpdatedDate = DateTime.UtcNow;
                        old.UpdatedBy = model.CreatedBy;
                        await _websiteRepository.UpdateAboutPageAsync(old);
                    }

                    entity = _mapper.Map<Domain.Website.AboutPage>(model);
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.CreatedBy = model.CreatedBy;
                    entity = await _websiteRepository.AddAboutPageAsync(entity);
                }

                var dto = _mapper.Map<AboutPageDto>(entity);
                return new APIResponse<AboutPageDto>
                {
                    Success = true,
                    Message = "About page saved successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<AboutPageDto>
                {
                    Success = false,
                    Message = $"Failed to save about page: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion

        #region Contact Info

        public async Task<APIResponse<ContactInfoDto>> GetContactInfoAsync()
        {
            try
            {
                var entity = await _websiteRepository.GetContactInfoAsync();
                if (entity == null)
                {
                    return new APIResponse<ContactInfoDto>
                    {
                        Success = false,
                        Message = "Contact info not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var dto = _mapper.Map<ContactInfoDto>(entity);
                return new APIResponse<ContactInfoDto>
                {
                    Success = true,
                    Message = "Contact info fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<ContactInfoDto>
                {
                    Success = false,
                    Message = $"Failed to get contact info: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<ContactInfoDto>> SaveContactInfoAsync(ContactInfoModel model)
        {
            try
            {
                Domain.Website.ContactInfo entity;

                if (model.Id.HasValue)
                {
                    entity = await _websiteRepository.GetContactInfoByIdAsync(model.Id.Value);
                    if (entity == null)
                    {
                        return new APIResponse<ContactInfoDto>
                        {
                            Success = false,
                            Message = "Contact info not found",
                            StatusCode = HttpStatusCode.NotFound
                        };
                    }

                    _mapper.Map(model, entity);
                    entity.UpdatedDate = DateTime.UtcNow;
                    entity.UpdatedBy = model.UpdatedBy;
                    await _websiteRepository.UpdateContactInfoAsync(entity);
                }
                else
                {
                    var oldInfos = await _websiteRepository.GetActiveContactInfosAsync();
                    foreach (var old in oldInfos)
                    {
                        old.IsActive = false;
                        old.UpdatedDate = DateTime.UtcNow;
                        old.UpdatedBy = model.CreatedBy;
                        await _websiteRepository.UpdateContactInfoAsync(old);
                    }

                    entity = _mapper.Map<Domain.Website.ContactInfo>(model);
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.CreatedBy = model.CreatedBy;
                    entity = await _websiteRepository.AddContactInfoAsync(entity);
                }

                var dto = _mapper.Map<ContactInfoDto>(entity);
                return new APIResponse<ContactInfoDto>
                {
                    Success = true,
                    Message = "Contact info saved successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<ContactInfoDto>
                {
                    Success = false,
                    Message = $"Failed to save contact info: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion

        #region Gallery Images

        public async Task<APIResponse<GalleryImageDto>> AddGalleryImageAsync(GalleryImageModel model)
        {
            try
            {
                var entity = _mapper.Map<Domain.Website.GalleryImage>(model);
                entity.CreatedDate = DateTime.UtcNow;
                entity.CreatedBy = model.CreatedBy;

                if (entity.DisplayOrder == 0)
                {
                    var existingImages = await _websiteRepository.GetAllGalleryImagesAsync();
                    entity.DisplayOrder = existingImages.Any() ? existingImages.Max(i => i.DisplayOrder) + 1 : 1;
                }

                var savedEntity = await _websiteRepository.AddGalleryImageAsync(entity);
                var dto = _mapper.Map<GalleryImageDto>(savedEntity);

                return new APIResponse<GalleryImageDto>
                {
                    Success = true,
                    Message = "Gallery image added successfully",
                    StatusCode = HttpStatusCode.Created,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<GalleryImageDto>
                {
                    Success = false,
                    Message = $"Failed to add gallery image: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<GalleryImageDto>>> GetAllGalleryImagesAsync()
        {
            try
            {
                var entities = await _websiteRepository.GetAllGalleryImagesAsync();
                var dtos = _mapper.Map<IEnumerable<GalleryImageDto>>(entities);

                return new APIResponse<IEnumerable<GalleryImageDto>>
                {
                    Success = true,
                    Message = "Gallery images fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<GalleryImageDto>>
                {
                    Success = false,
                    Message = $"Failed to get gallery images: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> DeleteGalleryImageAsync(int id)
        {
            try
            {
                var entity = await _websiteRepository.GetGalleryImageByIdAsync(id);
                if (entity == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Gallery image not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                // Delete image file from folder
                if (!string.IsNullOrEmpty(entity.ImageUrl))
                {
                    await _imageService.DeleteImageAsync(entity.ImageUrl);
                }

                await _websiteRepository.DeleteGalleryImageAsync(entity);

                return new APIResponse
                {
                    Success = true,
                    Message = "Gallery image deleted successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to delete gallery image: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion

        #region Achievements

        public async Task<APIResponse<AchievementDto>> AddAchievementAsync(AchievementModel model)
        {
            try
            {
                var entity = _mapper.Map<Domain.Website.Achievement>(model);
                entity.CreatedDate = DateTime.UtcNow;
                entity.CreatedBy = model.CreatedBy;

                if (entity.DisplayOrder == 0)
                {
                    var existingAchievements = await _websiteRepository.GetAllAchievementsAsync();
                    entity.DisplayOrder = existingAchievements.Any() ? existingAchievements.Max(a => a.DisplayOrder) + 1 : 1;
                }

                var savedEntity = await _websiteRepository.AddAchievementAsync(entity);
                var dto = _mapper.Map<AchievementDto>(savedEntity);

                return new APIResponse<AchievementDto>
                {
                    Success = true,
                    Message = "Achievement added successfully",
                    StatusCode = HttpStatusCode.Created,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<AchievementDto>
                {
                    Success = false,
                    Message = $"Failed to add achievement: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<AchievementDto>>> GetAllAchievementsAsync()
        {
            try
            {
                var entities = await _websiteRepository.GetAllAchievementsAsync();
                var dtos = _mapper.Map<IEnumerable<AchievementDto>>(entities);

                return new APIResponse<IEnumerable<AchievementDto>>
                {
                    Success = true,
                    Message = "Achievements fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<AchievementDto>>
                {
                    Success = false,
                    Message = $"Failed to get achievements: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> UpdateAchievementAsync(AchievementModel model)
        {
            try
            {
                if (!model.Id.HasValue)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Achievement ID is required",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var entity = await _websiteRepository.GetAchievementByIdAsync(model.Id.Value);
                if (entity == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Achievement not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                _mapper.Map(model, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                entity.UpdatedBy = model.UpdatedBy;
                await _websiteRepository.UpdateAchievementAsync(entity);

                return new APIResponse
                {
                    Success = true,
                    Message = "Achievement updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to update achievement: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> DeleteAchievementAsync(int id, string updatedBy)
        {
            try
            {
                var entity = await _websiteRepository.GetAchievementByIdAsync(id);
                if (entity == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Achievement not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                entity.UpdatedBy = updatedBy;
                await _websiteRepository.DeleteAchievementAsync(entity);

                return new APIResponse
                {
                    Success = true,
                    Message = "Achievement deleted successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to delete achievement: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion

        #region Team Members

        public async Task<APIResponse<TeamMemberDto>> AddTeamMemberAsync(TeamMemberModel model)
        {
            try
            {
                var entity = _mapper.Map<Domain.Website.TeamMember>(model);
                entity.CreatedDate = DateTime.UtcNow;
                entity.CreatedBy = model.CreatedBy;

                if (entity.DisplayOrder == 0)
                {
                    var existingMembers = await _websiteRepository.GetAllTeamMembersAsync();
                    entity.DisplayOrder = existingMembers.Any() ? existingMembers.Max(t => t.DisplayOrder) + 1 : 1;
                }

                var savedEntity = await _websiteRepository.AddTeamMemberAsync(entity);
                var dto = _mapper.Map<TeamMemberDto>(savedEntity);

                return new APIResponse<TeamMemberDto>
                {
                    Success = true,
                    Message = "Team member added successfully",
                    StatusCode = HttpStatusCode.Created,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<TeamMemberDto>
                {
                    Success = false,
                    Message = $"Failed to add team member: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse<IEnumerable<TeamMemberDto>>> GetAllTeamMembersAsync()
        {
            try
            {
                var entities = await _websiteRepository.GetAllTeamMembersAsync();
                var dtos = _mapper.Map<IEnumerable<TeamMemberDto>>(entities);

                return new APIResponse<IEnumerable<TeamMemberDto>>
                {
                    Success = true,
                    Message = "Team members fetched successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<TeamMemberDto>>
                {
                    Success = false,
                    Message = $"Failed to get team members: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> UpdateTeamMemberAsync(TeamMemberModel model)
        {
            try
            {
                if (!model.Id.HasValue)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Team member ID is required",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var entity = await _websiteRepository.GetTeamMemberByIdAsync(model.Id.Value);
                if (entity == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Team member not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                _mapper.Map(model, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                entity.UpdatedBy = model.UpdatedBy;
                await _websiteRepository.UpdateTeamMemberAsync(entity);

                return new APIResponse
                {
                    Success = true,
                    Message = "Team member updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to update team member: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<APIResponse> DeleteTeamMemberAsync(int id, string updatedBy)
        {
            try
            {
                var entity = await _websiteRepository.GetTeamMemberByIdAsync(id);
                if (entity == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Team member not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                entity.UpdatedBy = updatedBy;
                await _websiteRepository.DeleteTeamMemberAsync(entity);

                return new APIResponse
                {
                    Success = true,
                    Message = "Team member deleted successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = $"Failed to delete team member: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        #endregion
    }
}
