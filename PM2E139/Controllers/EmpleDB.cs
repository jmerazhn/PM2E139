using PM2E139.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM2E139.Controllers
{
    public class EmpleDB
    {
        readonly SQLiteAsyncConnection db;

        public EmpleDB() { }

        public EmpleDB(string pathbasedatos)
        {
            db = new SQLiteAsyncConnection(pathbasedatos);
            db.CreateTableAsync<Lugar>();
        }

        public Task<List<Lugar>> listaempleados()
        {
            return db.Table<Lugar>().ToListAsync();
        }

        public Task<Lugar> ObtenerEmpleado(int pcodigo)
        {
            return db.Table<Lugar>().Where(i => i.id == pcodigo).FirstOrDefaultAsync();
        }

        public Task<int> EmpleadoGuardar(Lugar emple)
        {
            if (emple.id != 0)
            {
                return db.UpdateAsync(emple);
            }
            else
            {
                return db.InsertAsync(emple);
            }
        }

        public Task<int> EmpleadoBorrar(Lugar emple)
        {
            return db.DeleteAsync(emple);
        }
    }
}
