/* -----------------------------------------------------------------------------
   SeedData.sql
   Purpose: populate Immotech sample database with dummy agencies, properties
            and photos for local development / demos.
   NOTE: keep this file separate from migrations so it can be rerun safely.
   Run it with:  sqlcmd -S localhost,1433 -U sa -P "Your_password123!" -d ImmotechDb -i SeedData.sql
 -----------------------------------------------------------------------------*/

SET NOCOUNT ON;

/* -------------------------------------------------------------------------
   1. Agencies
--------------------------------------------------------------------------*/

IF NOT EXISTS (SELECT 1 FROM Agencies)
BEGIN
    PRINT 'Inserting agencies...';
    INSERT INTO Agencies (Name, Address_Street, Address_City, Address_State, Address_ZipCode, ContactEmail, LogoUrl)
    VALUES
        ('Blue Brick Realty',  '123 Maple St.',  'Brussels', 'Brussels', '1000', 'contact@bluebrick.be', 'https://via.placeholder.com/64/0066cc/ffffff?text=BB'),
        ('Sunshine Properties', '50 Sunny Ave.',  'Antwerp',  'Antwerp',  '2000', 'hello@sunshine.be',   'https://via.placeholder.com/64/ffaa00/000000?text=SP'),
        ('GreenLeaf Homes',    '9 Forest Rd.',   'Ghent',    'East Flanders', '9000', 'info@greenleaf.be', 'https://via.placeholder.com/64/00aa66/ffffff?text=GL');
END;
GO

/* Capture generated Agency IDs for later use */
DECLARE @BlueBrickId   INT = (SELECT TOP 1 Id FROM Agencies WHERE Name = 'Blue Brick Realty');
DECLARE @SunshineId    INT = (SELECT TOP 1 Id FROM Agencies WHERE Name = 'Sunshine Properties');
DECLARE @GreenLeafId   INT = (SELECT TOP 1 Id FROM Agencies WHERE Name = 'GreenLeaf Homes');

/* -------------------------------------------------------------------------
   2. Properties
--------------------------------------------------------------------------*/

PRINT 'Inserting properties...';

/* Only add when table empty to avoid duplicates */
IF NOT EXISTS (SELECT 1 FROM Properties)
BEGIN
    INSERT INTO Properties (Id, Title, Description, Address_Street, Address_City, Address_State, Address_ZipCode,
                             Location, Price, Status, CreatedDate, AgencyId, Bedrooms, SurfaceArea)
    VALUES
        (NEWID(), 'Loft moderne dans le centre ville', 'Loft spacieux avec hauts plafonds et cuisine ouverte.',
         '12 Central Plaza', 'Brussels', 'Brussels', '1000', 'Brussels', 425000, 0, SYSDATETIMEOFFSET(), @BlueBrickId, 2, 120),

        (NEWID(), 'Maison familiale chaleureuse avec jardin', 'Maison de 3 chambres avec jardin ensoleillé.',
         '8 Tulip Street', 'Antwerp', 'Antwerp', '2000', 'Antwerp', 350000, 0, SYSDATETIMEOFFSET(), @SunshineId, 3, 150),

        (NEWID(), 'Appartement en bord de rivière', 'Magnifiques vues sur la rivière depuis la salle de séjour et la terrasse.',
         '77 River Rd.', 'Ghent', 'East Flanders', '9000', 'Ghent', 295000, 0, SYSDATETIMEOFFSET(), @GreenLeafId, 1, 80),

        (NEWID(), 'Penthouse de luxe', 'Penthouse au toit avec terrasse privée et jacuzzi.',
         '250 Skyline Ave.', 'Brussels', 'Brussels', '1000', 'Brussels', 890000, 0, SYSDATETIMEOFFSET(), @BlueBrickId, 4, 200),

        (NEWID(), 'Villa suburbaine avec piscine', 'Villa grande dans un quartier tranquille, avec piscine et garage.',
         '5 Lavender Lane', 'Antwerp', 'Antwerp', '2610', 'Antwerp', 725000, 0, SYSDATETIMEOFFSET(), @SunshineId, 5, 300);
END;
GO

/* -------------------------------------------------------------------------
   3. Photos – 3 pics per property, first = main
--------------------------------------------------------------------------*/

PRINT 'Inserting photos...';

DECLARE property_cursor CURSOR FOR SELECT Id FROM Properties;
DECLARE @propId UNIQUEIDENTIFIER;

OPEN property_cursor;
FETCH NEXT FROM property_cursor INTO @propId;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- main photo
    INSERT INTO Photos (PropertyId, Url, UploadedAt, IsMain)
    VALUES (@propId, 'https://source.unsplash.com/featured/800x600?house,' + CONVERT(varchar(36), @propId), SYSDATETIMEOFFSET(), 1);

    -- two extra photos
    INSERT INTO Photos (PropertyId, Url, UploadedAt, IsMain)
    VALUES (@propId, 'https://source.unsplash.com/featured/800x600?living-room,' + CONVERT(varchar(36), NEWID()), SYSDATETIMEOFFSET(), 0),
           (@propId, 'https://source.unsplash.com/featured/800x600?kitchen,' + CONVERT(varchar(36), NEWID()), SYSDATETIMEOFFSET(), 0);

    FETCH NEXT FROM property_cursor INTO @propId;
END;

CLOSE property_cursor;
DEALLOCATE property_cursor;

PRINT 'Seed data inserted.'; 