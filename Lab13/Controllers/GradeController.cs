using Lab13.Data;
using Lab13.DTO.Request;
using Lab13.DTO.Response;
using Lab13.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab13.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GradeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE
        [HttpPost]
        public void Insert(GradeRequestV1 request)
        {
            // Convertir el request a model
            Grade model = new Grade
            {
                Name = request.Name,
                Description = request.Description,
                isActive = true
            };

            _context.Grades.Add(model);
            _context.SaveChanges();
        }

        // READ: listar todos los grados activos
        [HttpGet]
        public List<GradeResponseV1> Get()
        {
            List<Grade> grades = _context.Grades
                .Where(x => x.isActive == true)
                .ToList();

            // Convertir lista de model a lista de response
            List<GradeResponseV1> response = grades
                .Select(x => new GradeResponseV1
                {
                    GradeId = x.GradeId,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList();

            return response;
        }

        // READ: buscar un grado activo por ID
        [HttpGet]
        public GradeResponseV1? GetById(int id)
        {
            Grade? grade = _context.Grades
                .FirstOrDefault(x =>
                    x.GradeId == id &&
                    x.isActive == true);

            if (grade == null)
            {
                return null;
            }

            // Convertir model a response
            GradeResponseV1 response = new GradeResponseV1
            {
                GradeId = grade.GradeId,
                Name = grade.Name,
                Description = grade.Description
            };

            return response;
        }

        // UPDATE
        [HttpPut]
        public void Update(int id, GradeRequestV1 request)
        {
            Grade? model = _context.Grades
                .FirstOrDefault(x =>
                    x.GradeId == id &&
                    x.isActive == true);

            if (model != null)
            {
                // Convertir los datos del request al model
                model.Name = request.Name;
                model.Description = request.Description;

                _context.SaveChanges();
            }
        }

        // DELETE: eliminación lógica
        [HttpDelete]
        public void Delete(int id)
        {
            Grade? model = _context.Grades
                .FirstOrDefault(x =>
                    x.GradeId == id &&
                    x.isActive == true);

            if (model != null)
            {
                model.isActive = false;
                _context.SaveChanges();
            }
        }
    }
}