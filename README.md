README – Real Estate Web Application
📋 Описание
Това е уеб базирано приложение за продажба и отдаване под наем на недвижими имоти, изградено с помощта на ASP.NET Core MVC (.NET 6) и Entity Framework Core. Системата предлага сигурна и лесна за използване платформа, която свързва наемодатели, наематели, брокери и администратори.
🎯 Основни функционалности
- Регистрация и вход с роли: Admin, Broker, Частно лице
- Публикуване и редактиране на обяви за имоти
- Филтриране по тип, цена  и екстри
- Вградена чат система между потребителите
- Кандидатстване за брокер чрез качване на CV
- Админ панел за управление на потребители и обяви
🚀 Технологии
- ASP.NET Core MVC (.NET 6)
- Entity Framework Core (Code First)
- MS SQL Server
- Bootstrap 5
- ASP.NET Identity
🛠️ Изисквания за стартиране
- Visual Studio 2022+
- .NET 6 SDK
- SQL Server Express или LocalDB
- Git (ако клонирате хранилището)
🧑‍💻 Как да стартирате проекта
1. Клонирайте хранилището:
   git clone https://github.com/yourusername/real-estate-app.git
2. Отворете `.sln` файла с Visual Studio
3. Конфигурирайте `appsettings.json`:
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=RealEstateApp;Trusted_Connection=True;"
}
4. Създайте базата:
   Update-Database
5. Стартирайте приложението:
   Натиснете F5 или бутона 'Start'.
👥 Роли и достъп
Admin – Управлява потребители, обяви, кандидатури
Broker – Създава, редактира и изтрива собствени обяви
User – Разглежда обяви, изпраща съобщения, кандидатства
📂 Структура на проекта
/Controllers – контролери за всяка секция
/Models – бизнес модели и form модели
/Views – Razor изгледи по MVC
/Data – ApplicationDbContext и seed логика
/wwwroot – статични ресурси (CSS, JS, изображения)
📝 Примерни акаунти (по избор)
Админ: admin@site.com / Admin123!
Брокер: broker@site.com / Broker123!
Потребител: guest1@site.com / Guest123!
📄 Лиценз
Този проект е създаден като част от дипломна работа в Частна професионална гимназия за дигитални науки „СофтУни Будител“.
