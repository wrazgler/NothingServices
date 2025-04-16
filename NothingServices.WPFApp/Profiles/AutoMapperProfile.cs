using AutoMapper;
using NothingServices.WPFApp.Dtos;
using NothingServices.WPFApp.ViewModels.Controls;

namespace NothingServices.WPFApp.Profiles;

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
        CreateMap<CreateNothingModelVM, CreateNothingModelDto>();
        CreateMap<CreateNothingModelVM, CreateNothingModelWebDto>();
        CreateMap<DeleteNothingModelVM, NothingModelIdDto>();
        CreateMap<UpdateNothingModelVM, UpdateNothingModelDto>();
        CreateMap<UpdateNothingModelVM, UpdateNothingModelWebDto>();
    }
}