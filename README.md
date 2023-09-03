# forum-g
Цель проекта - изучение современного бэкенда веб-разработки, которую я совместил с практической пользой, или просто proof of concept, компиляция текущих навыков. Форум ГУ. API и потребление. Регистрация, аутентификация, авторизация, базовый функционал с удалением, редактированием, созданием объектов форума. Фичи: лайки, личный кабинет, аватары пользователей. Также реализовано интеграционное тестирование. (API запросы, InMemoryDB, XUnit).

Миграции:

Add-Migration ForumInit -context ForumContext -o Migrations/Forum

Update-Database -context ForumContext

Remove-Migration -context ForumContext -f
