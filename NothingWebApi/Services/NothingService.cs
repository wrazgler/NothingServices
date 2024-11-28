using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NothingWebApi.DbContexts;
using NothingWebApi.Dtos;
using NothingWebApi.Models;

namespace NothingWebApi.Services;

/// <summary>
/// Сервис бизнес логики
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
/// <param name="mapper"><see cref="Mapper"/></param>
public class NothingService(
    NothingWebApiDbContext dbContext,
    ILogger<NothingService> logger,
    IMapper mapper)
    : INothingService
{
    private readonly NothingWebApiDbContext _dbContext = dbContext;
    private readonly ILogger<NothingService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Получить список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список моделей</returns>
    public Task<NothingModelDto[]> GetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return _dbContext.NothingModels
                .AsNoTracking()
                .OrderBy(model => model.Id)
                .Select(model => _mapper.Map<NothingModelDto>(model))
                .ToArrayAsync(cancellationToken);
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
    /// <param name="id">Идентификатор модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    public async Task<NothingModelDto> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var nothingModelDto = await _dbContext.NothingModels.AsNoTracking()
                .Where(model => model.Id == id)
                .Select(model => _mapper.Map<NothingModelDto>(model))
                .SingleOrDefaultAsync(cancellationToken);
            if(nothingModelDto == null)
                throw new ArgumentException($"Не удалось найти модель с идентификатором {id}.");
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
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="ArgumentNullException">
    /// Ошибка валидации входных данных
    /// </exception>
    public async Task<NothingModelDto> CreateAsync(
        CreateNothingModelDto createNothingModelDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if(string.IsNullOrEmpty(createNothingModelDto.Name))
                throw new ArgumentNullException(nameof(createNothingModelDto.Name), "Имя не может быть пустым.");
            var model = _mapper.Map<NothingModel>(createNothingModelDto);
            await _dbContext.NothingModels.AddAsync(model, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
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
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <exception cref="ArgumentNullException">
    /// Ошибка валидации входных данных
    /// </exception>
    public async Task<NothingModelDto> UpdateAsync(
        UpdateNothingModelDto updateNothingModelDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(updateNothingModelDto.Name))
                throw new ArgumentNullException(nameof(updateNothingModelDto.Name), "Имя не может быть пустым.");
            var model = await _dbContext.NothingModels
                .SingleOrDefaultAsync(model => model.Id == updateNothingModelDto.Id, cancellationToken);
            if(model == null)
                throw new ArgumentException($"Не удалось найти модель с идентификатором {updateNothingModelDto.Id}.");
            model.Name = updateNothingModelDto.Name;
            await _dbContext.SaveChangesAsync(cancellationToken);
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
    /// <param name="id">Идентификатор модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    public async Task<NothingModelDto> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var model = await _dbContext.NothingModels
                .SingleOrDefaultAsync(model => model.Id == id, cancellationToken);
            if(model == null)
                throw new ArgumentException($"Не удалось найти модель с идентификатором {id}.");
            _dbContext.NothingModels.Remove(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return _mapper.Map<NothingModelDto>(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }
}