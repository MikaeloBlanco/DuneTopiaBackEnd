using DunetopiaBackEnd.Models.Database.Entities;

namespace DunetopiaBackEnd.Models.Database;

public class DbSeeder
{
    private readonly MyDBContext _dbContextDuneTopia;

    public DbSeeder(MyDBContext dbContext)
    {
        _dbContextDuneTopia = dbContext;
    }

    public async Task SeedAsync()
    {
        bool created = await _dbContextDuneTopia.Database.EnsureCreatedAsync();

        if(created)
        {
            await SeedProductosAsync();
        }

        _dbContextDuneTopia.SaveChanges();
    }

    private async Task SeedProductosAsync()
    {
        Producto[] productos = [

                new Producto(){
                    Name = "Age of Empires DE",
                    Description = "Uno de los primeros titulos de Estrategia en tiempo real, ahora traido a la vida remasterizado. Enfrentate a más de 10 diferentes civilizaciones en esta aventura pro toda la historia del mundo",
                    Price = 25.50m,
                    Stock = 31,
                    Image = "Imagenes/age-of-empires-definitive-edition-definitive-edition-pc-juego-steam-cover.jpg"
                },
                new Producto(){
                    Name = "Age of Empires II DE",
                    Description = "La continuación de una saga clasica. Ahora, con diferentes nuevos DLCs más los antiguos DLCs de su versión HD, Age of Empires II DE te permite jugar con tus amigos con más de 25 tipos diferentes de Civilizaciones",
                    Price = 37.50m,
                    Stock = 10,
                    Image = "Imagenes/age-of-empires-ii-definitive-edition-definitive-edition.jpg"
                },
                new Producto(){
                    Name = " Age of Empires III DE",
                    Description = "El ultimo de los Age of Empires Clasicos en ser actualizados. Con las potencias emergentes de America, los samurais de la fuedal Japón o los independientes y guerreros indios-americanos junto a los caballeros de Malta, toma control de tu metropoli y coloniza el salvaje oeste por los recursos",
                    Price = 29.70m,
                    Stock = 16,
                    Image = "Imagenes/age-of-empires-iii-definitive-edition.jpg"
                },
                new Producto(){
                    Name = "Turmoil",
                    Description = "Un juego de conseguir petroleo en el salvaje oeste. Lucha contra tus competidores para conseguir ese Oro negro que aun sigue sin ser escavado",
                    Price = 25.30m,
                    Stock = 27,
                    Image = "Imagenes/turmoil"
                },
                new Producto(){
                    Name = "Kenshi",
                    Description = "El simulador rpg más duro y realista en una tierra extraña. Diferentes razas en este mundo arido se enfrentan por sobrevivir. Has de poder aprender a pelear, cultivar, ganar dinero y luchar sin tus brazos o armas. Lucha, muere y aprende",
                    Price = 20.00m,
                    Stock = 5,
                    Image = "Imagenes/kenshi"
                },
                new Producto(){
                    Name = "Dune Spice wars",
                    Description = "Un juego del estilo de Civilization que te lleva al mundo de Ciencia-Ficción de Dune en busca de las especias.",
                    Price = 23.45m,
                    Stock = 34,
                    Image = "Imagenes/dune-spice-wars"
                }

            ];
        await _dbContextDuneTopia.Productos.AddRangeAsync( productos );
    }
}
