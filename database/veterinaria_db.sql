-- Laboratorio 12: Sistema de gestión de veterinaria.
-- Ejecución local: abrir este archivo en SSMS, conectarse a
-- DESKTOP-7S1EHKS\SQLEXPRESS con Windows Authentication y ejecutar todo.
-- Para una base de datos en la nube, omitir el bloque CREATE DATABASE/USE
-- y ejecutar el resto dentro de la base de datos ya creada.

IF DB_ID(N'VeterinariaDB') IS NULL
BEGIN
    CREATE DATABASE VeterinariaDB;
END;
GO

USE VeterinariaDB;
GO

-- El script no elimina ni reemplaza tablas o datos existentes.
IF OBJECT_ID(N'dbo.Propietarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Propietarios
    (
        IdPropietario INT IDENTITY(1, 1) NOT NULL,
        Nombres       NVARCHAR(80) NOT NULL,
        Apellidos     NVARCHAR(80) NOT NULL,
        Dni           CHAR(8) NOT NULL,
        Telefono      NVARCHAR(20) NOT NULL,
        Correo        NVARCHAR(120) NOT NULL,
        Direccion     NVARCHAR(200) NOT NULL,

        CONSTRAINT PK_Propietarios PRIMARY KEY (IdPropietario),
        CONSTRAINT UQ_Propietarios_Dni UNIQUE (Dni),
        CONSTRAINT CK_Propietarios_Dni_Formato CHECK (Dni NOT LIKE '%[^0-9]%')
    );
END;
GO

IF OBJECT_ID(N'dbo.Mascotas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Mascotas
    (
        IdMascota       INT IDENTITY(1, 1) NOT NULL,
        Nombre          NVARCHAR(80) NOT NULL,
        Especie         NVARCHAR(50) NOT NULL,
        Raza            NVARCHAR(80) NOT NULL,
        Sexo            CHAR(1) NOT NULL,
        FechaNacimiento DATE NOT NULL,
        Peso            DECIMAL(6, 2) NOT NULL,
        IdPropietario   INT NOT NULL,

        CONSTRAINT PK_Mascotas PRIMARY KEY (IdMascota),
        CONSTRAINT CK_Mascotas_Sexo CHECK (Sexo IN ('M', 'H')),
        CONSTRAINT CK_Mascotas_FechaNacimiento CHECK (FechaNacimiento <= CAST(GETDATE() AS DATE)),
        CONSTRAINT CK_Mascotas_Peso CHECK (Peso > 0 AND Peso <= 500),
        CONSTRAINT FK_Mascotas_Propietarios
            FOREIGN KEY (IdPropietario)
            REFERENCES dbo.Propietarios (IdPropietario)
            ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Mascotas_IdPropietario'
      AND object_id = OBJECT_ID(N'dbo.Mascotas')
)
BEGIN
    CREATE INDEX IX_Mascotas_IdPropietario ON dbo.Mascotas (IdPropietario);
END;
GO

-- Datos iniciales: se insertan solo cuando las tablas aún no tienen datos.
IF NOT EXISTS (SELECT 1 FROM dbo.Propietarios)
BEGIN
    INSERT INTO dbo.Propietarios (Nombres, Apellidos, Dni, Telefono, Correo, Direccion)
    VALUES
        (N'Ana María', N'Torres Salazar', '45872136', N'987654321', N'ana.torres@example.com', N'Av. Los Olivos 245, Lima'),
        (N'Carlos', N'Ramírez Soto', '71234568', N'986123456', N'carlos.ramirez@example.com', N'Jr. Las Flores 510, Lima'),
        (N'Lucía', N'Vega Castro', '63458921', N'985456789', N'lucia.vega@example.com', N'Calle San Martín 118, Lima');
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Mascotas)
BEGIN
    INSERT INTO dbo.Mascotas (Nombre, Especie, Raza, Sexo, FechaNacimiento, Peso, IdPropietario)
    VALUES
        (N'Max', N'Perro', N'Labrador Retriever', 'M', '2021-05-14', 28.50, 1),
        (N'Luna', N'Gato', N'Siamés', 'H', '2022-09-02', 4.20, 1),
        (N'Rocky', N'Perro', N'Beagle', 'M', '2020-11-20', 13.80, 2),
        (N'Mía', N'Gato', N'Persa', 'H', '2023-01-10', 3.90, 3);
END;
GO

-- Verificación no destructiva.
SELECT * FROM dbo.Propietarios ORDER BY IdPropietario;
SELECT * FROM dbo.Mascotas ORDER BY IdMascota;
GO
