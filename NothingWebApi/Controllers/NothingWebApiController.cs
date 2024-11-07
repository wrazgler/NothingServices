using Microsoft.AspNetCore.Mvc;
using NothingWebApi.Dtos;
using NothingWebApi.Services;

namespace NothingWebApi.Controllers;

/// <summary>
/// Апи контроллера
/// </summary>
/// <param name="nothingService">Сервис бизнес логики</param>
[ApiController]
[Route("[controller]")]
public class NothingWebApiController(INothingService nothingService) : ControllerBase
{
    private readonly INothingService _nothingService = nothingService;

    /// <summary>
    /// Получить список моделей
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список моделей</returns>
    [HttpGet]
    [ProducesResponseType(typeof(NothingModelDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var models = await _nothingService.GetAsync(cancellationToken);
            return Ok(models);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получить модель с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(NothingModelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = await _nothingService.GetAsync(id, cancellationToken);
            return Ok(model);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Создать новую модель
    /// </summary>
    /// <param name="createNothingModelDto">Данные для создания модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <remarks>
    /// Пример запроса:
    ///
    ///     {
    ///	        "name": "Test"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(NothingModelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateNothingModelDto createNothingModelDto,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = await _nothingService.CreateAsync(createNothingModelDto, cancellationToken);
            return Ok(model);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновить существующую модель
    /// </summary>
    /// <param name="updateNothingModelDto">Данные для обновления модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    /// <remarks>
    /// Пример запроса:
    ///
    ///     {
    ///         "id": 1,
    ///	        "name": "Test"
    ///     }
    ///
    /// </remarks>
    [HttpPut]
    [ProducesResponseType(typeof(NothingModelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(
        [FromBody] UpdateNothingModelDto updateNothingModelDto,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = await _nothingService.UpdateAsync(updateNothingModelDto, cancellationToken);
            return Ok(model);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Удалить модель с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор модели</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект модели</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(NothingModelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = await _nothingService.DeleteAsync(id, cancellationToken);
            return Ok(model);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}