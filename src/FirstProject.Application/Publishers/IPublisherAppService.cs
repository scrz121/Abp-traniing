using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FirstProject.Publishers.Dto;

namespace FirstProject.Publishers
{
    public interface IPublisherAppService : IApplicationService
    {
        Task<ListResultDto<PublisherDto>> GetAll(PublisherFilter filter);
        Task Create(CreatePublisherDto input);
        Task Delete(DeletePublisherDto dto);
        Task Update(PublisherDto publisherDto);
        Task<PublisherDto> GetById(int id);
    }
}
