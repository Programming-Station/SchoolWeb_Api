using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using QRCoder;
using School.Domain;
using School.Domain.FeeManagnment;
using School.Domain.School;
using School.Domain.Student;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.School;
using School.Models.Academic;
using School.Models.Student;
using School.Models.Fee;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.School;
using School_DTOs.Academic;
using School_DTOs.Student;
using School_DTOs.Fee;

namespace School.Services
{
    public class AdmissionService : IAdmissionService
    {
        private readonly ICampusRepository _campusRepository;
        private readonly IEducationLevelRepository _educationLevelRepository;
        private readonly IProgramRepository _programRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IYearSemesterRepository _yearSemesterRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IAdmissionFormConfigRepository _formConfigRepository;
        private readonly IAdmissionRuleRepository _ruleRepository;
        private readonly IAdmissionApplicationRepository _applicationRepository;
        private readonly IFeeStructureRepository _feeStructureRepository;
        private readonly IMapper _mapper;

        public AdmissionService(
            ICampusRepository campusRepository,
            IEducationLevelRepository educationLevelRepository,
            IProgramRepository programRepository,
            IBranchRepository branchRepository,
            IYearSemesterRepository yearSemesterRepository,
            IBatchRepository batchRepository,
            IAdmissionFormConfigRepository formConfigRepository,
            IAdmissionRuleRepository ruleRepository,
            IAdmissionApplicationRepository applicationRepository,
            IFeeStructureRepository feeStructureRepository,
            IMapper mapper)
        {
            _campusRepository = campusRepository;
            _educationLevelRepository = educationLevelRepository;
            _programRepository = programRepository;
            _branchRepository = branchRepository;
            _yearSemesterRepository = yearSemesterRepository;
            _batchRepository = batchRepository;
            _formConfigRepository = formConfigRepository;
            _ruleRepository = ruleRepository;
            _applicationRepository = applicationRepository;
            _feeStructureRepository = feeStructureRepository;
            _mapper = mapper;
        }

        #region Campus CRUD
        public async Task<APIResponse<CampusDto>> CreateCampusAsync(CampusModel model, string username)
        {
            var entity = _mapper.Map<Campus>(model);
            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.Now;
            entity = await _campusRepository.AddAsync(entity);
            return new APIResponse<CampusDto> { Success = true, Data = _mapper.Map<CampusDto>(entity), StatusCode = HttpStatusCode.Created, Message = CommonResource.AddSuccess };
        }

