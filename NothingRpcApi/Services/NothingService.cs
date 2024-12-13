using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using NothingRpcApi.DbContexts;
using NothingRpcApi.Dtos;
using NothingRpcApi.Models;

namespace NothingRpcApi.Services;

/// <summary>
/// Сервис бизнес логики
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
/// <param name="logger">Экземпляр <see cref="ILogger"/></param>
/// <param name="mapper"><see cref="Mapper"/></param>
public class NothingService(
    NothingRpcApiDbContext dbContext,
    ILogger<NothingService> logger,
    IMapper mapper)
    : NothingRpcService.NothingRpcServiceBase, INothingService
{
    private readonly NothingRpcApiDbContext _dbContext = dbContext;
    private readonly ILogger<NothingService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Получить список моделей
    /// </summary>
    /// <param name="request">Пустой запрос</param>
    /// <param name="responseStream">Стрим, возвращающий результаты метода</param>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Список моделей</returns>
    public override async Task GetStream(
        Empty request,
        IServerStreamWriter<NothingModelDto> responseStream,
        ServerCallContext context)
    {
        try
        {
            var nothingModels = await _dbContext.NothingModels
                .AsNoTracking()
                .OrderBy(model => model.Id)
                .Select(model => _mapper.Map<NothingModelDto>(model))
                .ToArrayAsync(context.CancellationToken);
            foreach (var nothingModel in nothingModels)
            {
                await responseStream.WriteAsync(nothingModel, context.CancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// Получить модель с указанным идентификатором
    /// </summary>
    /// <param name="nothingModelIdDto">Идентификатор модели</param>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Объект модели</returns>
    public override async Task<NothingModelDto> Get(
        NothingModelIdDto nothingModelIdDto,
        ServerCallContext context)
    {
        try
        {
            var nothingModelDto = await  _dbContext.NothingModels
                .AsNoTracking()
                .Where(model => model.Id == nothingModelIdDto.Id)
                .Select(model => _mapper.Map<NothingModelDto>(model))
                .SingleOrDefaultAsync(context.CancellationToken);
            if(nothingModelDto == null)
                throw new ArgumentException($"Не удалось найти модель с идентификатором {nothingModelIdDto.Id}.");
            return nothingModelDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="createNothingModelDto">Данные для создания модели</param>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="ArgumentNullException">
    /// Ошибка валидации входных данных
    /// </exception>
    public override async Task<NothingModelDto> Create(
        CreateNothingModelDto createNothingModelDto,
        ServerCallContext context)
    {
        try
        {
            if(string.IsNullOrEmpty(createNothingModelDto.Name.Trim()))
                throw new ArgumentNullException(nameof(createNothingModelDto.Name), "Имя не может быть пустым.");
            var model = _mapper.Map<NothingModel>(createNothingModelDto);
            await _dbContext.NothingModels.AddAsync(model, context.CancellationToken);
            await _dbContext.SaveChangesAsync(context.CancellationToken);
            return _mapper.Map<NothingModelDto>(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="updateNothingModelDto">Данные для обновления модели</param>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="ArgumentNullException">
    /// Ошибка валидации входных данных
    /// </exception>
    public override async Task<NothingModelDto> Update(
        UpdateNothingModelDto updateNothingModelDto,
        ServerCallContext context)
    {
        try
        {
            if (string.IsNullOrEmpty(updateNothingModelDto.Name.Trim()))
                throw new ArgumentNullException(nameof(updateNothingModelDto.Name), "Имя не может быть пустым.");
            var model = await _dbContext.NothingModels
                .SingleOrDefaultAsync(model => model.Id == updateNothingModelDto.Id, context.CancellationToken);
            if(model == null)
                throw new ArgumentException($"Не удалось найти модель с идентификатором {updateNothingModelDto.Id}.");
            model.Name = updateNothingModelDto.Name.Trim();
            await _dbContext.SaveChangesAsync(context.CancellationToken);
            return _mapper.Map<NothingModelDto>(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// Удалить модель с указанным идентификатором
    /// </summary>
    /// <param name="nothingModelIdDto">Идентификатор модели</param>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Объект модели</returns>
    public override async Task<NothingModelDto> Delete(
        NothingModelIdDto nothingModelIdDto,
        ServerCallContext context)
    {
        try
        {
            var model = await _dbContext.NothingModels
                .SingleOrDefaultAsync(model => model.Id == nothingModelIdDto.Id, context.CancellationToken);
            if(model == null)
                throw new ArgumentException($"Не удалось найти модель с идентификатором {nothingModelIdDto.Id}.");
            _dbContext.NothingModels.Remove(model);
            await _dbContext.SaveChangesAsync(context.CancellationToken);
            return _mapper.Map<NothingModelDto>(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }
}