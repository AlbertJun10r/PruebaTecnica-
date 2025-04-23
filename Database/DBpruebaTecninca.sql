CREATE DATABASE PruebaTecnica;
USE PruebaTecnica;
GO

-- Tabla de Productos
CREATE TABLE Productos (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    precio DECIMAL(10, 2) NOT NULL,
    stock INT NOT NULL DEFAULT 0
);

-- Tabla de Clientes
CREATE TABLE Clientes (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    email VARCHAR(100),
    telefono VARCHAR(20)
);

-- Tabla de Ventas
CREATE TABLE Ventas (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    cliente_ID INT NOT NULL,
    total DECIMAL(12, 2) NOT NULL,
    FOREIGN KEY (cliente_ID) REFERENCES Clientes(ID)
);

-- Tabla intermedia para la relación muchos-a-muchos entre Ventas y Productos
CREATE TABLE Ventas_Productos (
    venta_ID INT NOT NULL,
    producto_ID INT NOT NULL,
    cantidad INT NOT NULL DEFAULT 1,
    precio_unitario DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY (venta_ID, producto_ID),
    FOREIGN KEY (venta_ID) REFERENCES Ventas(ID),
    FOREIGN KEY (producto_ID) REFERENCES Productos(ID)
);

-- Insertar datos en la tabla Clientes
INSERT INTO Clientes (nombre, email, telefono) VALUES
('Juan Pérez', 'juan.perez@email.com', '555-123-4567'),
('María García', 'maria.garcia@email.com', '555-234-5678'),
('Carlos López', 'carlos.lopez@email.com', '555-345-6789'),
('Ana Martínez', 'ana.martinez@email.com', '555-456-7890'),
('Luis Rodríguez', 'luis.rodriguez@email.com', '555-567-8901');

-- Insertar datos en la tabla Productos
INSERT INTO Productos (nombre, descripcion, precio, stock) VALUES
('Laptop HP', 'Laptop HP 15.6", 8GB RAM, 256GB SSD', 899.99, 15),
('Smartphone Samsung', 'Samsung Galaxy S21, 128GB, 5G', 749.99, 20),
('Tablet Apple', 'iPad Air 10.9", 64GB, Wi-Fi', 599.99, 12),
('Monitor LG', 'Monitor LG 27" 4K UHD', 349.99, 8),
('Teclado mecánico', 'Teclado mecánico RGB, switches azules', 89.99, 30),
('Mouse inalámbrico', 'Mouse Logitech inalámbrico, 1600DPI', 29.99, 50),
('Disco duro externo', 'Disco duro 1TB USB 3.0', 59.99, 25);

-- Insertar datos en la tabla Ventas
INSERT INTO Ventas (fecha, cliente_ID, total) VALUES
('2023-05-10 10:30:00', 1, 1649.98),
('2023-05-11 14:15:00', 3, 599.99),
('2023-05-12 09:45:00', 2, 439.98),
('2023-05-13 16:20:00', 5, 89.99),
('2023-05-14 11:10:00', 4, 1279.97);

-- Insertar los productos asociados a cada venta
INSERT INTO Ventas_Productos (venta_ID, producto_ID, cantidad, precio_unitario) VALUES
(1, 1, 1, 899.99),
(1, 2, 1, 749.99),
(2, 3, 1, 599.99),
(3, 4, 1, 349.99),
(3, 6, 3, 29.99),
(4, 5, 1, 89.99),
(5, 2, 1, 749.99),
(5, 7, 2, 59.99);


-- Verificar si la tabla existe pero le falta la columna
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Productos')
BEGIN
    -- Agregar la columna si no existe
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                   WHERE TABLE_NAME = 'Productos' AND COLUMN_NAME = 'FechaCreacion')
    BEGIN
        ALTER TABLE [dbo].[Productos]
        ADD [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE()
        
        PRINT 'Columna FechaCreacion agregada a la tabla'
    END
    ELSE
    BEGIN
        PRINT 'La columna FechaCreacion ya existe en la tabla'
    END
END
ELSE
BEGIN
    PRINT 'La tabla Clientes no existe. Ejecuta el script completo de creación.'
END

-- Verificar si la tabla existe pero le falta la columna
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Clientes')
BEGIN
    -- Agregar la columna si no existe
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                   WHERE TABLE_NAME = 'Clientes' AND COLUMN_NAME = 'FechaCreacion')
    BEGIN
        ALTER TABLE [dbo].[Clientes]
        ADD [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE()
        
        PRINT 'Columna FechaCreacion agregada a la tabla Clientes'
    END
    ELSE
    BEGIN
        PRINT 'La columna FechaCreacion ya existe en la tabla Clientes'
    END
END
ELSE
BEGIN
    PRINT 'La tabla Clientes no existe. Ejecuta el script completo de creación.'
END

-- Verificar si la tabla existe pero le falta la columna
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Ventas')
BEGIN
    -- Agregar la columna si no existe
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                   WHERE TABLE_NAME = 'Ventas' AND COLUMN_NAME = 'FechaCreacion')
    BEGIN
        ALTER TABLE [dbo].[Ventas]
        ADD [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE()
        
        PRINT 'Columna FechaCreacion agregada a la tabla Clientes'
    END
    ELSE
    BEGIN
        PRINT 'La columna FechaCreacion ya existe en la tabla Clientes'
    END
END
ELSE
BEGIN
    PRINT 'La tabla Clientes no existe. Ejecuta el script completo de creación.'
END