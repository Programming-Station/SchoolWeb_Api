using AutoMapper;
using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Event;
using School.Services.Interfaces;
using School.Utilities.Resources;
using School_DTOs;
using School_DTOs.Event;
using System.Net;

namespace School.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<EventDto>> AddEventAsync(EventModel model)
        {
            var entity = _mapper.Map<Event>(model);
            entity = await _eventRepository.AddEventAsync(entity);
            if (entity != null && entity.Id == 0)
            {
                return new APIResponse<EventDto>
                {
                    Success = false,
                    Data = _mapper.Map<EventDto>(entity),
                    Message = string.Format(CommonResource.AlreadyExists, typeof(Event).Name, model.Title),
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
            else if (entity != null && entity.Id > 0)
            {
                return new APIResponse<EventDto>
                {
                    Success = true,
                    Data = _mapper.Map<EventDto>(entity),
                    Message = CommonResource.AddSuccess,
                    StatusCode = HttpStatusCode.Created
                };
            }
            else
            {
                return new APIResponse<EventDto>
                {
                    Success = false,
                    Message = CommonResource.AddFailed,
                    StatusCode = HttpStatusCode.Forbidden
                };
            }
        }

        public async Task<APIResponse<EventDto>> GetEventByIdAsync(int id)
        {
            var result = await _eventRepository.GetEventByIdAsync(id);

            if (result != null && result.Id > 0)
            {
                return new APIResponse<EventDto>
                {
                    Data = _mapper.Map<EventDto>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<EventDto>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }

        public async Task<APIResponse<IEnumerable<EventDto>>> GetAllEventsAsync(bool? upcomingOnly = null)
        {
            var result = await _eventRepository.GetAllEventsAsync(upcomingOnly);
            if (result != null && result.Any())
            {
                return new APIResponse<IEnumerable<EventDto>>
                {
                    Data = _mapper.Map<IEnumerable<EventDto>>(result),
                    Message = CommonResource.FetchSuccess,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            else
            {
                return new APIResponse<IEnumerable<EventDto>>
                {
                    Message = CommonResource.RecordNotFound,
                    StatusCode = HttpStatusCode.OK,
                };
            }
        }

        public async Task<APIResponse> UpdateEventAsync(EventModel model)
        {
            var entity = _mapper.Map<Event>(model);
            var result = await _eventRepository.UpdateEventAsync(entity);
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

        public async Task<APIResponse> DeleteEventAsync(int id)
        {
            int changes = await _eventRepository.DeleteEventAsync(id);
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

        public async Task<APIResponse> ToggleEventStatusAsync(int id)
        {
            int changes = await _eventRepository.ToggleEventStatusAsync(id);
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

