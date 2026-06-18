**Структура проекта (ВАЖНО):**

```
MyFactory/
├─ MyFactory.sln
├─ Dockerfile          ← ОБЯЗАТЕЛЬНО здесь
├─ src/
│  ├─ MyFactory.WebApi/
│  ├─ MyFactory.Application/
│  ├─ MyFactory.Domain/
│  └─ MyFactory.Infrastructure/
```

**Dockerfile** — тот, который у вас сейчас (multi-stage, dotnet publish внутри контейнера).
👉 `dotnet publish` **локально НЕ делаем**.

---

# 🧠 Логика обновления (кратко)

1. Проверить, что проект собирается (`dotnet build`)
2. Собрать Docker-образ (он сам сделает publish)
3. Запушить образ в Docker Hub
4. На сервере:

   * остановить **ТОЛЬКО WebApi**
   * скачать новый образ
   * поднять WebApi обратно
5. БД **не трогается**

---

# 🖥️ ЛОКАЛЬНО (Windows, PowerShell)

> Рабочая директория: **корень решения (`MyFactory`)**

```powershell
cd V:\KworkProjects\MyFactory
```

---

## 1️⃣ Проверка сборки (ОБЯЗАТЕЛЬНО)

```powershell
dotnet build MyFactory.sln
```

❌ Ошибки — **СТОП**, Docker не трогаем
✅ Успешно — идём дальше

---

## 2️⃣ Сборка Docker-образа

```powershell
docker build -t delubis/myfactory-webapi:1.0.2 .
```

> Версию увеличивайте каждый раз (`1.0.3`, `1.0.4`, …)

---

## 3️⃣ Обновляем тег `latest`

```powershell
docker tag delubis/myfactory-webapi:1.0.2 delubis/myfactory-webapi:latest
```

---

## 4️⃣ Логин в Docker Hub (если нужно)

```powershell
docker login
```

---

## 5️⃣ Публикация образа

```powershell
docker push delubis/myfactory-webapi:latest
```

✅ На этом **локальная часть закончена**

---

# 🌍 НА СЕРВЕРЕ (Linux VM по SSH)

```powershell
ssh myfactory@<server-host>
```

вводим пароль сервера из локального хранилища паролей


```bash
cd /opt/myfactory
```

---

## 6️⃣ Удаляем Docker-контейнер с WebApi (pgdata с БД не удаляется)

```bash
docker compose down
```

---

## 7️⃣ Скачиваем новый образ

```bash
docker compose pull
```

---

## 8️⃣ Поднимаем WebApi обратно

```bash
docker compose up -d
```

---

## 9️⃣ Проверка

```bash
docker compose ps
```

Swagger:

```
http://<server-host>:5000/swagger
```

---

# ✅ Итог

✔ База данных сохранена
✔ Миграции не затронуты
✔ Обновлены Domain / Application
✔ Dockerfile не меняется
✔ Publish локально не нужен
✔ Повторяемо и безопасно

---

