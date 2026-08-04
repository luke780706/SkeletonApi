
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkeletonApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Tworzymy "buildera" aplikacji.
            // Tutaj konfigurujemy wszystko, czego aplikacja będzie potrzebować:
            // serwisy, konfigurację, logowanie, bazę danych itd.
            // args zawiera argumenty przekazane przy uruchamianiu programu.
            var builder = WebApplication.CreateBuilder(args);


            // Dodajemy kontrolery do kontenera Dependency Injection.
            // Dzięki temu ASP.NET Core będzie wiedział, że mamy klasy typu Controller
            // i będzie mógł je automatycznie tworzyć oraz obsługiwać żądania HTTP.
            builder.Services.AddControllers();


            // Dodajemy obsługę OpenAPI (dawniej często przez Swagger).
            // Dzięki temu aplikacja może wygenerować opis swojego API
            // np. listę endpointów, parametrów i modeli.

            //Element Co robi
            //OpenAPI opisuje API w standardowym formacie
            //AddOpenApi()    przygotowuje generowanie tego opisu
            //MapOpenApi()    wystawia dokument przez HTTP
            //Swagger narzędzia korzystające z OpenAPI
            //Swagger UI graficzna strona do testowania API

            builder.Services.AddOpenApi();


            // Budujemy gotową aplikację na podstawie wcześniejszej konfiguracji.
            // Od tego momentu mamy obiekt "app", którym konfigurujemy działanie API.
            var app = builder.Build();


            // Sprawdzamy, czy aplikacja działa w środowisku developerskim.
            // W Development pokazujemy dodatkowe narzędzia pomocne programiście.
            if (app.Environment.IsDevelopment())
            {
                // Udostępniamy dokumentację OpenAPI tylko podczas programowania.
                // Nie chcemy zwykle wystawiać takich narzędzi na produkcji.
                app.MapOpenApi();
            }


            // Automatycznie przekierowuje ruch z HTTP na HTTPS.
            // Czyli np.:
            // http://localhost:5000
            // zostanie zmienione na:
            // https://localhost:7000
            app.UseHttpsRedirection();


            // Włącza mechanizm autoryzacji.
            // Na razie nic nie robi, bo nie mamy jeszcze logowania.
            // Później tutaj będzie sprawdzanie tokenów JWT itp.
            app.UseAuthorization();


            // Podłącza nasze kontrolery do routingu.
            // Dzięki temu np.:
            // GET /api/products
            // trafi do ProductsController.
            app.MapControllers();


            // Uruchamia aplikację i zaczyna nasłuchiwać na żądania HTTP.
            // Od tego momentu API "żyje".
            app.Run();
        }
    }
}
