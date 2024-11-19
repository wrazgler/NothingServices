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
    IMapper mapper)
    : INothingService
{
    private readonly NothingWebApiDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Получить список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список моделей</returns>
    public Task<NothingModelDto[]> GetAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.NothingModels.AsNoTracking()
            .Select(model => _mapper.Map<NothingModelDto>(model))
            .OrderBy(model => model.Id)
            .ToArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Получить модель с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    public Task<NothingModelDto> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return _dbContext.NothingModels.AsNoTracking()
            .Where(model => model.Id == id)
            .Select(model => _mapper.Map<NothingModelDto>(model))
            .SingleAsync(cancellationToken);
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
        if(string.IsNullOrEmpty(createNothingModelDto.Name))
            throw new ArgumentNullException(nameof(createNothingModelDto.Name), "Name cannot be null or empty.");
        var model = _mapper.Map<NothingModel>(createNothingModelDto);
        await _dbContext.NothingModels.AddAsync(model, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<NothingModelDto>(model);
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
        if (string.IsNullOrEmpty(updateNothingModelDto.Name))
            throw new ArgumentNullException(nameof(updateNothingModelDto.Name), "Name cannot be null or empty.");
        var model = await _dbContext.NothingModels
            .SingleAsync(model => model.Id == updateNothingModelDto.Id, cancellationToken);
        model.Name = updateNothingModelDto.Name;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<NothingModelDto>(model);
    }

    /// <summary>
    /// Удалить модель с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    public async Task<NothingModelDto> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var model = await _dbContext.NothingModels
            .SingleAsync(model => model.Id == id, cancellationToken);
        _dbContext.NothingModels.Remove(model);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<NothingModelDto>(model);
    }
}