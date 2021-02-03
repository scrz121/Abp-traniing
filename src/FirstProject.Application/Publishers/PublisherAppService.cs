using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using FirstProject.Authorization;
using FirstProject.Helper;
using FirstProject.Publishers.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Publishers
{
    public class PublisherFilter
    {
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
        public string Keyword { get; set; }
    }
    [AbpAuthorize]
    public class PublisherAppService : FirstProjectAppServiceBase, IPublisherAppService
    {
        private readonly IRepository<Publisher> _publisherRepository;
        private readonly BookHelper _bookHelper;

        public PublisherAppService(IRepository<Publisher> publisherRepository,BookHelper bookHelper)
        {
            _bookHelper = bookHelper;
            _publisherRepository = publisherRepository;
        }

        [AbpAuthorize(PermissionNames.Pages_Publishers)]
        public async Task Create(CreatePublisherDto input)
        {
            try
            {
                var find = _publisherRepository.GetAll().ToList()
                            .WhereIf(!string.IsNullOrEmpty(input.PublisherName), x => x.PublisherName.Trim().ToLower() == input.PublisherName.Trim().ToLower()).ToList() ;
                if (find.Count != 0)
                {
                    throw new Exception("Publisher Name exist");
                }
                var entity = ObjectMapper.Map<Publisher>(input);
                await _publisherRepository.InsertAsync(entity);
            }catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Publishers)]
        public async Task Delete(DeletePublisherDto dto)
        {
            int id = dto.PublisherId;
            try
            {
                var books = await _bookHelper.GetByPublisherId(id);
                if(books!=null || books.Count > 0)
                {
                    throw new Exception("Tồn tại sách thuộc nhà xuất bản này");
                }
                await _publisherRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Publishers_Read)]
        public async Task<ListResultDto<PublisherDto>> GetAll(PublisherFilter filter = null)
        {
            try
            {
                var allPublishers = await _publisherRepository
                   .GetAll().OrderBy(x => x.PublisherName)
                   .ToListAsync();
                var publishers = allPublishers;
                int totalCount = publishers.Count;
                if (filter != null)
                {
                    string keyword = filter.Keyword;
                    publishers = allPublishers.WhereIf(!String.IsNullOrEmpty(keyword), x => x.PublisherName.ToLower().Contains(filter.Keyword.ToLower())).ToList();

                    totalCount = publishers.Count;

                    if (totalCount > filter.SkipCount)
                    {
                        publishers = publishers.Skip(filter.SkipCount).Take(filter.MaxResultCount).ToList();
                    }
                }
                var result = new PagedResultDto<PublisherDto>(totalCount, ObjectMapper.Map<List<PublisherDto>>(publishers));

                return result;
            }
            catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }        

        [AbpAuthorize(PermissionNames.Pages_Publishers_Read)]
        public async Task<PublisherDto> GetById(int id)
        {
            try
            {
                var publisher = await _publisherRepository.FirstOrDefaultAsync(id);
                var dto = ObjectMapper.Map<PublisherDto>(publisher);
                return dto;
            }
            catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Publishers)]
        public async Task Update(PublisherDto publisherDto)
        {
            try
            {
                var find = _publisherRepository.GetAll().ToList()
                                           .WhereIf(!string.IsNullOrEmpty(publisherDto.PublisherName), x => x.PublisherName.Trim().ToLower() == publisherDto.PublisherName.Trim().ToLower()).ToList(); 
                if (find.Count != 0)
                {
                    throw new Exception("Publisher Name exist");
                }
                var publisherCheck = await _publisherRepository.FirstOrDefaultAsync(publisherDto.Id);
                if (publisherCheck != null)
                {
                    publisherDto.CreatorUserId = publisherCheck.CreatorUserId;
                    ObjectMapper.Map(publisherDto,publisherCheck);
                }
                else
                {
                    throw new Exception("Not exist");
                }

            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
