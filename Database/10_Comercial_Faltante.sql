/* ============================================================
   10_Comercial_Faltante.sql
   Completa el módulo Comercial (Pasajero / Reserva / Boleto-Equipaje /
   Asiento / Pago) que faltaba en el esquema.
   Es IDEMPOTENTE: se puede ejecutar varias veces sin romper nada,
   solo crea lo que no exista todavía. Revisa los nombres de columnas
   contra tu BD real antes de ejecutar; siguen la misma convención
   (snake_case, esquema "brisky") que el resto del proyecto.
   ============================================================ */

-- 1) PASAJERO -------------------------------------------------
IF OBJECT_ID('brisky.pasajero', 'U') IS NULL
BEGIN
    CREATE TABLE brisky.pasajero (
        cod_pasajero      VARCHAR(20)   NOT NULL PRIMARY KEY,
        nombre            VARCHAR(60)   NOT NULL,
        apellido          VARCHAR(60)   NOT NULL,
        num_documento     VARCHAR(20)   NOT NULL UNIQUE,
        nacionalidad      VARCHAR(40)   NULL,
        fecha_nacimiento  DATE          NOT NULL,
        telefono          VARCHAR(20)   NULL,
        correo            VARCHAR(80)   NULL
    );
END
GO

-- 2) RESERVA (tabla base) + subtipos ---------------------------
IF OBJECT_ID('brisky.reserva', 'U') IS NULL
BEGIN
    CREATE TABLE brisky.reserva (
        cod_reserva   VARCHAR(20)     NOT NULL PRIMARY KEY,
        fecha         DATETIME        NOT NULL DEFAULT GETDATE(),
        precio        DECIMAL(10,2)   NOT NULL,
        estado        VARCHAR(20)     NOT NULL DEFAULT 'PENDIENTE', -- PENDIENTE / CONFIRMADA / CANCELADA
        cod_pasajero  VARCHAR(20)     NOT NULL REFERENCES brisky.pasajero(cod_pasajero),
        id_vuelo      INT             NOT NULL REFERENCES brisky.vuelo(id_vuelo),
        cod_tarifa    VARCHAR(20)     NOT NULL REFERENCES brisky.tarifa(cod_tarifa),
        tipo_reserva  VARCHAR(20)     NOT NULL -- OFICINA / INTERNET
    );
END
GO

IF OBJECT_ID('brisky.reserva_oficina', 'U') IS NULL
BEGIN
    CREATE TABLE brisky.reserva_oficina (
        cod_reserva   VARCHAR(20)  NOT NULL PRIMARY KEY
                      REFERENCES brisky.reserva(cod_reserva) ON DELETE CASCADE,
        cod_empleado  VARCHAR(20)  NOT NULL REFERENCES brisky.empleado(cod_empleado)
    );
END
GO

IF OBJECT_ID('brisky.reserva_internet', 'U') IS NULL
BEGIN
    CREATE TABLE brisky.reserva_internet (
        cod_reserva     VARCHAR(20)  NOT NULL PRIMARY KEY
                        REFERENCES brisky.reserva(cod_reserva) ON DELETE CASCADE,
        fecha_hora_web  DATETIME     NOT NULL DEFAULT GETDATE(),
        ip_origen       VARCHAR(45)  NULL
    );
END
GO

-- 3) ASIENTO ----------------------------------------------------
IF OBJECT_ID('brisky.asiento', 'U') IS NULL
BEGIN
    CREATE TABLE brisky.asiento (
        num_asiento  VARCHAR(5)  NOT NULL,
        id_vuelo     INT         NOT NULL REFERENCES brisky.vuelo(id_vuelo),
        disponible   BIT         NOT NULL DEFAULT 1,
        clase        VARCHAR(20) NULL,
        CONSTRAINT pk_asiento PRIMARY KEY (id_vuelo, num_asiento)
    );
END
GO

-- 4) EQUIPAJE (depende de Boleto) --------------------------------
IF OBJECT_ID('brisky.equipaje', 'U') IS NULL
BEGIN
    CREATE TABLE brisky.equipaje (
        cod_equipaje  VARCHAR(20)    NOT NULL PRIMARY KEY,
        num_boleto    VARCHAR(20)    NOT NULL REFERENCES brisky.boleto(num_boleto),
        tipo          VARCHAR(30)    NULL,
        peso          FLOAT          NOT NULL DEFAULT 0,
        cantidad      INT            NOT NULL DEFAULT 1
    );
END
GO

-- 5) PAGO (depende de Reserva) -------------------------------------
IF OBJECT_ID('brisky.pago', 'U') IS NULL
BEGIN
    CREATE TABLE brisky.pago (
        cod_pago      VARCHAR(20)    NOT NULL PRIMARY KEY,
        cod_reserva   VARCHAR(20)    NOT NULL REFERENCES brisky.reserva(cod_reserva),
        monto         DECIMAL(10,2)  NOT NULL,
        fecha         DATETIME       NOT NULL DEFAULT GETDATE(),
        metodo        VARCHAR(30)    NULL
    );
END
GO

/* ============================================================
   STORED PROCEDURES (mismo patrón que brisky.crear_empleado_oficina)
   ============================================================ */

IF OBJECT_ID('brisky.crear_reserva_oficina', 'P') IS NOT NULL
    DROP PROCEDURE brisky.crear_reserva_oficina;
GO
CREATE PROCEDURE brisky.crear_reserva_oficina
    @p_cod_reserva   VARCHAR(20),
    @p_fecha         DATETIME,
    @p_precio        DECIMAL(10,2),
    @p_estado        VARCHAR(20),
    @p_cod_pasajero  VARCHAR(20),
    @p_id_vuelo      INT,
    @p_cod_tarifa    VARCHAR(20),
    @p_cod_empleado  VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO brisky.reserva (cod_reserva, fecha, precio, estado, cod_pasajero, id_vuelo, cod_tarifa, tipo_reserva)
        VALUES (@p_cod_reserva, @p_fecha, @p_precio, @p_estado, @p_cod_pasajero, @p_id_vuelo, @p_cod_tarifa, 'OFICINA');

        INSERT INTO brisky.reserva_oficina (cod_reserva, cod_empleado)
        VALUES (@p_cod_reserva, @p_cod_empleado);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

IF OBJECT_ID('brisky.crear_reserva_internet', 'P') IS NOT NULL
    DROP PROCEDURE brisky.crear_reserva_internet;
GO
CREATE PROCEDURE brisky.crear_reserva_internet
    @p_cod_reserva     VARCHAR(20),
    @p_fecha           DATETIME,
    @p_precio          DECIMAL(10,2),
    @p_estado          VARCHAR(20),
    @p_cod_pasajero    VARCHAR(20),
    @p_id_vuelo        INT,
    @p_cod_tarifa      VARCHAR(20),
    @p_fecha_hora_web  DATETIME,
    @p_ip_origen       VARCHAR(45) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO brisky.reserva (cod_reserva, fecha, precio, estado, cod_pasajero, id_vuelo, cod_tarifa, tipo_reserva)
        VALUES (@p_cod_reserva, @p_fecha, @p_precio, @p_estado, @p_cod_pasajero, @p_id_vuelo, @p_cod_tarifa, 'INTERNET');

        INSERT INTO brisky.reserva_internet (cod_reserva, fecha_hora_web, ip_origen)
        VALUES (@p_cod_reserva, @p_fecha_hora_web, @p_ip_origen);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
