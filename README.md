# Events Web Application

## Описание

Это веб-приложение для работы с событиями, разработанное на .NET Core с использованием Entity Framework Core. Клиентская часть реализована с помощью Angular/React. Приложение разворачивается и запускается с помощью Docker.

## Функциональность

### Web API

- Получение списка всех событий.
- Получение определенного события по его ID.
- Получение события по его названию.
- Добавление нового события.
- Изменение информации о существующем событии.
- Удаление события.
- Получение списка событий по определенным критериям (по дате, месту проведения, категории события).
- Возможность добавления изображений к событиям и их хранение.

### Участники

- Регистрация участия пользователя в событии.
- Получение списка участников события.
- Получение определенного участника по его ID.
- Отмена участия пользователя в событии.
- Отправка уведомлений участникам события об изменениях в событии (опционально).

## Структура проекта

my-project/
├── client/ (клиентская часть)
└── server/ (серверная часть)
├── project1/
├── project2/
├── project3/
├── project4/
└── project5/

## Установка и запуск

### Предварительные требования

- Установленный Docker.
- Установленный PostgreSql.
- Установленный Git.

### Клонирование репозитория

Клонируйте репозиторий на локальную машину:

   bash
   git clone https://github.com/ODapTiK/EventApp_Test.git

### Запуск приложения
   
#### Запуск серверной части

Для начала работы необходимо открыть порт 5173 для входящих подключений в брандмауэре Windows.

Перейдите в директорию с серверной частью в папку с файлом docker-compose.yml. К ней можно перейти из корня репозитория командой:

   cd EventApp.Backend/EventApp

Далее необходимо запустить серверную часть. Для этого необходимо запустить Docker Desktop и развернуть API командой:

   docker-compose up --build

После этого серверная часть запустится в docker-контейнере и станет доступна по адресу http://localhost:5173/.

#### Запуск клиентской части

Далее перейдите в директорию с package.json файлом клиентской части. К ней можно перейти из корня репозитория командой:

   cd EventApp.Frontend/event_app

Далее необходимо запустить клиентскую часть командой npm start.
Клиентская чать станет доступна по адресу http://localhost:3000.

### Заметки

 - Создание и удаление администраторов не предусмотрено в общедоступной клиентской части.
 - Для создания администратора необходимо перейти в Swagger(доступен по адресу http://localhost:5173), и возпользоваться эндпоинтом [POST] "/api/Admin"