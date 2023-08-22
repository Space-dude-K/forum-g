# forum-g
Форум ГУ. API и потребление. Регистрация, аутентификация, авторизация, базовый функционал с удалением, редактированием, созданием.
Миграции:
Add-Migration ForumInit -context ForumContext -o Migrations/Forum
Update-Database -context ForumContext
Remove-Migration -context ForumContext -f
