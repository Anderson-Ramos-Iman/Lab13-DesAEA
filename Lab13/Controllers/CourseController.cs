using Lab13.Data;
using Lab13.DTO.Request;
using Lab13.DTO.Response;
using Lab13.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab13.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE
        [HttpPost]
        public void Insert(CourseRequestV1 request)
        {
            // Convertir request a model
            Course model = new Course
            {
                Name = request.Name,
                Credit = request.Credit,
                isActive = true
            };

            _context.Courses.Add(model);
            _context.SaveChanges();
        }

        // READ: listar cursos activos
        [HttpGet]
        public List<CourseResponseV1> Get()
        {
            List<Course> courses = _context.Courses
                .Where(x => x.isActive == true)
                .ToList();

            // Convertir lista de model a lista de response
            List<CourseResponseV1> response = courses
                .Select(x => new CourseResponseV1
                {
                    CourseId = x.CourseId,
                    Name = x.Name,
                    Credit = x.Credit
                })
                .ToList();

            return response;
        }

        // READ: buscar un curso activo por ID
        [HttpGet]
        public CourseResponseV1? GetById(int id)
        {
            Course? course = _context.Courses
                .FirstOrDefault(x =>
                    x.CourseId == id &&
                    x.isActive == true);

            if (course == null)
            {
                return null;
            }

            // Convertir model a response
            CourseResponseV1 response = new CourseResponseV1
            {
                CourseId = course.CourseId,
                Name = course.Name,
                Credit = course.Credit
            };

            return response;
        }

        // UPDATE
        [HttpPut]
        public void Update(int id, CourseRequestV1 request)
        {
            Course? model = _context.Courses
                .FirstOrDefault(x =>
                    x.CourseId == id &&
                    x.isActive == true);

            if (model != null)
            {
                // Convertir los datos del request al model
                model.Name = request.Name;
                model.Credit = request.Credit;

                _context.SaveChanges();
            }
        }

        // DELETE: eliminación lógica
        [HttpDelete]
        public void Delete(int id)
        {
            Course? model = _context.Courses
                .FirstOrDefault(x =>
                    x.CourseId == id &&
                    x.isActive == true);

            if (model != null)
            {
                model.isActive = false;
                _context.SaveChanges();
            }
        }
    }
}