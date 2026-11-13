using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaControlServiciosLimpieza.Api.Data;

[ApiController]
[Route("api/[controller]")]
public class BaseCatalogController<T> : ControllerBase where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _entities;  // Esto nos dará acceso al conjunto de T

    public BaseCatalogController(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();  // Obtiene el DbSet<> correspondiente al tipo T
    }

    // GET: api/[controller]
    [HttpGet]
    public virtual ActionResult<IEnumerable<T>> GetAll()
    {
        var data = _entities.ToList();
        return Ok(data);
    }

    // GET: api/[controller]/{id}
    [HttpGet("{id}")]
    public virtual ActionResult<T> GetById(int id)
    {
        // Usamos Find ya que T debe tener una clave primaria definida
        var entity = _entities.Find(id);
        if (entity == null)
            return NotFound();
        return Ok(entity);
    }

    //// POST: api/[controller]
    //[HttpPost]
    //public virtual ActionResult<T> Create([FromBody] T newEntity)
    //{
    //    _entities.Add(newEntity);
    //    _context.SaveChanges();
    //    // Devuelve el nuevo recurso creado (201 Created)
    //    return CreatedAtAction(nameof(GetById), new { id = GetEntityId(newEntity) }, newEntity);
    //}

    // DELETE: api/[controller]/{id}
    [HttpDelete("{id}")]
    public virtual IActionResult Delete(int id)
    {
        var entity = _entities.Find(id);
        if (entity == null)
            return NotFound();
        _entities.Remove(entity);
        _context.SaveChanges();
        return NoContent();  // 204 No Content, eliminación exitosa
    }

}
