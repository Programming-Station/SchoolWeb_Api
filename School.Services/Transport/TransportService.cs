using School_DTOs;
using Microsoft.EntityFrameworkCore;
using School.Domain.Transport;
using School.Infrastructure.Repositories.IRepositories;
using School.Infrastructure.UnitOfWork.Interfaces;
using School.Services.Interfaces.Transport;
using School_DTOs.Transport;
using School_DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace School.Services.Transport
{
    public class VehicleService : IVehicleService
    {
        private readonly IRepository<Vehicle> _repo; private readonly IUnitOfWork _uow;
        public VehicleService(IRepository<Vehicle> repo,IUnitOfWork uow){_repo=repo;_uow=uow;}
        public async Task<APIResponse<List<VehicleDto>>> GetAllAsync(){var d=await _repo.List().Select(x=>new VehicleDto{Id=x.Id,Name=x.Name,RegistrationNumber=x.RegistrationNumber,DriverName=x.DriverName,DriverPhone=x.DriverPhone,Capacity=x.Capacity,Status=x.Status}).ToListAsync();return new APIResponse<List<VehicleDto>>{StatusCode=HttpStatusCode.OK,Message="Success",Data=d};}
        public async Task<APIResponse<VehicleDto>> GetByIdAsync(int id){var x=await _repo.List().Where(v=>v.Id==id).Select(x=>new VehicleDto{Id=x.Id,Name=x.Name,RegistrationNumber=x.RegistrationNumber,DriverName=x.DriverName,DriverPhone=x.DriverPhone,Capacity=x.Capacity,Status=x.Status}).FirstOrDefaultAsync();if(x==null)return new APIResponse<VehicleDto>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};return new APIResponse<VehicleDto>{StatusCode=HttpStatusCode.OK,Message="Success",Data=x};}
        public async Task<APIResponse<object>> CreateAsync(CreateVehicleDto dto,string username){await _repo.AddAsync(new Vehicle{Name=dto.Name,RegistrationNumber=dto.RegistrationNumber,DriverName=dto.DriverName,DriverPhone=dto.DriverPhone,Capacity=dto.Capacity,Status=dto.Status,CreatedBy=username,CreatedDate=DateTime.UtcNow});await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Created successfully"};}
        public async Task<APIResponse<object>> UpdateAsync(int id,UpdateVehicleDto dto,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};e.Name=dto.Name;e.RegistrationNumber=dto.RegistrationNumber;e.DriverName=dto.DriverName;e.DriverPhone=dto.DriverPhone;e.Capacity=dto.Capacity;e.Status=dto.Status;e.UpdatedBy=username;e.UpdatedDate=DateTime.UtcNow;_repo.Update(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Updated successfully"};}
        public async Task<APIResponse<object>> DeleteAsync(int id,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};_repo.Delete(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Deleted successfully"};}
    }
    public class TransportRouteService : ITransportRouteService
    {
        private readonly IRepository<TransportRoute> _repo; private readonly IUnitOfWork _uow;
        public TransportRouteService(IRepository<TransportRoute> repo,IUnitOfWork uow){_repo=repo;_uow=uow;}
        public async Task<APIResponse<List<TransportRouteDto>>> GetAllAsync(){var d=await _repo.List().Select(x=>new TransportRouteDto{Id=x.Id,RouteName=x.RouteName,Description=x.Description,VehicleId=x.VehicleId,Status=x.Status}).ToListAsync();return new APIResponse<List<TransportRouteDto>>{StatusCode=HttpStatusCode.OK,Message="Success",Data=d};}
        public async Task<APIResponse<TransportRouteDto>> GetByIdAsync(int id){var x=await _repo.List().Where(r=>r.Id==id).Select(x=>new TransportRouteDto{Id=x.Id,RouteName=x.RouteName,Description=x.Description,VehicleId=x.VehicleId,Status=x.Status}).FirstOrDefaultAsync();if(x==null)return new APIResponse<TransportRouteDto>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};return new APIResponse<TransportRouteDto>{StatusCode=HttpStatusCode.OK,Message="Success",Data=x};}
        public async Task<APIResponse<object>> CreateAsync(CreateTransportRouteDto dto,string username){await _repo.AddAsync(new TransportRoute{RouteName=dto.RouteName,Description=dto.Description,VehicleId=dto.VehicleId,Status=dto.Status,CreatedBy=username,CreatedDate=DateTime.UtcNow});await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Created successfully"};}
        public async Task<APIResponse<object>> UpdateAsync(int id,UpdateTransportRouteDto dto,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};e.RouteName=dto.RouteName;e.Description=dto.Description;e.VehicleId=dto.VehicleId;e.Status=dto.Status;e.UpdatedBy=username;e.UpdatedDate=DateTime.UtcNow;_repo.Update(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Updated successfully"};}
        public async Task<APIResponse<object>> DeleteAsync(int id,string username){var e=await _repo.List().Where(x=>x.Id==id).FirstOrDefaultAsync();if(e==null)return new APIResponse<object>{StatusCode=HttpStatusCode.NotFound,Message="Not found"};_repo.Delete(e);await _uow.CommitAsync();return new APIResponse<object>{StatusCode=HttpStatusCode.OK,Message="Deleted successfully"};}
    }
}

