using Zadanie3.Models;

namespace Zadanie3.Repositories
{
    public interface IAnimalRepository
    {
        IEnumerable<Animal> GetAnimals();

        Animal GetAnimalById(int id);

        Animal GetAnimalByName(string name);

        public int CreateAnimal(Animal animal);

        public int DeleteAnimal(Animal animal); 
    }
}
