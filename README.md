# 🎓 SchoolApp-Redux

Complete school management application built with **.NET** for the backend and **Angular** for the frontend. It leverages modern technologies like **MongoDB**, **Auth0**, **NgxPermissions**, **Redux**, and follows a **Hexagonal Architecture** in the backend. On the frontend, it uses standalone components and **Angular Signals** for advanced reactivity.

---

## 🧱 Project Structure

```
SchoolApp-Redux/
├── SchoolApp-back/   # Backend in .NET with Hexagonal Architecture
└── SchoolApp-front/  # Frontend in Angular using standalone components + Redux
```

---

## 🚀 Technologies Used

### Backend - [.NET](https://dotnet.microsoft.com/)

- .NET 8
- MongoDB
- Auth0 (OAuth2/JWT)
- Hexagonal Architecture (Ports & Adapters)
- Dependency Injection
- Clean Code & SOLID Principles

### Frontend - [Angular](https://angular.io/)

- Angular 18+
- Standalone Components
- Angular Signals
- Redux (store, actions, reducers, selectors)
- NgxPermissionsModule
- Angular Material
- Lazy Loading
- Owl DateTime Picker
- Reactive Forms

---

## 🔐 Authentication

- Integrated with **Auth0** using OAuth2 and JWT
- Route protection
- Dynamic role-based permissions with `NgxPermissionsModule`

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) v18+
- [Angular CLI](https://angular.io/cli)
- [MongoDB](https://www.mongodb.com/)

### Backend

```bash
cd SchoolApp-back
dotnet restore
dotnet run
```

API will be available at: `https://localhost:5001` or `http://localhost:5000`

### Frontend

```bash
cd SchoolApp-front
npm install
ng serve
```

Angular app will be available at: `http://localhost:4200`

---

## 🧪 Testing

### Backend

```bash
dotnet test
```

### Frontend

```bash
ng test
```

---

## 📁 Hexagonal Architecture Overview

```
SchoolApp-back/
├── Application/      # Use cases
├── Domain/           # Entities and interfaces (ports)
├── Infrastructure/   # Implementations (adapters)
└── WebApi/           # Entry point (controllers)
```

---

## 💡 Features

- Role-based permission control
- Auth0 login system
- Interactive dashboard
- Reactive forms with validations
- Full CRUD integrated with Redux
- Standalone components using Angular Signals for reactivity

---

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## 👨‍💻 Author

Made with 💙 by [@BetynMineiro](https://github.com/BetynMineiro)
