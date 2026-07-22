using System.Net;
using AutoMapper;
using School.Domain.Location;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.State;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.State;

namespace School.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;
        private readonly IMapper _mapper;

        public StateService(IStateRepository stateRepository, IMapper mapper)
        {
            _stateRepository = stateRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<StateDto>> AddStateAsync(StateModel model)
        {
            var entity = _mapper.Map<State>(model);
            entity = await _stateRepository.AddStateAsync(entity);
            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<StateDto>
                {
                    Success = false,
                    Data = _mapper.Map<StateDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(State).Name, model.Name),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                return new APIResponse<StateDto>
                {
                    Success = true,
                    Data = _mapper.Map<StateDto>(entity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<StateDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<StateDto>> GetStateByIdAsync(int id)
        {
            var result = await _stateRepository.GetStateByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<StateDto>
                {
                    Data = _mapper.Map<StateDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<StateDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<IEnumerable<StateDto>>> GetAllStatesAsync()
        {
            var result = await _stateRepository.GetAllAsync();
            if (result != null && result.Any())
            {
                return new APIResponse<IEnumerable<StateDto>>
                {
                    Data = _mapper.Map<IEnumerable<StateDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<StateDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateStateAsync(StateModel model)
        {
            var entity = _mapper.Map<State>(model);
            var result = await _stateRepository.UpdateStateAsync(entity);
            if (result > 0)
            {
                return new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Message = CommonResource.UpdateSuccess
                };
            }
            else
            {
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> DeleteStateAsync(int id)
        {
            int changes = await _stateRepository.DeleteStateAsync(id);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.DeleteSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.DeleteFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }

        public async Task<APIResponse> ToggleStateStatusAsync(int id)
        {
            int changes = await _stateRepository.ToggleStateStatusAsync(id);
            if (changes > 0)
                return new APIResponse
                {
                    Success = true,
                    Message = CommonResource.UpdateSuccess,
                    StatusCode = HttpStatusCode.OK,
                };
            else
                return new APIResponse
                {
                    Message = CommonResource.UpdateFailed,
                    StatusCode = HttpStatusCode.BadRequest,
                };
        }
    }
}
