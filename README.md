# forum-g
Целью проекта - изучение современного бэканда веб разарботки, компиляция моих навыков, которую я совместил с практической пользой, или просто proof of concept. Форум ГУ. API и потребление. Регистрация, аутентификация, авторизация, базовый функционал с удалением, редактированием, созданием.

Миграции:

Add-Migration ForumInit -context ForumContext -o Migrations/Forum

Update-Database -context ForumContext

Remove-Migration -context ForumContext -f
