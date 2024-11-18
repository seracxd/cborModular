using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cborModular.LocalStorageSqLite
{
    public class DatabaseSqlite
    {
        private readonly SQLiteConnection _db;

        public DatabaseSqlite()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "kuberg.db");
          
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<MotorcycleModel>(); // Vytvoření tabulky
        }

        public void AddMotorcycle(MotorcycleModel motorcycle)
        {
            _db.Insert(motorcycle);
        }

        public List<MotorcycleModel> GetAllMotorcycles()
        {
            return _db.Table<MotorcycleModel>().ToList();
        }

        public void DeleteMotorcycle(int id)
        {
            _db.Delete<MotorcycleModel>(id);
        }
    }
}
