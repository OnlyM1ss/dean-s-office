
# dean-s-office
Короче, Артем, я тебя спас и в благородство играть не буду: кинешь мне денег за приложуху — и мы в расчете. Заодно посмотрим, как быстро у тебя после этого кода башка прояснится. А по твоей теме постараюсь разузнать. Хрен его знает, на кой ляд тебе этот диплом шараги сдался, но я в чужие дела не лезу, хочешь диплом, значит есть за что...
![alt text](ContosoApp/Assets/J8B8Dm-PhD0oG9nF3L69BOLZLbFTjsR0TjzVYQPbb0IQ7J7qfnAIogN-5AjgCnV9JjGtnU67nqQX04hum5UO5-KC-93uyoXtKV5JWYq-rBbljbtxBpyB9tq4UrTC32iQUW_F_-50G7YUK3-I4Mhytuy5agCAZZOunkxDPc8AmnJlBWtY4AXUeiISVZYesrTf7BgocZRAnHoM.jpg)
## Что это такое???
Это приложение написаное на [UWP](//metanit.com/sharp/uwp/1.1.php), и разделен на 4 логических решения:
- [Contoso.App](Contoso.EFCoreCLIHelper)
  - это основной проект с приложением, написан по паттерну [MVVM](//metanit.com/sharp/wpf/22.1.php)
    - VM - связь между моделью и вью
    - View - как раз те самые представления
    - UserControl - вспомогательные контролы (грубо говоря как page, только маленькие и их можно вставлять в page)
    - Styles - стили для определенных элементов приложения
    - State Triggers - триггер для мобилки
    - Assets - картино4ки)
- [ContosoModels](ContosoModels)
  - проект с моделями, думаю разберешься, но по некоторым моментам уточнимся
    -  DbObject - это класс с id для каждого класса, используем [GUID](https://ru.wikipedia.org/wiki/GUID)
    -  [IEquarable](https://docs.microsoft.com/ru-ru/dotnet/api/system.iequatable-1?view=netframework-4.8) - [хорошее обьяснение](https://ru.stackoverflow.com/questions/841939/%D0%97%D0%B0%D1%87%D0%B5%D0%BC-%D0%BC%D1%8B-%D1%80%D0%B5%D0%B0%D0%BB%D0%B8%D0%B7%D0%BE%D0%B2%D1%8B%D0%B2%D0%B0%D0%B5%D0%BC-iequatablet-%D0%B5%D1%81%D0%BB%D0%B8-equals-%D0%B5%D1%81%D1%82%D1%8C-%D0%B2-object)
    -  Constants - константы для хранения переменных среды, то есть тут мы записываем всякие хни типа api ключа, url и прочее для работы с сервером
- [ContosoRepository](ContosoRepository)
  - репозиторий для работы с бд и сервером  есть интерфейс для каждой модели - это специальные интерфейсы определяющие методы для работы с этими самыми моделями, мы их делаем для того, что бы использовать паттерн [стратегия](https://refactoring.guru/ru/design-patterns/strategy) - то есть мы можем выделить общий интерфейс для работы как и с бд, так и с серверной бд
    - REST - папочка, которая хранит в себе реализацию логики с сервером
    - SQl - папаня для бд, тут мы работаем непосредственно с локальной базой
    - Migrations - тут хранятся наши [миграции](https://metanit.com/sharp/entityframework/3.12.php) от [ef](https://metanit.com/sharp/entityframework/)
- [ContosoService](ContosoService)
  - [asp net](https://metanit.com/sharp/aspnet5/) проект, который берет на себя логику работы с сервером, написан что-то типа по [MVC](https://ru.wikipedia.org/wiki/Model-View-Controller)расмотрим поподробнее:
    - Controller - тут у нас логика обращения к серверу
    - все остальное при изучени Asp net XD
