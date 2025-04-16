using AutoMapper;
using NothingRpcApi.Dtos;
using NothingRpcApi.Models;

namespace NothingRpcApi.Profiles;

/// <summary>
/// Профиль конфигурации для <see cref="Mapper"/>
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Конструктор профиля конфигурации для <see cref="Mapper"/>
    /// </summary>
    public AutoMapperProfile()
    {
        AllowNullCollections = true;
        CreateMap<NothingModel, NothingModelDto>();
        CreateMap<CreateNothingModelDto, NothingModel>()
            .ForMember(model => model.Id, member => member.Ignore())
            .ForMember(model => model.Name, member => member.MapFrom(dto => dto.Name.Trim()));
    }
}