# Catálogo de Videojuegos – WPF + SQLite

Aplicación de escritorio en **C# / WPF** para gestionar videojuegos organizados por plataformas, con base de datos **SQLite**.

## 📋 Modelo de datos

**Relación 1:N entre Plataformas (tabla principal) y Juegos (tabla secundaria).**

### Tabla `Plataformas`
- `Id` – INTEGER PRIMARY KEY AUTOINCREMENT
- `Nombre` – TEXT NOT NULL

### Tabla `Juegos`
- `Id` – INTEGER PRIMARY KEY AUTOINCREMENT
- `Titulo` – TEXT NOT NULL
- `Genero` – TEXT NOT NULL
- `Nota` – REAL
- `PlataformaId` – INTEGER NOT NULL (FOREIGN KEY)

---

## 🗂️ Estructura del proyecto

```
VideoGameCatalog/
├── App.xaml
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── Database/
│   └── DatabaseManager.cs
├── Models/
│   ├── Plataforma.cs
│   └── Juego.cs
└── videogames.db
```

---

## 🎯 Funcionalidades

### Gestión de Plataformas (CRUD)
- ListBox con todas las plataformas
- Insertar, actualizar y eliminar plataformas
- Validación: nombre obligatorio

### Gestión de Juegos (CRUD)
- ListBox sincronizado: muestra juegos de la plataforma seleccionada
- Insertar, actualizar y eliminar juegos
- Panel de edición: Título, Género, Nota, Plataforma
- Validación: Título, Género y Plataforma obligatorios; Nota numérica

### Listados sincronizados
- Al seleccionar una plataforma, se cargan automáticamente sus juegos
- ComboBox de plataformas se actualiza dinámicamente

---

## 🎨 Diseño

**Grid 3 columnas:**
- Columna 1: Listado de plataformas + panel edición
- Columna 2: Listado sincronizado de juegos
- Columna 3: Panel de edición de juegos

**Colores:**
- Encabezado: azul oscuro (#003366) con texto blanco
- Fondo: gris claro (#F0F0F0)
- Botones: Verde (Guardar), Azul (Limpiar), Rojo (Eliminar)

---

## 🛠️ Tecnologías

- **Lenguaje:** C#
- **Interfaz:** WPF / XAML
- **Base de datos:** SQLite (System.Data.SQLite.Core)

---

## ▶️ Cómo ejecutar

1. Abrir `VideoGameCatalog.sln` en Visual Studio
2. Compilar (`Ctrl + B`)
3. Ejecutar (`F5`)
4. La BD se crea automáticamente en la primera ejecución

---

## ✅ Características implementadas

✓ CRUD completo (Insertar, Leer, Actualizar, Eliminar)  
✓ Listados sincronizados (maestro-detalle)  
✓ Validaciones (campos obligatorios)  
✓ MessageBox (confirmaciones y errores)  
✓ Grid con 3 columnas  
✓ Colores diferenciados  
✓ Base de datos auto-inicializable