        public async Task<APIResponse<CampusDto>> GetCampusByIdAsync(int id)
        {
            var entity = await _campusRepository.GetByIdAsync(id);
            if (entity.Id == 0) return new APIResponse<CampusDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
            return new APIResponse<CampusDto> { Success = true, Data = _mapper.Map<CampusDto>(entity), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<IEnumerable<CampusDto>>> GetAllCampusesAsync(bool? isActive = null)
        {
            var list = await _campusRepository.GetAllAsync(isActive);
            return new APIResponse<IEnumerable<CampusDto>> { Success = true, Data = _mapper.Map<IEnumerable<CampusDto>>(list), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse> UpdateCampusAsync(CampusModel model, string username)
        {
            if (!model.Id.HasValue) return new APIResponse { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "Id is required for update" };
            var entity = await _campusRepository.GetByIdAsync(model.Id.Value);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            _mapper.Map(model, entity);
            entity.UpdatedBy = username;
            await _campusRepository.UpdateAsync(entity);
            return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.UpdateSuccess };
        }

        public async Task<APIResponse> DeleteCampusAsync(int id)
        {
            var res = await _campusRepository.DeleteAsync(id);
            if (res > 0) return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.DeleteSuccess };
            return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
        }
        #endregion

        #region Education Level CRUD
        public async Task<APIResponse<EducationLevelDto>> CreateEducationLevelAsync(EducationLevelModel model, string username)
        {
            var entity = _mapper.Map<EducationLevel>(model);
            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.Now;
            entity = await _educationLevelRepository.AddAsync(entity);
            return new APIResponse<EducationLevelDto> { Success = true, Data = _mapper.Map<EducationLevelDto>(entity), StatusCode = HttpStatusCode.Created, Message = CommonResource.AddSuccess };
        }

        public async Task<APIResponse<EducationLevelDto>> GetEducationLevelByIdAsync(int id)
        {
            var entity = await _educationLevelRepository.GetByIdAsync(id);
            if (entity.Id == 0) return new APIResponse<EducationLevelDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
            return new APIResponse<EducationLevelDto> { Success = true, Data = _mapper.Map<EducationLevelDto>(entity), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<IEnumerable<EducationLevelDto>>> GetAllEducationLevelsAsync(bool? isActive = null)
        {
            var list = await _educationLevelRepository.GetAllAsync(isActive);
            return new APIResponse<IEnumerable<EducationLevelDto>> { Success = true, Data = _mapper.Map<IEnumerable<EducationLevelDto>>(list), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse> UpdateEducationLevelAsync(EducationLevelModel model, string username)
        {
            if (!model.Id.HasValue) return new APIResponse { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "Id is required for update" };
            var entity = await _educationLevelRepository.GetByIdAsync(model.Id.Value);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            _mapper.Map(model, entity);
            entity.UpdatedBy = username;
            await _educationLevelRepository.UpdateAsync(entity);
            return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.UpdateSuccess };
        }

        public async Task<APIResponse> DeleteEducationLevelAsync(int id)
        {
            var res = await _educationLevelRepository.DeleteAsync(id);
            if (res > 0) return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.DeleteSuccess };
            return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
        }
        #endregion

        #region Program CRUD
        public async Task<APIResponse<ProgramDto>> CreateProgramAsync(ProgramModel model, string username)
        {
            var entity = _mapper.Map<Program>(model);
            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.Now;
            entity = await _programRepository.AddAsync(entity);
            var result = await _programRepository.GetByIdAsync(entity.Id);
            return new APIResponse<ProgramDto> { Success = true, Data = _mapper.Map<ProgramDto>(result), StatusCode = HttpStatusCode.Created, Message = CommonResource.AddSuccess };
        }

        public async Task<APIResponse<ProgramDto>> GetProgramByIdAsync(int id)
        {
            var entity = await _programRepository.GetByIdAsync(id);
            if (entity.Id == 0) return new APIResponse<ProgramDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
            return new APIResponse<ProgramDto> { Success = true, Data = _mapper.Map<ProgramDto>(entity), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<IEnumerable<ProgramDto>>> GetAllProgramsAsync(int? educationLevelId = null, bool? isActive = null)
        {
            var list = await _programRepository.GetAllAsync(educationLevelId, isActive);
            return new APIResponse<IEnumerable<ProgramDto>> { Success = true, Data = _mapper.Map<IEnumerable<ProgramDto>>(list), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse> UpdateProgramAsync(ProgramModel model, string username)
        {
            if (!model.Id.HasValue) return new APIResponse { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "Id is required" };
            var entity = await _programRepository.GetByIdAsync(model.Id.Value);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            _mapper.Map(model, entity);
            entity.UpdatedBy = username;
            await _programRepository.UpdateAsync(entity);
            return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.UpdateSuccess };
        }

        public async Task<APIResponse> DeleteProgramAsync(int id)
        {
            var res = await _programRepository.DeleteAsync(id);
            if (res > 0) return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.DeleteSuccess };
            return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
        }
        #endregion

        #region Branch CRUD
        public async Task<APIResponse<BranchDto>> CreateBranchAsync(BranchModel model, string username)
        {
            var entity = _mapper.Map<Branch>(model);
            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.Now;
            entity = await _branchRepository.AddAsync(entity);
            var result = await _branchRepository.GetByIdAsync(entity.Id);
            return new APIResponse<BranchDto> { Success = true, Data = _mapper.Map<BranchDto>(result), StatusCode = HttpStatusCode.Created, Message = CommonResource.AddSuccess };
        }

        public async Task<APIResponse<BranchDto>> GetBranchByIdAsync(int id)
        {
            var entity = await _branchRepository.GetByIdAsync(id);
            if (entity.Id == 0) return new APIResponse<BranchDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
            return new APIResponse<BranchDto> { Success = true, Data = _mapper.Map<BranchDto>(entity), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<IEnumerable<BranchDto>>> GetAllBranchesAsync(int? programId = null, bool? isActive = null)
        {
            var list = await _branchRepository.GetAllAsync(programId, isActive);
            return new APIResponse<IEnumerable<BranchDto>> { Success = true, Data = _mapper.Map<IEnumerable<BranchDto>>(list), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse> UpdateBranchAsync(BranchModel model, string username)
        {
            if (!model.Id.HasValue) return new APIResponse { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "Id is required" };
            var entity = await _branchRepository.GetByIdAsync(model.Id.Value);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            _mapper.Map(model, entity);
            entity.UpdatedBy = username;
            await _branchRepository.UpdateAsync(entity);
            return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.UpdateSuccess };
        }

        public async Task<APIResponse> DeleteBranchAsync(int id)
        {
            var res = await _branchRepository.DeleteAsync(id);
            if (res > 0) return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.DeleteSuccess };
            return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
        }
        #endregion

        #region Year/Semester CRUD
        public async Task<APIResponse<YearSemesterDto>> CreateYearSemesterAsync(YearSemesterModel model, string username)
        {
            var entity = _mapper.Map<YearSemester>(model);
            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.Now;
            entity = await _yearSemesterRepository.AddAsync(entity);
            return new APIResponse<YearSemesterDto> { Success = true, Data = _mapper.Map<YearSemesterDto>(entity), StatusCode = HttpStatusCode.Created, Message = CommonResource.AddSuccess };
        }

        public async Task<APIResponse<YearSemesterDto>> GetYearSemesterByIdAsync(int id)
        {
            var entity = await _yearSemesterRepository.GetByIdAsync(id);
            if (entity.Id == 0) return new APIResponse<YearSemesterDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
            return new APIResponse<YearSemesterDto> { Success = true, Data = _mapper.Map<YearSemesterDto>(entity), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<IEnumerable<YearSemesterDto>>> GetAllYearSemestersAsync(bool? isActive = null)
        {
            var list = await _yearSemesterRepository.GetAllAsync(isActive);
            return new APIResponse<IEnumerable<YearSemesterDto>> { Success = true, Data = _mapper.Map<IEnumerable<YearSemesterDto>>(list), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse> UpdateYearSemesterAsync(YearSemesterModel model, string username)
        {
            if (!model.Id.HasValue) return new APIResponse { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "Id is required" };
            var entity = await _yearSemesterRepository.GetByIdAsync(model.Id.Value);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            _mapper.Map(model, entity);
            entity.UpdatedBy = username;
            await _yearSemesterRepository.UpdateAsync(entity);
            return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.UpdateSuccess };
        }

        public async Task<APIResponse> DeleteYearSemesterAsync(int id)
        {
            var res = await _yearSemesterRepository.DeleteAsync(id);
            if (res > 0) return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.DeleteSuccess };
            return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
        }
        #endregion

        #region Batch CRUD
        public async Task<APIResponse<BatchDto>> CreateBatchAsync(BatchModel model, string username)
        {
            var entity = _mapper.Map<Batch>(model);
            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.Now;
            entity = await _batchRepository.AddAsync(entity);
            var result = await _batchRepository.GetByIdAsync(entity.Id);
            return new APIResponse<BatchDto> { Success = true, Data = _mapper.Map<BatchDto>(result), StatusCode = HttpStatusCode.Created, Message = CommonResource.AddSuccess };
        }

        public async Task<APIResponse<BatchDto>> GetBatchByIdAsync(int id)
        {
            var entity = await _batchRepository.GetByIdAsync(id);
            if (entity.Id == 0) return new APIResponse<BatchDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
            return new APIResponse<BatchDto> { Success = true, Data = _mapper.Map<BatchDto>(entity), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<IEnumerable<BatchDto>>> GetAllBatchesAsync(int? programId = null, bool? isActive = null)
        {
            var list = await _batchRepository.GetAllAsync(programId, isActive);
            return new APIResponse<IEnumerable<BatchDto>> { Success = true, Data = _mapper.Map<IEnumerable<BatchDto>>(list), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse> UpdateBatchAsync(BatchModel model, string username)
        {
            if (!model.Id.HasValue) return new APIResponse { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "Id is required" };
            var entity = await _batchRepository.GetByIdAsync(model.Id.Value);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            _mapper.Map(model, entity);
            entity.UpdatedBy = username;
            await _batchRepository.UpdateAsync(entity);
            return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.UpdateSuccess };
        }

        public async Task<APIResponse> DeleteBatchAsync(int id)
        {
            var res = await _batchRepository.DeleteAsync(id);
            if (res > 0) return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.DeleteSuccess };
            return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
        }
        #endregion

        #region Form Config CRUD
        public async Task<APIResponse<AdmissionFormConfigDto>> CreateFormConfigAsync(AdmissionFormConfigModel model, string username)
        {
            var entity = _mapper.Map<AdmissionFormConfig>(model);
            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.Now;
            entity = await _formConfigRepository.AddAsync(entity);
            var result = await _formConfigRepository.GetByIdAsync(entity.Id);
            return new APIResponse<AdmissionFormConfigDto> { Success = true, Data = _mapper.Map<AdmissionFormConfigDto>(result), StatusCode = HttpStatusCode.Created, Message = CommonResource.AddSuccess };
        }

        public async Task<APIResponse<AdmissionFormConfigDto>> GetFormConfigByIdAsync(int id)
        {
            var entity = await _formConfigRepository.GetByIdAsync(id);
            if (entity.Id == 0) return new APIResponse<AdmissionFormConfigDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
            return new APIResponse<AdmissionFormConfigDto> { Success = true, Data = _mapper.Map<AdmissionFormConfigDto>(entity), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<AdmissionFormConfigDto>> GetFormConfigAsync(int campusId, int educationLevelId, int? programId)
        {
            var config = await _formConfigRepository.GetConfigAsync(campusId, educationLevelId, programId);
            if (config == null) return new APIResponse<AdmissionFormConfigDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Admission configuration not found for selected campus/program." };
            return new APIResponse<AdmissionFormConfigDto> { Success = true, Data = _mapper.Map<AdmissionFormConfigDto>(config), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<IEnumerable<AdmissionFormConfigDto>>> GetAllFormConfigsAsync()
        {
            var list = await _formConfigRepository.GetAllAsync();
            return new APIResponse<IEnumerable<AdmissionFormConfigDto>> { Success = true, Data = _mapper.Map<IEnumerable<AdmissionFormConfigDto>>(list), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse> UpdateFormConfigAsync(AdmissionFormConfigModel model, string username)
        {
            if (!model.Id.HasValue) return new APIResponse { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "Id is required" };
            var entity = await _formConfigRepository.GetByIdAsync(model.Id.Value);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            _mapper.Map(model, entity);
            entity.UpdatedBy = username;
            await _formConfigRepository.UpdateAsync(entity);
            return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.UpdateSuccess };
        }

        public async Task<APIResponse> DeleteFormConfigAsync(int id)
        {
            var res = await _formConfigRepository.DeleteAsync(id);
            if (res > 0) return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.DeleteSuccess };
            return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
        }
        #endregion

        #region Rule CRUD
        public async Task<APIResponse<AdmissionRuleDto>> CreateRuleAsync(AdmissionRuleModel model, string username)
        {
            var entity = _mapper.Map<AdmissionRule>(model);
            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.Now;
            entity = await _ruleRepository.AddAsync(entity);
            var result = await _ruleRepository.GetByIdAsync(entity.Id);
            return new APIResponse<AdmissionRuleDto> { Success = true, Data = _mapper.Map<AdmissionRuleDto>(result), StatusCode = HttpStatusCode.Created, Message = CommonResource.AddSuccess };
        }

        public async Task<APIResponse<AdmissionRuleDto>> GetRuleByIdAsync(int id)
        {
            var entity = await _ruleRepository.GetByIdAsync(id);
            if (entity.Id == 0) return new APIResponse<AdmissionRuleDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
            return new APIResponse<AdmissionRuleDto> { Success = true, Data = _mapper.Map<AdmissionRuleDto>(entity), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<IEnumerable<AdmissionRuleDto>>> GetAllRulesAsync()
        {
            var list = await _ruleRepository.GetAllAsync();
            return new APIResponse<IEnumerable<AdmissionRuleDto>> { Success = true, Data = _mapper.Map<IEnumerable<AdmissionRuleDto>>(list), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse> UpdateRuleAsync(AdmissionRuleModel model, string username)
        {
            if (!model.Id.HasValue) return new APIResponse { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "Id is required" };
            var entity = await _ruleRepository.GetByIdAsync(model.Id.Value);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            _mapper.Map(model, entity);
            entity.UpdatedBy = username;
            await _ruleRepository.UpdateAsync(entity);
            return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.UpdateSuccess };
        }

        public async Task<APIResponse> DeleteRuleAsync(int id)
        {
            var res = await _ruleRepository.DeleteAsync(id);
            if (res > 0) return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.DeleteSuccess };
            return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
        }
        #endregion

        #region Fee Structure CRUD
        public async Task<APIResponse<FeeStructureDto>> CreateFeeStructureAsync(FeeStructureModel model, string username)
        {
            var entity = _mapper.Map<FeeStructure>(model);
            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.Now;
            
            entity = await _feeStructureRepository.AddAsync(entity);

            if (model.FeeStructureItems != null)
            {
                foreach (var item in model.FeeStructureItems)
                {
                    await _feeStructureRepository.AddFeeStructureItemAsync(new FeeStructureItem
                    {
                        FeeStructureId = entity.Id,
                        FeeTypeId = item.FeeTypeId,
                        Amount = item.Amount,
                        CreatedBy = username,
                        CreatedDate = DateTime.Now
                    });
                }
            }

            var result = await _feeStructureRepository.GetByIdAsync(entity.Id);
            return new APIResponse<FeeStructureDto> { Success = true, Data = _mapper.Map<FeeStructureDto>(result), StatusCode = HttpStatusCode.Created, Message = CommonResource.AddSuccess };
        }

        public async Task<APIResponse<FeeStructureDto>> GetFeeStructureByIdAsync(int id)
        {
            var entity = await _feeStructureRepository.GetByIdAsync(id);
            if (entity.Id == 0) return new APIResponse<FeeStructureDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
            return new APIResponse<FeeStructureDto> { Success = true, Data = _mapper.Map<FeeStructureDto>(entity), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<FeeStructureDto>> GetFeeStructureAsync(int campusId, int programId, int batchId)
        {
            var fee = await _feeStructureRepository.GetFeeStructureAsync(campusId, programId, batchId);
            if (fee == null) return new APIResponse<FeeStructureDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "No active Fee Structure found for selected campus/program/batch." };
            return new APIResponse<FeeStructureDto> { Success = true, Data = _mapper.Map<FeeStructureDto>(fee), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<IEnumerable<FeeStructureDto>>> GetAllFeeStructuresAsync()
        {
            var list = await _feeStructureRepository.GetAllAsync();
            return new APIResponse<IEnumerable<FeeStructureDto>> { Success = true, Data = _mapper.Map<IEnumerable<FeeStructureDto>>(list), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse> UpdateFeeStructureAsync(FeeStructureModel model, string username)
        {
            if (!model.Id.HasValue) return new APIResponse { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "Id is required" };
            var entity = await _feeStructureRepository.GetByIdAsync(model.Id.Value);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            _mapper.Map(model, entity);
            entity.UpdatedBy = username;
            await _feeStructureRepository.UpdateAsync(entity);

            // Re-sync items
            await _feeStructureRepository.RemoveFeeStructureItemsAsync(entity.Id);
            if (model.FeeStructureItems != null)
            {
                foreach (var item in model.FeeStructureItems)
                {
                    await _feeStructureRepository.AddFeeStructureItemAsync(new FeeStructureItem
                    {
                        FeeStructureId = entity.Id,
                        FeeTypeId = item.FeeTypeId,
                        Amount = item.Amount,
                        CreatedBy = username,
                        CreatedDate = DateTime.Now
                    });
                }
            }

            return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.UpdateSuccess };
        }

        public async Task<APIResponse> DeleteFeeStructureAsync(int id)
        {
            var res = await _feeStructureRepository.DeleteAsync(id);
            if (res > 0) return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = CommonResource.DeleteSuccess };
            return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
        }
        #endregion

        #region Core Admission Applications
        public async Task<APIResponse<AdmissionApplicationDto>> SaveDraftAsync(AdmissionApplicationModel model, string username, int tenantId)
        {
            var entity = model.Id.HasValue && model.Id.Value > 0 
                ? await _applicationRepository.GetByIdAsync(model.Id.Value) 
                : new AdmissionApplication();

            bool isNew = entity.Id == 0;

            _mapper.Map(model, entity);
            entity.Status = "Draft";
            entity.SchoolRegistrationId = tenantId;

            if (isNew)
            {
                // Generate sequential draft application number
                string prefix = $"APP-{DateTime.Now:yyyyMM}-";
                string lastNo = await _applicationRepository.GetLastApplicationNoAsync(prefix);
                int seq = 1;
                if (!string.IsNullOrEmpty(lastNo) && lastNo.Length > prefix.Length)
                {
                    if (int.TryParse(lastNo.Substring(prefix.Length), out int lastSeq))
                        seq = lastSeq + 1;
                }
                entity.ApplicationNo = $"{prefix}{seq:D4}";
                entity.CreatedBy = username;
                entity.CreatedDate = DateTime.Now;

                entity = await _applicationRepository.AddAsync(entity);
            }
            else
            {
                entity.UpdatedBy = username;
                await _applicationRepository.UpdateAsync(entity);
            }

            // Log Audit
            await _applicationRepository.AddAuditLogAsync(new AdmissionAuditLog
            {
                AdmissionApplicationId = entity.Id,
                Action = "DraftSaved",
                StatusFrom = isNew ? "None" : "Draft",
                StatusTo = "Draft",
                PerformedBy = username,
                PerformedDate = DateTime.Now,
                DetailsJson = "Application draft saved."
            });

            var resultDto = _mapper.Map<AdmissionApplicationDto>(await _applicationRepository.GetByIdAsync(entity.Id));
            return new APIResponse<AdmissionApplicationDto> { Success = true, Data = resultDto, StatusCode = HttpStatusCode.OK, Message = "Draft saved successfully" };
        }

        public async Task<APIResponse<AdmissionApplicationDto>> SubmitApplicationAsync(AdmissionApplicationModel model, string username, int tenantId)
        {
            // Evaluate custom pre-admission rules before submission
            var rules = await _ruleRepository.GetRulesAsync(model.CampusId, model.EducationLevelId, model.ProgramId);
            foreach (var rule in rules)
            {
                bool isViolated = false;
                if (rule.RuleType == "MinAge")
                {
                    int minAge = int.Parse(rule.RuleValue);
                    var age = DateTime.Today.Year - model.DateOfBirth.Year;
                    if (model.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
                    if (age < minAge) isViolated = true;
                }
                // Add more custom rule types (e.g. MinPercentage checks) if specified in rule config.

                if (isViolated)
                {
                    return new APIResponse<AdmissionApplicationDto>
                    {
                        Success = false,
                        Message = $"Rule Validation Failed: {rule.ErrorMessage}",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }

            var entity = model.Id.HasValue && model.Id.Value > 0 
                ? await _applicationRepository.GetByIdAsync(model.Id.Value) 
                : new AdmissionApplication();

            bool isNew = entity.Id == 0;
            string statusFrom = isNew ? "None" : entity.Status;

            _mapper.Map(model, entity);
            entity.Status = "Submitted";
            entity.SchoolRegistrationId = tenantId;

            if (isNew)
            {
                string prefix = $"APP-{DateTime.Now:yyyyMM}-";
                string lastNo = await _applicationRepository.GetLastApplicationNoAsync(prefix);
                int seq = 1;
                if (!string.IsNullOrEmpty(lastNo) && lastNo.Length > prefix.Length)
                {
                    if (int.TryParse(lastNo.Substring(prefix.Length), out int lastSeq))
                        seq = lastSeq + 1;
                }
                entity.ApplicationNo = $"{prefix}{seq:D4}";
                entity.CreatedBy = username;
                entity.CreatedDate = DateTime.Now;
            }

            // Generate Registration Number upon submission
            if (string.IsNullOrEmpty(entity.RegistrationNo))
            {
                string regPrefix = $"REG-{DateTime.Now:yyyy}-";
                string lastReg = await _applicationRepository.GetLastApplicationNoAsync(regPrefix); // reuse helper or separate index
                int regSeq = 1;
                if (!string.IsNullOrEmpty(lastReg) && lastReg.Length > regPrefix.Length)
                {
                    if (int.TryParse(lastReg.Substring(regPrefix.Length), out int lastSeq))
                        regSeq = lastSeq + 1;
                }
                entity.RegistrationNo = $"{regPrefix}{regSeq:D4}";
            }

            if (isNew)
            {
                entity = await _applicationRepository.AddAsync(entity);
            }
            else
            {
                entity.UpdatedBy = username;
                await _applicationRepository.UpdateAsync(entity);
            }

            await _applicationRepository.AddAuditLogAsync(new AdmissionAuditLog
            {
                AdmissionApplicationId = entity.Id,
                Action = "Submitted",
                StatusFrom = statusFrom,
                StatusTo = "Submitted",
                PerformedBy = username,
                PerformedDate = DateTime.Now,
                DetailsJson = "Application submitted for review."
            });

            var resultDto = _mapper.Map<AdmissionApplicationDto>(await _applicationRepository.GetByIdAsync(entity.Id));
            return new APIResponse<AdmissionApplicationDto> { Success = true, Data = resultDto, StatusCode = HttpStatusCode.OK, Message = "Application submitted successfully" };
        }

        public async Task<APIResponse<AdmissionApplicationDto>> GetApplicationByIdAsync(int id)
        {
            var entity = await _applicationRepository.GetByIdAsync(id);
            if (entity.Id == 0) return new APIResponse<AdmissionApplicationDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };
            return new APIResponse<AdmissionApplicationDto> { Success = true, Data = _mapper.Map<AdmissionApplicationDto>(entity), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<PagedResponse<AdmissionApplicationDto>> GetApplicationsListAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? status = null, int? campusId = null, int? programId = null)
        {
            var list = await _applicationRepository.GetAllAsync(pageNumber, pageSize, searchTerm, status, campusId, programId);
            var total = await _applicationRepository.GetTotalCountAsync(searchTerm, status, campusId, programId);
            var dtos = _mapper.Map<IEnumerable<AdmissionApplicationDto>>(list);
            return new PagedResponse<AdmissionApplicationDto>
            {
                Data = dtos,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalRecords = total,
                Success = true,
                Message = CommonResource.FetchSuccess,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<APIResponse> VerifyDocumentAsync(int applicationId, string documentName, string status, string? notes, string username)
        {
            var entity = await _applicationRepository.GetByIdAsync(applicationId);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            // Parse DocumentsJson, update document status, save back
            try
            {
                var docs = JsonSerializer.Deserialize<List<DocumentVerificationItem>>(entity.DocumentsJson);
                if (docs != null)
                {
                    var doc = docs.FirstOrDefault(d => d.DocumentName.Equals(documentName, StringComparison.OrdinalIgnoreCase));
                    if (doc != null)
                    {
                        doc.VerificationStatus = status;
                        doc.Notes = notes ?? "";
                        doc.VerifiedBy = username;
                        doc.VerifiedDate = DateTime.Now;
                    }
                    entity.DocumentsJson = JsonSerializer.Serialize(docs);
                    entity.Status = "Under Verification";
                    await _applicationRepository.UpdateAsync(entity);

                    await _applicationRepository.AddAuditLogAsync(new AdmissionAuditLog
                    {
                        AdmissionApplicationId = entity.Id,
                        Action = "DocumentVerified",
                        StatusFrom = "Submitted",
                        StatusTo = "Under Verification",
                        PerformedBy = username,
                        PerformedDate = DateTime.Now,
                        DetailsJson = $"Document '{documentName}' marked as {status}."
                    });

                    return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = "Document status updated" };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = $"Failed to verify document: {ex.Message}", StatusCode = HttpStatusCode.InternalServerError };
            }

            return new APIResponse { Success = false, Message = "Document checklist empty or not matched", StatusCode = HttpStatusCode.BadRequest };
        }

        public async Task<APIResponse> AssignFeeAsync(int applicationId, string assignedFeesJson, string username)
        {
            var entity = await _applicationRepository.GetByIdAsync(applicationId);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            entity.AssignedFeesJson = assignedFeesJson;
            await _applicationRepository.UpdateAsync(entity);

            await _applicationRepository.AddAuditLogAsync(new AdmissionAuditLog
            {
                AdmissionApplicationId = entity.Id,
                Action = "FeeAssigned",
                StatusFrom = entity.Status,
                StatusTo = entity.Status,
                PerformedBy = username,
                PerformedDate = DateTime.Now,
                DetailsJson = "Custom fees structures & installments assigned."
            });

            return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = "Fees assigned successfully" };
        }

        public async Task<APIResponse> UpdateApplicationStatusAsync(UpdateAdmissionApplicationStatusDto dto, string username)
        {
            var entity = await _applicationRepository.GetByIdAsync(dto.Id);
            if (entity.Id == 0) return new APIResponse { Success = false, StatusCode = HttpStatusCode.NotFound, Message = CommonResource.RecordNotFound };

            string statusFrom = entity.Status;
            entity.Status = dto.Status;
            if (!string.IsNullOrEmpty(dto.Remarks)) entity.Remarks = dto.Remarks;
            if (!string.IsNullOrEmpty(dto.VerificationNotes)) entity.VerificationNotes = dto.VerificationNotes;

            await _applicationRepository.UpdateAsync(entity);

            await _applicationRepository.AddAuditLogAsync(new AdmissionAuditLog
            {
                AdmissionApplicationId = entity.Id,
                Action = "StatusChanged",
                StatusFrom = statusFrom,
                StatusTo = dto.Status,
                PerformedBy = username,
                PerformedDate = DateTime.Now,
                DetailsJson = $"Status changed to {dto.Status}. Remarks: {dto.Remarks}"
            });

            return new APIResponse { Success = true, StatusCode = HttpStatusCode.OK, Message = "Application status updated successfully" };
        }

        public async Task<APIResponse<IEnumerable<AdmissionAuditLogDto>>> GetAuditLogsAsync(int applicationId)
        {
            var list = await _applicationRepository.GetAuditLogsAsync(applicationId);
            return new APIResponse<IEnumerable<AdmissionAuditLogDto>> { Success = true, Data = _mapper.Map<IEnumerable<AdmissionAuditLogDto>>(list), StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }

        public async Task<APIResponse<object>> GetDashboardStatsAsync(int tenantId)
        {
            // Gather stats per status inside SQL Server matching tenantId
            var allApps = await _applicationRepository.GetAllAsync(1, int.MaxValue, null, null, null, null);
            var stats = new
            {
                Total = allApps.Count(),
                Draft = allApps.Count(x => x.Status == "Draft"),
                Submitted = allApps.Count(x => x.Status == "Submitted"),
                UnderVerification = allApps.Count(x => x.Status == "Under Verification"),
                Approved = allApps.Count(x => x.Status == "Approved"),
                Rejected = allApps.Count(x => x.Status == "Rejected"),
                WaitingList = allApps.Count(x => x.Status == "Waiting List"),
                Cancelled = allApps.Count(x => x.Status == "Cancelled"),
                Enrolled = allApps.Count(x => x.Status == "Enrolled")
            };

            return new APIResponse<object> { Success = true, Data = stats, StatusCode = HttpStatusCode.OK, Message = CommonResource.FetchSuccess };
        }
        #endregion

        #region QR Code & Barcode Utils
        public async Task<APIResponse<string>> GenerateQrCodeBase64Async(string url)
        {
            try
            {
                using var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new PngByteQRCode(qrCodeData);
                var qrBytes = qrCode.GetGraphic(20);
                var base64 = Convert.ToBase64String(qrBytes);
                return new APIResponse<string> { Success = true, Data = $"data:image/png;base64,{base64}", StatusCode = HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new APIResponse<string> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<APIResponse<string>> GenerateBarcodeBase64Async(string data)
        {
            try
            {
                var svg = BarcodeService.GenerateCode39Svg(data);
                var base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(svg));
                return new APIResponse<string> { Success = true, Data = $"data:image/svg+xml;base64,{base64}", StatusCode = HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new APIResponse<string> { Success = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }
        #endregion
    }

    public class DocumentVerificationItem
    {
        public string DocumentName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string VerificationStatus { get; set; } = "Pending"; // Pending, Verified, Rejected
        public string Notes { get; set; } = string.Empty;
        public string VerifiedBy { get; set; } = string.Empty;
        public DateTime? VerifiedDate { get; set; }
    }

    public static class BarcodeService
    {
        private static readonly Dictionary<char, string> Code39Map = new Dictionary<char, string>
        {
            {'0', "101001101101"}, {'1', "110100101011"}, {'2', "101100101011"}, {'3', "110110010101"},
            {'4', "101001101011"}, {'5', "110100110101"}, {'6', "101100110101"}, {'7', "101001011011"},
            {'8', "110100101101"}, {'9', "101100101101"}, {'A', "110101001011"}, {'B', "101101001011"},
            {'C', "110110100101"}, {'D', "101011001011"}, {'E', "110101100101"}, {'F', "101101100101"},
            {'G', "101010011011"}, {'H', "110101001101"}, {'I', "101101001101"}, {'J', "101011001101"},
            {'K', "110101010011"}, {'L', "101101010011"}, {'M', "110110101001"}, {'N', "101011010011"},
            {'O', "110101101001"}, {'P', "101101101001"}, {'Q', "101010110011"}, {'R', "110101011001"},
            {'S', "101101011001"}, {'T', "101011011001"}, {'U', "110010101011"}, {'V', "100110101011"},
            {'W', "110011010101"}, {'X', "100101101011"}, {'Y', "110010110101"}, {'Z', "100110110101"},
            {'-', "100101011011"}, {'.', "110010101101"}, {' ', "100110101101"}, {'*', "100101101101"}
        };

        public static string GenerateCode39Svg(string data)
        {
            data = $"*{data.ToUpper()}*";
            var result = new System.Text.StringBuilder();
            result.Append("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"250\" height=\"80\" viewBox=\"0 0 250 80\" preserveAspectRatio=\"none\">");
            result.Append("<g fill=\"black\">");

            float x = 10;
            float width = 1.2f; // narrow bar
            float wideWidth = 3.0f; // wide bar

            foreach (var ch in data)
            {
                if (!Code39Map.TryGetValue(ch, out var pattern)) continue;
                for (int i = 0; i < pattern.Length; i++)
                {
                    float w = pattern[i] == '1' ? wideWidth : width;
                    if (i % 2 == 0) // Black bar
                    {
                        result.Append($"<rect x=\"{x}\" y=\"5\" width=\"{w}\" height=\"70\" />");
                    }
                    x += w;
                }
                x += width;
            }

            result.Append("</g></svg>");
            return result.ToString();
        }
    }
}
