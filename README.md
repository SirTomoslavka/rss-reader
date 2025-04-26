# RSS Reader

> A simple ASP .NET Core web app for reading and managing RSS feeds.

---

## 📖 Overview

RSS Reader lets you:

- Add any RSS feed by URL (with optional custom name).  
- List all your feeds.  
- View feed details (url, description, image).  
- Browse and filter articles by date range.  
- Search articles by title with autocomplete.  
- Delete single or multiple feeds at once.  
- Enjoy a responsive UI powered by Tailwind CSS and vanilla JavaScript.  
- See a loading overlay on navigation and form submissions.  
- Deploy easily to Azure App Service + Azure Database for PostgreSQL.

---

## 🚀 Features

- **Add Feed** – URL + optional name, server‐side & client‐side validation.  
- **Feed List** – bulk‐delete via checkboxes and modal confirmation.  
- **Feed Detail** – display feed url, description (HTML), and image.  
- **Article Listing** – automatic fetch on add, manual reload button.  
- **Date Filter** – “from”/“to” date picker with JS validation.  
- **Search** – `<datalist>` autocomplete + enable/disable submit button.  
- **Delete Modal** – reusable partial with Tailwind styling.  
- **Custom Error Page** – friendly “Something went wrong” UI.  
- **Loading Overlay** – full‐page spinner on navigation & submit.  

---

## 🛠️ Tech Stack

- **Backend**:  
  - ASP .NET Core 9.0  
  - Entity Framework Core (Code-First + Migrations)  
  - Npgsql (PostgreSQL provider)

- **Database**:  
  - PostgreSQL 11 (Azure Database for PostgreSQL)

- **Frontend**:  
  - Tailwind CSS (via npm)  
  - Vanilla JavaScript (modals, validation, overlay)

- **Deployment**:  
  - Azure App Service on Linux  
  - Azure Database for PostgreSQL  

---

## ⚙️ Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/)  
- Node.js & npm (for Tailwind build)  
- PostgreSQL
- Git  

---

## 💻 Local Setup

1. **Clone the repo**  
   ```bash
   git clone git@github.com:SirTomoslavka/rss-reader.git
   cd rss-reader
   npm i 
   dotnet build
   docker-compose up -d
   dotnet ef migrations add init
   dotnet ef database update
