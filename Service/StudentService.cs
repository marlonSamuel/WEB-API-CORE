using Microsoft.EntityFrameworkCore;
using Model;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> Getall();
        Task<Student> Get(int id);
        Task<bool> Add(Student model);
        Task<bool> Update(Student model);

        Task<bool> Delete(int id);
    }

    public class StudentService : IStudentService
    {
        public readonly StudentDbContext _studentDbContext;

        public StudentService(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }

        //obtener lista de estudiantes
        public async Task<IEnumerable<Student>> Getall()
        {
            var result = new List<Student>();

            try
            {
                result = await _studentDbContext.Student.ToListAsync();
            }
            catch(Exception e)
            {

            }

            return result;
        }

        //obtener estudiante por id
        public async Task<Student> Get(int id)
        {
            var result = new Student();

            try
            {
                result = await _studentDbContext.Student.SingleAsync(x => x.StudentId == id);
            }catch(Exception e)
            {

            }
            return result;
        }

        //agregar nuevo estudiante
        public async Task<bool> Add(Student model)
        {
            try
            {
                await _studentDbContext.Student.AddAsync(model);
                await _studentDbContext.SaveChangesAsync();
            }catch(Exception e)
            {
                return false;
            }
            return true;
        }

        //actualizar estudiante
        public async Task<bool> Update(Student model)
        {
            try
            {
                var orginalModel = await this.Get(model.StudentId);
                orginalModel.Name = model.Name;
                orginalModel.LastName = model.LastName;
                orginalModel.Bio = model.Bio;

                _studentDbContext.Update(orginalModel);
                await _studentDbContext.SaveChangesAsync();
            }catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var student = new Student { StudentId = id };
                //_studentDbContext.Student.Attach(student);
                _studentDbContext.Student.Remove(student);

                await _studentDbContext.SaveChangesAsync();
            }catch(Exception e)
            {
                return false;
            }

            return true;
        }

    }
}
