using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NothingKafka.Configs;
using NothingKafka.DbContexts;
using NothingKafka.Dtos;
using NothingKafka.Models;

namespace NothingKafka.Services;

/// <summary>
/// Сервис бизнес логики
/// </summary>
/// <param name="config">Заголовки Kafka сервиса NothingService</param>
/// <param name="dbContext">Контекст базы данных</param>
/// <param name="logger">Экземпляр <see cref="ILogger"/></param>
/// <param name="mapper"><see cref="Mapper"/></param>
/// <param name="producerService">Сервис отправки сообщений в Kafka</param>
public sealed class NothingService(
    NothingKafkaDbContext dbContext,
    ILogger<NothingService> logger,
    IMapper mapper,
    IOptions<NothingServiceConfig> config,
    IProducerService producerService)
    : INothingService
{
    private readonly NothingKafkaDbContext _dbContext = dbContext;
    private readonly NothingServiceConfig _nothingServiceConfig = config.Value;
    private readonly ILogger<NothingService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IProducerService _producerService = producerService;

    /// <summary>
    /// Получить список моделей
    /// </summary>
    /// <param name="getNothingsModelsDto">Данные для получения моделей</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список моделей</returns>
    public async Task Get(
        GetNothingModelsDto getNothingsModelsDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _dbContext.NothingModels
                .AsNoTracking()
                .OrderBy(model => model.Id)
                .ProjectTo<NothingModelDto>(_mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);
            await _producerService.SendMessage(result, _nothingServiceConfig.GetModelsTopic, cancellationToken);
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
    /// <param name="getNothingModelDto">Данные для получения модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    public async Task Get(
        GetNothingModelDto getNothingModelDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var nothingModelDto = await _dbContext.NothingModels
                .AsNoTracking()
                .Where(model => model.Id == getNothingModelDto.Id)
                .ProjectTo<NothingModelDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
            if(nothingModelDto == null)
                throw new ArgumentException($"Не удалось найти модель с идентификатором {getNothingModelDto.Id}.");
            var result =  nothingModelDto;
            await _producerService.SendMessage(result, _nothingServiceConfig.GetModelTopic, cancellationToken);
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
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="ArgumentNullException">
    /// Ошибка валидации входных данных
    /// </exception>
    public async Task Create(
        CreateNothingModelDto createNothingModelDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if(string.IsNullOrEmpty(createNothingModelDto.Name.Trim()))
                throw new ArgumentNullException(nameof(createNothingModelDto.Name), "Имя не может быть пустым.");
            var model = _mapper.Map<NothingModel>(createNothingModelDto);
            await _dbContext.NothingModels.AddAsync(model, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            var result =  _mapper.Map<NothingModelDto>(model);
            await _producerService.SendMessage(result, _nothingServiceConfig.CreateTopic, cancellationToken);
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
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="ArgumentNullException">
    /// Ошибка валидации входных данных
    /// </exception>
    public async Task Update(
        UpdateNothingModelDto updateNothingModelDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(updateNothingModelDto.Name.Trim()))
                throw new ArgumentNullException(nameof(updateNothingModelDto.Name), "Имя не может быть пустым.");
            var model = await _dbContext.NothingModels
                .SingleOrDefaultAsync(model => model.Id == updateNothingModelDto.Id, cancellationToken);
            if(model == null)
                throw new ArgumentException($"Не удалось найти модель с идентификатором {updateNothingModelDto.Id}.");
            model.Name = updateNothingModelDto.Name.Trim();
            await _dbContext.SaveChangesAsync(cancellationToken);
            var result =  _mapper.Map<NothingModelDto>(model);
            await _producerService.SendMessage(result, _nothingServiceConfig.UpdateTopic, cancellationToken);
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
    /// <param name="deleteNothingModelDto">Данные для удаления модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    public async Task Delete(
        DeleteNothingModelDto deleteNothingModelDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var model = await _dbContext.NothingModels
                .SingleOrDefaultAsync(model => model.Id == deleteNothingModelDto.Id, cancellationToken);
            if(model == null)
                throw new ArgumentException($"Не удалось найти модель с идентификатором {deleteNothingModelDto.Id}.");
            _dbContext.NothingModels.Remove(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            var result = _mapper.Map<NothingModelDto>(model);
            await _producerService.SendMessage(result, _nothingServiceConfig.DeleteTopic, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }
}